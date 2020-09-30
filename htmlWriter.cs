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
        private _db db { get; set; }

        public htmlWriter(string filename, _db pdb)
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

            string[] keys_t = { "Field", "Type", "Collation", "Null", "Key", "Default", "Extra", "Privileges", "Comment" };
            if (db.query(keys_t, db.sqlColumns(table.name)))
            {
                foreach (Dictionary<string, object> row in db)
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
            else
            {
                if (db.hasError)
                    MessageBox.Show($"Error getting table data:\r\n{db.ErrorMsg}", DbDoc.MsgTitle);
            }
            WriteLine("\t</table>");

            // INDEXES
            if (Information["IDX"].Checked)
            {
                string[] keys_i = { "Table", "Non_unique", "Key_name", "Seq_in_index", "Column_name", "Collation", "Cardinality", "Sub_part", "Packed", "Null", "Index_type", "Comment", "Index_comment" };
                if (db.query(keys_i, db.sqlIndexes(table.name)))
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

                    foreach (Dictionary<string, object> row in db)
                    {
                        Write("\t\t<tr>");
                        Write($"<td>{row["Column_name"]}</td>");
                        Write($"<td>{row["Key_name"]}</td>");
                        string unique = Convert.ToInt32(row["Non_unique"]) == 0 ? "No" : "Yes";
                        Write($"<td>{unique}</td>");
                        Write($"<td>{row["Index_type"]}</td>");
                        Write($"<td>{row["Null"]}</td>");
                        WriteLine("</tr>");
                    }
                    Write("\t</table>\r\n</div>\r\n");
                }
                else
                {
                    if (db.hasError)
                        MessageBox.Show($"Error getting index data:\r\n{db.ErrorMsg}", DbDoc.MsgTitle);
                }
            }

            // FOREIGN KEYS
            if (Information["FKS"].Checked)
            {
                string[] keys_f = { "column_name", "foreign_db", "foreign_table", "foreign_column" };
                if (db.query(keys_f, db.sqlForeignKeys(table.name)))
                {

                    Write("<div class=\"keep_together\">\r\n\t<h2>Foreign Keys</h2>\r\n");
                    WriteLine("\t<table class=\"db_table_info\">");

                    Write("\t\t<tr>");
                    Write("<th>Column Name</th>");
                    Write("<th>Foreign DB</th>");
                    Write("<th>Foreign Table</th>");
                    Write("<th>Foreign Column</th>");
                    WriteLine("</tr>");

                    foreach (Dictionary<string, object> row in db)
                    {
                        Write("\t\t<tr>");
                        Write($"<td>{row["column_name"]}</td>");
                        Write($"<td>{row["foreign_db"]}</td>");
                        Write($"<td>{row["foreign_table"]}</td>");
                        Write($"<td>{row["foreign_column"]}</td>");
                        WriteLine("</tr>");
                    }
                    Write("\t</table>\r\n</div>\r\n");
                }
                else
                {
                    if (db.hasError)
                        MessageBox.Show($"Error getting foreign keys:\r\n{db.ErrorMsg}", DbDoc.MsgTitle);
                }

            }



            // TRIGGERS
            if (Information["TRI"].Checked)
            {
                string[] keys_tr = { "trigger_name", "action_order", "action_timing", "trigger_event", "definition" };

                if (db.query(keys_tr, db.sqlTriggers(table.name)))
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

                    foreach (Dictionary<string, object> row in db)
                    {
                        Write("\t\t<tr>");
                        Write($"<td>{row["trigger_name"]}</td>");
                        Write($"<td>{row["action_order"]}</td>");
                        Write($"<td>{row["action_timing"]}</td>");
                        Write($"<td>{row["trigger_event"]}</td>");
                        Write($"<td>{row["definition"]}</td>");
                        WriteLine("</tr>");
                    }
                    Write("\t</table>\r\n</div>\r\n");
                }
                else
                {
                    if (db.hasError)
                        MessageBox.Show($"Error getting triggers:\r\n{db.ErrorMsg}", DbDoc.MsgTitle);
                }
            }


            // CREATE SQL
            if (Information["SQL"].Checked)
            {
                string[] keys_csql = { "Table", "Create Table" };
                if (db.query(keys_csql, db.sqlCreateTable(table.name)))
                {
                    Write("<div class=\"keep_together\">\r\n\t<h2>Create SQL</h2>\r\n");
                    WriteLine("\t<div class=\"create\">");
                    foreach (Dictionary<string, object> row in db)
                    {
                        Write(row["Create Table"].ToString().Replace(",", ",<br />"));
                    }
                    WriteLine("\r\n\t</div>");
                    WriteLine("</div>");
                }
                else
                {
                    if (db.hasError)
                        MessageBox.Show($"Error getting create sql:\r\n{db.ErrorMsg}", DbDoc.MsgTitle);
                }

            }
        }

        public void DoProcs()
        {

            // PROCEDURES & FUNCTIONS
            string[] keys_procs = { "ROUTINE_NAME", "ROUTINE_TYPE", "ROUTINE_COMMENT" };
            if (db.query(keys_procs, db.sqlProcs()))
            {
                Write("<div class=\"keep_together\">\r\n\t<h1>Stored Procedures and Functions</h1>\r\n");
                WriteLine("\t<table class=\"db_table_info\">");

                Write("\t\t<tr>");
                Write("<th>Procedure Name</th>");
                Write("<th>Type</th>");
                Write("<th>Comment</th>");
                WriteLine("</tr>");

                foreach (Dictionary<string, object> row in db)
                {
                    Write("\t\t<tr>");
                    Write($"<td>{row["ROUTINE_NAME"]}</td>");
                    Write($"<td>{row["ROUTINE_TYPE"]}</td>");
                    Write($"<td>{row["ROUTINE_COMMENT"]}</td>");
                    WriteLine("</tr>");
                }
                Write("\t</table>\r\n</div>\r\n");
                WriteLine("\r\n\t</div>");
                WriteLine("</div>");
            }
            else
            {
                if (db.hasError)
                    MessageBox.Show($"Error reading procedure list for {db.database}\r\n{db.ErrorMsg}", DbDoc.MsgTitle);
            }
        }
    }
}

