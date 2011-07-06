
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MTNotes
{

    // The name AppDelegateIPad is referenced in the MainWindowIPad.xib file.
    public partial class AppDelegateIPad : AppDelegateBase
    {

        UISplitViewController _splitController;
        NoteDetailControllerIPad _noteDetailController;
        NotesTableControllerIPad _notesController;
        NoteDetailControllerIPad.SplitDelegate _splitDelegate;

        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            base.FinishedLaunching (app, options);
            
            _splitController = new UISplitViewController ();         
            _notesController = new NotesTableControllerIPad ();
            _noteDetailController = new NoteDetailControllerIPad ();
            
            _splitDelegate = new NoteDetailControllerIPad.SplitDelegate (_noteDetailController);
            _splitController.Delegate = _splitDelegate;

            _splitController.ViewControllers = new UIViewController[] { _notesController, _noteDetailController };
            
            window.AddSubview (_splitController.View);
            
            window.MakeKeyAndVisible ();
            
            return true;
        }
    }
}

