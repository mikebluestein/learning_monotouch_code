
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace LMT62
{
    public partial class CustomImageViewController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public CustomImageViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public CustomImageViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public CustomImageViewController () : base("CustomImageViewController", null)
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
            
            // draw a new banana image              
            addBanana.TouchUpInside += delegate {
                
                // create a graphics context with an in memory bitmap backing store
                UIGraphics.BeginImageContext (new SizeF (100.0f, 100.0f));
                
                // get graphics context
                CGContext gctx = UIGraphics.GetCurrentContext ();
                
                // set up drawing attributes
                gctx.SetLineWidth (5);
                UIColor.Brown.SetStroke ();
                UIColor.Yellow.SetFill ();
                
                // create geometry
                var path = new CGPath ();
                path.AddArc (0, 0, 50, 0, (float)Math.PI / 2, false);
                path.CloseSubpath ();
                
                // add geometry to graphics context and draw it
                gctx.AddPath (path);
                gctx.DrawPath (CGPathDrawingMode.FillStroke);
                
                // get a UIImage from the context
                UIImage bananaImage = UIGraphics.GetImageFromCurrentImageContext ();
                
                // clean up
                UIGraphics.EndImageContext ();
                
                // use the UIImage
                iv.Image = bananaImage;
            };
        }
    }
}

