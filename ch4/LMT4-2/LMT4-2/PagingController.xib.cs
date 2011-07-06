
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace LMT42
{
    public partial class PagingController : UIViewController
    {
        UIScrollView _scroll;
        List<UIView> _pages;
        UIPageControl _pager;
        
        int _numPages = 4;
        float _padding = 10;
        float _pageHeight = 400;
        float _pageWidth = 300;

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public PagingController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public PagingController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public PagingController () : base("PagingController", null)
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
            
            View.BackgroundColor = UIColor.Black;
            
            _pages = new List<UIView> ();
            
            _scroll = new UIScrollView { 
                Frame = View.Frame, 
                PagingEnabled = true, 
                ContentSize = new SizeF (
                    _numPages * _pageWidth + _padding + 2 * _padding * (_numPages - 1), 
                    View.Frame.Height) 
            };
            
            View.AddSubview (_scroll);
            
            for (int i = 0; i < _numPages; i++) {
                UIView v = new UIView ();
                v.Add( new UILabel{
                    Frame = new RectangleF (100, 50, 100, 25), 
                    Text = String.Format("Page {0}", i+1)}
                );
                
                _pages.Add (v);
                v.BackgroundColor = UIColor.Gray;
                
                v.Frame = new RectangleF (
                    i * + _pageWidth + _padding + (2 * _padding * i), 
                    0, _pageWidth, _pageHeight);
                
                _scroll.AddSubview (v);
            }

            _scroll.Scrolled += delegate {
    
               _pager.CurrentPage = (int)Math.Round(_scroll.ContentOffset.X/_pageWidth);

            };
            
            _pager = new UIPageControl();
            _pager.Pages = _numPages;
            _pager.Frame = new RectangleF(0, 420, View.Frame.Width, 50);
 
            View.AddSubview(_pager);
        }
        
        
    }
}

