
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace LMT31
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
//        SampleViewController _svc;

        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {   
            // to create in code rather than xib
//            _svc = new SampleViewController ();
//            _svc.View.Frame = new RectangleF (0, 20, 
//                UIScreen.MainScreen.Bounds.Width, 
//                UIScreen.MainScreen.Bounds.Height - 20);
//            window.AddSubview (_svc.View);
            
            window.AddSubview (sampleVC.View);
            window.MakeKeyAndVisible ();
            
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}

