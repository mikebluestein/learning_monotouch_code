
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace LMT44
{
    public partial class ImageDemoController : UIViewController
    {
        UIImageView _imageView;
        
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public ImageDemoController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public ImageDemoController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public ImageDemoController () : base("ImageDemoController", null)
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
            
            _imageView = new UIImageView ();
            _imageView.Frame = new RectangleF(0,0,View.Frame.Width, View.Frame.Height);
            _imageView.Image = UIImage.FromFile("monkey.png");      
            _imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                       
            View.AddSubview(_imageView);
        }
    }
}

