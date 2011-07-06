using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;

namespace MTNotes
{
    public class AppDelegateBase : UIApplicationDelegate
    {
        public AppDelegateBase ()
        {
        }
        
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            CopyDBToDocuments ();
            ShowLastAccessed ();
            return true;
        }
        
        void CopyDBToDocuments ()
        {
            //Files in app bundle are read only, so you must copy
            //the database file to a writeable location, such as the 
            //documents directory, in order to write to the db.
            
            string dbPath = NoteDBUtil.GetDBPath ();
            
            if (!File.Exists (dbPath)) {
                File.Copy ("MTNotesDB.sqlite", dbPath);
            }
        }

        public override void WillEnterForeground (UIApplication application)
        {
            ShowLastAccessed ();
        }
        
        void ShowLastAccessed ()
        {
            NSObject lastAccessed = NSUserDefaults.StandardUserDefaults["LastAccessed"];
            
            if(lastAccessed != null)
            {
                NSDateFormatter df = new NSDateFormatter();
                df.DateStyle = NSDateFormatterStyle.Full;
                
                var alert = new UIAlertView("Last Accessed", df.StringFor(lastAccessed), null, "OK");
                alert.Show ();
            }
        }

        public override void DidEnterBackground (UIApplication application)
        {
            NSUserDefaults.StandardUserDefaults["LastAccessed"] = NSDate.Now;
        }

        public override void WillTerminate (UIApplication application)
        {
            if (!UIDevice.CurrentDevice.IsMultitaskingSupported) {
                NSUserDefaults.StandardUserDefaults["LastAccessed"] = NSDate.Now;
            }
        }
    }
}

