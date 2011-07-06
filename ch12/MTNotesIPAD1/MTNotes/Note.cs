using System;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

namespace MTNotes
{
    [Serializable]
    public class Note
    {
        long _id;

        //for a new note
        public Note ()
        {
            _id = -1;
        }

        //for an existing note
        Note (long id, string title, string body)
        {
            _id = id;
            Title = title;
            Body = body;
        }

        public string Title { get; set; }
        public string Body { get; set; }

        public void Save ()
        {
            var connection = NoteDBUtil.CreateConnnection ();
            
            using (var cmd = connection.CreateCommand ()) {
                connection.Open ();
                
                if (_id < 0)
                    InsertNote (cmd);
                else
                    UpdateNote (cmd);
                
                connection.Close ();
            }
        }

        public void Delete ()
        {
            var connection = NoteDBUtil.CreateConnnection ();
            
            using (var cmd = connection.CreateCommand ()) {
                connection.Open ();
                string sql = "Delete From Note Where id=@id";
                SqliteParameter idParam = new SqliteParameter ("@id", _id);
                cmd.Parameters.Add (idParam);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery ();
                connection.Close ();
            }
        }

        void InsertNote (SqliteCommand cmd)
        {
            string sql = "Insert Into Note (title, body) Values (@title, @body)";
            
            SqliteParameter titleParam = new SqliteParameter ("@title", Title);
            SqliteParameter bodyParam = new SqliteParameter ("@body", Body);
            
            cmd.Parameters.Add (titleParam);
            cmd.Parameters.Add (bodyParam);
            cmd.CommandText = sql;
            
            cmd.ExecuteNonQuery ();
            
            sql = "select last_insert_rowid()";
            cmd.CommandText = sql;
            using (var reader = cmd.ExecuteReader ()) {
                reader.Read ();
                _id = (long)reader[0];
            }
        }

        void UpdateNote (SqliteCommand cmd)
        {
            string sql = "Update Note Set title=@title, body=@body Where id = @id";
            
            SqliteParameter titleParam = new SqliteParameter ("@title", Title);
            SqliteParameter bodyParam = new SqliteParameter ("@body", Body);
            SqliteParameter idParam = new SqliteParameter ("@id", _id);
            
            cmd.Parameters.Add (titleParam);
            cmd.Parameters.Add (bodyParam);
            cmd.Parameters.Add (idParam);
            cmd.CommandText = sql;
            
            cmd.ExecuteNonQuery ();
        }

        public static List<Note> ReadNotes ()
        {
            var notes = new List<Note> ();
            
            var connection = NoteDBUtil.CreateConnnection ();
            
            using (var cmd = connection.CreateCommand ()) {
                connection.Open ();
                
                string sql = "Select * From Note";
                cmd.CommandText = sql;
                
                using (var reader = cmd.ExecuteReader ()) {
                    while (reader.Read ()) {
                        long id = (long)reader["id"];
                        string title = (string)reader["title"];
                        string body = (string)reader["body"];
                        notes.Add (new Note (id, title, body));
                    }
                }
                
                connection.Close ();
            }
            return notes;
        }
        
    }
}
