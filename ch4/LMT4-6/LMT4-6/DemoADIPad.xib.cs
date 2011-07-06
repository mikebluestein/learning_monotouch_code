
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.iAd;

namespace LMT46
{
    public partial class DemoADIPad : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for controllers that need 
        // to be able to be created from a xib rather than from managed code

        public DemoADIPad (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public DemoADIPad (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public DemoADIPad () : base("DemoADIPad", null)
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

            adBanner.AdLoaded += (s, e) => { Console.WriteLine ("Ad Loaded"); 
               ((ADBannerView)s).Hidden = false; 
            };
            
            adBanner.FailedToReceiveAd += delegate(object sender, AdErrorEventArgs e) {
                Console.WriteLine("Ad failed to load. error code = {0}", e.Error.Code);
                ((ADBannerView)sender).Hidden = true;
            };
        }

        public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillRotate (toInterfaceOrientation, duration);
            if ((toInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft) || 
                (toInterfaceOrientation == UIInterfaceOrientation.LandscapeRight)) 
            {
                adBanner.CurrentContentSizeIdentifier = ADBannerView.SizeIdentifierLandscape;
            } 
            else 
            {
                adBanner.CurrentContentSizeIdentifier = ADBannerView.SizeIdentifierPortrait;
            }
        }

        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }
        
    }
}

