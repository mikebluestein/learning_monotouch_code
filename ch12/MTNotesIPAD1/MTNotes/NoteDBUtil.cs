using System;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.IO;


namespace MTNotes
{
    public class NoteDBUtil
    {

        public NoteDBUtil ()
        {
        }

        public static SqliteConnection CreateConnnection ()
        {
            string dbPath = GetDBPath ();
            var connection = new SqliteConnection ("Data Source=" + dbPath);
            return connection;
        }

        public static string GetDBPath ()
        {
            return Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "MTNotesDB.sqlite");
        }
        
    }
}

