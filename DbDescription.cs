using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
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
        private XElement elDesc { get; set; }
        private bool chg { get; set; }
        private XDocument xdoc { get; set; }
        public DbDescription(string pDatabase)
        {
            InitializeComponent();
            database = pDatabase;
            lblTitle.Text += $" \"{database}\"";
            Size s = TextRenderer.MeasureText(lblTitle.Text, lblTitle.Font);
            lblTitle.Left = (this.Width - s.Width) / 2;
            xmlFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), xmlFileName);
            elDesc = getDescription();
            if (elDesc.FirstNode == null)
                txtDescription.Text = string.Empty;
            else
                txtDescription.Text = elDesc.FirstNode.ToString();
            txtDescription.Select(0, 0);
            chg = false;
            btnSave.Enabled = false;
        }

        private XElement getDescription()
        {
            if (!File.Exists(xmlFilePath))
            {
                int rc = CreateXMLFile(xmlFilePath);
                if (rc<0)
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DbDocjc"));
                    CreateXMLFile(xmlFilePath);
                }
            }

            XElement elDesc = null;
            xdoc = XDocument.Load(xmlFilePath);
            XElement root = xdoc.Element("Descriptions");
            foreach(XElement el in root.Descendants("Description"))
            {
                if (el.FirstAttribute.Value == database)
                {
                    elDesc = el;
                    break;
                }
            }

            if (elDesc == null)
            {
                elDesc = new XElement("Description", string.Empty);
                XAttribute attribute = new XAttribute("name", database);
                elDesc.Add(attribute);
                root.Add(elDesc);
            }

            return elDesc;
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
            if (chg)
            {
                if (MessageBox.Show("Quit and lose changes?", DbDoc.MsgTitle, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDescription.Text.Length == 0)
                return;

            elDesc.ReplaceWith(new XElement ("Description", new XAttribute("name", database), txtDescription.Text));
            xdoc.Save(xmlFilePath);
            chg = false;
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            chg = true;
            btnSave.Enabled = txtDescription.Text.Length > 0;
        }
    }
}
