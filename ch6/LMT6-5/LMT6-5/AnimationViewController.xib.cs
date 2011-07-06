
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreAnimation;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace LMT65
{
    public partial class AnimationViewController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public AnimationViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public AnimationViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public AnimationViewController () : base("AnimationViewController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        CALayer _sublayer;
      
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            _sublayer = new CALayer ();
            _sublayer.Bounds = new RectangleF (0, 0, 100, 100);
            _sublayer.Position = new PointF (100, 100);
            
            // to provide content directly from CGImage
            //_sublayer.Contents = UIImage.FromFile ("monkey.png").CGImage;
            //_sublayer.ContentsGravity = CALayer.GravityResizeAspectFill;
            
            // to provide content through CALayerDelegate
            _sublayer.Delegate = new LayerDelegate();
            _sublayer.SetNeedsDisplay();
            
            View.Layer.AddSublayer (_sublayer);
            
            animateButton.TouchUpInside += HandleAnimateButtonTouchUpInside;
        }

        void HandleAnimateButtonTouchUpInside (object sender, EventArgs e)
        {
            //CreateImplicitAnimation();
            //CreateExplicitAnimation ();
            //CreateKeyframeAnimation ();
            CreateAnimationGroup ();
        }

        void CreateImplicitAnimation ()
        {
            CATransaction.Begin ();
            CATransaction.AnimationDuration = 3;
            CATransaction.AnimationTimingFunction = CAMediaTimingFunction.FromName ("easeInEaseOut");
            _sublayer.Position = new PointF (200, 300);
            _sublayer.Opacity = 0.5f;
            CATransaction.Commit ();
        }

        void CreateExplicitAnimation ()
        {
            PointF fromPt = _sublayer.Position;
            _sublayer.Position = new PointF (200, 300);
            CABasicAnimation anim = CABasicAnimation.FromKeyPath ("position");
            anim.TimingFunction = CAMediaTimingFunction.FromName ("easeOut");
            anim.From = NSValue.FromPointF (fromPt);
            anim.To = NSValue.FromPointF (new PointF (200, 300));
            anim.Duration = 1;
            _sublayer.AddAnimation (anim, "position");
        }

        void CreateKeyframeAnimation ()
        {
            // animate the position
            PointF fromPt = _sublayer.Position;
            _sublayer.Position = new PointF (200, 300);
            CGPath path = new CGPath ();
            path.AddLines (new PointF[] { fromPt, new PointF (250, 225), new PointF (100, 250), new PointF (200, 300) });
            CAKeyFrameAnimation animPosition = (CAKeyFrameAnimation)CAKeyFrameAnimation.FromKeyPath ("position");
            animPosition.Path = path;
            animPosition.Duration = 2;
            _sublayer.AddAnimation (animPosition, "position");
            
            // animate the layer transform
            _sublayer.Transform = CATransform3D.MakeRotation ((float)Math.PI, 0, 0, 1);
            CAKeyFrameAnimation animRotate = (CAKeyFrameAnimation)CAKeyFrameAnimation.FromKeyPath ("transform");
            
            animRotate.Values = new NSObject[] { NSNumber.FromCATransform3D (CATransform3D.MakeRotation (0, 0, 0, 1)), 
                NSNumber.FromCATransform3D (CATransform3D.MakeRotation ((float)Math.PI / 2f, 0, 0, 1)), 
                NSNumber.FromCATransform3D (CATransform3D.MakeRotation ((float)Math.PI, 0, 0, 1)) };
            
            /* see bug comment below
            animRotate.Values = new NSObject[] { 
                NSNumber.FromFloat (0f), 
                NSNumber.FromFloat ((float)Math.PI / 2f), 
                NSNumber.FromFloat ((float)Math.PI) };
            */
            //BUG: MonoTouch does not have this class method bound as of MonoTouch Versino 3.1.3
            //animRotate.ValueFunction = CAValueFunction.FunctionWithName(CAValueFunction.RotateZ);

            animRotate.Duration = 2;
            _sublayer.AddAnimation (animRotate, "transform");
        }

        void CreateAnimationGroup ()
        {
            PointF fromPt = _sublayer.Position;
            _sublayer.Position = new PointF (200, 300);
            CGPath path = new CGPath ();
            path.AddLines (new PointF[] { fromPt, new PointF (250, 225), new PointF (100, 250), new PointF (200, 300) });
            CAKeyFrameAnimation animPosition = (CAKeyFrameAnimation)CAKeyFrameAnimation.FromKeyPath ("position");
            animPosition.Path = path;
            
            _sublayer.Transform = CATransform3D.MakeRotation ((float)Math.PI, 0, 0, 1);
            CAKeyFrameAnimation animRotate = (CAKeyFrameAnimation)CAKeyFrameAnimation.FromKeyPath ("transform");
            animRotate.Values = new NSObject[] { NSNumber.FromCATransform3D (CATransform3D.MakeRotation (0, 0, 0, 1)), 
                NSNumber.FromCATransform3D (CATransform3D.MakeRotation ((float)Math.PI / 2f, 0, 0, 1)), 
                NSNumber.FromCATransform3D (CATransform3D.MakeRotation ((float)Math.PI, 0, 0, 1)) };
            
            CAAnimationGroup spinningMonkeyGroup = CAAnimationGroup.CreateAnimation ();
            spinningMonkeyGroup.Duration = 2;
            spinningMonkeyGroup.Animations = new CAAnimation[] { animPosition, animRotate };
            _sublayer.AddAnimation (spinningMonkeyGroup, null);
        }
        
        class LayerDelegate : CALayerDelegate
        {
            public override void DrawLayer (CALayer layer, CGContext context)
            {   
                context.SetLineWidth (4);
                var path = new CGPath ();
                path.AddLines(new PointF[]{new PointF(0,0), new PointF(100,100), new PointF(100,0)});
                path.CloseSubpath ();
                context.AddPath (path);
                context.DrawPath (CGPathDrawingMode.Stroke);            
            }
        }
    }
}
