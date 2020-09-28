using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DbDocjc
{
    public class mysql_db : IEnumerable<Dictionary<string, object>>
    {
        private MySqlConnection conn { get; set; }
        public string database { get; private set; }
        public string server { get; private set; }

        public bool hasError { get; private set; }
        public string ErrorMsg { get; private set; }

        private MySqlCommand _cmd { get; set; }
        private MySqlDataReader _rdr { get; set; }
        private string[] rdr_keys { get; set; }

        public mysql_db()
        {
            conn = null;
            database = string.Empty;
        }

        public void close()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn = null;
            }
        }

        public void Connect(string srvr, string user, string password)
        {
            hasError = false;
            ErrorMsg = string.Empty;

            server = srvr;

            if (conn == null)
                conn = new MySqlConnection();
            else if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.ConnectionString = $"Server={srvr};user={user};password={password};SslMode=none;Allow Batch=true;Allow User Variables=true";
            try
            {
                conn.Open();
            }
            catch (MySqlException Ex)
            {
                conn = null;
                hasError = true;
                const string ChkSettings = "\rCheck settings";
                if (Ex.Message.Contains("Unable to connect"))
                {
                    ErrorMsg = $"Cannot connect to \"{srvr}\"{ChkSettings}";
                }
                else if (Ex.Message.StartsWith("Authentication", StringComparison.Ordinal))
                {
                    ErrorMsg = Ex.Message + ChkSettings;
                }
                else
                {
                    ErrorMsg = Ex.Message;
                }
            }
        }

        public void FillDatabaseCombo(ComboBox cmb)
        {
            hasError = false;
            ErrorMsg = string.Empty;

            try
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SHOW DATABASES";
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        cmb.Items.Clear();
                        while (rdr.Read())
                        {
                            cmb.Items.Add(rdr.GetString(0));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                ErrorMsg = $"Error getting database list\r\n{ex.Message}";
                hasError = true;
            }
        }

        public void SetDatabase(string dbname)
        {
            hasError = false;
            ErrorMsg = string.Empty;

            try
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    cmd.CommandText = "USE " + dbname;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    cmd.ExecuteNonQuery();
                    database = dbname;
                }
            }
            catch (MySqlException ex)
            {
                hasError = true;
                ErrorMsg = $"Error setting database\r\n{ex.Message}";
            }

        }

        public void GetTables(CheckedListBox cListbox)
        {
            hasError = false;
            ErrorMsg = string.Empty;

            try
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SHOW FULL TABLES";
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        cListbox.Items.Clear();
                        while (rdr.Read())
                        {
                            cListbox.Items.Add(new tableInfo(rdr.GetString(0),
                                rdr.GetString(1).Equals("VIEW", StringComparison.OrdinalIgnoreCase)));
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                hasError = true;
                ErrorMsg= $"Error getting tables\r\n{ex.Message}";
            }
        }

/// <summary>
/// Runs a non-parameterised query against the database
/// </summary>
/// <param name="keys">Array of keys in the order data will be returned</param>
/// <param name="sql">Query SQL</param>
/// <returns>A dictionary (associative array) of results.</returns>
        public bool query(string[] keys, string sql) 
        {
            hasError = false;
            ErrorMsg = string.Empty;

            rdr_keys = keys;

            try
            {
                _cmd = conn.CreateCommand();
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                _cmd.CommandText = sql;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                _rdr = _cmd.ExecuteReader();
                return _rdr.HasRows;
            }
            catch(MySqlException ex)
            {
                hasError = true;
                ErrorMsg = ex.Message;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // call the generic version of the method
            return this.GetEnumerator();
        }

        IEnumerator<Dictionary<string, object>> IEnumerable<Dictionary<string, object>>.GetEnumerator()
        {
            return (IEnumerator< Dictionary<string, object>>)GetEnumerator();
        }

        public IEnumerator<Dictionary<string,object>> GetEnumerator()
        {
            while (_rdr.Read())
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                int item_count = 0;
                foreach (string data_item in rdr_keys)
                {
                    dict.Add(data_item, _rdr.GetValue(item_count++));
                }
                yield return dict;
            }
            yield break;
        }


    }
}
