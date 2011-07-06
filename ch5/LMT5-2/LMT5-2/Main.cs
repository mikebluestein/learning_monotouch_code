
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT52
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
        UINavigationController _navController;
        CustomersViewController _customersVC;

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            _customersVC = new CustomersViewController (new List<Customer> { 
                new Customer ("Jane", "Doe") { IsFavorite = true }, 
                new Customer ("Joe", "Smith"), 
                new Customer ("Steve", "Jones") { Note = "Send email", IsFavorite = true }, 
                new Customer ("Alice", "Smith") { Note = "New customer" } 
            });
            
            _navController = new UINavigationController (_customersVC);
            
            window.AddSubview (_navController.View);
            
            window.MakeKeyAndVisible ();
            
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}

