using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace LMT62
{
    [Register("CustomImageView")]
    public class CustomImageView : UIView
    {
        CGImage _monkeyImage;

        public CustomImageView (IntPtr p) : base(p)
        {
            _monkeyImage = UIImage.FromFile ("monkey.jpg").CGImage;
        }

        public override void Draw (RectangleF rect)
        {
            base.Draw (rect);
            
            CGContext gctx = UIGraphics.GetCurrentContext ();
            
            gctx.ScaleCTM (1, -1);
            gctx.TranslateCTM (0, -Bounds.Height);
                
            gctx.DrawImage (rect, _monkeyImage);      
        }
        
        
    }
}

