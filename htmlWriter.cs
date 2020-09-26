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
        public htmlWriter(string filename)
            : base(filename)
        {

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
        public void DoTable(string table, Dictionary<string, Info> Information)
        {
            WriteLine($"\t<h1>Table: {table}</h1>");
            try
            {
                DbDoc.conn.Open();
                using (MySqlCommand cmd = DbDoc.conn.CreateCommand())
                {
                    cmd.CommandText = $"show full fields from {table}";
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // Fields are: Field, Type, Collation, Null, Key, Default, Extra, Privileges, Comment
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

                        while (rdr.Read())
                        {
                            Write("\t\t<tr>");
                            Write($"<td>{rdr.GetString("Field")}</td>");
                            Write($"<td>{rdr.GetString("Type")}</td>");
                            Write($"<td>{rdr.GetString("Null")}</td>");
                            Write($"<td>{rdr.GetString("Key")}</td>");
                            string dbDefault = rdr.IsDBNull(5) ? string.Empty : rdr.GetString("Default");
                            Write($"<td>{dbDefault}</td>");
                            Write($"<td>{rdr.GetString("Extra")}</td>");
                            Write($"<td>{rdr.GetString("Comment")}</td>");
                            WriteLine("</tr>");
                        }

                        WriteLine("\t</table>");
                    }

                    // INDEXES
                    if (Information["IDX"].Checked)
                    {
                        WriteLine("\t<h2>Indexes</h2>");
                        cmd.CommandText = $"show indexes from {table}";

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            // Fields are:  Table, Non_unique, Key_name, Seq_in_index, Column_name, Collation, Cardinality, Sub_part, Packed, Null, Index_type, Comment, Index_comment

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

                            WriteLine("\t</table>");
                        }
                    }

                    // FOREIGN KEYS
                    if (Information["FKS"].Checked)
                    {
                        WriteLine("\t<h2>Foreign Keys</h2>");
                        cmd.CommandText = "SELECT `column_name`, `referenced_table_schema` AS foreign_db," +
                            "`referenced_table_name` AS foreign_table, `referenced_column_name`  AS foreign_column" +
                            " FROM `information_schema`.`KEY_COLUMN_USAGE` WHERE `constraint_schema` = SCHEMA()" +
                            $" AND `table_name` = '{table}' AND `referenced_column_name` IS NOT NULL ORDER BY `column_name`;";

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            // Fields are:  column_name, foreign_db, foreign_table, foreign_column

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

                            WriteLine("\t</table>");
                        }
                    }

                    // CREATE SQL
                    if (Information["SQL"].Checked)
                    {
                        WriteLine("\t<h2>Create SQL</h2>");
                        cmd.CommandText = $"SHOW CREATE TABLE {table}";

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            // Fields are:  Table, Create Table

                            WriteLine("\t<div class=\"create\">");
                            if (rdr.Read())
                            {
                                Write(rdr.GetString(1).Replace(",", ",<br />"));
                            }
                            WriteLine("\t</div>");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Error reading table definition for {table}\r\n{ex.Message}", DbDoc.MsgTitle);
            }
            finally
            {
                if (DbDoc.conn.State == ConnectionState.Open)
                    DbDoc.conn.Close();
            }

        }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities


    }
}
