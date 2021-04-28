using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class YoutubeDownloader : Form
    {
        NYoutubeDL.YoutubeDL dl;
        YoutubeSyncer syncer;

        public YoutubeDownloader(YoutubeSyncer youtubeSyncer)
        {
            InitializeComponent();
            syncer = youtubeSyncer;
        }

        private void YoutubeDownloader_Load(object sender, EventArgs e)
        {
            
        }

        private void txtLink_ButtonClick(object sender, EventArgs e)
        {
            Process.Start("https://music.youtube.com/");
        }

        private void txtDirectory_ButtonClick(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtDirectory.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void txtLink_TextChanged(object sender, EventArgs e)
        {
            chkSync.Enabled = txtLink.Text.Contains("list=");
        }

        string docs = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\reAudioPlayer\\Syncs";

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtDirectory.Text))
            {
                MessageBox.Show("Directory Invalid!", "reAudioPlayer Downloader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // chkSync.Enabled is true, when link is playlist
            bool noPlaylist = chkSync.Enabled && (chkSync.Checked || (MessageBox.Show("Do you want to download this as a playlist?", "Apollo Downloader", MessageBoxButtons.YesNo) == DialogResult.No));

            syncer.createAndDownload(txtLink.Text, txtDirectory.Text, noPlaylist, chkSync.Checked && chkSync.Enabled);

            //dl = syncer.createDownloader(txtLink.Text, txtDirectory.Text, noPlaylist);
            
            //Debug.WriteLine(dl.PrepareDownload());

            /*if (chkSync.Checked && chkSync.Enabled)
            {
                syncer.createSyncJob(txtLink.Text);
            }
            else
            {
                dl.DownloadAsync();
            }*/
        }
    }
}
