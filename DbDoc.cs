using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JEncrypt;

namespace DbDocjc
{
    public partial class DbDoc : Form
    {
        public _db db = new mysql_db();
        public const string MsgTitle = "DbDocjc";
        public DbDoc()
        {
            InitializeComponent();
            Left = (int)(0.1 * Screen.PrimaryScreen.Bounds.Right);
            Top = (int)(0.1 * Screen.PrimaryScreen.Bounds.Bottom);
            txtServer.Text = Properties.Settings.Default.Server;
            txtUser.Text = Properties.Settings.Default.User;
            txtPassword.Text = Encryption.Decrypt(Properties.Settings.Default.Password);

            stxtServer.Text = txtServer.Text;
            stxtConnStatus.Text = "Not Connected";
            slblDatabase.Visible = false;
            stxtDatabase.Visible = false;
            stxtVersion.Text = Application.ProductVersion;
#if DEBUG
            btnTest.Visible = true;
#endif

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DbDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Server = txtServer.Text;
            Properties.Settings.Default.User = txtUser.Text;
            Properties.Settings.Default.Password = Encryption.Encrypt(txtPassword.Text);
            Properties.Settings.Default.Save();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            db.Connect(txtServer.Text, txtUser.Text, txtPassword.Text);
            if (db.hasError)
            {
                MessageBox.Show(db.ErrorMsg, MsgTitle);
                stxtConnStatus.Text = "Failed";
                return;
            }
            stxtServer.Text = txtServer.Text;
            stxtConnStatus.Text = "OK";

            db.FillDatabaseCombo(lstDatabases);
            if (db.hasError)
            {
                MessageBox.Show(db.ErrorMsg, MsgTitle);
            }
        }


        private void lstDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDatabases.SelectedIndex > 0)
            {
                string database = lstDatabases.SelectedItem.ToString();
                db.SetDatabase(database);
                if (db.hasError)
                {
                    MessageBox.Show(db.ErrorMsg, MsgTitle);
                }
                else
                {
                    btnSelectObjects.Enabled = true;
                    slblDatabase.Visible = true;
                    stxtDatabase.Text = database;
                    stxtDatabase.Visible = true;
                }
            }
        }

        private void btnSelectObjects_Click(object sender, EventArgs e)
        {
            using (SelectObjects selectObjects = new SelectObjects(db))
            {
                selectObjects.ShowDialog(this);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnConnect_Click(btnTest, new EventArgs());
            lstDatabases.SelectedIndex = lstDatabases.FindString("tennis");
            btnSelectObjects_Click(btnTest, new EventArgs());
        }

        private void DbDoc_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.close();
        }
    }
}

// Template database query function
//private void GetTables()
//{
//    try
//    {
//        DbDoc.conn.Open();
//    }
//    catch (MySqlException ex)
//    {
//        MessageBox.Show("Error setting database\r\n" + ex.Message, DbDoc.MsgTitle);
//    }
//    finally
//    {
//        if (DbDoc.conn.State == ConnectionState.Open)
//            DbDoc.conn.Close();
//    }

//}
