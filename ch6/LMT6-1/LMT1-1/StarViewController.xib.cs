
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT11
{
    public partial class StarViewController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public StarViewController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public StarViewController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public StarViewController () : base("StarViewController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }
        
        #endregion
        
        
    }
    
    
}

