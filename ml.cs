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
using LibGit2Sharp;

namespace reAudioPlayerML
{
    public partial class ml : Form
    {
        Logger logger = new Logger();
        MediaPlayer mediaPlayer;
        YoutubeSyncer youtubeSyncer = new YoutubeSyncer();
        HotkeyManager hotkeyManager;
        HttpServer.HttpWebServer server;
        Search.Spotify spotify;
        RevealedStream revealedStream;
        Radio radio;

        string[] args;

        public ml(string[] iArgs)
        {
            //Debugger.Launch();

            args = iArgs;

            InitializeComponent();

            // taskbar buttons (not working as admin)
            /*
            ThumbnailToolBarButton buttonFirst = new ThumbnailToolBarButton(Properties.Resources.last1, "Last");
            buttonFirst.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(playPause);
            buttonFirst.Visible = buttonFirst.Enabled = true;

            ThumbnailToolBarButton buttonMiddle = new ThumbnailToolBarButton(Properties.Resources.playPause1, "Play");
            buttonMiddle.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(playPause);
            buttonMiddle.Visible = buttonMiddle.Enabled = true;

            ThumbnailToolBarButton buttonLast = new ThumbnailToolBarButton(Properties.Resources.next1, "Next");
            buttonLast.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(playPause);
            buttonLast.Visible = buttonLast.Enabled = true;

            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(Handle, buttonFirst, buttonMiddle, buttonLast);
            */

            PlayerManager.mediaPlayer = mediaPlayer = new MediaPlayer(logger, notifyIcon);

            mediaPlayer.linkVolume(prgVolume);
            mediaPlayer.linkPlayPauseButton(btnPlayPause);
            mediaPlayer.linkTrackbar(prgTimeDone);
            mediaPlayer.linkTimeLabels(lblTimeDone, lblTimeUp);
            mediaPlayer.linkUpNowLabels(lblUpNowTitle, lblUpNowArtist);
            mediaPlayer.linkCover(imgCover);

            hotkeyManager = new HotkeyManager(mediaPlayer, this.prgVolume);
            
            tbControl.SelectTab(1);

            server = new HttpServer.HttpWebServer(mediaPlayer, logger, prgVolume, args, forceServer: true);

            spotify = new Search.Spotify(listView1, lviewSpotifySync, spotifyContextMenu, spotifySyncContextMenu, notifyIcon, mediaPlayer, logger);
            spotify.lblSyncProgress = lblSyncProgress;
            spotify.txtSyncIn = txtLocalInput;
            spotify.txtSyncOut = txtSyncOut;
            spotify.cmbSyncPlaylist = cmbSyncPlaylist;
            spotify.cmbLocalInput = cmbLocalInput;
            cmbLocalInput.SelectedIndex = 0;

            spotify.authoriseUser();
            PlayerManager.logger = PlaylistManager.logger = logger;
            PlaylistManager.AutoPlaylists.updateSpecialPlaylists();

            GameChecker gameChecker = new GameChecker();
            GameLibraryManager.Initialise(gameChecker, server);

            Task.Factory.StartNew(() => updateUpdater());

            keyYoutube.Text = Settings.APIKeys.youtubeKey;
            keyIGDBId.Text = Settings.APIKeys.igdbId;
            keyIGDBSecret.Text = Settings.APIKeys.igdbSecret;
            keySpotifyID.Text = Settings.APIKeys.spotifyId;
            keySpotifySecret.Text = Settings.APIKeys.spotifySecret;
            keyTMDB.Text = Settings.APIKeys.tmdbKey;
        }

