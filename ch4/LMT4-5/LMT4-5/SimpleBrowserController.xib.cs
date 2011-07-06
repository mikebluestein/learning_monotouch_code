
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT45
{
    public partial class SimpleBrowserController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public SimpleBrowserController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public SimpleBrowserController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public SimpleBrowserController () : base("SimpleBrowserController", null)
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
            
            webView.ScalesPageToFit = true;
            
            urlTextField.ShouldReturn = textField =>
            {
                textField.ResignFirstResponder ();
                string url = textField.Text;
                if (!url.StartsWith ("http"))
                    url = String.Format ("http://{0}", url);
                NSUrl nsurl = new NSUrl (url);
                NSUrlRequest req = new NSUrlRequest (nsurl);
                webView.LoadRequest (req);
                return true;
            };
            
            backButton.Clicked += delegate { webView.GoBack (); };
            forwardButton.Clicked += delegate { webView.GoForward (); };
            refreshButton.Clicked += delegate { webView.Reload (); };
        }
    }
}