using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace LMT52
{
    public class CustomerCell : UITableViewCell
    {
        UILabel _nameLabel;
        UILabel _noteLabel;
        UIImageView _newCustomerIcon;
        UIFont _noteFont;

        public Customer Customer { get; set; }

        public CustomerCell (Customer customer, string reuseIdentifier) : base(UITableViewCellStyle.Default, reuseIdentifier)
        {
            this.Customer = customer;
            
            _nameLabel = new UILabel ();
            _noteLabel = new UILabel ();
            _newCustomerIcon = new UIImageView ();
            
            // uncomment to highlite how content view resizes when toggling edit mode
//            _nameLabel.BackgroundColor = UIColor.Yellow;
//            _nameLabel.TextColor = UIColor.Black;
//            _noteLabel.BackgroundColor = UIColor.Green;
//            _noteLabel.TextColor = UIColor.Black;
            
            _noteFont = UIFont.ItalicSystemFontOfSize(12.0f);
            
            this.ContentView.AddSubview (_nameLabel);
            this.ContentView.AddSubview (_noteLabel);
            this.ContentView.AddSubview (_newCustomerIcon);
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            
            _nameLabel.Text = String.Format ("{0} {1}", Customer.FName, Customer.LName);      
            _noteLabel.Font = _noteFont;
            _noteLabel.Text = String.IsNullOrEmpty (Customer.Note) ? "enter notes in customer details" : Customer.Note; 
            _newCustomerIcon.Image = Customer.IsFavorite ? UIImage.FromFile ("Favorite.png") : null;
            
            RectangleF b = ContentView.Bounds;
            
            float leftPadding = 10.0f;
            float rightPadding = 10.0f;
            float totalPadding = leftPadding + rightPadding;
            float iconWidth = b.Height / 2;
            float iconHeight = b.Height / 2;
                         
            RectangleF nameRect = new RectangleF (b.Left + leftPadding, b.Top, b.Width/1.5f - totalPadding, b.Height / 2);
            _nameLabel.Frame = nameRect;  
            RectangleF noteRect = new RectangleF (b.Left + leftPadding, b.Top + b.Height / 2, b.Width - totalPadding, b.Height / 2);
            _noteLabel.Frame = noteRect;
            RectangleF imageRect = new RectangleF (b.Right - iconWidth , b.Top, iconWidth, iconHeight);
            _newCustomerIcon.Frame = imageRect;
        }
    }
}
