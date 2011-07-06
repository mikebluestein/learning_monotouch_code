
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MTNotes
{
    public partial class ColorSelectionController : UIViewController
    {
        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public ColorSelectionController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public ColorSelectionController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public ColorSelectionController () : base("ColorSelectionController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        public UIPopoverController Popover { get; set; }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            whiteButton.TouchUpInside += delegate {
                NSUserDefaults.StandardUserDefaults["TableColor"] = NSNumber.FromInt32 (1);
                Popover.Dismiss (true);
            };
            
            grayButton.TouchUpInside += delegate { 
                NSUserDefaults.StandardUserDefaults["TableColor"] = NSNumber.FromInt32 (2); 
                Popover.Dismiss (true);
            };
            
            redButton.TouchUpInside += delegate { 
                NSUserDefaults.StandardUserDefaults["TableColor"] = NSNumber.FromInt32 (3); 
                Popover.Dismiss (true);
            };
            
            ContentSizeForViewInPopover = new System.Drawing.SizeF( 320, 180);
        }
    }
}

