﻿namespace reAudioPlayerML
{
    partial class ml
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ml));
            this.pnlHead = new System.Windows.Forms.Panel();
            this.lblUpNowArtist = new System.Windows.Forms.Label();
            this.lblUpNowTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTimeUp = new System.Windows.Forms.Label();
            this.lblTimeDone = new System.Windows.Forms.Label();
            this.prgVolume = new MetroFramework.Controls.MetroTrackBar();
            this.prgTimeDone = new MetroFramework.Controls.MetroTrackBar();
            this.btnNext = new System.Windows.Forms.PictureBox();
            this.btnPlayPause = new System.Windows.Forms.PictureBox();
            this.btnLast = new System.Windows.Forms.PictureBox();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tbControl = new MetroFramework.Controls.MetroTabControl();
            this.pgQuickAccess = new MetroFramework.Controls.MetroTabPage();
            this.btnSync = new System.Windows.Forms.Button();
            this.imgQR = new System.Windows.Forms.PictureBox();
            this.btnAddGame = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnApolloOnAir = new System.Windows.Forms.Button();
            this.btnRevealedRadio = new System.Windows.Forms.Button();
            this.btnLoadPlaylist = new System.Windows.Forms.Button();
            this.btnWebsite = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.pgPlay = new MetroFramework.Controls.MetroTabPage();
            this.imgCover = new System.Windows.Forms.PictureBox();
            this.pgConnectSpotify = new MetroFramework.Controls.MetroTabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.artist = new System.Windows.Forms.ColumnHeader();
            this.album = new System.Windows.Forms.ColumnHeader();
            this.date = new System.Windows.Forms.ColumnHeader();
            this.spotifyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openOnSpotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddToPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.addToPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.searchOnYoutubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pgSettings = new MetroFramework.Controls.MetroTabPage();
            this.tmrAccentColour = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlHead.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlayPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLast)).BeginInit();
            this.tbControl.SuspendLayout();
            this.pgQuickAccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgQR)).BeginInit();
            this.pgPlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgCover)).BeginInit();
            this.pgConnectSpotify.SuspendLayout();
            this.spotifyContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHead
            // 
            this.pnlHead.Controls.Add(this.lblUpNowArtist);
            this.pnlHead.Controls.Add(this.lblUpNowTitle);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(816, 82);
            this.pnlHead.TabIndex = 0;
            this.pnlHead.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHead_Paint);
            // 
            // lblUpNowArtist
            // 
            this.lblUpNowArtist.AutoSize = true;
            this.lblUpNowArtist.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUpNowArtist.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblUpNowArtist.Location = new System.Drawing.Point(16, 53);
            this.lblUpNowArtist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpNowArtist.Name = "lblUpNowArtist";
            this.lblUpNowArtist.Size = new System.Drawing.Size(26, 13);
            this.lblUpNowArtist.TabIndex = 1;
            this.lblUpNowArtist.Text = "N/A";
            this.lblUpNowArtist.UseMnemonic = false;
            // 
            // lblUpNowTitle
            // 
            this.lblUpNowTitle.AutoSize = true;
            this.lblUpNowTitle.Font = new System.Drawing.Font("Segoe UI Light", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUpNowTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblUpNowTitle.Location = new System.Drawing.Point(9, 10);
            this.lblUpNowTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpNowTitle.Name = "lblUpNowTitle";
            this.lblUpNowTitle.Size = new System.Drawing.Size(63, 37);
            this.lblUpNowTitle.TabIndex = 0;
            this.lblUpNowTitle.Text = "N/A";
            this.lblUpNowTitle.UseMnemonic = false;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.pictureBox2);
            this.pnlFooter.Controls.Add(this.pictureBox1);
            this.pnlFooter.Controls.Add(this.lblTimeUp);
            this.pnlFooter.Controls.Add(this.lblTimeDone);
            this.pnlFooter.Controls.Add(this.prgVolume);
            this.pnlFooter.Controls.Add(this.prgTimeDone);
            this.pnlFooter.Controls.Add(this.btnNext);
            this.pnlFooter.Controls.Add(this.btnPlayPause);
            this.pnlFooter.Controls.Add(this.btnLast);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 506);
            this.pnlFooter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(816, 148);
            this.pnlFooter.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::reAudioPlayerML.Properties.Resources.volume_down2;
            this.pictureBox2.Location = new System.Drawing.Point(68, 103);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(23, 23);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::reAudioPlayerML.Properties.Resources.volume_up;
            this.pictureBox1.Location = new System.Drawing.Point(732, 105);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 21);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblTimeUp
            // 
            this.lblTimeUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeUp.AutoEllipsis = true;
            this.lblTimeUp.AutoSize = true;
            this.lblTimeUp.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTimeUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.lblTimeUp.Location = new System.Drawing.Point(760, 40);
            this.lblTimeUp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeUp.Name = "lblTimeUp";
            this.lblTimeUp.Size = new System.Drawing.Size(33, 15);
            this.lblTimeUp.TabIndex = 7;
            this.lblTimeUp.Text = "-0:00";
            // 
            // lblTimeDone
            // 
            this.lblTimeDone.AutoSize = true;
            this.lblTimeDone.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTimeDone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.lblTimeDone.Location = new System.Drawing.Point(14, 40);
            this.lblTimeDone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeDone.Name = "lblTimeDone";
            this.lblTimeDone.Size = new System.Drawing.Size(28, 15);
            this.lblTimeDone.TabIndex = 6;
            this.lblTimeDone.Text = "0:00";
            // 
            // prgVolume
            // 
            this.prgVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgVolume.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.prgVolume.Location = new System.Drawing.Point(100, 103);
            this.prgVolume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.prgVolume.Name = "prgVolume";
            this.prgVolume.Size = new System.Drawing.Size(617, 27);
            this.prgVolume.Style = MetroFramework.MetroColorStyle.Silver;
            this.prgVolume.TabIndex = 5;
            this.prgVolume.Text = "metroTrackBar2";
            this.prgVolume.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.prgVolume.UseCustomBackColor = true;
            // 
            // prgTimeDone
            // 
            this.prgTimeDone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgTimeDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.prgTimeDone.Location = new System.Drawing.Point(14, 15);
            this.prgTimeDone.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.prgTimeDone.Maximum = 1000;
            this.prgTimeDone.Name = "prgTimeDone";
            this.prgTimeDone.Size = new System.Drawing.Size(788, 27);
            this.prgTimeDone.TabIndex = 4;
            this.prgTimeDone.Text = "metroTrackBar1";
            this.prgTimeDone.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.prgTimeDone.UseCustomBackColor = true;
            this.prgTimeDone.Scroll += new System.Windows.Forms.ScrollEventHandler(this.prgTimeDone_Scroll);
            this.prgTimeDone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.prgTimeDone_KeyDown);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Image = global::reAudioPlayerML.Properties.Resources.next;
            this.btnNext.Location = new System.Drawing.Point(490, 50);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(47, 46);
            this.btnNext.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnNext.TabIndex = 2;
            this.btnNext.TabStop = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlayPause.Image = global::reAudioPlayerML.Properties.Resources.play;
            this.btnPlayPause.Location = new System.Drawing.Point(383, 50);
            this.btnPlayPause.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(47, 46);
            this.btnPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnPlayPause.TabIndex = 1;
            this.btnPlayPause.TabStop = false;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // btnLast
            // 
            this.btnLast.Image = global::reAudioPlayerML.Properties.Resources.last;
            this.btnLast.Location = new System.Drawing.Point(268, 50);
            this.btnLast.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(47, 46);
            this.btnLast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnLast.TabIndex = 0;
            this.btnLast.TabStop = false;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // pnlRight
            // 
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(663, 82);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(153, 424);
            this.pnlRight.TabIndex = 2;
            // 
            // tbControl
            // 
            this.tbControl.AllowDrop = true;
            this.tbControl.Controls.Add(this.pgQuickAccess);
            this.tbControl.Controls.Add(this.pgPlay);
            this.tbControl.Controls.Add(this.pgConnectSpotify);
            this.tbControl.Controls.Add(this.pgSettings);
            this.tbControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbControl.Location = new System.Drawing.Point(0, 82);
            this.tbControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbControl.Name = "tbControl";
            this.tbControl.Padding = new System.Drawing.Point(6, 8);
            this.tbControl.SelectedIndex = 2;
            this.tbControl.Size = new System.Drawing.Size(663, 424);
            this.tbControl.Style = MetroFramework.MetroColorStyle.Silver;
            this.tbControl.TabIndex = 3;
            this.tbControl.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tbControl.UseSelectable = true;
            this.tbControl.SelectedIndexChanged += new System.EventHandler(this.tbControl_SelectedIndexChanged);
            this.tbControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.ml_DragDrop);
            this.tbControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.ml_DragEnter);
            // 
            // pgQuickAccess
            // 
            this.pgQuickAccess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.pgQuickAccess.Controls.Add(this.btnSync);
            this.pgQuickAccess.Controls.Add(this.imgQR);
            this.pgQuickAccess.Controls.Add(this.btnAddGame);
            this.pgQuickAccess.Controls.Add(this.btnDownload);
            this.pgQuickAccess.Controls.Add(this.btnApolloOnAir);
            this.pgQuickAccess.Controls.Add(this.btnRevealedRadio);
            this.pgQuickAccess.Controls.Add(this.btnLoadPlaylist);
            this.pgQuickAccess.Controls.Add(this.btnWebsite);
            this.pgQuickAccess.Controls.Add(this.btnMove);
            this.pgQuickAccess.HorizontalScrollbarBarColor = true;
            this.pgQuickAccess.HorizontalScrollbarHighlightOnWheel = false;
            this.pgQuickAccess.HorizontalScrollbarSize = 12;
            this.pgQuickAccess.Location = new System.Drawing.Point(4, 38);
            this.pgQuickAccess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pgQuickAccess.Name = "pgQuickAccess";
            this.pgQuickAccess.Size = new System.Drawing.Size(655, 382);
            this.pgQuickAccess.TabIndex = 0;
            this.pgQuickAccess.Text = "Quick Access";
            this.pgQuickAccess.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgQuickAccess.UseCustomBackColor = true;
            this.pgQuickAccess.VerticalScrollbarBarColor = true;
            this.pgQuickAccess.VerticalScrollbarHighlightOnWheel = false;
            this.pgQuickAccess.VerticalScrollbarSize = 12;
            // 
            // btnSync
            // 
            this.btnSync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSync.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSync.Location = new System.Drawing.Point(9, 249);
            this.btnSync.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(147, 27);
            this.btnSync.TabIndex = 10;
            this.btnSync.Text = "Sync Now!";
            this.btnSync.UseVisualStyleBackColor = false;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // imgQR
            // 
            this.imgQR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgQR.Location = new System.Drawing.Point(522, 15);
            this.imgQR.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imgQR.Name = "imgQR";
            this.imgQR.Size = new System.Drawing.Size(117, 115);
            this.imgQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgQR.TabIndex = 9;
            this.imgQR.TabStop = false;
            // 
            // btnAddGame
            // 
            this.btnAddGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnAddGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGame.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddGame.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddGame.Location = new System.Drawing.Point(9, 216);
            this.btnAddGame.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAddGame.Name = "btnAddGame";
            this.btnAddGame.Size = new System.Drawing.Size(147, 27);
            this.btnAddGame.TabIndex = 8;
            this.btnAddGame.Text = "Add New Game";
            this.btnAddGame.UseVisualStyleBackColor = false;
            this.btnAddGame.Click += new System.EventHandler(this.btnGameLauncher_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDownload.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnDownload.Location = new System.Drawing.Point(9, 82);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(147, 27);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnApolloOnAir
            // 
            this.btnApolloOnAir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnApolloOnAir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApolloOnAir.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnApolloOnAir.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnApolloOnAir.Location = new System.Drawing.Point(9, 149);
            this.btnApolloOnAir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnApolloOnAir.Name = "btnApolloOnAir";
            this.btnApolloOnAir.Size = new System.Drawing.Size(147, 27);
            this.btnApolloOnAir.TabIndex = 6;
            this.btnApolloOnAir.Text = "Apollo On Air";
            this.btnApolloOnAir.UseVisualStyleBackColor = false;
            this.btnApolloOnAir.Click += new System.EventHandler(this.btnApolloOnAir_Click);
            // 
            // btnRevealedRadio
            // 
            this.btnRevealedRadio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnRevealedRadio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevealedRadio.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRevealedRadio.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnRevealedRadio.Location = new System.Drawing.Point(9, 115);
            this.btnRevealedRadio.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnRevealedRadio.Name = "btnRevealedRadio";
            this.btnRevealedRadio.Size = new System.Drawing.Size(147, 27);
            this.btnRevealedRadio.TabIndex = 5;
            this.btnRevealedRadio.Text = "Revealed Radio";
            this.btnRevealedRadio.UseVisualStyleBackColor = false;
            this.btnRevealedRadio.Click += new System.EventHandler(this.btnRevealedRadio_Click);
            // 
            // btnLoadPlaylist
            // 
            this.btnLoadPlaylist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnLoadPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadPlaylist.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLoadPlaylist.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnLoadPlaylist.Location = new System.Drawing.Point(9, 15);
            this.btnLoadPlaylist.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLoadPlaylist.Name = "btnLoadPlaylist";
            this.btnLoadPlaylist.Size = new System.Drawing.Size(147, 27);
            this.btnLoadPlaylist.TabIndex = 4;
            this.btnLoadPlaylist.Text = "Load Playlist";
            this.btnLoadPlaylist.UseVisualStyleBackColor = false;
            this.btnLoadPlaylist.Click += new System.EventHandler(this.btnLoadPlaylist_Click);
            // 
            // btnWebsite
            // 
            this.btnWebsite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnWebsite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWebsite.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnWebsite.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnWebsite.Location = new System.Drawing.Point(9, 48);
            this.btnWebsite.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnWebsite.Name = "btnWebsite";
            this.btnWebsite.Size = new System.Drawing.Size(147, 27);
            this.btnWebsite.TabIndex = 3;
            this.btnWebsite.Text = "WebClient";
            this.btnWebsite.UseVisualStyleBackColor = false;
            this.btnWebsite.Click += new System.EventHandler(this.btnWebsite_Click);
            // 
            // btnMove
            // 
            this.btnMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMove.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMove.Location = new System.Drawing.Point(9, 182);
            this.btnMove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(147, 27);
            this.btnMove.TabIndex = 2;
            this.btnMove.Text = "Move";
            this.btnMove.UseVisualStyleBackColor = false;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // pgPlay
            // 
            this.pgPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.pgPlay.Controls.Add(this.imgCover);
            this.pgPlay.HorizontalScrollbarBarColor = true;
            this.pgPlay.HorizontalScrollbarHighlightOnWheel = false;
            this.pgPlay.HorizontalScrollbarSize = 12;
            this.pgPlay.Location = new System.Drawing.Point(4, 38);
            this.pgPlay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pgPlay.Name = "pgPlay";
            this.pgPlay.Size = new System.Drawing.Size(655, 382);
            this.pgPlay.TabIndex = 1;
            this.pgPlay.Text = "Play";
            this.pgPlay.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgPlay.VerticalScrollbarBarColor = true;
            this.pgPlay.VerticalScrollbarHighlightOnWheel = false;
            this.pgPlay.VerticalScrollbarSize = 12;
            this.pgPlay.Visible = false;
            // 
            // imgCover
            // 
            this.imgCover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.imgCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgCover.Location = new System.Drawing.Point(0, 0);
            this.imgCover.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imgCover.Name = "imgCover";
            this.imgCover.Size = new System.Drawing.Size(655, 382);
            this.imgCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgCover.TabIndex = 2;
            this.imgCover.TabStop = false;
            // 
            // pgConnectSpotify
            // 
            this.pgConnectSpotify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.pgConnectSpotify.Controls.Add(this.listView1);
            this.pgConnectSpotify.HorizontalScrollbarBarColor = true;
            this.pgConnectSpotify.HorizontalScrollbarHighlightOnWheel = false;
            this.pgConnectSpotify.HorizontalScrollbarSize = 12;
            this.pgConnectSpotify.Location = new System.Drawing.Point(4, 38);
            this.pgConnectSpotify.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pgConnectSpotify.Name = "pgConnectSpotify";
            this.pgConnectSpotify.Size = new System.Drawing.Size(655, 382);
            this.pgConnectSpotify.TabIndex = 4;
            this.pgConnectSpotify.Text = "Spotify Releases";
            this.pgConnectSpotify.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgConnectSpotify.VerticalScrollbarBarColor = true;
            this.pgConnectSpotify.VerticalScrollbarHighlightOnWheel = false;
            this.pgConnectSpotify.VerticalScrollbarSize = 12;
            this.pgConnectSpotify.Visible = false;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.artist,
            this.album,
            this.date});
            this.listView1.ContextMenuStrip = this.spotifyContextMenu;
            this.listView1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listView1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(655, 382);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // artist
            // 
            this.artist.Text = "Artist";
            // 
            // album
            // 
            this.album.Text = "Album";
            // 
            // date
            // 
            this.date.Text = "Release Date";
            // 
            // spotifyContextMenu
            // 
            this.spotifyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOnSpotifyToolStripMenuItem,
            this.btnAddToPlaylist,
            this.toolStripSeparator1,
            this.searchOnYoutubeToolStripMenuItem,
            this.toolStripSeparator2,
            this.previewToolStripMenuItem});
            this.spotifyContextMenu.Name = "spotifyContextMenu";
            this.spotifyContextMenu.Size = new System.Drawing.Size(174, 104);
            // 
            // openOnSpotifyToolStripMenuItem
            // 
            this.openOnSpotifyToolStripMenuItem.Name = "openOnSpotifyToolStripMenuItem";
            this.openOnSpotifyToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.openOnSpotifyToolStripMenuItem.Text = "Open on Spotify";
            // 
            // btnAddToPlaylist
            // 
            this.btnAddToPlaylist.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToPlaylistToolStripMenuItem});
            this.btnAddToPlaylist.Name = "btnAddToPlaylist";
            this.btnAddToPlaylist.Size = new System.Drawing.Size(173, 22);
            this.btnAddToPlaylist.Text = "Add to Playlist";
            // 
            // addToPlaylistToolStripMenuItem
            // 
            this.addToPlaylistToolStripMenuItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.addToPlaylistToolStripMenuItem.Name = "addToPlaylistToolStripMenuItem";
            this.addToPlaylistToolStripMenuItem.Size = new System.Drawing.Size(180, 23);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // searchOnYoutubeToolStripMenuItem
            // 
            this.searchOnYoutubeToolStripMenuItem.Name = "searchOnYoutubeToolStripMenuItem";
            this.searchOnYoutubeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.searchOnYoutubeToolStripMenuItem.Text = "Search on Youtube";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(170, 6);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.previewToolStripMenuItem.Text = "Preview";
            // 
            // pgSettings
            // 
            this.pgSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.pgSettings.HorizontalScrollbarBarColor = true;
            this.pgSettings.HorizontalScrollbarHighlightOnWheel = false;
            this.pgSettings.HorizontalScrollbarSize = 12;
            this.pgSettings.Location = new System.Drawing.Point(4, 38);
            this.pgSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pgSettings.Name = "pgSettings";
            this.pgSettings.Size = new System.Drawing.Size(655, 382);
            this.pgSettings.TabIndex = 3;
            this.pgSettings.Text = "Settings";
            this.pgSettings.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgSettings.UseCustomBackColor = true;
            this.pgSettings.VerticalScrollbarBarColor = true;
            this.pgSettings.VerticalScrollbarHighlightOnWheel = false;
            this.pgSettings.VerticalScrollbarSize = 12;
            this.pgSettings.Visible = false;
            // 
            // tmrAccentColour
            // 
            this.tmrAccentColour.Enabled = true;
            this.tmrAccentColour.Interval = 1000;
            this.tmrAccentColour.Tick += new System.EventHandler(this.tmrAccentColour_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "reAudioPlayer Apollo";
            this.notifyIcon.Visible = true;
            // 
            // ml
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(816, 654);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ml";
            this.Text = "reAudioPlayer Apollo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ml_FormClosing);
            this.Load += new System.EventHandler(this.launcher_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ml_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.ml_DragEnter);
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPlayPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLast)).EndInit();
            this.tbControl.ResumeLayout(false);
            this.pgQuickAccess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgQR)).EndInit();
            this.pgPlay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgCover)).EndInit();
            this.pgConnectSpotify.ResumeLayout(false);
            this.spotifyContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlRight;
        private MetroFramework.Controls.MetroTabControl tbControl;
        private MetroFramework.Controls.MetroTabPage pgQuickAccess;
        private MetroFramework.Controls.MetroTabPage pgPlay;
        private MetroFramework.Controls.MetroTabPage pgSettings;
        private System.Windows.Forms.Label lblUpNowTitle;
        private System.Windows.Forms.PictureBox btnLast;
        private System.Windows.Forms.PictureBox btnNext;
        private System.Windows.Forms.PictureBox btnPlayPause;
        private MetroFramework.Controls.MetroTrackBar prgTimeDone;
        private MetroFramework.Controls.MetroTrackBar prgVolume;
        private System.Windows.Forms.Label lblTimeUp;
        private System.Windows.Forms.Label lblTimeDone;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox imgCover;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Timer tmrAccentColour;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnWebsite;
        private System.Windows.Forms.Button btnLoadPlaylist;
        private MetroFramework.Controls.MetroTabPage pgConnectSpotify;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader artist;
        private System.Windows.Forms.ColumnHeader album;
        private System.Windows.Forms.ColumnHeader date;
        private System.Windows.Forms.Label lblUpNowArtist;
        private System.Windows.Forms.ContextMenuStrip spotifyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openOnSpotifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchOnYoutubeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnAddToPlaylist;
        private System.Windows.Forms.ToolStripComboBox addToPlaylistToolStripMenuItem;
        private System.Windows.Forms.Button btnRevealedRadio;
        private System.Windows.Forms.Button btnApolloOnAir;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnAddGame;
        private System.Windows.Forms.PictureBox imgQR;
        private System.Windows.Forms.Button btnSync;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
    }
}

