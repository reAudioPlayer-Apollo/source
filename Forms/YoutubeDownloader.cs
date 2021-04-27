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

            syncer.addOptimiseWatcher(txtDirectory.Text);

            dl = new NYoutubeDL.YoutubeDL();
            dl.YoutubeDlPath = AppContext.BaseDirectory + "ressources\\youtube-dl.exe";
            dl.Options.FilesystemOptions.Continue = true;
            dl.Options.GeneralOptions.IgnoreErrors = true;
            dl.Options.FilesystemOptions.NoOverwrites = true;
            dl.Options.PostProcessingOptions.AddMetadata = true;
            dl.Options.PostProcessingOptions.ExtractAudio = true;
            dl.Options.PostProcessingOptions.AudioFormat = NYoutubeDL.Helpers.Enums.AudioFormat.mp3;
            dl.Options.FilesystemOptions.Output = txtDirectory.Text + "\\%(title)s.%(ext)s";
            dl.VideoUrl = txtLink.Text;
            dl.Options.GeneralOptions.Update = true;
            dl.Options.VideoSelectionOptions.NoPlaylist = chkSync.Enabled && (chkSync.Checked || (MessageBox.Show("Do you want to download this as a playlist?", "Apollo Downloader", MessageBoxButtons.YesNo) == DialogResult.No));

            if (dl.Options.VideoSelectionOptions.NoPlaylist)
            {
                var arg = dl.VideoUrl.Split('?')[1].Split('&')[0];
                dl.VideoUrl = dl.VideoUrl.Split('?')[0] + "?" + arg;
            }

            Debug.WriteLine(dl.PrepareDownload());

            if (chkSync.Checked && chkSync.Enabled)
            {
                var args = txtLink.Text.Split('?')[1].Split('&');

                string filename = "";

                foreach (var arg in args)
                {
                    if (arg.Contains("list="))
                        filename = arg;
                }

                filename = Path.Combine(docs, filename + ".conf");

                File.WriteAllText(filename, $"-ciw --add-metadata --embed-thumbnail -x --audio-format mp3 -o \"{dl.Options.FilesystemOptions.Output}\"");

                if (!File.Exists(filename.Replace(".conf", ".arch")))
                    File.Create(filename.Replace(".conf", ".arch"));

                syncer.sync();
            }

            else
            {
                dl.DownloadAsync();
                dl.StandardOutputEvent += Dl_StandardOutputEvent;
                dl.StandardErrorEvent += Dl_StandardErrorEvent; ;
            }
        }

        private void Dl_StandardErrorEvent(object sender, string e)
        {
            syncer.Dl_StandardErrorEvent(sender, e);
        }

        private void Dl_StandardOutputEvent(object sender, string e)
        {
            syncer.Dl_StandardOutputEvent(sender, e);
        }
    }
}
