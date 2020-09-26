using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DbDocjc
{
    public partial class SelectObjects : Form
    {
        public string Database { get; private set; }

        private readonly Dictionary<string, Info> Information = new Dictionary<string, Info>()
        {
            {"COL", new Info {key="COL", name="Columns",initiallyChecked=true, index=0 } },
            {"IDX", new Info {key="IDX",name="Indexes",initiallyChecked=true, index=1 } },
            {"FKY", new Info {key="FKY",name="Foreign Keys",initiallyChecked=true, index=2 } },
            {"SQL", new Info {key="SQL",name="Create SQL",initiallyChecked=false, index=3 } },
            {"SPF", new Info {key="SPF",name="Stored Procedures and Functions",initiallyChecked=false, index=4 } },
            {"TRI", new Info {key="TRI",name="Triggers",initiallyChecked=false, index=5 } },
            {"EVT", new Info {key="EVT",name="Events",initiallyChecked=false, index=6 } },
        };

        private readonly Dictionary<string, string> htmlData = new Dictionary<string, string>();

        public SelectObjects(string database, string server)
        {
            InitializeComponent();
            txtOutputPath.Text = Properties.Settings.Default.OutputPath;
            Database = database;
            lblTitle.Text += $" '{database}'";
            GetTables();
            SetUpInfoList();
            htmlData.Add("database", database);
            htmlData.Add("server", server);
            htmlData.Add("doc_date", DateTime.Today.ToString("dd MMMM yyyy"));
            htmlData.Add("description", string.Empty);

        }

        private void GetTables()
        {
            try
            {
                DbDoc.conn.Open();
                using (MySqlCommand cmd = DbDoc.conn.CreateCommand())
                {
                    cmd.CommandText = "SHOW FULL TABLES";
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        lstTables.Items.Clear();
                        while (rdr.Read())
                        {
                            string item = rdr.GetString(0);
                            if (rdr.GetString(1).Equals("VIEW", StringComparison.OrdinalIgnoreCase))
                            {
                                item += " (View)";
                            }
                            lstTables.Items.Add(item);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error setting database\r\n" + ex.Message, DbDoc.MsgTitle);
            }
            finally
            {
                if (DbDoc.conn.State == ConnectionState.Open)
                    DbDoc.conn.Close();
            }

        }

        private void SetUpInfoList()
        {
            foreach (KeyValuePair<string, Info> kv in Information)
            {
                Info info = kv.Value;
                int i = lstInfo.Items.Add(info.name);
                if (info.initiallyChecked)
                    lstInfo.SetItemChecked(i, true);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            const string sSelect = "Select All";
            const string sDeselect = "Deselect All";
            if (btnSelectAll.Text.Equals(sSelect, StringComparison.Ordinal))
            {
                for (int i = 0; i < lstTables.Items.Count; i++)
                {
                    lstTables.SetItemChecked(i, true);
                }
                btnSelectAll.Text = sDeselect;
            }
            else
            {
                for (int i = 0; i < lstTables.Items.Count; i++)
                {
                    lstTables.SetItemChecked(i, false);
                }
                btnSelectAll.Text = sSelect;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!CheckDescription())
                return;
            string opFilename = Path.Combine(txtOutputPath.Text, Database + "_documentation.html");
            string cssFilename = Path.Combine(txtOutputPath.Text, "DbDoc.css");
            // While writing project always replace the css file
            if (File.Exists(cssFilename))
            {
                File.Delete(cssFilename);
            }
            File.Copy("./DbDoc.css", cssFilename);

            using (htmlWriter hw = new htmlWriter(opFilename))
            {
                DoPage1(hw);
                int page = 2;
                if (lstInfo.GetItemChecked(Information["COL"].index))
                {
                    foreach (string table in lstTables.CheckedItems)
                    {
                        DoTable(hw, table, page++);
                    }
                }
                hw.Close();
            }
            Process.Start(opFilename);
        }

        private void DoPage1(StreamWriter hw)
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
            hw.Write(page1);
        }

        private void DoTable(StreamWriter hw, string table, int page)
        {
            hw.WriteLine($"\t<h1>Table: {table}</h1>");
            try
            {
                DbDoc.conn.Open();
                using (MySqlCommand cmd = DbDoc.conn.CreateCommand())
                {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    cmd.CommandText = $"show full fields from {table}";
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // Fields are: Field, Type, Collation, Null, Key, Default, Extra, Privileges, Comment
                        hw.WriteLine("\t<table class=\"db_table_info\">");

                        hw.Write("\t\t<tr>");
                        hw.Write("<th>Field</th>");
                        hw.Write("<th>Type</th>");
                        hw.Write("<th>Null</th>");
                        hw.Write("<th>Key</th>");
                        hw.Write("<th>Default</th>");
                        hw.Write("<th>Extra</th>");
                        hw.Write("<th>Comment</th>");
                        hw.WriteLine("</tr>");

                        while (rdr.Read())
                        {
                            hw.Write("\t\t<tr>");
                            hw.Write($"<td>{rdr.GetString("Field")}</td>");
                            hw.Write($"<td>{rdr.GetString("Type")}</td>");
                            hw.Write($"<td>{rdr.GetString("Null")}</td>");
                            hw.Write($"<td>{rdr.GetString("Key")}</td>");
                            string dbDefault = rdr.IsDBNull(5) ? string.Empty : rdr.GetString("Default");
                            hw.Write($"<td>{dbDefault}</td>");
                            hw.Write($"<td>{rdr.GetString("Extra")}</td>");
                            hw.Write($"<td>{rdr.GetString("Comment")}</td>");
                            hw.WriteLine("</tr>");
                        }

                        hw.WriteLine("\t</table>");
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
            // ToDo: page footer
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool CheckDescription()
        {
            using (DbDescription dbDescription = new DbDescription(Database))
            {
                if (dbDescription.ShowDialog() == DialogResult.OK)
                {
                    htmlData["description"] = dbDescription.description;
                    return true;
                }
            }
            return false;
        }
    }

    public class Info
    {
        public string key { get; set; }
        public string name { get; set; }
        public bool initiallyChecked { get; set; }
        public int index { get; set; } // index in lstInfo
    }


}
