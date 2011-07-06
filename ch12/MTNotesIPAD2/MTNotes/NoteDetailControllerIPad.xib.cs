
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MTNotes
{
    public enum NoteSaveMode
    {
        Insert,
        Update
    }

    public partial class NoteDetailControllerIPad : UIViewController
    {
        UIPopoverController _colorSelectionPopover;
        ColorSelectionController _colorSelectionController;
        
        public NoteSaveMode SaveMode { get; set; }

        Note _note;

        public Note Note {
            get { return _note; }

            set {
                _note = value;
                if (_note != null) {
                    InitTextControls (_note.Title, _note.Body);
                } else {
                    InitTextControls ("", "");
                }
            }
        }

        void InitTextControls (string title, string body)
        {
            if (titleTextField != null && bodyTextView != null) {
                titleTextField.Text = title;
                bodyTextView.Text = body;
            }
        }

        public UIPopoverController Popover { get; set; }

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for controllers that need 
        // to be able to be created from a xib rather than from managed code

        public NoteDetailControllerIPad (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public NoteDetailControllerIPad (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public NoteDetailControllerIPad () : base("NoteDetailControllerIPad", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
            SaveMode = NoteSaveMode.Update;
        }

        #endregion

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            // only show save button in toolbar when adding a new note
            if (SaveMode == NoteSaveMode.Insert) {
                var items = new UIBarButtonItem[] { toolbar.Items[0] };
                toolbar.SetItems (items, false);
            }
            
            saveButton.Clicked += delegate {
                
                if (Note != null) {
                    
                    Note.Title = titleTextField.Text;
                    Note.Body = bodyTextView.Text;
                    
                    if (SaveMode == NoteSaveMode.Insert) {
                        NotesCoordinator.Coordinator.AddNote (Note);
                        this.DismissModalViewControllerAnimated (true);
                    } else {
                        NotesCoordinator.Coordinator.UpdateNote (Note);
                    }
                    
                } else {
                    var alert = new UIAlertView ("", "Please select a note", null, "OK");
                    alert.Show ();
                }
            };
            
            addButton.Clicked += delegate {
                
                var addNoteController = new NoteDetailControllerIPad { Note = new Note (), SaveMode = NoteSaveMode.Insert, 
                    ModalPresentationStyle = UIModalPresentationStyle.FormSheet };
                
                this.PresentModalViewController (addNoteController, true);
            };
            
            setColorButton.Clicked += delegate {
                if(_colorSelectionController == null)
                    _colorSelectionController = new ColorSelectionController ();
                
                if(_colorSelectionPopover == null)
                {
                    _colorSelectionPopover = new UIPopoverController (_colorSelectionController);
                    _colorSelectionController.Popover = _colorSelectionPopover;
                }
                
                _colorSelectionPopover.PresentFromBarButtonItem (setColorButton, UIPopoverArrowDirection.Any, true);
            };
            
            NotesCoordinator.Coordinator.NoteSaved += delegate { Note = null; };
            
            NotesCoordinator.Coordinator.NoteDeleted += delegate { Note = null; };
            
        }

        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        public class SplitDelegate : UISplitViewControllerDelegate
        {
            NoteDetailControllerIPad _controller;

            public SplitDelegate (NoteDetailControllerIPad controller)
            {
                _controller = controller;
            }

            public override void WillHideViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem barButtonItem, UIPopoverController pc)
            {
                var items = _controller.toolbar.Items.ToList ();
                items.Insert (0, barButtonItem);
                
                _controller.toolbar.SetItems (items.ToArray (), true);
                _controller.Popover = pc;
            }

            public override void WillShowViewController (UISplitViewController svc, UIViewController aViewController, UIBarButtonItem button)
            {
                var items = _controller.toolbar.Items.ToList ();
                items.RemoveAt (0);
                
                _controller.toolbar.SetItems (items.ToArray (), false);
                _controller.Popover = null;
            }
        }
        
    }
}
