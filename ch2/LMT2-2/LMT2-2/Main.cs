
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace LMT22
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
        UIActionSheet _changePictureSheet;
        
        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {   
            window.MakeKeyAndVisible ();
            
            return true;
        }

        class ChangePictureActionSheetDelegate : UIActionSheetDelegate
        {
            AppDelegate _appDel;

            public ChangePictureActionSheetDelegate (AppDelegate appDel)
            {
                _appDel = appDel;
            }

            public override void Clicked (UIActionSheet actionSheet, int buttonIndex)
            {
                switch (buttonIndex) {
                case 1:
                    _appDel.imageView.Image = UIImage.FromFile ("image1.jpg");
                    break;
                case 2:
                    _appDel.imageView.Image = UIImage.FromFile ("image2.jpg");
                    break;
                }
            }
            
        }
       
        partial void changePicture (MonoTouch.UIKit.UIButton sender)
        {
            Console.WriteLine("changePicture called in MonoTouch");
            
            _changePictureSheet = new UIActionSheet (
                "Change Picture", 
                new ChangePictureActionSheetDelegate (this), "Cancel", 
                null, "Image 1", "Image 2");
            
            _changePictureSheet.ShowInView (imageView);     
        }

        // This method is required in iPhoneOS 3.0
        public override void OnActivated (UIApplication application)
        {
        }
    }
}
