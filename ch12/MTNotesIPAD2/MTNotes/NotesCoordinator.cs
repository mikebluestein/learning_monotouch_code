using System;
using System.Collections.Generic;

namespace MTNotes
{
    public sealed class NotesCoordinator
    {
        public event EventHandler NoteSaved;
        public event EventHandler NoteDeleted;

        List<Note> _notes;

        static NotesCoordinator _coordinator = new NotesCoordinator ();

        public static NotesCoordinator Coordinator {
            get { return _coordinator; }
        }

        public List<Note> Notes {
            get { return _notes; }
            set { _notes = value; }
        }

        public void AddNote (Note note)
        {
            if (_notes != null) {
                _notes.Add (note);
                note.Save ();
                RaiseNoteSaved ();
            }
        }
        
        public void UpdateNote (Note note)
        {
            note.Save ();
            RaiseNoteSaved ();
        }

        public void DeleteNote (int index)
        {
            if (_notes != null) {
                _notes[index].Delete ();           
                _notes.RemoveAt (index);
                RaiseNoteDeleted ();
            }
        }

        void RaiseNoteSaved ()
        {
            if (NoteSaved != null)
                NoteSaved (this, new EventArgs ());
        }
        
        void RaiseNoteDeleted ()
        {
            if(NoteDeleted != null)
                NoteDeleted (this, new EventArgs ());
        }
    }
}
