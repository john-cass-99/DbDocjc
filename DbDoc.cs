using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DbDocjc
{
    public partial class DbDoc : Form
    {
        public static MySqlConnection conn { get; private set; }
        private const string MsgTitle = "DbDocjc";
        public DbDoc()
        {
            InitializeComponent();
            txtServer.Text = Properties.Settings.Default.Server;
            txtUser.Text = Properties.Settings.Default.User;
            txtPassword.Text = Properties.Settings.Default.Password;

            stxtServer.Text = txtServer.Text;
            stxtConnStatus.Text = "Not Connected";
            slblDatabase.Visible = false;
            stxtDatabase.Visible = false;
            stxtVersion.Text = Application.ProductVersion;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DbDoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Server = txtServer.Text;
            Properties.Settings.Default.User = txtUser.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.Save();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            conn = OpenConnection();
            if (conn != null)
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SHOW DATABASES";
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            lstDatabases.Items.Clear();
                            while (rdr.Read())
                            {
                                lstDatabases.Items.Add(rdr.GetString(0));
                            }
                        }
                    }
                    stxtConnStatus.Text = "OK";
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error getting database list\r\n" + ex.Message, MsgTitle);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }

        }

        private MySqlConnection OpenConnection()
        {
            MySqlConnection tempDB_Conn = null;

            stxtServer.Text = Properties.Settings.Default.Server;
            // stxtDatabase.Text = Properties.Settings.Default.Database;

            using (MySqlConnection localDB_Conn = new MySqlConnection
            {
                ConnectionString = "Server=" + Properties.Settings.Default.Server
                + ";user=" + Properties.Settings.Default.User
                + ";password=" + Properties.Settings.Default.Password
                /*                + ";database=" + Properties.Settings.Default.Database */
                + ";SslMode=none;Allow Batch=true;Allow User Variables=true"
            })
            {
                try
                {
                    localDB_Conn.Open();
                    localDB_Conn.Close();
                    tempDB_Conn = localDB_Conn;
                    stxtConnStatus.Text = "OK";
                }
                catch (MySqlException Ex)
                {
                    const string ChkSettings = "\rCheck settings";
                    tempDB_Conn = null;
                    stxtConnStatus.Text = "Failed";
                    if (Ex.Message.Contains("Unable to connect"))
                    {
                        MessageBox.Show("Cannot connect to \"" + Properties.Settings.Default.Server + "\"" + ChkSettings, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else if (Ex.Message.StartsWith("Authentication", StringComparison.Ordinal))
                    {
                        MessageBox.Show(Ex.Message + ChkSettings, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    else
                    {
                        MessageBox.Show(Ex.Message, MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }

                }
            }
            return tempDB_Conn;
        }

        private void lstDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDatabases.SelectedIndex > 0)
            {
                stxtDatabase.Text = lstDatabases.SelectedItem.ToString();
                try
                {
                    conn.Open();
                    using ( MySqlCommand cmd = conn.CreateCommand())
                    {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                        cmd.CommandText = "USE " + stxtDatabase.Text;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error setting database\r\n" + ex.Message, MsgTitle);
                }

                slblDatabase.Visible = true;
                stxtDatabase.Visible = true;
            }
        }
    }
}
