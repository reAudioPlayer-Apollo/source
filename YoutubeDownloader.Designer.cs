namespace reAudioPlayerML
{
    partial class YoutubeDownloader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YoutubeDownloader));
            this.txtLink = new MetroFramework.Controls.MetroTextBox();
            this.txtDirectory = new MetroFramework.Controls.MetroTextBox();
            this.chkSync = new System.Windows.Forms.CheckBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // txtLink
            // 
            this.txtLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtLink.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            this.txtLink.CustomButton.Location = new System.Drawing.Point(283, 1);
            this.txtLink.CustomButton.Name = "";
            this.txtLink.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtLink.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtLink.CustomButton.TabIndex = 1;
            this.txtLink.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtLink.CustomButton.UseSelectable = true;
            this.txtLink.Lines = new string[0];
            this.txtLink.Location = new System.Drawing.Point(12, 134);
            this.txtLink.MaxLength = 32767;
            this.txtLink.Name = "txtLink";
            this.txtLink.PasswordChar = '\0';
            this.txtLink.PromptText = "enter a link...";
            this.txtLink.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtLink.SelectedText = "";
            this.txtLink.SelectionLength = 0;
            this.txtLink.SelectionStart = 0;
            this.txtLink.ShortcutsEnabled = true;
            this.txtLink.ShowButton = true;
            this.txtLink.Size = new System.Drawing.Size(305, 23);
            this.txtLink.TabIndex = 49;
            this.txtLink.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtLink.UseCustomBackColor = true;
            this.txtLink.UseSelectable = true;
            this.txtLink.WaterMark = "enter a link...";
            this.txtLink.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtLink.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtLink.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.txtLink_ButtonClick);
            this.txtLink.TextChanged += new System.EventHandler(this.txtLink_TextChanged);
            // 
            // txtDirectory
            // 
            this.txtDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtDirectory.CustomButton.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            this.txtDirectory.CustomButton.Location = new System.Drawing.Point(283, 1);
            this.txtDirectory.CustomButton.Name = "";
            this.txtDirectory.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtDirectory.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtDirectory.CustomButton.TabIndex = 1;
            this.txtDirectory.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtDirectory.CustomButton.UseSelectable = true;
            this.txtDirectory.Lines = new string[0];
            this.txtDirectory.Location = new System.Drawing.Point(12, 163);
            this.txtDirectory.MaxLength = 32767;
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.PasswordChar = '\0';
            this.txtDirectory.PromptText = "enter a directory...";
            this.txtDirectory.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtDirectory.SelectedText = "";
            this.txtDirectory.SelectionLength = 0;
            this.txtDirectory.SelectionStart = 0;
            this.txtDirectory.ShortcutsEnabled = true;
            this.txtDirectory.ShowButton = true;
            this.txtDirectory.Size = new System.Drawing.Size(305, 23);
            this.txtDirectory.TabIndex = 50;
            this.txtDirectory.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtDirectory.UseCustomBackColor = true;
            this.txtDirectory.UseSelectable = true;
            this.txtDirectory.WaterMark = "enter a directory...";
            this.txtDirectory.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtDirectory.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtDirectory.ButtonClick += new MetroFramework.Controls.MetroTextBox.ButClick(this.txtDirectory_ButtonClick);
            // 
            // chkSync
            // 
            this.chkSync.AutoSize = true;
            this.chkSync.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.chkSync.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.chkSync.Location = new System.Drawing.Point(12, 193);
            this.chkSync.Name = "chkSync";
            this.chkSync.Size = new System.Drawing.Size(49, 17);
            this.chkSync.TabIndex = 51;
            this.chkSync.Text = "Sync";
            this.chkSync.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnDownload.Location = new System.Drawing.Point(12, 216);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(305, 23);
            this.btnDownload.TabIndex = 52;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // YoutubeDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(329, 368);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.chkSync);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.txtLink);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "YoutubeDownloader";
            this.Text = "Youtube Downloader";
            this.Load += new System.EventHandler(this.YoutubeDownloader_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox txtLink;
        private MetroFramework.Controls.MetroTextBox txtDirectory;
        private System.Windows.Forms.CheckBox chkSync;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}