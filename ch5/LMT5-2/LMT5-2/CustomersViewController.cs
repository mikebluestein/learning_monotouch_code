using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace LMT52
{
    public class CustomersViewController : UITableViewController
    {
        List<Customer> Customers { get; set; }

        public CustomersViewController (List<Customer> customers)
        {
            Customers = customers;
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            this.Title = "Customers";
            
            this.NavigationItem.RightBarButtonItem = this.EditButtonItem;
            
            //TODO: does this need to be a class var to avoid gc?
            TableView.Source = new CustomersTableViewSource (this);
        }

        public override void SetEditing (bool editing, bool animated)
        {   
            base.SetEditing (editing, animated);
            
            (TableView.Source as CustomersTableViewSource).IsEditing = editing;
            
            if (editing) {
                TableView.InsertRows (new NSIndexPath[] { NSIndexPath.FromRowSection (Customers.Count, 0) }, UITableViewRowAnimation.None);
            } else { 
                TableView.DeleteRows (new NSIndexPath[] { NSIndexPath.FromRowSection (Customers.Count, 0) }, UITableViewRowAnimation.None);
            }
        }

        public override void ViewWillAppear (bool animated)
        {
            base.ViewWillAppear (animated);
            
            TableView.ReloadData ();
        }

        class CustomersTableViewSource : UITableViewSource
        {
            CustomersViewController _vc;
            CustomerDetailViewController _customerDetail;

            const string CUSTOMER_CELL = "customerCell";
            
            public bool IsEditing { get; set; }

            public CustomersTableViewSource (CustomersViewController vc)
            {
                IsEditing = false;
                _vc = vc;
            }
            
            public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
            {
                float h;
                if(indexPath.Row == _vc.Customers.Count){
                    h = 50.0f;
                }
                else{
                    h = 70.0f;
                }
                return h;
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                int c = _vc.Customers.Count;
                
                if (_vc.TableView.Editing && IsEditing) {
                    c++;
                }
                
                return c;
            }

            public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCellEditingStyle editingStyle;
                
                if (indexPath.Row < _vc.Customers.Count) {
                    editingStyle = UITableViewCellEditingStyle.Delete;
                } else {
                    editingStyle = UITableViewCellEditingStyle.Insert;
                }
                return editingStyle;
            }

            public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
            {
                return (indexPath.Row != _vc.Customers.Count);
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell;
                
                int row = indexPath.Row;
                
                if (row == _vc.Customers.Count) {
                    cell = new UITableViewCell ();
                    cell.TextLabel.Text = "Add Customer";
                } else {              
                    Customer aCustomer = _vc.Customers[row];
                    cell = tableView.DequeueReusableCell (CUSTOMER_CELL);
                    if (cell == null)     
                        cell = new CustomerCell(aCustomer, CUSTOMER_CELL);
                    else
                        (cell as CustomerCell).Customer = aCustomer;
                }
                
                return cell;
            }

            public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                // check if the edit operation was a delete
                if (editingStyle == UITableViewCellEditingStyle.Delete) {
                    
                    // remove the customer from the underlying data
                    _vc.Customers.RemoveAt (indexPath.Row);
                    
                    // remove the associated row from the tableView
                    tableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Middle);
                } else if (editingStyle == UITableViewCellEditingStyle.Insert) {
                    _vc.Customers.Add (new Customer ("First", "Last"));
                    tableView.InsertRows (new NSIndexPath[] { NSIndexPath.FromRowSection (_vc.Customers.Count - 1, 0) }, UITableViewRowAnimation.Middle);
                }
            }

            public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
            {
                
                Customer c = _vc.Customers[sourceIndexPath.Row];
                _vc.Customers.RemoveAt (sourceIndexPath.Row);
                _vc.Customers.Insert (destinationIndexPath.Row, c);
            }

            public override NSIndexPath CustomizeMoveTarget (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath proposedIndexPath)
            {
                NSIndexPath targetIndexPath;
                
                if (proposedIndexPath.Row == _vc.Customers.Count) {
                    targetIndexPath = NSIndexPath.FromRowSection (proposedIndexPath.Row - 1, 0);
                } else {
                    targetIndexPath = proposedIndexPath;
                }
                return targetIndexPath;
            }

            public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                Customer selectedCustomer = _vc.Customers[indexPath.Row];
                _customerDetail = new CustomerDetailViewController (selectedCustomer);
                _vc.NavigationController.PushViewController (_customerDetail, true);
            }

        }
        
    }
}
