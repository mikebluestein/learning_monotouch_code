
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MTNotes
{

    // The name AppDelegateIPhone is referenced in the MainWindowIPhone.xib file.
    public partial class AppDelegateIPhone : AppDelegateBase
    {
        UINavigationController _navController;
        NotesTableController _notesController;

        // This method is invoked when the application has loaded its UI and its ready to run
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            base.FinishedLaunching(app, options);
            
            _notesController = new NotesTableController ();
            _navController = new UINavigationController (_notesController);
            
            window.AddSubview (_navController.View);
            
            window.MakeKeyAndVisible ();
            
            return true;
        }


        
    }
}

