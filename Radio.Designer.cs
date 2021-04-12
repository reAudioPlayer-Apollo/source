﻿namespace reAudioPlayerML
{
    partial class Radio
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Radio));
            this.panel1 = new System.Windows.Forms.Panel();
            this.prgVolume = new MetroFramework.Controls.MetroTrackBar();
            this.btnNext = new System.Windows.Forms.PictureBox();
            this.lblProgramme = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblArtist = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.metroProgressSpinner1 = new MetroFramework.Controls.MetroProgressSpinner();
            this.imgCover = new System.Windows.Forms.PictureBox();
            this.reAudioPlayer = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrSpinner = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnNext)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgCover)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.prgVolume);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.lblProgramme);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 67);
            this.panel1.TabIndex = 0;
            // 
            // prgVolume
            // 
            this.prgVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.prgVolume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.prgVolume.Location = new System.Drawing.Point(566, 22);
            this.prgVolume.Name = "prgVolume";
            this.prgVolume.Size = new System.Drawing.Size(176, 23);
            this.prgVolume.Style = MetroFramework.MetroColorStyle.Silver;
            this.prgVolume.TabIndex = 6;
            this.prgVolume.Text = "metroTrackBar2";
            this.prgVolume.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.prgVolume.UseCustomBackColor = true;
            this.prgVolume.ValueChanged += new System.EventHandler(this.prgVolume_ValueChanged);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Image = global::reAudioPlayerML.Properties.Resources.next;
            this.btnNext.Location = new System.Drawing.Point(748, 13);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 40);
            this.btnNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnNext.TabIndex = 3;
            this.btnNext.TabStop = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblProgramme
            // 
            this.lblProgramme.AutoSize = true;
            this.lblProgramme.Font = new System.Drawing.Font("Segoe UI Light", 18F);
            this.lblProgramme.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProgramme.Location = new System.Drawing.Point(3, 28);
            this.lblProgramme.Name = "lblProgramme";
            this.lblProgramme.Size = new System.Drawing.Size(56, 32);
            this.lblProgramme.TabIndex = 2;
            this.lblProgramme.Text = "N/A";
            this.lblProgramme.UseMnemonic = false;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.lblTime.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTime.Location = new System.Drawing.Point(7, 9);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(180, 19);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "NOW PLAYING | XPM - XPM";
            this.lblTime.UseMnemonic = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.panel3.Controls.Add(this.lblArtist);
            this.panel3.Controls.Add(this.lblTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 316);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 67);
            this.panel3.TabIndex = 3;
            // 
            // lblArtist
            // 
            this.lblArtist.AutoSize = true;
            this.lblArtist.Font = new System.Drawing.Font("Segoe UI Light", 8.25F);
            this.lblArtist.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblArtist.Location = new System.Drawing.Point(12, 44);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(26, 13);
            this.lblArtist.TabIndex = 3;
            this.lblArtist.Text = "N/A";
            this.lblArtist.UseMnemonic = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 20.25F);
            this.lblTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTitle.Location = new System.Drawing.Point(6, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(63, 37);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "N/A";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblTitle.UseMnemonic = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.metroProgressSpinner1);
            this.panel2.Controls.Add(this.imgCover);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 383);
            this.panel2.TabIndex = 1;
            // 
            // metroProgressSpinner1
            // 
            this.metroProgressSpinner1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroProgressSpinner1.CustomBackground = true;
            this.metroProgressSpinner1.Location = new System.Drawing.Point(284, 44);
            this.metroProgressSpinner1.Maximum = 100;
            this.metroProgressSpinner1.Name = "metroProgressSpinner1";
            this.metroProgressSpinner1.Size = new System.Drawing.Size(220, 220);
            this.metroProgressSpinner1.Style = MetroFramework.MetroColorStyle.White;
            this.metroProgressSpinner1.TabIndex = 4;
            this.metroProgressSpinner1.UseCustomBackColor = true;
            this.metroProgressSpinner1.UseSelectable = true;
            // 
            // imgCover
            // 
            this.imgCover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.imgCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgCover.Location = new System.Drawing.Point(0, 0);
            this.imgCover.Name = "imgCover";
            this.imgCover.Size = new System.Drawing.Size(800, 316);
            this.imgCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCover.TabIndex = 3;
            this.imgCover.TabStop = false;
            // 
            // reAudioPlayer
            // 
            this.reAudioPlayer.Icon = ((System.Drawing.Icon)(resources.GetObject("reAudioPlayer.Icon")));
            this.reAudioPlayer.Text = "reAudioPlayer Radio";
            this.reAudioPlayer.Visible = true;
            // 
            // tmrSpinner
            // 
            this.tmrSpinner.Enabled = true;
            this.tmrSpinner.Interval = 200;
            this.tmrSpinner.Tick += new System.EventHandler(this.tmrSpinner_Tick);
            // 
            // Radio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Radio";
            this.Text = "Apollo On Air";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Radio_FormClosed);
            this.Load += new System.EventHandler(this.Radio_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnNext)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgCover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox imgCover;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblProgramme;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.NotifyIcon reAudioPlayer;
        private MetroFramework.Controls.MetroProgressSpinner metroProgressSpinner1;
        private System.Windows.Forms.Timer tmrSpinner;
        private System.Windows.Forms.PictureBox btnNext;
        private MetroFramework.Controls.MetroTrackBar prgVolume;
    }
}