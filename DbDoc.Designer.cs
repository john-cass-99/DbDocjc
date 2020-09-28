namespace DbDocjc
{
    partial class DbDoc
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblServer = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lstDatabases = new System.Windows.Forms.ComboBox();
            this.btnSelectObjects = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.slblServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.stxtServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblDatabase = new System.Windows.Forms.ToolStripStatusLabel();
            this.stxtDatabase = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblConnStat = new System.Windows.Forms.ToolStripStatusLabel();
            this.stxtConnStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.slblVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.stxtVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnTest = new System.Windows.Forms.Button();
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(114, 75);
            this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(58, 21);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "Server:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(155, 27);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(272, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Select Database to Document";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(178, 72);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(228, 29);
            this.txtServer.TabIndex = 2;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(178, 116);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(228, 29);
            this.txtUser.TabIndex = 4;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(127, 119);
            this.lblUser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(45, 21);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "User:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(178, 160);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(228, 29);
            this.txtPassword.TabIndex = 6;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(93, 163);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(79, 21);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(50, 209);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(122, 21);
            this.lblDatabase.TabIndex = 8;
            this.lblDatabase.Text = "Select Database:";
            // 
            // btnConnect
            // 
            this.btnConnect.AutoSize = true;
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnConnect.Location = new System.Drawing.Point(433, 160);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 31);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lstDatabases
            // 
            this.lstDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstDatabases.FormattingEnabled = true;
            this.lstDatabases.Location = new System.Drawing.Point(178, 206);
            this.lstDatabases.Name = "lstDatabases";
            this.lstDatabases.Size = new System.Drawing.Size(228, 29);
            this.lstDatabases.TabIndex = 9;
            this.lstDatabases.SelectedIndexChanged += new System.EventHandler(this.lstDatabases_SelectedIndexChanged);
            // 
            // btnSelectObjects
            // 
            this.btnSelectObjects.AutoSize = true;
            this.btnSelectObjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnSelectObjects.Enabled = false;
            this.btnSelectObjects.Location = new System.Drawing.Point(178, 250);
            this.btnSelectObjects.Name = "btnSelectObjects";
            this.btnSelectObjects.Size = new System.Drawing.Size(117, 31);
            this.btnSelectObjects.TabIndex = 10;
            this.btnSelectObjects.Text = "Select Objects";
            this.btnSelectObjects.UseVisualStyleBackColor = false;
            this.btnSelectObjects.Click += new System.EventHandler(this.btnSelectObjects_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = true;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnClose.Location = new System.Drawing.Point(433, 295);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 31);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slblServer,
            this.stxtServer,
            this.slblDatabase,
            this.stxtDatabase,
            this.slblConnStat,
            this.stxtConnStatus,
            this.slblVersion,
            this.stxtVersion});
            this.Status.Location = new System.Drawing.Point(0, 359);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(629, 22);
            this.Status.TabIndex = 12;
            this.Status.Text = "statusStrip1";
            // 
            // slblServer
            // 
            this.slblServer.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.slblServer.Name = "slblServer";
            this.slblServer.Size = new System.Drawing.Size(52, 17);
            this.slblServer.Text = "Server:";
            // 
            // stxtServer
            // 
            this.stxtServer.Name = "stxtServer";
            this.stxtServer.Size = new System.Drawing.Size(52, 17);
            this.stxtServer.Text = "(server)";
            // 
            // slblDatabase
            // 
            this.slblDatabase.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.slblDatabase.Name = "slblDatabase";
            this.slblDatabase.Size = new System.Drawing.Size(69, 17);
            this.slblDatabase.Text = "Database:";
            // 
            // stxtDatabase
            // 
            this.stxtDatabase.Name = "stxtDatabase";
            this.stxtDatabase.Size = new System.Drawing.Size(70, 17);
            this.stxtDatabase.Text = "(database)";
            // 
            // slblConnStat
            // 
            this.slblConnStat.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.slblConnStat.Name = "slblConnStat";
            this.slblConnStat.Size = new System.Drawing.Size(124, 17);
            this.slblConnStat.Text = "Connection Status:";
            // 
            // stxtConnStatus
            // 
            this.stxtConnStatus.Name = "stxtConnStatus";
            this.stxtConnStatus.Size = new System.Drawing.Size(117, 17);
            this.stxtConnStatus.Text = "(connection status)";
            // 
            // slblVersion
            // 
            this.slblVersion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.slblVersion.Name = "slblVersion";
            this.slblVersion.Size = new System.Drawing.Size(72, 17);
            this.slblVersion.Spring = true;
            this.slblVersion.Text = "Version:";
            this.slblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stxtVersion
            // 
            this.stxtVersion.Name = "stxtVersion";
            this.stxtVersion.Size = new System.Drawing.Size(58, 17);
            this.stxtVersion.Text = "(version)";
            // 
            // btnTest
            // 
            this.btnTest.AutoSize = true;
            this.btnTest.BackColor = System.Drawing.Color.Red;
            this.btnTest.Location = new System.Drawing.Point(530, 0);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(98, 37);
            this.btnTest.TabIndex = 13;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // DbDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 381);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSelectObjects);
            this.Controls.Add(this.lstDatabases);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblServer);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DbDoc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Database Documenter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DbDoc_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DbDoc_FormClosed);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox lstDatabases;
        private System.Windows.Forms.Button btnSelectObjects;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel slblServer;
        private System.Windows.Forms.ToolStripStatusLabel stxtServer;
        private System.Windows.Forms.ToolStripStatusLabel slblDatabase;
        private System.Windows.Forms.ToolStripStatusLabel slblConnStat;
        private System.Windows.Forms.ToolStripStatusLabel stxtConnStatus;
        private System.Windows.Forms.ToolStripStatusLabel slblVersion;
        private System.Windows.Forms.ToolStripStatusLabel stxtVersion;
        private System.Windows.Forms.ToolStripStatusLabel stxtDatabase;
        private System.Windows.Forms.Button btnTest;
    }
}

