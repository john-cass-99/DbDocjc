using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

    public SelectObjects(string database)
        {
            InitializeComponent();
            txtOutputPath.Text = Properties.Settings.Default.OutputPath;
            Database = database;
            lblTitle.Text += $" '{database}'";
            GetTables();
            SetUpInfoList();
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

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAddDescription_Click(object sender, EventArgs e)
        {
            using ( DbDescription dbDescription = new DbDescription(Database))
            {
                dbDescription.ShowDialog();
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
