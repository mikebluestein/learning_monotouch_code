
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT33
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
        UITabBarController _tabController;
        SampleViewController _sampleVC;
        SecondViewController _secondVC;

        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            _tabController = new UITabBarController ();
            
            _sampleVC = new SampleViewController ();
            _sampleVC.TabBarItem.Title = "tab 1";
            
            _secondVC = new SecondViewController ();
            _secondVC.TabBarItem.Title = "tab 2";
            
            _tabController.ViewControllers = new UIViewController[] { _sampleVC, _secondVC };
            
            window.AddSubview (_tabController.View);
            
            window.MakeKeyAndVisible ();
            
            return true;
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}

