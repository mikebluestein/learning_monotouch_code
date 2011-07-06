
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT73
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
            Console.WriteLine ("something woke me up!");
            
            if (options != null) {
                
                NSObject launchedFromLocation;
                if (options.TryGetValue (UIApplication.LaunchOptionsLocationKey, out launchedFromLocation)) {
                    
                    if (((NSNumber)launchedFromLocation).BoolValue) {
                        Console.WriteLine ("Launched From Location Event");
                        
                        // wrapper method to ensure the location manager is created
                        // in the case where the app was launched in the background 
                        // due to a location update
                        LocationHelper.Initialize ();
                    }
                }
            }
            
            window.AddSubview (locationTableController.View);
            
            window.MakeKeyAndVisible ();
            
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}

