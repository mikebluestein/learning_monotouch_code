
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

namespace LMT72
{
    public partial class LocationTableViewController : UIViewController
    {
        LocationTableSource _source;
        List<CLLocation> _locations;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public LocationTableViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public LocationTableViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public LocationTableViewController () : base("LocationTableViewController", null)
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
            
            _locations = LocationHelper.Instance.Locations;
            LocationHelper.Instance.LocationAdded += delegate { locationTable.ReloadData (); };
            
            startLocation.Clicked += delegate { 
                LocationHelper.Instance.StartLocationUpdates (); 
                LocationHelper.Instance.StartHeadingUpdates ();
            };
            
            stopLocation.Clicked += delegate { 
                LocationHelper.Instance.StopLocationUpdates ();
                LocationHelper.Instance.StopHeadingUpdates ();
            };
            
            _source = new LocationTableSource (this);
            locationTable.Source = _source;
        }

        class LocationTableSource : UITableViewSource
        {
            static string cellId = "locationCell";

            LocationTableViewController _controller;

            public LocationTableSource (LocationTableViewController controller)
            {
                _controller = controller;
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _controller._locations.Count ();
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                
                UITableViewCell cell = tableView.DequeueReusableCell (cellId);
                if (cell == null)
                    cell = new UITableViewCell (UITableViewCellStyle.Subtitle, cellId);
                
                cell.TextLabel.Text = String.Format ("(lat/lon) {0}, {1}", 
                                                     _controller._locations[indexPath.Row].Coordinate.Latitude, 
                                                     _controller._locations[indexPath.Row].Coordinate.Longitude);
                
                cell.DetailTextLabel.Text = String.Format ("(timestamp) {0}",
                                                           _controller._locations[indexPath.Row].Timestamp.ToString ());
                
                return cell;
            }
        }
    }
}

