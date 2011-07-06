
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace LMT11
{
    public class Application
    {
        static void Main (string[] args)
        {
            UIApplication.Main (args);
        }
    }

    // The name AppDelegate is referenced in the MainWindow.xib file.
    public partial class AppDelegate : UIApplicationDelegate
    {
        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            // If you have defined a view, add it here:
            window.AddSubview (starViewController.View);
            
            //rotate the view 20 degrees
            starViewController.View.Transform = CGAffineTransform.MakeRotation ((float)Math.PI / 9.0f);
            
            window.MakeKeyAndVisible ();
            
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}

