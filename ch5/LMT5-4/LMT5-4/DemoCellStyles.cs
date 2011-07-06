using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT54
{
    [Register("DemoCellStyles")]
    public partial class DemoCellStyles : UITableViewController
    {

        public DemoCellStyles (IntPtr p) : base(p)
        {
        }

        class Source : UITableViewSource
        {
            public Source ()
            {
            }

            public override int RowsInSection (UITableView tableView, int section)
            {
                return 4;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell;
                UIImage img = null;
                UITableViewCellStyle cellStyle = UITableViewCellStyle.Default;
                UITableViewCellAccessory cellAccessory = UITableViewCellAccessory.None;
                int row = indexPath.Row;
                string text = "";
                string detail = "detail";  
                
                switch (row) {
                case 0:
                    cellStyle = UITableViewCellStyle.Default;
                    cellAccessory = UITableViewCellAccessory.Checkmark;
                    text = "Default";
                    img = UIImage.FromFile("Image.png");
                    break;
                case 1:
                    cellStyle = UITableViewCellStyle.Subtitle;
                    cellAccessory = UITableViewCellAccessory.DetailDisclosureButton;
                    text = "Subtitle";
                    img = UIImage.FromFile("Image.png");
                    break;
                case 2:
                    cellStyle = UITableViewCellStyle.Value1;
                    cellAccessory = UITableViewCellAccessory.DisclosureIndicator;
                    text = "Value1";
                    break;
                case 3:
                    cellStyle = UITableViewCellStyle.Value2;
                    cellAccessory = UITableViewCellAccessory.None;
                    text = "Value2";
                    break;
                }
                
                cell = new UITableViewCell (cellStyle, "cell");
                
                cell.TextLabel.Text = text;
                
                if (cell.DetailTextLabel != null)
                    cell.DetailTextLabel.Text = detail;
                
                if(img != null)
                    cell.ImageView.Image = img;
                               
                cell.Accessory = cellAccessory;
                
                return cell;
            }
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            Title = "DemoCellStyles";
            
            TableView.Source = new Source ();
        }
    }
}


