using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbDocjc
{
    public class sqlserver_db : _db
    {
        private SqlConnection conn { get; set; }

        public sqlserver_db()
        {
            conn = null;
        }
        
        public override void Connect(string srvr, string user, string password)
        {
            hasError = false;
            ErrorMsg = string.Empty;
            
            string strConn = $"Data Source={srvr};Integrated Security=True";
            try
            {
                conn = new SqlConnection(strConn);
                conn.Open();
            }
            catch(SqlException ex)
            {
                hasError = true;
                ErrorMsg = $"Failed to connect to {srvr}\r\n{ex.Message}";
            }
        }

        public override void FillDatabaseCombo(ComboBox cmb)
        {
            using (SqlCommand cmd = conn.CreateCommand()) {
                cmd.CommandText = "SELECT name FROM sys.databases";
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            cmb.Items.Add(rdr.GetString(0));
                        }
                    }
                }
            }
        }

        public override IEnumerator<Dictionary<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override void GetTables(CheckedListBox cListbox)
        {
            hasError = false;
            ErrorMsg = string.Empty;

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Fields are: TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE
                    cmd.CommandText = $"SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE FROM {database}.INFORMATION_SCHEMA.TABLES";
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        cListbox.Items.Clear();
                        while (rdr.Read())
                        {
                            cListbox.Items.Add(new tableInfo(rdr.GetString(2),
                                rdr.GetString(3).Equals("VIEW", StringComparison.OrdinalIgnoreCase)));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                hasError = true;
                ErrorMsg = $"Error getting tables\r\n{ex.Message}";
            }

        }

        public override bool query(string[] keys, string sql)
        {
            throw new NotImplementedException();
        }

        public override void SetDatabase(string dbname)
        {
            hasError = false;
            ErrorMsg = string.Empty;

            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    cmd.CommandText = "USE " + dbname;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    cmd.ExecuteNonQuery();
                    database = dbname;
                }
            }
            catch (SqlException ex)
            {
                hasError = true;
                ErrorMsg = $"Error setting database\r\n{ex.Message}";
            }

        }

        public override string sqlColumns(string table)
        {
            throw new NotImplementedException();
        }

        public override string sqlCreateTable(string table)
        {
            throw new NotImplementedException();
        }

        public override string sqlForeignKeys(string table)
        {
            throw new NotImplementedException();
        }

        public override string sqlIndexes(string table)
        {
            throw new NotImplementedException();
        }

        public override string sqlProcs()
        {
            throw new NotImplementedException();
        }

        public override string sqlTriggers(string table)
        {
            throw new NotImplementedException();
        }
        public override void close()
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn = null;
            }
        }

    }
}
