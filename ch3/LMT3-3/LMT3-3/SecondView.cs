using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.Foundation;

namespace LMT33
{
    public class SecondView : UIView
    {
        CGPath _path;
        string _title;
        UILabel _titleLabel;

        public string Title {
            get { return _title; }
            set {
                _title = value;
                _titleLabel.Text = _title;
            }
        }

        public SecondView ()
        {
            _titleLabel = new UILabel ();
            MultipleTouchEnabled = false;
        }

        public override void Draw (RectangleF rect)
        {
            base.Draw (rect);
            
            //get graphics context
            CGContext gctx = UIGraphics.GetCurrentContext ();
            
            //set up drawing attributes
            gctx.SetLineWidth (2);
            UIColor.Gray.SetFill ();
            UIColor.Black.SetStroke ();
            
            //create geometry
            _path = new CGPath ();
            
            _path.AddLines (new PointF[] { new PointF (110, 100), new PointF (210, 100), new PointF (210, 200), new PointF (110, 200) });
            
            _path.CloseSubpath ();
            
            //add geometry to graphics context and draw it
            gctx.AddPath (_path);
            gctx.DrawPath (CGPathDrawingMode.FillStroke);
            
            _titleLabel.Frame = new RectangleF (5, 5, Bounds.Width - 10, 25);
            this.AddSubview (_titleLabel);
        }

        public override void TouchesBegan (NSSet touches, UIEvent evt)
        {
            base.TouchesBegan (touches, evt);
            
            UITouch touch = touches.AnyObject as UITouch;
            
            if (touch != null) {
                PointF pt = touch.LocationInView (this);
                
                if (_path.ContainsPoint (pt, true)) {
                    Title = "You touched the square";
                } else {
                    Title = "You didn't touch the square";
                }
            }
        }
    }
}