        private void updateUpdater()
        {
            var loc = AppDomain.CurrentDomain.BaseDirectory;
            var parent = new DirectoryInfo(loc).Parent.Name;

            if (parent == "reAudioPlayer Apollo")
            {
                using (var repo = new Repository("../Updater"))
                {
                    var sig = new Signature("test", "test@test.com", new DateTimeOffset(DateTime.Now));
                    var opt = new PullOptions();
                    opt.MergeOptions = new MergeOptions();
                    opt.MergeOptions.FileConflictStrategy = CheckoutFileConflictStrategy.Theirs;
                    opt.MergeOptions.MergeFileFavor = MergeFileFavor.Theirs;

                    repo.Stashes.Add(sig, "Stash on master");

                    Commands.Pull(repo, sig, opt);
                }
            }
        }

        private void last(object sender, EventArgs e) { mediaPlayer.last(); }
        private void playPause(object sender, EventArgs e) { mediaPlayer.playPause(); }
        private void next(object sender, EventArgs e) { mediaPlayer.next(); }

        private void launcher_Load(object sender, EventArgs e)
        {
            imgQR.Image = QRCodeForWebClient.getImage();

            string dir = "";

            if (args.Length > 0)
            {
                dir = args[0];

                mediaPlayer.loadPlaylist(dir);
            }

            youtubeSyncer.sync();
            cleanUp();
            Directory.CreateDirectory(AppContext.BaseDirectory + "spotify");
        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            mediaPlayer.playPause();
        }

        private void prgTimeDone_Scroll(object sender, ScrollEventArgs e)
        {
            mediaPlayer.jumpTo(prgTimeDone.Value);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            mediaPlayer.next();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            mediaPlayer.last();
        }

