namespace reAudioPlayerML
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
            this.artist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.album = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.spotifyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openOnSpotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAddToPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.addToPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.searchOnYoutubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pgSettings = new MetroFramework.Controls.MetroTabPage();
            this.tmrAccentColour = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(699, 71);
            this.pnlHead.TabIndex = 0;
            this.pnlHead.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHead_Paint);
            // 
            // lblUpNowArtist
            // 
            this.lblUpNowArtist.AutoSize = true;
            this.lblUpNowArtist.Font = new System.Drawing.Font("Segoe UI Light", 8.25F);
            this.lblUpNowArtist.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblUpNowArtist.Location = new System.Drawing.Point(14, 46);
            this.lblUpNowArtist.Name = "lblUpNowArtist";
            this.lblUpNowArtist.Size = new System.Drawing.Size(26, 13);
            this.lblUpNowArtist.TabIndex = 1;
            this.lblUpNowArtist.Text = "N/A";
            this.lblUpNowArtist.UseMnemonic = false;
            // 
            // lblUpNowTitle
            // 
            this.lblUpNowTitle.AutoSize = true;
            this.lblUpNowTitle.Font = new System.Drawing.Font("Segoe UI Light", 20.25F);
            this.lblUpNowTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblUpNowTitle.Location = new System.Drawing.Point(8, 9);
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
            this.pnlFooter.Location = new System.Drawing.Point(0, 439);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(699, 128);
            this.pnlFooter.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::reAudioPlayerML.Properties.Resources.volume_down2;
            this.pictureBox2.Location = new System.Drawing.Point(58, 89);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::reAudioPlayerML.Properties.Resources.volume_up;
            this.pictureBox1.Location = new System.Drawing.Point(627, 91);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(18, 18);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblTimeUp
            // 
            this.lblTimeUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeUp.AutoEllipsis = true;
            this.lblTimeUp.AutoSize = true;
            this.lblTimeUp.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.lblTimeUp.Location = new System.Drawing.Point(651, 35);
            this.lblTimeUp.Name = "lblTimeUp";
            this.lblTimeUp.Size = new System.Drawing.Size(33, 15);
            this.lblTimeUp.TabIndex = 7;
            this.lblTimeUp.Text = "-0:00";
            // 
            // lblTimeDone
            // 
            this.lblTimeDone.AutoSize = true;
            this.lblTimeDone.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeDone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.lblTimeDone.Location = new System.Drawing.Point(12, 35);
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
            this.prgVolume.Location = new System.Drawing.Point(86, 89);
            this.prgVolume.Name = "prgVolume";
            this.prgVolume.Size = new System.Drawing.Size(529, 23);
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
            this.prgTimeDone.Location = new System.Drawing.Point(12, 13);
            this.prgTimeDone.Maximum = 1000;
            this.prgTimeDone.Name = "prgTimeDone";
            this.prgTimeDone.Size = new System.Drawing.Size(675, 23);
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
            this.btnNext.Location = new System.Drawing.Point(420, 43);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(40, 40);
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
            this.btnPlayPause.Location = new System.Drawing.Point(328, 43);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(40, 40);
            this.btnPlayPause.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnPlayPause.TabIndex = 1;
            this.btnPlayPause.TabStop = false;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // btnLast
            // 
            this.btnLast.Image = global::reAudioPlayerML.Properties.Resources.last;
            this.btnLast.Location = new System.Drawing.Point(230, 43);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(40, 40);
            this.btnLast.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnLast.TabIndex = 0;
            this.btnLast.TabStop = false;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // pnlRight
            // 
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(568, 71);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(131, 368);
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
            this.tbControl.Location = new System.Drawing.Point(0, 71);
            this.tbControl.Name = "tbControl";
            this.tbControl.SelectedIndex = 2;
            this.tbControl.Size = new System.Drawing.Size(568, 368);
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
            this.pgQuickAccess.HorizontalScrollbarSize = 10;
            this.pgQuickAccess.Location = new System.Drawing.Point(4, 38);
            this.pgQuickAccess.Name = "pgQuickAccess";
            this.pgQuickAccess.Size = new System.Drawing.Size(560, 326);
            this.pgQuickAccess.TabIndex = 0;
            this.pgQuickAccess.Text = "Quick Access";
            this.pgQuickAccess.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgQuickAccess.UseCustomBackColor = true;
            this.pgQuickAccess.VerticalScrollbarBarColor = true;
            this.pgQuickAccess.VerticalScrollbarHighlightOnWheel = false;
            this.pgQuickAccess.VerticalScrollbarSize = 10;
            // 
            // btnSync
            // 
            this.btnSync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSync.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSync.Location = new System.Drawing.Point(8, 216);
            this.btnSync.Name = "btnSync";
            this.btnSync.Size = new System.Drawing.Size(126, 23);
            this.btnSync.TabIndex = 10;
            this.btnSync.Text = "Sync Now!";
            this.btnSync.UseVisualStyleBackColor = false;
            this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
            // 
            // imgQR
            // 
            this.imgQR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgQR.Location = new System.Drawing.Point(447, 13);
            this.imgQR.Name = "imgQR";
            this.imgQR.Size = new System.Drawing.Size(100, 100);
            this.imgQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgQR.TabIndex = 9;
            this.imgQR.TabStop = false;
            // 
            // btnAddGame
            // 
            this.btnAddGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnAddGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGame.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddGame.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddGame.Location = new System.Drawing.Point(8, 187);
            this.btnAddGame.Name = "btnAddGame";
            this.btnAddGame.Size = new System.Drawing.Size(126, 23);
            this.btnAddGame.TabIndex = 8;
            this.btnAddGame.Text = "Add New Game";
            this.btnAddGame.UseVisualStyleBackColor = false;
            this.btnAddGame.Click += new System.EventHandler(this.btnGameLauncher_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnDownload.Location = new System.Drawing.Point(8, 71);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(126, 23);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnApolloOnAir
            // 
            this.btnApolloOnAir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnApolloOnAir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApolloOnAir.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApolloOnAir.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnApolloOnAir.Location = new System.Drawing.Point(8, 129);
            this.btnApolloOnAir.Name = "btnApolloOnAir";
            this.btnApolloOnAir.Size = new System.Drawing.Size(126, 23);
            this.btnApolloOnAir.TabIndex = 6;
            this.btnApolloOnAir.Text = "Apollo On Air";
            this.btnApolloOnAir.UseVisualStyleBackColor = false;
            this.btnApolloOnAir.Click += new System.EventHandler(this.btnApolloOnAir_Click);
            // 
            // btnRevealedRadio
            // 
            this.btnRevealedRadio.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnRevealedRadio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevealedRadio.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevealedRadio.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnRevealedRadio.Location = new System.Drawing.Point(8, 100);
            this.btnRevealedRadio.Name = "btnRevealedRadio";
            this.btnRevealedRadio.Size = new System.Drawing.Size(126, 23);
            this.btnRevealedRadio.TabIndex = 5;
            this.btnRevealedRadio.Text = "Revealed Radio";
            this.btnRevealedRadio.UseVisualStyleBackColor = false;
            this.btnRevealedRadio.Click += new System.EventHandler(this.btnRevealedRadio_Click);
            // 
            // btnLoadPlaylist
            // 
            this.btnLoadPlaylist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnLoadPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadPlaylist.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadPlaylist.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnLoadPlaylist.Location = new System.Drawing.Point(8, 13);
            this.btnLoadPlaylist.Name = "btnLoadPlaylist";
            this.btnLoadPlaylist.Size = new System.Drawing.Size(126, 23);
            this.btnLoadPlaylist.TabIndex = 4;
            this.btnLoadPlaylist.Text = "Load Playlist";
            this.btnLoadPlaylist.UseVisualStyleBackColor = false;
            this.btnLoadPlaylist.Click += new System.EventHandler(this.btnLoadPlaylist_Click);
            // 
            // btnWebsite
            // 
            this.btnWebsite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnWebsite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWebsite.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWebsite.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnWebsite.Location = new System.Drawing.Point(8, 42);
            this.btnWebsite.Name = "btnWebsite";
            this.btnWebsite.Size = new System.Drawing.Size(126, 23);
            this.btnWebsite.TabIndex = 3;
            this.btnWebsite.Text = "WebClient";
            this.btnWebsite.UseVisualStyleBackColor = false;
            this.btnWebsite.Click += new System.EventHandler(this.btnWebsite_Click);
            // 
            // btnMove
            // 
            this.btnMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMove.Font = new System.Drawing.Font("Segoe UI Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMove.Location = new System.Drawing.Point(8, 158);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(126, 23);
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
            this.pgPlay.HorizontalScrollbarSize = 10;
            this.pgPlay.Location = new System.Drawing.Point(4, 38);
            this.pgPlay.Name = "pgPlay";
            this.pgPlay.Size = new System.Drawing.Size(560, 326);
            this.pgPlay.TabIndex = 1;
            this.pgPlay.Text = "Play";
            this.pgPlay.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgPlay.VerticalScrollbarBarColor = true;
            this.pgPlay.VerticalScrollbarHighlightOnWheel = false;
            this.pgPlay.VerticalScrollbarSize = 10;
            this.pgPlay.Visible = false;
            // 
            // imgCover
            // 
            this.imgCover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.imgCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgCover.Location = new System.Drawing.Point(0, 0);
            this.imgCover.Name = "imgCover";
            this.imgCover.Size = new System.Drawing.Size(560, 326);
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
            this.pgConnectSpotify.HorizontalScrollbarSize = 10;
            this.pgConnectSpotify.Location = new System.Drawing.Point(4, 38);
            this.pgConnectSpotify.Name = "pgConnectSpotify";
            this.pgConnectSpotify.Size = new System.Drawing.Size(560, 326);
            this.pgConnectSpotify.TabIndex = 4;
            this.pgConnectSpotify.Text = "Spotify Releases";
            this.pgConnectSpotify.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgConnectSpotify.VerticalScrollbarBarColor = true;
            this.pgConnectSpotify.VerticalScrollbarHighlightOnWheel = false;
            this.pgConnectSpotify.VerticalScrollbarSize = 10;
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
            this.listView1.Font = new System.Drawing.Font("Segoe UI Light", 9F);
            this.listView1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(560, 326);
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
            this.openOnSpotifyToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openOnSpotifyToolStripMenuItem.Text = "Open on Spotify";
            // 
            // btnAddToPlaylist
            // 
            this.btnAddToPlaylist.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToPlaylistToolStripMenuItem});
            this.btnAddToPlaylist.Name = "btnAddToPlaylist";
            this.btnAddToPlaylist.Size = new System.Drawing.Size(180, 22);
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
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // searchOnYoutubeToolStripMenuItem
            // 
            this.searchOnYoutubeToolStripMenuItem.Name = "searchOnYoutubeToolStripMenuItem";
            this.searchOnYoutubeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.searchOnYoutubeToolStripMenuItem.Text = "Search on Youtube";
            // 
            // pgSettings
            // 
            this.pgSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.pgSettings.HorizontalScrollbarBarColor = true;
            this.pgSettings.HorizontalScrollbarHighlightOnWheel = false;
            this.pgSettings.HorizontalScrollbarSize = 10;
            this.pgSettings.Location = new System.Drawing.Point(4, 38);
            this.pgSettings.Name = "pgSettings";
            this.pgSettings.Size = new System.Drawing.Size(560, 326);
            this.pgSettings.TabIndex = 3;
            this.pgSettings.Text = "Settings";
            this.pgSettings.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.pgSettings.UseCustomBackColor = true;
            this.pgSettings.VerticalScrollbarBarColor = true;
            this.pgSettings.VerticalScrollbarHighlightOnWheel = false;
            this.pgSettings.VerticalScrollbarSize = 10;
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.previewToolStripMenuItem.Text = "Preview";
            // 
            // ml
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(699, 567);
            this.Controls.Add(this.tbControl);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHead);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
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

