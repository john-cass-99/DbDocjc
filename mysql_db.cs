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
    public class mysql_db : _db
    {
        private MySqlConnection conn { get; set; }
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

        public override void Connect(string srvr, string user, string password)
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

        public override void FillDatabaseCombo(ComboBox cmb)
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

        public override void SetDatabase(string dbname)
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

        public override void GetTables(CheckedListBox cListbox)
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
        public override bool query(string[] keys, string sql) 
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
                if (_rdr != null && !_rdr.IsClosed)
                    _rdr.Close();
                _rdr = _cmd.ExecuteReader();
                return _rdr.HasRows;
            }
            catch(MySqlException ex)
            {
                hasError = true;
                ErrorMsg = ex.Message;
                if (!_rdr.IsClosed)
                    _rdr.Close();
            }
            return false;
        }

        public override IEnumerator<Dictionary<string,object>> GetEnumerator()
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
            _rdr.Close();
            yield break;
        }

        public override string sqlColumns(string table)
        {
            return $"show full fields from {table}";
        }
        public override string sqlIndexes(string table)
        {
            return $"show indexes from {table}";
        }
        public override string sqlForeignKeys(string table)
        {
            return $"SELECT `column_name`, `referenced_table_schema` AS foreign_db," +
                                    "`referenced_table_name` AS foreign_table, `referenced_column_name`  AS foreign_column" +
                                    " FROM `information_schema`.`KEY_COLUMN_USAGE` WHERE `constraint_schema` = SCHEMA()" +
                                    $" AND `table_name` = '{table}' AND `referenced_column_name` IS NOT NULL ORDER BY `column_name`";
        }
        public override string sqlTriggers(string table)
        {
            return $"select trigger_name, action_order, action_timing," +
                        " event_manipulation as trigger_event, action_statement as 'definition' from information_schema.TRIGGERS" +
                        $" where event_object_schema = '{database}' and event_object_table = '{table}'";
        }
        public override string sqlCreateTable(string table)
        {
            return $"SHOW CREATE TABLE {table}";
        }
        public override string sqlProcs()
        {
            return $"SELECT ROUTINE_NAME, ROUTINE_TYPE, ROUTINE_COMMENT FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA = '{database}'";
        }

    }
}
