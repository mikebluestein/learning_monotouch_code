using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;

namespace MTNotes
{

    public class NotesTableControllerIPad : UITableViewController
    {
        public List<Note> Notes {
            get { return NotesCoordinator.Coordinator.Notes; }
            set { NotesCoordinator.Coordinator.Notes = value; }
        }

        public NotesTableControllerIPad ()
        {
            Notes = Note.ReadNotes ();
            
            NotesCoordinator.Coordinator.NoteSaved += delegate {
                TableView.ReloadData ();            
            };
            
            NSNotificationCenter.DefaultCenter.AddObserver (this, new Selector ("updateSettings:"), new NSString ("NSUserDefaultsDidChangeNotification"), null);
        }

        [Export("updateSettings:")]
        void UpdateSettings ()
        {
            SetTableBackgroundColorFromSettings ();
        }

        void SetTableBackgroundColorFromSettings ()
        {
            int i = NSUserDefaults.StandardUserDefaults.IntForKey ("TableColor");
            
            switch (i) {
            case 1:
                TableView.BackgroundColor = UIColor.White;
                break;
            case 2:
                TableView.BackgroundColor = UIColor.Gray;
                break;
            case 3:
                TableView.BackgroundColor = UIColor.Red;
                break;
            default:
                TableView.BackgroundColor = UIColor.White;
                break;
            }
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            
            SetTableBackgroundColorFromSettings ();
            
            Title = "Notes";
            
            TableView.Source = new NotesTableSource (this);
        }

        public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
        {
            return true;
        }

        class NotesTableSource : UITableViewSource
        {
            NotesTableControllerIPad _controller;

            const string NOTE_CELL = "noteCell";

            public NotesTableSource (NotesTableControllerIPad controller)
            {
                _controller = controller;
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _controller.Notes.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                var noteCell = tableView.DequeueReusableCell (NOTE_CELL);
                
                if (noteCell == null)
                    noteCell = new UITableViewCell (UITableViewCellStyle.Default, NOTE_CELL);
                
                noteCell.TextLabel.Text = _controller.Notes[indexPath.Row].Title;
                
                return noteCell;
            }

            public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                if (editingStyle == UITableViewCellEditingStyle.Delete) {
                                     
                    NotesCoordinator.Coordinator.DeleteNote (indexPath.Row);

                    tableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                }
            }

            public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                var note = _controller.Notes[indexPath.Row];    
                var detail = _controller.SplitViewController.ViewControllers[1] as NoteDetailControllerIPad;
                    
                detail.Note = note;
                
                if (detail.Popover != null)
                    detail.Popover.Dismiss (true);      
            }
        }
    }
    
}
