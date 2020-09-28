using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace DbDocjc
{
    public class htmlWriter : StreamWriter
    {
        private mysql_db db { get; set; }

        public htmlWriter(string filename, mysql_db pdb)
            : base(filename)
        {
            db = pdb;
        }

        public override void Close()
        {
            Write("</body>\r\n</html>");
            Flush();
            base.Close();
        }
        public void DoPage1(Dictionary<string, string> htmlData)
        {
            string[] keys = { "database", "server", "doc_date", "description" };

            string page1;
            using (StreamReader rdr = new StreamReader("./Page-01.html"))
            {
                page1 = rdr.ReadToEnd();
            }

            foreach (string key in keys)
            {
                if (htmlData.TryGetValue(key, out string data))
                {
                    string full_key = $"^{key}^";
                    page1 = page1.Replace(full_key, data);
                }
            }
            Write(page1);
        }


#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
        public void DoTable(tableInfo table, Dictionary<string, Info> Information)
        {
            string tableLabel = table.IsView ? "View" : "Table";
            WriteLine($"\t<h1>{tableLabel}: {table}</h1>");
            WriteLine("\t<table class=\"db_table_info\">");

            Write("\t\t<tr>");
            Write("<th>Field</th>");
            Write("<th>Type</th>");
            Write("<th>Null</th>");
            Write("<th>Key</th>");
            Write("<th>Default</th>");
            Write("<th>Extra</th>");
            Write("<th>Comment</th>");
            WriteLine("</tr>");

            string[] keys = { "Field", "Type", "Collation", "Null", "Key", "Default", "Extra", "Privileges", "Comment" };
            if (db.query(keys, $"show full fields from {table}"))
            {
                foreach(Dictionary<string,object> row in db)
                {
                    Write("\t\t<tr>");
                    Write($"<td>{row["Field"]}</td>");
                    Write($"<td>{row["Type"]}</td>");
                    Write($"<td>{row["Null"]}</td>");
                    Write($"<td>{row["Key"]}</td>");
                    string dbDefault = row["Default"] == null ? string.Empty : row["Default"].ToString();
                    Write($"<td>{dbDefault}</td>");
                    Write($"<td>{row["Extra"]}</td>");
                    Write($"<td>{row["Comment"]}</td>");
                    WriteLine("</tr>");
                }
            }
            WriteLine("\t</table>");
/*
                // INDEXES
                if (Information["IDX"].Checked)
                {
                    cmd.CommandText = $"show indexes from {table}";
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // Fields are:  Table, Non_unique, Key_name, Seq_in_index, Column_name, Collation, Cardinality, Sub_part, Packed, Null, Index_type, Comment, Index_comment

                        if (rdr.HasRows)
                        {
                            Write("<div class=\"keep_together\">\r\n\t<h2>Indexes</h2>\r\n");

                            WriteLine("\t<table class=\"db_table_info\">");

                            Write("\t\t<tr>");
                            Write("<th>Column Name</th>");
                            Write("<th>Index Name</th>");
                            Write("<th>Is Unique</th>");
                            Write("<th>Index Type</th>");
                            Write("<th>Accepts Null</th>");
                            WriteLine("</tr>");

                            while (rdr.Read())
                            {
                                Write("\t\t<tr>");
                                Write($"<td>{rdr.GetString("Column_name")}</td>");
                                Write($"<td>{rdr.GetString("Key_name")}</td>");
                                string unique = rdr.GetInt32("Non_unique") == 0 ? "No" : "Yes";
                                Write($"<td>{unique}</td>");
                                Write($"<td>{rdr.GetString("Index_type")}</td>");
                                Write($"<td>{rdr.GetString("Null")}</td>");
                                WriteLine("</tr>");
                            }
                            Write("\t</table>\r\n</div>\r\n");
                        }
                    }
                }

                // FOREIGN KEYS
                if (Information["FKS"].Checked)
                {
                    cmd.CommandText = "SELECT `column_name`, `referenced_table_schema` AS foreign_db," +
                        "`referenced_table_name` AS foreign_table, `referenced_column_name`  AS foreign_column" +
                        " FROM `information_schema`.`KEY_COLUMN_USAGE` WHERE `constraint_schema` = SCHEMA()" +
                        $" AND `table_name` = '{table}' AND `referenced_column_name` IS NOT NULL ORDER BY `column_name`;";

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // Fields are:  column_name, foreign_db, foreign_table, foreign_column

                        if (rdr.HasRows)
                        {
                            Write("<div class=\"keep_together\">\r\n\t<h2>Foreign Keys</h2>\r\n");
                            WriteLine("\t<table class=\"db_table_info\">");

                            Write("\t\t<tr>");
                            Write("<th>Column Name</th>");
                            Write("<th>Foreign DB</th>");
                            Write("<th>Foreign Table</th>");
                            Write("<th>Foreign Column</th>");
                            WriteLine("</tr>");

                            while (rdr.Read())
                            {
                                Write("\t\t<tr>");
                                Write($"<td>{rdr.GetString("column_name")}</td>");
                                Write($"<td>{rdr.GetString("foreign_db")}</td>");
                                Write($"<td>{rdr.GetString("foreign_table")}</td>");
                                Write($"<td>{rdr.GetString("foreign_column")}</td>");
                                WriteLine("</tr>");
                            }
                            Write("\t</table>\r\n</div>\r\n");
                        }
                    }
                }

                // TRIGGERS
                if (Information["TRI"].Checked)
                {
                    cmd.CommandText = "select trigger_name, action_order, action_timing," +
                        " event_manipulation as trigger_event, action_statement as 'definition' from information_schema.TRIGGERS" +
                        $" where event_object_schema = '{db.database}' and event_object_table = '{table}';";

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // Fields are:  trigger_name, action_order, action_timing, trigger_event, definition

                        if (rdr.HasRows)
                        {
                            Write("<div class=\"keep_together\">\r\n\t<h2>Triggers</h2>\r\n");
                            WriteLine("\t<table class=\"db_table_info\">");

                            Write("\t\t<tr>");
                            Write("<th>Trigger Name</th>");
                            Write("<th>Action Order</th>");
                            Write("<th>Action Timing</th>");
                            Write("<th>Trigger Event</th>");
                            Write("<th>Definition</th>");
                            WriteLine("</tr>");

                            while (rdr.Read())
                            {
                                Write("\t\t<tr>");
                                Write($"<td>{rdr.GetString("trigger_name")}</td>");
                                Write($"<td>{rdr.GetString("action_order")}</td>");
                                Write($"<td>{rdr.GetString("action_timing")}</td>");
                                Write($"<td>{rdr.GetString("trigger_event")}</td>");
                                Write($"<td>{rdr.GetString("definition")}</td>");
                                WriteLine("</tr>");
                            }
                            Write("\t</table>\r\n</div>\r\n");
                        }
                    }
                }


                // CREATE SQL
                if (Information["SQL"].Checked)
                {
                    Write("<div class=\"keep_together\">\r\n\t<h2>Create SQL</h2>\r\n");
                    cmd.CommandText = $"SHOW CREATE TABLE {table}";

                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // Fields are:  Table, Create Table

                        WriteLine("\t<div class=\"create\">");
                        if (rdr.Read())
                        {
                            Write(rdr.GetString(1).Replace(",", ",<br />"));
                        }
                        WriteLine("\r\n\t</div>");
                    }
                    WriteLine("</div>");
                }

            */
        }

        public void DoProcs()
        {

            // PROCEDURES & FUNCTIONS
                try
                {
                    DbDoc.conn.Open();
                    using (MySqlCommand cmd = DbDoc.conn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT ROUTINE_NAME, ROUTINE_TYPE, ROUTINE_COMMENT FROM information_schema.ROUTINES WHERE ROUTINE_SCHEMA = '{db.database}';";

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                Write("<div class=\"keep_together\">\r\n\t<h1>Stored Procedures and Functions</h1>\r\n");
                                WriteLine("\t<table class=\"db_table_info\">");

                                Write("\t\t<tr>");
                                Write("<th>Procedure Name</th>");
                                Write("<th>Type</th>");
                                Write("<th>Comment</th>");
                                WriteLine("</tr>");

                                while (rdr.Read())
                                {
                                    Write("\t\t<tr>");
                                    Write($"<td>{rdr.GetString("ROUTINE_NAME")}</td>");
                                    Write($"<td>{rdr.GetString("ROUTINE_TYPE")}</td>");
                                    Write($"<td>{rdr.GetString("ROUTINE_COMMENT")}</td>");
                                    WriteLine("</tr>");
                                }
                                Write("\t</table>\r\n</div>\r\n");

                                WriteLine("\r\n\t</div>");
                            }
                            WriteLine("</div>");
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Error reading procedure list for {db.database}\r\n{ex.Message}", DbDoc.MsgTitle);
                }
                finally
                {
                    if (DbDoc.conn.State == ConnectionState.Open)
                        DbDoc.conn.Close();
                }
            
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
       }
    }
}
