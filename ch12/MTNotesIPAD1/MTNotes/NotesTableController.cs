using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MonoTouch.ObjCRuntime;
using System.Runtime.Serialization.Formatters;

namespace MTNotes
{

    public class NotesTableController : UITableViewController
    {
        List<Note> _notes;

        UIBarButtonItem _addNoteButtonItem;

        public NotesTableController ()
        {
            // read from SQLite database
            _notes = Note.ReadNotes ();
            
            NSNotificationCenter.DefaultCenter.AddObserver (
                this, 
                new Selector ("updateSettings:"), 
                new NSString ("NSUserDefaultsDidChangeNotification"),
                null);
            
            // alternately, read notes from serialized file
            // _notes = NoteListSerializationUtil.ReadNotes ();
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
            
            _addNoteButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.Add);
            _addNoteButtonItem.Clicked += delegate {
                
                var noteDetailVC = new NoteDetailController { Notes = _notes };
                NavigationController.PushViewController (noteDetailVC, true);
            };
            
            NavigationItem.RightBarButtonItem = _addNoteButtonItem;
            NavigationItem.LeftBarButtonItem = EditButtonItem;
            
            TableView.Source = new NotesTableSource (this);
        }

        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);
            
            TableView.ReloadData ();
        }

        class NotesTableSource : UITableViewSource
        {
            NotesTableController _controller;

            const string NOTE_CELL = "noteCell";

            public NotesTableSource (NotesTableController controller)
            {
                _controller = controller;
            }

            public override int RowsInSection (UITableView tableview, int section)
            {
                return _controller._notes.Count;
            }

            public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
            {
                var noteCell = tableView.DequeueReusableCell (NOTE_CELL);
                
                if (noteCell == null)
                    noteCell = new UITableViewCell (UITableViewCellStyle.Default, NOTE_CELL);
                
                noteCell.TextLabel.Text = _controller._notes[indexPath.Row].Title;
                
                return noteCell;
            }

            public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
            {
                if (editingStyle == UITableViewCellEditingStyle.Delete) {
                    
                    // remove the note row from the SQLite backing store
                    _controller._notes[indexPath.Row].Delete ();
                    
                    // alternately, can perist note list using serialization instead of SQLite
                    // _controller._notes.Save ();
                    
                    // remove the note from the list
                    _controller._notes.RemoveAt (indexPath.Row);
                    
                    // remove the associated row from the tableView
                    tableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
                }
            }

            public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
            {
                var note = _controller._notes[indexPath.Row];
                var noteDetailVC = new NoteDetailController { Notes = _controller._notes, Note = note };
                _controller.NavigationController.PushViewController (noteDetailVC, true);
            }
        }
        
    }

    //helper used to persist List<Note> when using serialization
    public static class NoteListSerializationUtil
    {
        public static void Save (this List<Note> notes)
        {
            using (Stream s = File.Open (GetPath (), FileMode.Create)) {
                BinaryFormatter bf = new BinaryFormatter ();
                bf.Serialize (s, notes);
            }
        }

        static string GetPath ()
        {
            string documentsDir = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
            string notesPath = Path.Combine (documentsDir, "notes.bin");
            
            return notesPath;
        }

        public static List<Note> ReadNotes ()
        {
            List<Note> notes = new List<Note> ();
            
            string path = GetPath ();
            
            if (File.Exists (path)) {
                using (Stream stream = File.Open (path, FileMode.Open)) {
                    BinaryFormatter bf = new BinaryFormatter ();
                    notes = (List<Note>)bf.Deserialize (stream);
                }
            }
            return notes;
        }
    }
}
