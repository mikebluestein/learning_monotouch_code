
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BonjourClient
{
    public partial class ClientViewController : UIViewController
    {
        List<NSNetService> _serviceList;
        NSNetServiceBrowser _netBrowser;
        ServicesTableSource _source;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public ClientViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public ClientViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public ClientViewController () : base("ClientViewController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            InitNetBrowser ();
        }

        internal void InitNetBrowser ()
        {
            _serviceList = new List<NSNetService> ();
            _netBrowser = new NSNetServiceBrowser ();
            
            _source = new ServicesTableSource (this);
            servicesTable.Source = _source;
            
            _netBrowser.SearchForServices ("_bonjourdemoservice._tcp", "");
            
            _netBrowser.FoundService += delegate(object sender, NSNetServiceEventArgs e) {
                logView.AppendTextLine (String.Format ("{0} added", e.Service.Name));
                
                _serviceList.Add (e.Service);
                
                e.Service.AddressResolved += ServiceAddressResolved;
                
                // NOTE: could also insert and remove rows in a
                // more fine grained fashion here as well
                servicesTable.ReloadData ();
            };
            
            _netBrowser.ServiceRemoved += delegate(object sender, NSNetServiceEventArgs e) {
                logView.AppendTextLine (String.Format ("{0} removed", e.Service.Name));
                
                var nsService = _serviceList.Single (s => s.Name.Equals (e.Service.Name));
                _serviceList.Remove (nsService);
                servicesTable.ReloadData ();
            };
        }

        void ServiceAddressResolved (object sender, EventArgs e)
        {
            NSNetService ns = sender as NSNetService;
            
            if (ns != null)
                CallServer (ns);
        }

        void CallServer (NSNetService ns)
        {
            if (ns != null) {
                string hostName = ns.HostName;
                int port = ns.Port;
                
                try {
                    
                    TcpClient tcpClient = new TcpClient (hostName, port);
                    using (NetworkStream netStream = tcpClient.GetStream ()) {
                        
                        string hello = "hello from TcpClient";
                        byte[] sendBuffer = Encoding.ASCII.GetBytes (hello);
                        netStream.Write (sendBuffer, 0, sendBuffer.Length);
                        
                        int maxSize = 1024;
                        byte[] receiveBuffer = new Byte[maxSize];
                        int length = netStream.Read (receiveBuffer, 0, receiveBuffer.Length);
                        string response = Encoding.ASCII.GetString (receiveBuffer, 0, length);
                        
                        logView.AppendTextLine (response);
                        
                    }
                    tcpClient.Close ();
                } catch (Exception ex) {
                    logView.AppendTextLine (String.Format ("exception calling server: {0}", ex));
                }
            }
        }

        class ServicesTableSource : UITableViewSource
        {
            ClientViewController _controller;
            const string SERVICE_CELL_ID = "servicecell";

            public ServicesTableSource (ClientViewController controller)
            {
                _controller = controller;
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _controller._serviceList.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                var serviceCell = tableView.DequeueReusableCell (SERVICE_CELL_ID) ?? 
                    new UITableViewCell (UITableViewCellStyle.Value1, SERVICE_CELL_ID);
                
                NSNetService ns = _controller._serviceList[indexPath.Row];
                
                serviceCell.TextLabel.Text = ns.Name;
                
                return serviceCell;
            }

            public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                NSNetService ns = _controller._serviceList[indexPath.Row];
                if (String.IsNullOrEmpty (ns.HostName))
                    ns.Resolve (60);
                else
                    _controller.CallServer (ns);
                
                tableView.DeselectRow (indexPath, true);
            }
        }
        
    }
}

