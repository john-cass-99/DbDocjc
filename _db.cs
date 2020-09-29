using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;


namespace DbDocjc
{
    /// <summary>
    /// Derive from this class to support a new database type.
    /// (Currently, MySql is supported in the derived class mysql_db).
    /// </summary>
    public abstract class _db : IEnumerable<Dictionary<string, object>>
    {
        /// <summary>
        /// The class will need a connection variable of an unknown type.
        /// </summary>
        // private MySqlConnection conn { get; set; }

        public string database { get; protected set; }
        public string server { get; protected set; }

        /// <summary>
        /// If an error occurs in any function, set hasError=true and set a value for ErrorMsg
        /// </summary>
        public bool hasError { get; protected set; }
        public string ErrorMsg { get; protected set; }

        protected _db()
        {
            // conn = null;
            database = string.Empty;
        }

        /// <summary>
        /// Implement setting up the connection
        /// </summary>
        /// <param name="srvr">Server Name</param>
        /// <param name="user">Username</param>
        /// <param name="password">Password</param>
        public abstract void Connect(string srvr, string user, string password);

        /// <summary>
        /// Obtain a list of databases from the server and fill the ComboBox
        /// </summary>
        /// <param name="cmb"></param>
        public abstract void FillDatabaseCombo(ComboBox cmb);

        /// <summary>
        /// Set the current database for the server (USE statement with MySql)
        /// </summary>
        /// <param name="dbname"></param>
        public abstract void SetDatabase(string dbname);

        /// <summary>
        /// Fill the CheckedListBox with a set of tableInfo objects
        /// </summary>
        /// <param name="cListbox"></param>
        public abstract void GetTables(CheckedListBox cListbox);

        public abstract bool query(string[] keys, string sql);

        public abstract string sqlColumns(string table);

        public abstract string sqlIndexes(string table);

        public abstract string sqlForeignKeys(string table);

        public abstract string sqlTriggers(string table);

        public abstract string sqlCreateTable(string table);

        public abstract string sqlProcs();

        IEnumerator IEnumerable.GetEnumerator()
        {
            // call the generic version of the method
            return this.GetEnumerator();
        }

        IEnumerator<Dictionary<string, object>> IEnumerable<Dictionary<string, object>>.GetEnumerator()
        {
            return (IEnumerator<Dictionary<string, object>>)GetEnumerator();
        }

        public abstract IEnumerator<Dictionary<string, object>> GetEnumerator();



    }
}
