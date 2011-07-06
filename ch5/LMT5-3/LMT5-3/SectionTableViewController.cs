using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace LMT53
{
    [Register("SectionTableViewController")]
    public class SectionTableViewController : UITableViewController
    {
        public List<string> SectionOneList { get; set; }
        public List<string> SectionTwoList { get; set; }

        public SectionTableViewController (IntPtr p) : base (p) {}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            this.TableView.Source = new SectionSource (this);
        }

        class SectionSource : UITableViewSource
        {
            const string SECTION_ONE_CELL = "sectionOneCell";
            const string SECTION_TWO_CELL = "sectionTwoCell";

            SectionTableViewController _controller;

            public SectionSource (
                SectionTableViewController controller)
            {
                _controller = controller;
            }

            public override int NumberOfSections (
                UITableView tableView)
            {
                return 2;
            }

            public override int RowsInSection (
                UITableView tableview, int section)
            {
                if (section == 0) {
                    return _controller.SectionOneList.Count;
                } else {
                    return _controller.SectionTwoList.Count;
                }
            }

            public override UITableViewCell GetCell (
                UITableView tableView, 
                MonoTouch.Foundation.NSIndexPath indexPath)
            {
                UITableViewCell cell;
                
                if (indexPath.Section == 0) {
                    cell = tableView.DequeueReusableCell (
                        SECTION_ONE_CELL);
                    
                    if (cell == null)
                        cell = new UITableViewCell (
                            UITableViewCellStyle.Value1, 
                            SECTION_ONE_CELL);
                    
                    cell.TextLabel.Text = 
                        _controller.SectionOneList[indexPath.Row];
                    cell.DetailTextLabel.Text = 
                        "this is a section 1 cell";
                    
                } else {
                    cell = tableView.DequeueReusableCell (
                        SECTION_TWO_CELL);
                    
                    if (cell == null)
                        cell = new UITableViewCell (
                            UITableViewCellStyle.Value2, 
                            SECTION_TWO_CELL);
                    
                    cell.TextLabel.Text = 
                        _controller.SectionTwoList[indexPath.Row];
                    cell.DetailTextLabel.Text = 
                        "this is a section 2 cell";
                }
                
                return cell;
            }
        }
    }
}

