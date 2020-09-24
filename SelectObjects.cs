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
            {"COL", new Info {key="COL", name="Columns",initiallyChecked=true } },
            {"IDX", new Info {key="IDX",name="Indexes",initiallyChecked=true } },
            {"FKY", new Info {key="FKY",name="Foreign Keys",initiallyChecked=true } },
            {"SQL", new Info {key="SQL",name="Create SQL",initiallyChecked=false } },
            {"SPF", new Info {key="SPF",name="Stored Procedures and Functions",initiallyChecked=false } },
            {"TRI", new Info {key="TRI",name="Triggers",initiallyChecked=false } },
            {"EVT", new Info {key="EVT",name="Events",initiallyChecked=false } },
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
                using ( MySqlCommand cmd=DbDoc.conn.CreateCommand())
                {
                    cmd.CommandText = "SHOW FULL TABLES";
                    using(MySqlDataReader rdr = cmd.ExecuteReader())
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
            foreach (KeyValuePair<string,Info> kv in Information )
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
                for(int i=0; i<lstTables.Items.Count; i++)
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
            string opFilename = Path.Combine(txtOutputPath.Text, Database + "_documentation.html");
            string cssFilename = Path.Combine(txtOutputPath.Text, "DbDoc.css");
            // While writing project always replace the css file
            if (File.Exists(cssFilename))
            {
                File.Delete(cssFilename);
            }
            File.Copy("./DbDoc.css", cssFilename);

            DoPage1(opFilename);
            Process.Start(opFilename);
        }

        private void DoPage1(string filename)
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

            using ( StreamWriter swr = new StreamWriter(filename))
            {
                swr.Write(page1);
                swr.Flush();
                swr.Close();
            }

        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddDescription_Click(object sender, EventArgs e)
        {
            using ( DbDescription dbDescription = new DbDescription(Database))
            {
                if (dbDescription.ShowDialog() == DialogResult.OK)
                    htmlData["description"]= dbDescription.description;
            }
        }
    }

    public class Info
    {
        public Info()
        {
            include = false;
        }
        
        public string key { get; set; }
        public string name { get; set; }
        public bool initiallyChecked { get; set; }
        public bool include { get; set; }
    }


}