        private void prgTimeDone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                mediaPlayer.playPause();
                e.Handled = true;
            }
        }

        private void imgCover_Click(object sender, EventArgs e)
        {

        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (mediaPlayer.playlist.Count > 0) // TODO
            {
                ofd.FileName = Path.GetFileName(mediaPlayer.getLoadedSong());
                ofd.InitialDirectory = Path.GetDirectoryName(mediaPlayer.getLoadedSong());
            }
            ofd.Title = "choose the song you want to move";

            if (!(ofd.ShowDialog() == DialogResult.OK))
                return;

            SaveFileDialog sfd= new SaveFileDialog();
            sfd.FileName = Path.GetFileName( ofd.FileName );
            sfd.InitialDirectory = ofd.InitialDirectory;

            var key = Path.GetDirectoryName(ofd.FileName).Replace(@"\\", @"\");

            if (logger.moveTrainerDictionary.ContainsKey(key))
                sfd.InitialDirectory = logger.moveTrainerDictionary[Path.GetDirectoryName(ofd.FileName)];

            sfd.Title = "choose the new location";
            var ext = Path.GetExtension(ofd.FileName);
            sfd.Filter = $"{ext.Replace(".", "")} files (*{ext})|*{ext}";

            if (!(sfd.ShowDialog() == DialogResult.OK))
                return;

            logger.addFileMove(Path.GetDirectoryName(ofd.FileName), Path.GetDirectoryName(sfd.FileName));
            File.Move(ofd.FileName, sfd.FileName);
        }

        private void tmrAccentColour_Tick(object sender, EventArgs e)
        {
            btnRevealedRadio.FlatAppearance.BorderColor =
                btnSyncAnalyse.FlatAppearance.BorderColor =
                btnSyncExport.FlatAppearance.BorderColor =
                btnSyncAutomate.FlatAppearance.BorderColor =
                btnSync.FlatAppearance.BorderColor =
                btnAddGame.FlatAppearance.BorderColor =
                btnApolloOnAir.FlatAppearance.BorderColor =
                btnDownload.FlatAppearance.BorderColor =
                btnLoadPlaylist.FlatAppearance.BorderColor =
                btnWebsite.FlatAppearance.BorderColor =
                btnMove.FlatAppearance.BorderColor =
                mediaPlayer.accentColour;
            btnRevealedRadio.FlatAppearance.MouseDownBackColor =
                btnSyncAnalyse.FlatAppearance.MouseDownBackColor =
                btnSyncExport.FlatAppearance.MouseDownBackColor =
                btnSyncAutomate.FlatAppearance.MouseDownBackColor =
                btnSync.FlatAppearance.MouseDownBackColor =
                btnAddGame.FlatAppearance.MouseDownBackColor =
                btnApolloOnAir.FlatAppearance.MouseDownBackColor =
                btnDownload.FlatAppearance.MouseDownBackColor =
                btnLoadPlaylist.FlatAppearance.MouseDownBackColor =
                btnWebsite.FlatAppearance.MouseDownBackColor =
                btnMove.FlatAppearance.MouseDownBackColor =
                mediaPlayer.accentColour;
            /*
            BindingSource listBoxBindingSource = new BindingSource();
            listBoxBindingSource.DataSource = mediaPlayer;
            listBoxBindingSource.DataMember = "accentColour";
            btnWebsite.DataBindings.Add("FlatAppearance.BorderColor", listBoxBindingSource, "accentColour");
            */
            this.Refresh();
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            var ps = new ProcessStartInfo("http://localhost:8080/")
            {
                UseShellExecute = true,
                Verb = "open"
            };
            System.Diagnostics.Process.Start(ps);
        }

        private void btnLoadPlaylist_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                mediaPlayer.loadPlaylist(fbd.SelectedPath);
            }
        }

        private void ml_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void ml_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var dir = files[0];

            if (File.Exists(dir))
                dir = Path.GetDirectoryName(dir);

            mediaPlayer.loadPlaylist(dir);
        }

        private void btnRevealedRadio_Click(object sender, EventArgs e)
        {
            PlayerManager.launchRevealedRadio();
        }

        private void btnApolloOnAir_Click(object sender, EventArgs e)
        {
            PlayerManager.launchApolloOnAir();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            YoutubeDownloader youtubeDownloader = new YoutubeDownloader(youtubeSyncer);
            youtubeDownloader.Show();
        }

        private void tbControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlHead_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGameLauncher_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Executables (*.exe)|*.exe";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                GameAdder gameAdder = new GameAdder(ofd.FileName);
                gameAdder.Show();
            }

            /*GameChecker gameChecker = new GameChecker();

            GameLauncher gameLauncher = new GameLauncher(gameChecker, server);
            gameLauncher.Show();*/
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            youtubeSyncer.sync();
        }

        private void cleanUp()
        {
            if (Directory.Exists(AppContext.BaseDirectory + "spotify"))
            {
                Directory.Delete(AppContext.BaseDirectory + "spotify", true);
            }
        }

        private void ml_FormClosing(object sender, FormClosingEventArgs e)
        {
            cleanUp();

            var loc = AppDomain.CurrentDomain.BaseDirectory;
            var parent = new DirectoryInfo(loc).Parent;

            if (parent.Name == "reAudioPlayer Apollo")
            {
                string updater = parent.FullName + "\\Updater\\Updater.exe";
                Process.Start(updater);
            }
        }

        private void prgVolume_Scroll(object sender, ScrollEventArgs e)
        {
            PlayerManager.volume = e.NewValue;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSyncAnalyse_Click(object sender, EventArgs e)
        {
            var playlist = cmbSyncPlaylist.Text;
            Task.Factory.StartNew(() => spotify.syncPlaylistByName(playlist));
        }

        private void btnSyncExport_Click(object sender, EventArgs e)
        {
            spotify.export();
        }

        private void keyYoutube_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.youtubeKey = keyYoutube.Text;
        }

        private void keySpotifyID_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.spotifyId = keySpotifyID.Text;
        }

        private void keySpotifySecret_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.spotifySecret = keySpotifySecret.Text;
        }

        private void keyIGDBId_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.igdbId = keyIGDBId.Text;
        }

        private void keyIGDBSecret_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.igdbSecret = keyIGDBSecret.Text;
        }

        private void keyTMDB_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.tmdbKey = keyTMDB.Text;
        }
    }
}
