
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using System.Threading;

namespace LMT43
{
    public partial class DemoController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public DemoController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public DemoController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public DemoController () : base("DemoController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        UIActivityIndicatorView _activityView;
        UIProgressView _progressView;

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            showActivityButton.TouchUpInside += HandleShowActivityButtonTouchUpInside;
        }
  
// Uncomment to demonstrate UIActiviyIndicatorView
//        void HandleShowActivityButtonTouchUpInside (object sender, EventArgs e)
//        {
//            _activityView = new UIActivityIndicatorView ();
//            
//            _activityView.Frame = new RectangleF (0, 0, 50, 50);
//            _activityView.Center = View.Center;
//            
//            _activityView.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
//            View.AddSubview (_activityView);
//            _activityView.StartAnimating ();
//            
//            Thread t = new Thread (DoSomething);
//            t.Start ();
//        }

        void DoSomething ()
        {
            Thread.Sleep (3000);
            
            using (var pool = new NSAutoreleasePool ()) {
                this.InvokeOnMainThread (delegate { _activityView.StopAnimating (); });
            }
        }

        void HandleShowActivityButtonTouchUpInside (object sender, EventArgs e)
        {
            _progressView = new UIProgressView ();         
            _progressView.Frame = new RectangleF (0, 0, View.Frame.Width - 20, 100);
            _progressView.Center = View.Center;
            _progressView.Style = UIProgressViewStyle.Default;
            
            View.AddSubview (_progressView);
            
            Thread t = new Thread (DoSomethingElse);
            t.Start ();
        }

        void DoSomethingElse ()
        {
            int n = 3;
            
            for (int i = 0; i < n; i++) {
                Thread.Sleep (1000);
                
                using (var pool = new NSAutoreleasePool ()) {

                    this.InvokeOnMainThread (delegate { 
                        _progressView.Progress = (float)(i + 1) / n; });
                }
            }
        }
        
    }
}

