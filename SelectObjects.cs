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
            string[] info = { "Columns", "Indexes", "Foreign Keys", "Stored Procedures and Functions", "Triggers", "Events" };
            for (int i= 0;i< info.Length;i++)
            {
                lstInfo.Items.Add(info[i]);
                if (i < 3)
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
    }
}
