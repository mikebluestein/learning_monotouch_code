
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT63
{
    public partial class PDFViewController : UIViewController
    {
        string _text;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public PDFViewController (IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public PDFViewController (NSCoder coder) : base(coder)
        {
        }

        public PDFViewController (string text) : base("PDFViewController", null)
        {
            _text = text;
        }

        #endregion
        
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            // previous page
            pdfToolbar.Items[2].Clicked += delegate {
                
                PDFView pdfV = View as PDFView;
                pdfV.PageNumber--;
            };
            
            // next page
            pdfToolbar.Items[3].Clicked += delegate {
                PDFView pdfView = View as PDFView;
                pdfView.PageNumber++;
            };
            
            // close
            pdfToolbar.Items[0].Clicked += delegate {
                this.DismissModalViewControllerAnimated (true);
            };
        }

        public override void LoadView ()
        {
            base.LoadView ();
            
            PDFView pv = (View as PDFView);
            
            if (pv != null)
                pv.AnnotatedText = _text;
        }
    }
}

