
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT53
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
            List<string> list1 = new List<string> { "one", "two" };
            List<string> list2 = new List<string> { "three", "four" };
            
            sectionController.SectionOneList = list1;
            sectionController.SectionTwoList = list2;
 
            window.AddSubview (sectionController.View);     
            window.MakeKeyAndVisible ();
            
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}

