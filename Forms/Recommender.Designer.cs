
namespace reAudioPlayerML
{
    partial class Recommender
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Track");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Artists");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Genres");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Seeds", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnAddGenre = new System.Windows.Forms.Button();
            this.cmbGenres = new System.Windows.Forms.ComboBox();
            this.btnAddArtist = new System.Windows.Forms.Button();
            this.cmbArtists = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvSeeds = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.clmnArtist = new System.Windows.Forms.ColumnHeader();
            this.clmnTitle = new System.Windows.Forms.ColumnHeader();
            this.clmnAlbum = new System.Windows.Forms.ColumnHeader();
            this.ctxRecommendation = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.preview = new System.Windows.Forms.ToolStripMenuItem();
            this.open = new System.Windows.Forms.ToolStripMenuItem();
            this.addToPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.playlists = new System.Windows.Forms.ToolStripComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ctxRecommendation.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Controls.Add(this.btnAddGenre);
            this.panel1.Controls.Add(this.cmbGenres);
            this.panel1.Controls.Add(this.btnAddArtist);
            this.panel1.Controls.Add(this.cmbArtists);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 100);
            this.panel1.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Location = new System.Drawing.Point(12, 42);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(770, 52);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnAddGenre
            // 
            this.btnAddGenre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnAddGenre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGenre.Location = new System.Drawing.Point(348, 12);
            this.btnAddGenre.Name = "btnAddGenre";
            this.btnAddGenre.Size = new System.Drawing.Size(75, 23);
            this.btnAddGenre.TabIndex = 3;
            this.btnAddGenre.Text = "Add Genre";
            this.btnAddGenre.UseVisualStyleBackColor = false;
            this.btnAddGenre.Click += new System.EventHandler(this.btnAddGenre_Click);
            // 
            // cmbGenres
            // 
            this.cmbGenres.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenres.FormattingEnabled = true;
            this.cmbGenres.Location = new System.Drawing.Point(221, 13);
            this.cmbGenres.Name = "cmbGenres";
            this.cmbGenres.Size = new System.Drawing.Size(121, 23);
            this.cmbGenres.TabIndex = 2;
            // 
            // btnAddArtist
            // 
            this.btnAddArtist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnAddArtist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddArtist.Location = new System.Drawing.Point(140, 13);
            this.btnAddArtist.Name = "btnAddArtist";
            this.btnAddArtist.Size = new System.Drawing.Size(75, 23);
            this.btnAddArtist.TabIndex = 1;
            this.btnAddArtist.Text = "Add Artist";
            this.btnAddArtist.UseVisualStyleBackColor = false;
            this.btnAddArtist.Click += new System.EventHandler(this.btnAddArtist_Click);
            // 
            // cmbArtists
            // 
            this.cmbArtists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArtists.FormattingEnabled = true;
            this.cmbArtists.Location = new System.Drawing.Point(13, 13);
            this.cmbArtists.Name = "cmbArtists";
            this.cmbArtists.Size = new System.Drawing.Size(121, 23);
            this.cmbArtists.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(794, 414);
            this.panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvSeeds);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(794, 414);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvSeeds
            // 
            this.tvSeeds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.tvSeeds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSeeds.ForeColor = System.Drawing.Color.White;
            this.tvSeeds.Location = new System.Drawing.Point(0, 0);
            this.tvSeeds.Name = "tvSeeds";
            treeNode1.Name = "track";
            treeNode1.Text = "Track";
            treeNode2.Name = "artists";
            treeNode2.Text = "Artists";
            treeNode3.Name = "genres";
            treeNode3.Text = "Genres";
            treeNode4.Name = "root";
            treeNode4.Text = "Seeds";
            this.tvSeeds.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4});
            this.tvSeeds.Size = new System.Drawing.Size(264, 414);
            this.tvSeeds.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnArtist,
            this.clmnTitle,
            this.clmnAlbum});
            this.listView1.ContextMenuStrip = this.ctxRecommendation;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.ForeColor = System.Drawing.Color.White;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(526, 414);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.Recommender_Click);
            // 
            // clmnArtist
            // 
            this.clmnArtist.Text = "Artist";
            // 
            // clmnTitle
            // 
            this.clmnTitle.Text = "Title";
            // 
            // clmnAlbum
            // 
            this.clmnAlbum.Text = "Album";
            // 
            // ctxRecommendation
            // 
            this.ctxRecommendation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preview,
            this.open,
            this.addToPlaylist});
            this.ctxRecommendation.Name = "ctxRecommendation";
            this.ctxRecommendation.Size = new System.Drawing.Size(163, 70);
            // 
            // preview
            // 
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(162, 22);
            this.preview.Text = "Preview";
            // 
            // open
            // 
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(162, 22);
            this.open.Text = "Open On Spotify";
            // 
            // addToPlaylist
            // 
            this.addToPlaylist.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlists});
            this.addToPlaylist.Name = "addToPlaylist";
            this.addToPlaylist.Size = new System.Drawing.Size(162, 22);
            this.addToPlaylist.Text = "Add To Playlist";
            // 
            // playlists
            // 
            this.playlists.Name = "playlists";
            this.playlists.Size = new System.Drawing.Size(121, 23);
            // 
            // Recommender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.ClientSize = new System.Drawing.Size(794, 514);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Recommender";
            this.Text = "Recommender";
            this.Load += new System.EventHandler(this.Recommender_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ctxRecommendation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnAddGenre;
        private System.Windows.Forms.ComboBox cmbGenres;
        private System.Windows.Forms.Button btnAddArtist;
        private System.Windows.Forms.ComboBox cmbArtists;
        private System.Windows.Forms.TreeView tvSeeds;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.ColumnHeader clmnArtist;
        private System.Windows.Forms.ColumnHeader clmnTitle;
        private System.Windows.Forms.ColumnHeader clmnAlbum;
        private System.Windows.Forms.ContextMenuStrip ctxRecommendation;
        private System.Windows.Forms.ToolStripMenuItem preview;
        private System.Windows.Forms.ToolStripMenuItem open;
        private System.Windows.Forms.ToolStripMenuItem addToPlaylist;
        private System.Windows.Forms.ToolStripComboBox playlists;
    }
}