
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.ObjCRuntime;

namespace LMT64
{
    public partial class AnimationDemoViewController : UIViewController
    {
        PointF p0;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public AnimationDemoViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public AnimationDemoViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public AnimationDemoViewController () : base("AnimationDemoViewController", null)
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
            
            // iOS 3.x style animations
            startAnimation.TouchUpInside += delegate {
                
                p0 = monkeyImageView.Center;
                
                UIView.BeginAnimations ("slideMonkeyAnimation");
                
                UIView.SetAnimationDuration(2);
                UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
                UIView.SetAnimationRepeatCount(2);
                UIView.SetAnimationRepeatAutoreverses(true);
                UIView.SetAnimationDelegate(this);
                UIView.SetAnimationDidStopSelector(new Selector("slideMonkeyStopped:"));
                    
                monkeyImageView.Center = new PointF (UIScreen.MainScreen.Bounds.Right - monkeyImageView.Frame.Width / 2, monkeyImageView.Center.Y);
       
                UIView.CommitAnimations ();
                
            };

            // or using new iOS 4+ blocks from MonoTouch
//            
//                startAnimation.TouchUpInside += delegate { 
//                p0 = monkeyImageView.Center;
//                
//                UIView.Animate (2, 0, 
//                    UIViewAnimationOptions.CurveEaseInOut | UIViewAnimationOptions.Autoreverse,
//                    () => { monkeyImageView.Center = 
//                        new PointF (UIScreen.MainScreen.Bounds.Right - monkeyImageView.Frame.Width / 2, 
//                            monkeyImageView.Center.Y);
//                    }, 
//                    () => { monkeyImageView.Center = p0; }
//                ); 
//            };     
                  
        }

        [Export("slideMonkeyStopped:")]
        void SlideMonkeyStopped ()
        {
            monkeyImageView.Center = p0;
        }
    }
}

