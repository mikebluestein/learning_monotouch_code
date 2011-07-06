
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace BonjourServer
{
    public class Application
    {
        static void Main (string[] args)
        {
            UIApplication.Main (args);
        }
    }

    // The name AppDelegate is referenced in the MainWindow.xib file.
    public partial class AppDelegate : UIApplicationDelegate
    {
        NetDelegate _netDel;
        NSNetService _ns;
        TcpListener _tcpServer;

        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            _ns = new NSNetService ("", "_bonjourdemoservice._tcp", UIDevice.CurrentDevice.Name, 9999);
            
            _netDel = new NetDelegate (this);
            
            _ns.Delegate = _netDel;
            
            //BUG: monotouch 3.3.1 has TXTRecordData incorrectly bound to a bool. will be fixed in v.Next
            //string message = "some message about the service...";     
            //_ns.TxtRecordData = NSData.FromString(message);
            
            _ns.Publish ();
            
            window.MakeKeyAndVisible ();
            
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }

        public override void WillTerminate (UIApplication application)
        {
            _ns.Stop ();
            _tcpServer.Stop ();
        }

        public override void WillEnterForeground (UIApplication application)
        {
            _ns.Publish ();
        }

        public override void DidEnterBackground (UIApplication application)
        {
            _ns.Stop ();
            _tcpServer.Stop ();
        }

        class NetDelegate : NSNetServiceDelegate
        {
            AppDelegate _controller;

            public NetDelegate (AppDelegate controller)
            {
                _controller = controller;
            }

            public override void Published (NSNetService sender)
            {
                ThreadStart ts = new ThreadStart (delegate {
                    using (var pool = new NSAutoreleasePool ()) {
                        try {
                            string hostName = String.Format ("{0}.local", Dns.GetHostName ());
                            IPHostEntry hostEntry = Dns.GetHostEntry (hostName);
                            IPAddress serverAddress = hostEntry.AddressList[1];
                            _controller._tcpServer = new TcpListener (serverAddress, sender.Port);
                            _controller._tcpServer.Start ();
                            
                            Log ("server started");
                            
                            int maxReadSize = 1024;
                            byte[] requestBuffer = new Byte[maxReadSize];
                            
                            while (true) {
                                TcpClient connectingClient = _controller._tcpServer.AcceptTcpClient ();
                                
                                using (NetworkStream netStream = connectingClient.GetStream ()) {
                                    
                                    int size = netStream.Read (requestBuffer, 0, requestBuffer.Length);
                                    
                                    string request = Encoding.ASCII.GetString (requestBuffer, 0, size);
                                    
                                    Log (String.Format ("server received: {0}", request));
                                    
                                    string response = String.Format ("server echoed: {0}", request);
                                    
                                    byte[] responseBuffer = Encoding.ASCII.GetBytes (response);
                                    
                                    netStream.Write (responseBuffer, 0, responseBuffer.Length);
                                    
                                    Log (response);
                                }
                                
                                connectingClient.Close ();
                            }
                        } catch (SocketException e) {
                            Log (String.Format ("SocketException: {0}, Native Error Code = {0}", e.Message, e.NativeErrorCode));
                        }
                    }
                });
                Thread t = new Thread (ts);
                t.Start ();
            }

            public override void PublishFailure (NSNetService sender, NSDictionary errors)
            {
                Log (String.Format ("{0} publish failed", sender.Name));
            }

            void Log (string text)
            {
                InvokeOnMainThread (delegate { _controller.serverLogView.AppendTextLine (text); });
            }
        }
    }

    public static class UITextViewExtension
    {
        public static void AppendTextLine (this UITextView textView, string text)
        {
            textView.Text += String.Format ("\r\n{0}", text);
            textView.ScrollToBottom ();
        }

        public static void ScrollToBottom (this UITextView textView)
        {
            textView.ScrollRangeToVisible (new NSRange (textView.Text.Length - 1, 1));
        }
    }
}
