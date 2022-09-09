namespace oLink
{
    partial class FormLink
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.tabEntries = new System.Windows.Forms.TabControl();
            this.tpIAM = new System.Windows.Forms.TabPage();
            this.lbsrdssf = new System.Windows.Forms.Label();
            this.lbls3sf = new System.Windows.Forms.Label();
            this.tbxRDSsf = new System.Windows.Forms.TextBox();
            this.tbxS3sf = new System.Windows.Forms.TextBox();
            this.tbxcurUser = new System.Windows.Forms.TextBox();
            this.lbltbxcurUser = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbxDbConnString = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbxDbPwdP = new System.Windows.Forms.TextBox();
            this.tbxDbUserP = new System.Windows.Forms.TextBox();
            this.tbxProfName = new System.Windows.Forms.TextBox();
            this.tbxS3buckets = new System.Windows.Forms.TextBox();
            this.tbxSecKey = new System.Windows.Forms.TextBox();
            this.tbxAccKey = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbxDBep = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cbxProfiles = new System.Windows.Forms.ComboBox();
            this.lblProfNm = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSaveCred = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxS3Id = new System.Windows.Forms.TextBox();
            this.textBoxS3Key = new System.Windows.Forms.TextBox();
            this.textBoxS3Bucket = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button5 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.ckbcloudfront = new System.Windows.Forms.CheckBox();
            this.ckbShortURL = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelTop = new System.Windows.Forms.Label();
            this.panelMsg = new System.Windows.Forms.Panel();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.labelMsg = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            this.tabEntries.SuspendLayout();
            this.tpIAM.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelMsg.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(110)))), ((int)(((byte)(27)))));
            this.panelMain.Controls.Add(this.panelMiddle);
            this.panelMain.Controls.Add(this.panelTop);
            this.panelMain.Controls.Add(this.panelMsg);
            this.panelMain.Controls.Add(this.panelBottom);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(3);
            this.panelMain.Size = new System.Drawing.Size(952, 453);
            this.panelMain.TabIndex = 1;
            // 
            // panelMiddle
            // 
            this.panelMiddle.BackColor = System.Drawing.Color.White;
            this.panelMiddle.Controls.Add(this.tabEntries);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(3, 29);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(946, 305);
            this.panelMiddle.TabIndex = 13;
            // 
            // tabEntries
            // 
            this.tabEntries.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabEntries.Controls.Add(this.tpIAM);
            this.tabEntries.Controls.Add(this.tabPage1);
            this.tabEntries.Controls.Add(this.tabPage2);
            this.tabEntries.Controls.Add(this.tabPage3);
            this.tabEntries.Controls.Add(this.tabPage4);
            this.tabEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEntries.Location = new System.Drawing.Point(0, 0);
            this.tabEntries.Name = "tabEntries";
            this.tabEntries.SelectedIndex = 0;
            this.tabEntries.Size = new System.Drawing.Size(946, 305);
            this.tabEntries.TabIndex = 1;
            this.tabEntries.SelectedIndexChanged += new System.EventHandler(this.tabEntries_SelectedIndexChanged);
            // 
            // tpIAM
            // 
            this.tpIAM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(225)))));
            this.tpIAM.Controls.Add(this.lbsrdssf);
            this.tpIAM.Controls.Add(this.lbls3sf);
            this.tpIAM.Controls.Add(this.tbxRDSsf);
            this.tpIAM.Controls.Add(this.tbxS3sf);
            this.tpIAM.Controls.Add(this.tbxcurUser);
            this.tpIAM.Controls.Add(this.lbltbxcurUser);
            this.tpIAM.Controls.Add(this.label7);
            this.tpIAM.Controls.Add(this.tbxDbConnString);
            this.tpIAM.Controls.Add(this.label9);
            this.tpIAM.Controls.Add(this.tbxDbPwdP);
            this.tpIAM.Controls.Add(this.tbxDbUserP);
            this.tpIAM.Controls.Add(this.tbxProfName);
            this.tpIAM.Controls.Add(this.tbxS3buckets);
            this.tpIAM.Controls.Add(this.tbxSecKey);
            this.tpIAM.Controls.Add(this.tbxAccKey);
            this.tpIAM.Controls.Add(this.label16);
            this.tpIAM.Controls.Add(this.label11);
            this.tpIAM.Controls.Add(this.tbxDBep);
            this.tpIAM.Controls.Add(this.label15);
            this.tpIAM.Controls.Add(this.cbxProfiles);
            this.tpIAM.Controls.Add(this.lblProfNm);
            this.tpIAM.Controls.Add(this.label12);
            this.tpIAM.Controls.Add(this.btnSaveCred);
            this.tpIAM.Controls.Add(this.label13);
            this.tpIAM.Controls.Add(this.label14);
            this.tpIAM.Location = new System.Drawing.Point(4, 25);
            this.tpIAM.Name = "tpIAM";
            this.tpIAM.Padding = new System.Windows.Forms.Padding(3);
            this.tpIAM.Size = new System.Drawing.Size(938, 276);
            this.tpIAM.TabIndex = 5;
            this.tpIAM.Text = "AWS Credentials";
            // 
            // lbsrdssf
            // 
            this.lbsrdssf.AutoSize = true;
            this.lbsrdssf.Enabled = false;
            this.lbsrdssf.Location = new System.Drawing.Point(655, 252);
            this.lbsrdssf.Name = "lbsrdssf";
            this.lbsrdssf.Size = new System.Drawing.Size(59, 13);
            this.lbsrdssf.TabIndex = 43;
            this.lbsrdssf.Text = "RDS Suffix";
            this.lbsrdssf.Visible = false;
            // 
            // lbls3sf
            // 
            this.lbls3sf.AutoSize = true;
            this.lbls3sf.Enabled = false;
            this.lbls3sf.Location = new System.Drawing.Point(429, 254);
            this.lbls3sf.Name = "lbls3sf";
            this.lbls3sf.Size = new System.Drawing.Size(49, 13);
            this.lbls3sf.TabIndex = 42;
            this.lbls3sf.Text = "S3 Suffix";
            this.lbls3sf.Visible = false;
            // 
            // tbxRDSsf
            // 
            this.tbxRDSsf.Enabled = false;
            this.tbxRDSsf.Location = new System.Drawing.Point(731, 250);
            this.tbxRDSsf.Name = "tbxRDSsf";
            this.tbxRDSsf.PasswordChar = '*';
            this.tbxRDSsf.Size = new System.Drawing.Size(100, 20);
            this.tbxRDSsf.TabIndex = 41;
            this.tbxRDSsf.TabStop = false;
            this.tbxRDSsf.Text = "-RDS";
            this.tbxRDSsf.Visible = false;
            // 
            // tbxS3sf
            // 
            this.tbxS3sf.Enabled = false;
            this.tbxS3sf.Location = new System.Drawing.Point(525, 249);
            this.tbxS3sf.Name = "tbxS3sf";
            this.tbxS3sf.PasswordChar = '*';
            this.tbxS3sf.Size = new System.Drawing.Size(100, 20);
            this.tbxS3sf.TabIndex = 40;
            this.tbxS3sf.Text = "-S3";
            this.tbxS3sf.Visible = false;
            // 
            // tbxcurUser
            // 
            this.tbxcurUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxcurUser.Enabled = false;
            this.tbxcurUser.Location = new System.Drawing.Point(115, 250);
            this.tbxcurUser.Name = "tbxcurUser";
            this.tbxcurUser.PasswordChar = '*';
            this.tbxcurUser.Size = new System.Drawing.Size(282, 20);
            this.tbxcurUser.TabIndex = 38;
            this.tbxcurUser.Visible = false;
            // 
            // lbltbxcurUser
            // 
            this.lbltbxcurUser.AutoSize = true;
            this.lbltbxcurUser.BackColor = System.Drawing.Color.Transparent;
            this.lbltbxcurUser.Location = new System.Drawing.Point(19, 252);
            this.lbltbxcurUser.Name = "lbltbxcurUser";
            this.lbltbxcurUser.Size = new System.Drawing.Size(66, 13);
            this.lbltbxcurUser.TabIndex = 39;
            this.lbltbxcurUser.Text = "User Current";
            this.lbltbxcurUser.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(19, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "DbConnString";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Visible = false;
            // 
            // tbxDbConnString
            // 
            this.tbxDbConnString.Location = new System.Drawing.Point(115, 211);
            this.tbxDbConnString.Name = "tbxDbConnString";
            this.tbxDbConnString.PasswordChar = '-';
            this.tbxDbConnString.Size = new System.Drawing.Size(786, 20);
            this.tbxDbConnString.TabIndex = 36;
            this.tbxDbConnString.UseSystemPasswordChar = true;
            this.tbxDbConnString.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(429, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "DB Password";
            // 
            // tbxDbPwdP
            // 
            this.tbxDbPwdP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxDbPwdP.Location = new System.Drawing.Point(525, 92);
            this.tbxDbPwdP.Name = "tbxDbPwdP";
            this.tbxDbPwdP.PasswordChar = '*';
            this.tbxDbPwdP.Size = new System.Drawing.Size(222, 20);
            this.tbxDbPwdP.TabIndex = 34;
            this.tbxDbPwdP.UseSystemPasswordChar = true;
            // 
            // tbxDbUserP
            // 
            this.tbxDbUserP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxDbUserP.Location = new System.Drawing.Point(525, 52);
            this.tbxDbUserP.Name = "tbxDbUserP";
            this.tbxDbUserP.PasswordChar = '*';
            this.tbxDbUserP.Size = new System.Drawing.Size(222, 20);
            this.tbxDbUserP.TabIndex = 32;
            this.tbxDbUserP.UseSystemPasswordChar = true;
            // 
            // tbxProfName
            // 
            this.tbxProfName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxProfName.Enabled = false;
            this.tbxProfName.Location = new System.Drawing.Point(115, 48);
            this.tbxProfName.Name = "tbxProfName";
            this.tbxProfName.Size = new System.Drawing.Size(186, 20);
            this.tbxProfName.TabIndex = 25;
            // 
            // tbxS3buckets
            // 
            this.tbxS3buckets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxS3buckets.Enabled = false;
            this.tbxS3buckets.Location = new System.Drawing.Point(115, 175);
            this.tbxS3buckets.Name = "tbxS3buckets";
            this.tbxS3buckets.PasswordChar = '*';
            this.tbxS3buckets.Size = new System.Drawing.Size(282, 20);
            this.tbxS3buckets.TabIndex = 21;
            // 
            // tbxSecKey
            // 
            this.tbxSecKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxSecKey.Enabled = false;
            this.tbxSecKey.Location = new System.Drawing.Point(115, 130);
            this.tbxSecKey.Name = "tbxSecKey";
            this.tbxSecKey.PasswordChar = '*';
            this.tbxSecKey.Size = new System.Drawing.Size(282, 20);
            this.tbxSecKey.TabIndex = 18;
            // 
            // tbxAccKey
            // 
            this.tbxAccKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxAccKey.Enabled = false;
            this.tbxAccKey.Location = new System.Drawing.Point(115, 90);
            this.tbxAccKey.Name = "tbxAccKey";
            this.tbxAccKey.PasswordChar = '*';
            this.tbxAccKey.Size = new System.Drawing.Size(282, 20);
            this.tbxAccKey.TabIndex = 16;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(429, 54);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(78, 13);
            this.label16.TabIndex = 33;
            this.label16.Text = "DB User Name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(429, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Db Server";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbxDBep
            // 
            this.tbxDBep.Enabled = false;
            this.tbxDBep.Location = new System.Drawing.Point(525, 11);
            this.tbxDBep.Name = "tbxDBep";
            this.tbxDBep.Size = new System.Drawing.Size(376, 20);
            this.tbxDBep.TabIndex = 30;
            this.tbxDBep.UseSystemPasswordChar = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(19, 14);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(55, 13);
            this.label15.TabIndex = 29;
            this.label15.Text = "Profile List";
            // 
            // cbxProfiles
            // 
            this.cbxProfiles.FormattingEnabled = true;
            this.cbxProfiles.Location = new System.Drawing.Point(115, 11);
            this.cbxProfiles.Name = "cbxProfiles";
            this.cbxProfiles.Size = new System.Drawing.Size(186, 21);
            this.cbxProfiles.TabIndex = 28;
            this.cbxProfiles.SelectedIndexChanged += new System.EventHandler(this.cbxProfiles_SelectedIndexChanged);
            // 
            // lblProfNm
            // 
            this.lblProfNm.AutoSize = true;
            this.lblProfNm.BackColor = System.Drawing.Color.Transparent;
            this.lblProfNm.Location = new System.Drawing.Point(19, 52);
            this.lblProfNm.Name = "lblProfNm";
            this.lblProfNm.Size = new System.Drawing.Size(67, 13);
            this.lblProfNm.TabIndex = 26;
            this.lblProfNm.Text = "Profile Name";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(19, 179);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "File Bucket";
            // 
            // btnSaveCred
            // 
            this.btnSaveCred.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveCred.FlatAppearance.BorderSize = 0;
            this.btnSaveCred.Location = new System.Drawing.Point(525, 136);
            this.btnSaveCred.Name = "btnSaveCred";
            this.btnSaveCred.Size = new System.Drawing.Size(131, 23);
            this.btnSaveCred.TabIndex = 20;
            this.btnSaveCred.Text = "Use Profile";
            this.btnSaveCred.UseVisualStyleBackColor = false;
            this.btnSaveCred.Click += new System.EventHandler(this.btnSaveCred_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(19, 134);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Profile Key";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(19, 94);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(48, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Profile Id";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(225)))));
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBoxS3Id);
            this.tabPage1.Controls.Add(this.textBoxS3Key);
            this.tabPage1.Controls.Add(this.textBoxS3Bucket);
            this.tabPage1.Controls.Add(this.textBoxName);
            this.tabPage1.Controls.Add(this.textBoxNote);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(938, 276);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Management Console";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(742, 17);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 16;
            this.button7.Text = "Del";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(8, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Name";
            this.label6.Visible = false;
            // 
            // textBox3
            // 
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox3.Location = new System.Drawing.Point(313, 20);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(351, 20);
            this.textBox3.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(8, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Note";
            this.label5.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(8, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "S3 Bucket";
            this.label4.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(225)))));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Location = new System.Drawing.Point(208, 57);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(609, 201);
            this.dataGridView1.TabIndex = 9;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(662, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Add";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.Enabled = false;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(88, 235);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(205, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Website Url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "S3 Key";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "S3 Id";
            this.label1.Visible = false;
            // 
            // textBoxS3Id
            // 
            this.textBoxS3Id.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxS3Id.Enabled = false;
            this.textBoxS3Id.Location = new System.Drawing.Point(72, 20);
            this.textBoxS3Id.Name = "textBoxS3Id";
            this.textBoxS3Id.PasswordChar = '*';
            this.textBoxS3Id.Size = new System.Drawing.Size(66, 20);
            this.textBoxS3Id.TabIndex = 0;
            this.textBoxS3Id.UseSystemPasswordChar = true;
            this.textBoxS3Id.Visible = false;
            this.textBoxS3Id.TextChanged += new System.EventHandler(this.textBoxS3Id_TextChanged);
            // 
            // textBoxS3Key
            // 
            this.textBoxS3Key.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxS3Key.Enabled = false;
            this.textBoxS3Key.Location = new System.Drawing.Point(72, 60);
            this.textBoxS3Key.Name = "textBoxS3Key";
            this.textBoxS3Key.PasswordChar = '*';
            this.textBoxS3Key.Size = new System.Drawing.Size(66, 20);
            this.textBoxS3Key.TabIndex = 2;
            this.textBoxS3Key.UseSystemPasswordChar = true;
            this.textBoxS3Key.Visible = false;
            this.textBoxS3Key.TextChanged += new System.EventHandler(this.textBoxS3Key_TextChanged);
            // 
            // textBoxS3Bucket
            // 
            this.textBoxS3Bucket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxS3Bucket.Enabled = false;
            this.textBoxS3Bucket.Location = new System.Drawing.Point(72, 100);
            this.textBoxS3Bucket.Name = "textBoxS3Bucket";
            this.textBoxS3Bucket.PasswordChar = '*';
            this.textBoxS3Bucket.Size = new System.Drawing.Size(66, 20);
            this.textBoxS3Bucket.TabIndex = 10;
            this.textBoxS3Bucket.UseSystemPasswordChar = true;
            this.textBoxS3Bucket.Visible = false;
            // 
            // textBoxName
            // 
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxName.Enabled = false;
            this.textBoxName.Location = new System.Drawing.Point(72, 140);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(66, 20);
            this.textBoxName.TabIndex = 14;
            this.textBoxName.Visible = false;
            // 
            // textBoxNote
            // 
            this.textBoxNote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNote.Enabled = false;
            this.textBoxNote.Location = new System.Drawing.Point(72, 180);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNote.Size = new System.Drawing.Size(66, 40);
            this.textBoxNote.TabIndex = 12;
            this.textBoxNote.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(225)))));
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(938, 276);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = " Content Retrieval";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Transparent;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(432, 127);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Retrieve";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(225)))));
            this.tabPage3.Controls.Add(this.button5);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(938, 276);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Content Reconstruction";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Transparent;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(432, 140);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "Upload";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(432, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Reconstruct";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(225)))));
            this.tabPage4.Controls.Add(this.ckbcloudfront);
            this.tabPage4.Controls.Add(this.ckbShortURL);
            this.tabPage4.Controls.Add(this.button6);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(938, 276);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "URL Generator";
            // 
            // ckbcloudfront
            // 
            this.ckbcloudfront.AutoSize = true;
            this.ckbcloudfront.Checked = true;
            this.ckbcloudfront.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbcloudfront.Location = new System.Drawing.Point(420, 130);
            this.ckbcloudfront.Name = "ckbcloudfront";
            this.ckbcloudfront.Size = new System.Drawing.Size(139, 17);
            this.ckbcloudfront.TabIndex = 7;
            this.ckbcloudfront.Text = "Use URL Cloudfront.net";
            this.ckbcloudfront.UseVisualStyleBackColor = true;
            this.ckbcloudfront.CheckedChanged += new System.EventHandler(this.ckbcloudfront_CheckedChanged);
            // 
            // ckbShortURL
            // 
            this.ckbShortURL.AutoSize = true;
            this.ckbShortURL.Checked = true;
            this.ckbShortURL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbShortURL.Location = new System.Drawing.Point(420, 92);
            this.ckbShortURL.Name = "ckbShortURL";
            this.ckbShortURL.Size = new System.Drawing.Size(98, 17);
            this.ckbShortURL.TabIndex = 6;
            this.ckbShortURL.Text = "Use Short URL";
            this.ckbShortURL.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Transparent;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(434, 160);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 5;
            this.button6.Text = "Generate";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(242)))), ((int)(((byte)(225)))));
            this.panelTop.Controls.Add(this.buttonClose);
            this.panelTop.Controls.Add(this.labelTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(946, 26);
            this.panelTop.TabIndex = 12;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            this.panelTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseMove);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.Transparent;
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.ForeColor = System.Drawing.Color.Black;
            this.buttonClose.Location = new System.Drawing.Point(871, 1);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.TabStop = false;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.BackColor = System.Drawing.Color.Transparent;
            this.labelTop.Location = new System.Drawing.Point(12, 6);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(33, 13);
            this.labelTop.TabIndex = 0;
            this.labelTop.Text = "oLink";
            // 
            // panelMsg
            // 
            this.panelMsg.BackColor = System.Drawing.Color.White;
            this.panelMsg.Controls.Add(this.textBoxMsg);
            this.panelMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelMsg.Location = new System.Drawing.Point(3, 334);
            this.panelMsg.Name = "panelMsg";
            this.panelMsg.Padding = new System.Windows.Forms.Padding(12, 6, 0, 6);
            this.panelMsg.Size = new System.Drawing.Size(946, 80);
            this.panelMsg.TabIndex = 9;
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.BackColor = System.Drawing.Color.White;
            this.textBoxMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMsg.Location = new System.Drawing.Point(12, 6);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMsg.Size = new System.Drawing.Size(934, 68);
            this.textBoxMsg.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.labelMsg);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(3, 414);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(946, 36);
            this.panelBottom.TabIndex = 6;
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.BackColor = System.Drawing.Color.Transparent;
            this.labelMsg.Location = new System.Drawing.Point(12, 11);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(47, 13);
            this.labelMsg.TabIndex = 0;
            this.labelMsg.Text = "Running";
            // 
            // FormLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 453);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLink";
            this.Text = "oLink";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            this.tabEntries.ResumeLayout(false);
            this.tpIAM.ResumeLayout(false);
            this.tpIAM.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMsg.ResumeLayout(false);
            this.panelMsg.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.Panel panelMsg;
        private System.Windows.Forms.TextBox textBoxMsg;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabControl tabEntries;
        private System.Windows.Forms.TabPage tpIAM;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxDbPwdP;
        private System.Windows.Forms.TextBox tbxDbUserP;
        private System.Windows.Forms.TextBox tbxProfName;
        private System.Windows.Forms.TextBox tbxS3buckets;
        private System.Windows.Forms.TextBox tbxSecKey;
        private System.Windows.Forms.TextBox tbxAccKey;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbxDBep;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbxProfiles;
        private System.Windows.Forms.Label lblProfNm;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnSaveCred;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.TextBox textBoxS3Bucket;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBoxS3Key;
        private System.Windows.Forms.TextBox textBoxS3Id;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbxDbConnString;
        private System.Windows.Forms.TextBox tbxcurUser;
        private System.Windows.Forms.Label lbltbxcurUser;
        private System.Windows.Forms.CheckBox ckbShortURL;
        private System.Windows.Forms.Label lbsrdssf;
        private System.Windows.Forms.Label lbls3sf;
        private System.Windows.Forms.TextBox tbxRDSsf;
        private System.Windows.Forms.TextBox tbxS3sf;
        private System.Windows.Forms.CheckBox ckbcloudfront;
    }
}

