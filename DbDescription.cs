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
using System.Xml.Linq;

namespace DbDocjc
{
    public partial class DbDescription : Form
    {
        private const string xmlFileName = "DbDocjc\\Descriptions.xml";
        private string xmlFilePath { get; set; }
        private readonly string database;
        public DbDescription(string pDatabase)
        {
            InitializeComponent();
            database = pDatabase;
            lblTitle.Text += $" \"{database}\"";
            Size s = TextRenderer.MeasureText(lblTitle.Text, lblTitle.Font);
            lblTitle.Left = (this.Width - s.Width) / 2;
            xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), xmlFileName);
            txtDescription.Text = getDescription();
        }

        private string getDescription()
        {
            if (!File.Exists(xmlFilePath))
            {
                int rc = CreateXMLFile(xmlFilePath);
                if (rc<0)
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DbDocjc"));
                    CreateXMLFile(xmlFilePath);
                }
                return string.Empty;
            }

            string desc = null;
            XDocument xdoc = XDocument.Load(xmlFilePath);
            XElement root = xdoc.Element("Descriptions");
            foreach(XElement el in root.Descendants("Description"))
            {
                if (el.FirstAttribute.Value == database)
                {
                    desc = el.FirstNode.ToString();
                    break;
                }
            }

            if (desc != null)
                return desc.ToString();
            else
                return string.Empty;
        }

        private int CreateXMLFile(string filename)
        {
            try
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true
                };
                using (XmlWriter textWriter = XmlWriter.Create(filename, xmlWriterSettings))
                {
                    textWriter.WriteStartDocument();
                    textWriter.WriteComment("This file holds descriptions for databases.");
                    textWriter.WriteElementString("Descriptions", string.Empty);
                    textWriter.WriteEndDocument();
                    textWriter.Close();
                }
                return 0;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch(DirectoryNotFoundException)
            {
                return -1;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDescription.Text.Length == 0)
                return;

        }
    }
}
