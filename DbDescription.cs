using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DbDocjc
{
    public partial class DbDescription : Form
    {
        private const string xmlFileName = "DbDocjc\\Descriptions.xml";
        private readonly string database;
        public DbDescription(string pDatabase)
        {
            InitializeComponent();
            database = pDatabase;
            lblTitle.Text += $" \"{database}\"";
            Size s = TextRenderer.MeasureText(lblTitle.Text, lblTitle.Font);
            lblTitle.Left = (this.Width - s.Width) / 2;
            txtDescription.Text = getDescription();
        }

        private string getDescription()
        {
            string Filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), xmlFileName);
            if (!File.Exists(Filename))
            {
                CreateXMLFile(Filename);
                return string.Empty;
            }

            return string.Empty;
        }

        private void CreateXMLFile(string filename)
        {
            using (XmlTextWriter textWriter = new XmlTextWriter(filename, null))
            {
                textWriter.WriteStartDocument();
                textWriter.WriteComment("This file holds descriptions for databases.");
                textWriter.WriteElementString("Descriptions", string.Empty);
                textWriter.WriteEndDocument();
                textWriter.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
