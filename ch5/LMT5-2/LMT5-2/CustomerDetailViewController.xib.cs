
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT52
{
    public partial class CustomerDetailViewController : UIViewController
    {
        Customer _customer;
        UIBarButtonItem _saveButton;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public CustomerDetailViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public CustomerDetailViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public CustomerDetailViewController (Customer c) : base("CustomerDetailViewController", null)
        {
            Initialize ();
            
            _customer = c;
        }

        void Initialize ()
        {
        }

        #endregion

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            _saveButton = new UIBarButtonItem (UIBarButtonSystemItem.Save);
            _saveButton.Clicked += Handle_saveButtonClicked;
            this.NavigationItem.RightBarButtonItem = _saveButton;
            
            firstNameTextField.BecomeFirstResponder ();
            
            firstNameTextField.ShouldReturn += tf =>
            {
                lastNameTextField.BecomeFirstResponder ();
                return true;
            };
            
            lastNameTextField.ShouldReturn += tf =>
            {
                noteTextField.BecomeFirstResponder ();
                return true;
            };
            
            noteTextField.ShouldReturn += tf =>
            {
                firstNameTextField.BecomeFirstResponder ();
                return true;
            };
            
            firstNameTextField.Text = _customer.FName;
            lastNameTextField.Text = _customer.LName;
            noteTextField.Text = _customer.Note;
            
            UILabel redLabel = new UILabel ();
            redLabel.Frame = new System.Drawing.RectangleF (0, 0, 150, 44);
            redLabel.TextAlignment = UITextAlignment.Center;
            redLabel.Font = UIFont.BoldSystemFontOfSize (20);
            redLabel.BackgroundColor = UIColor.Red;
            redLabel.TextColor = UIColor.White;
            redLabel.Text = "I am the title";
            this.NavigationItem.TitleView = redLabel;
            
        }

        void Handle_saveButtonClicked (object sender, EventArgs e)
        {
            _customer.FName = firstNameTextField.Text;
            _customer.LName = lastNameTextField.Text;
            _customer.Note = noteTextField.Text;
            this.NavigationController.PopViewControllerAnimated (true);
        }
    }
}

