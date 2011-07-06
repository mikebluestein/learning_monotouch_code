
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MTNotes
{
    public partial class NoteDetailController : UIViewController
    {
        public Note Note {get; set;}
        public List<Note> Notes { get; set; }

        #region Constructors

        // The IntPtr and initWithCoder constructors are required for items that need 
        // to be able to be created from a xib rather than from managed code

        public NoteDetailController (IntPtr handle) : base(handle)
        {
            Initialize ();
        }

        [Export("initWithCoder:")]
        public NoteDetailController (NSCoder coder) : base(coder)
        {
            Initialize ();
        }

        public NoteDetailController () : base("NoteDetailController", null)
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        #endregion

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            if (Note == null)
            {
                Note = new Note ();
                Notes.Add (Note);
            }
            else
            {
                titleTextField.Text = Note.Title;
                bodyTextView.Text = Note.Body;
            }
            
            titleTextField.BecomeFirstResponder ();
            
            titleTextField.ShouldReturn += tf =>
            {
                bodyTextView.BecomeFirstResponder ();
                return true;
            };
        }

        public override void ViewWillDisappear (bool animated)
        {
            base.ViewWillDisappear (animated);
            
            Note.Title = titleTextField.Text;
            Note.Body = bodyTextView.Text; 
            
            // save to SQLite database
            Note.Save();   
            
            // alternately, can perist note list using serialization instead of SQLite
            // Notes.Save ();
        }
        
    }
}

