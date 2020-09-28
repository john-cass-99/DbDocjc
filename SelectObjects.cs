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
        private mysql_db db { get; set; }

        private readonly Dictionary<string, Info> Information = new Dictionary<string, Info>()
        {
            {"IDX", new Info {key="IDX",name="Indexes",Checked=true } },
            {"FKS", new Info {key="FKS",name="Foreign Keys",Checked=true } },
            {"SQL", new Info {key="SQL",name="Create SQL",Checked=false } },
            {"SPF", new Info {key="SPF",name="Stored Procedures and Functions",Checked=false } },
            {"TRI", new Info {key="TRI",name="Triggers",Checked=false } },
            /*{"EVT", new Info {key="EVT",name="Events",Checked=false } },*/
        };

        private readonly Dictionary<string, string> htmlData = new Dictionary<string, string>();

        public SelectObjects(mysql_db pdb)
        {
            InitializeComponent();
            txtOutputPath.Text = Properties.Settings.Default.OutputPath;
            lblTitle.Text += $" '{pdb.database}'";
            pdb.GetTables(lstTables);
            SetUpInfoList();
            lstInfo.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(lstInfo_ItemCheck);
            htmlData.Add("database", pdb.database);
            htmlData.Add("server", pdb.server);
            htmlData.Add("doc_date", DateTime.Today.ToString("dd MMMM yyyy"));
            htmlData.Add("description", string.Empty);
            db = pdb;
        }

        private void SetUpInfoList()
        {
            foreach (KeyValuePair<string, Info> kv in Information)
            {
                Info info = kv.Value;
                int i = lstInfo.Items.Add(info);
                if (info.Checked)
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
            string opFilename = Path.Combine(txtOutputPath.Text, db.database + "_documentation.html");
            string cssFilename = Path.Combine(txtOutputPath.Text, "DbDoc.css");
            // While writing project always replace the css file
            if (File.Exists(cssFilename))
            {
                File.Delete(cssFilename);
            }
            File.Copy("./DbDoc.css", cssFilename);

            using (htmlWriter hw = new htmlWriter(opFilename, db))
            {
                hw.DoPage1(htmlData);
                foreach (tableInfo table in lstTables.CheckedItems)
                {
                    hw.DoTable(table, Information);
                }
                if (Information["SPF"].Checked)
                    hw.DoProcs();
                hw.Close();
            }
            Process.Start(opFilename);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool CheckDescription()
        {
            using (DbDescription dbDescription = new DbDescription(db.database))
            {
                if (dbDescription.ShowDialog() == DialogResult.OK)
                {
                    htmlData["description"] = dbDescription.description;
                    return true;
                }
            }
            return false;
        }

        private void lstInfo_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Info inf = (Info)lstInfo.Items[e.Index];
            inf.Checked = e.NewValue == CheckState.Checked;
            Information[inf.key] = inf;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using ( FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = txtOutputPath.Text;
                fbd.Description = "Select folder for html output";
                if (fbd.ShowDialog()== DialogResult.OK)
                {
                    txtOutputPath.Text = fbd.SelectedPath;
                    Properties.Settings.Default.OutputPath = txtOutputPath.Text;
                    Properties.Settings.Default.Save();
                }

            }
        }
    }

    public class Info
    {
        public string key { get; set; }
        public string name { get; set; }
        public bool Checked { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class tableInfo
    {
        public string name { get; private set; }
        public bool IsView { get; private set; }

        public tableInfo(string pName, bool pIsView)
        {
            name = pName;
            IsView = pIsView;
        }

        public override string ToString()
        {
            return name;
        }
    }
}