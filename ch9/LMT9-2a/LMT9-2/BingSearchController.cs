using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace LMT92
{
    [Register("BingSearchController")]
    public partial class BingSearchController : UITableViewController
    {
        static NSString _cellId = new NSString ("SearchResultCell");

        List<SearchResultItem> _results;
        UISearchBar _searchBar;

        public BingSearchController (IntPtr p) : base(p)
        {
            _results = new List<SearchResultItem> ();
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            Title = "Bing Search Example";
            TableView.Source = new BingSource (this);
            
            _searchBar = CreateSearchBar ();
            _searchBar.SearchButtonClicked += OnSearchBarSearchButtonClicked;
            
            TableView.TableHeaderView = _searchBar;
        }

        void Search ()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            BingServiceGateway bg = new BingServiceGateway (SyncToMain);
            
            //bg.Search1 (_searchBar.Text);
            //bg.Search2 (_searchBar.Text);
            //bg.Search3 (_searchBar.Text);
            //bg.Search4(_searchBar.Text);
            //bg.Search5(_searchBar.Text);    
            //bg.Search6(_searchBar.Text); //BUG: Currently doesn't work...pending MonoTouch bug fixes in WCF stack
            bg.Search7(_searchBar.Text);
        }

        // iOS specific function to sync to main thread
        void SyncToMain (List<SearchResultItem> results)
        {
            this.InvokeOnMainThread (delegate {
                using (var pool = new NSAutoreleasePool ()) {
                    _results = results;
                    
                    TableView.ReloadData ();
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
                }   
            });
        }

        void OnSearchBarSearchButtonClicked (object sender, EventArgs e)
        {
            Search ();
        }

        static UISearchBar CreateSearchBar ()
        {
            UISearchBar search = new UISearchBar ();
            search.Placeholder = "Search Bing";
            search.SizeToFit ();
            search.AutocorrectionType = UITextAutocorrectionType.No;
            search.AutocapitalizationType = UITextAutocapitalizationType.None;
            return search;
        }

        class BingSource : UITableViewSource
        {
            BingSearchController _bingController;

            public BingSource (BingSearchController bingController)
            {
                _bingController = bingController;
            }

            public override int RowsInSection (UITableView tableView, int section)
            {
                return _bingController._results.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell (_cellId);
                
                if (cell == null)
                    cell = new UITableViewCell (UITableViewCellStyle.Default, _cellId);
                
                cell.TextLabel.Text = _bingController._results[indexPath.Row].Title;
                
                return cell;
            }
        }
        
    }
}


