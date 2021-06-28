using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class ml : Form
    {
        private readonly Logger logger = new Logger();
        private MediaPlayer mediaPlayer;
        private readonly YoutubeSyncer youtubeSyncer = new YoutubeSyncer();
        private HotkeyManager hotkeyManager;
        private HttpServer.HttpWebServer server;
        private readonly string[] args;

        public void cacheStates()
        {
            foreach (ListViewItem item in listView1.Items)
            {
                spotifyCache.Add(item);
            }
        }

        List<ListViewItem> spotifyCache = new List<ListViewItem>();

        public void init()
        {
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

            //Classes.Localiser.SetCulture("de");

            tbControl.SelectTab(1);

            mediaPlayer.linkVolume(prgVolume);
            mediaPlayer.linkPlayPauseButton(btnPlayPause);
            mediaPlayer.linkTrackbar(prgTimeDone);
            mediaPlayer.linkTimeLabels(lblTimeDone, lblTimeUp);
            mediaPlayer.linkUpNowLabels(lblUpNowTitle, lblUpNowArtist);
            mediaPlayer.linkCover(imgCover);

            imgQR.Image = QRCodeForWebClient.getImage();

            if (spotifyCache.Count > 0)
            {
                listView1.Items.Clear();
                foreach (var item in spotifyCache)
                {
                    listView1.Items.Add(item.Clone() as ListViewItem);
                }
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }

            Logger.txtLogger = txtLogs;
            YoutubeSyncer.txtLogger = txtLogs;
        }

        public ml(string[] iArgs)
        {
            //Debugger.Launch();

            args = iArgs;

            PlayerManager.mediaPlayer = mediaPlayer = new MediaPlayer(logger, notifyIcon);

            init();

            server = new HttpServer.HttpWebServer(mediaPlayer, logger, prgVolume, args, forceServer: true);
            hotkeyManager = new HotkeyManager(mediaPlayer, this.prgVolume);

            PlayerManager.logger = PlaylistManager.logger = logger;
            PlaylistManager.AutoPlaylists.updateSpecialPlaylists();

            GameChecker gameChecker = new GameChecker();
            GameLibraryManager.Initialise(gameChecker, server);

            Task.Factory.StartNew(() => updateUpdater());

            keyYoutube.Text = Settings.APIKeys.youtube;
            keyIGDBId.Text = Settings.APIKeys.igdb.id;
            keyIGDBSecret.Text = Settings.APIKeys.igdb.secret;
            keySpotifyID.Text = Settings.APIKeys.spotify.id;
            keySpotifySecret.Text = Settings.APIKeys.spotify.secret;
            keyTMDB.Text = Settings.APIKeys.tmdb;

            if (!Settings.APIKeys.spotify.isSet)
            {
                Search.Spotify.UIHandler.releaseView = listView1;
                Search.Spotify.UIHandler.syncView = lviewSpotifySync;

                Search.Spotify.UIHandler.ctxRelease = spotifyContextMenu;
                Search.Spotify.UIHandler.ctxSync = spotifySyncContextMenu;

                Search.Spotify.UIHandler.txtArtistDelimiter = txtExportArtistDelimiter;
                Search.Spotify.UIHandler.txtSyncIn = txtLocalInput;
                Search.Spotify.UIHandler.txtSyncOut = txtSyncOut;

                Search.Spotify.UIHandler.cmbSyncPlaylist = cmbSyncPlaylist;

                Search.Spotify.UIHandler.lblSyncProgress = lblSyncProgress;

                Search.Spotify.UIHandler.notifyIcon = notifyIcon;
                Search.Spotify.UIHandler.SetBuffers();

                Search.Spotify.Init.logger = logger;

                Search.Spotify.Init.AuthoriseUser();
            }

            Logger.Log("Initialised");
        }

        private void updateUpdater()
        {
            string loc = AppDomain.CurrentDomain.BaseDirectory;
            string parent = new DirectoryInfo(loc).Parent.Name;

            if (parent == "reAudioPlayer Apollo")
            {
                using (Repository repo = new Repository("../Updater"))
                {
                    Signature sig = new Signature("test", "test@test.com", new DateTimeOffset(DateTime.Now));
                    PullOptions opt = new PullOptions();
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
            Properties.Settings.Default.virtualPlaylists = "";
            Properties.Settings.Default.Save();

            string dir = "";

            if (args.Length > 0)
            {
                dir = args[0];

                mediaPlayer.loadPlaylist(dir);
            }

            Task.Factory.StartNew(() => youtubeSyncer.sync());
            HttpServer.API.Static.syncer = youtubeSyncer;
            cleanUp();
            Directory.CreateDirectory(AppContext.BaseDirectory + "spotify");

            Logger.Log("Loaded");
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
            {
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Path.GetFileName(ofd.FileName);
            sfd.InitialDirectory = ofd.InitialDirectory;

            string key = Path.GetDirectoryName(ofd.FileName).Replace(@"\\", @"\");

            if (logger.moveTrainerDictionary.ContainsKey(key))
            {
                sfd.InitialDirectory = logger.moveTrainerDictionary[Path.GetDirectoryName(ofd.FileName)];
            }

            sfd.Title = "choose the new location";
            string ext = Path.GetExtension(ofd.FileName);
            sfd.Filter = $"{ext.Replace(".", "")} files (*{ext})|*{ext}";

            if (!(sfd.ShowDialog() == DialogResult.OK))
            {
                return;
            }

            logger.addFileMove(Path.GetDirectoryName(ofd.FileName), Path.GetDirectoryName(sfd.FileName));
            File.Move(ofd.FileName, sfd.FileName);
        }

        private void tmrAccentColour_Tick(object sender, EventArgs e)
        {
            btnRevealedRadio.FlatAppearance.BorderColor =
                btnLoadIndependentAsPlaylist.FlatAppearance.BorderColor =
                btnSyncLocalToSpotify.FlatAppearance.BorderColor =
                btnManualAcrop.FlatAppearance.BorderColor =
                btnSyncAnalyse.FlatAppearance.BorderColor =
                btnSyncExport.FlatAppearance.BorderColor =
                btnSync.FlatAppearance.BorderColor =
                btnAddGame.FlatAppearance.BorderColor =
                btnApolloOnAir.FlatAppearance.BorderColor =
                btnDownload.FlatAppearance.BorderColor =
                btnLoadPlaylist.FlatAppearance.BorderColor =
                btnWebsite.FlatAppearance.BorderColor =
                btnMove.FlatAppearance.BorderColor =
                mediaPlayer.accentColour;
            btnRevealedRadio.FlatAppearance.MouseDownBackColor =
                btnLoadIndependentAsPlaylist.FlatAppearance.MouseDownBackColor =
                btnSyncLocalToSpotify.FlatAppearance.MouseDownBackColor =
                btnManualAcrop.FlatAppearance.MouseDownBackColor =
                btnSyncAnalyse.FlatAppearance.MouseDownBackColor =
                btnSyncExport.FlatAppearance.MouseDownBackColor =
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
            try
            {
                this.Invoke(new Action(() => this.Refresh()));
            } catch { }
        }

        private void btnWebsite_Click(object sender, EventArgs e)
        {
            ProcessStartInfo ps = new ProcessStartInfo("http://localhost:8080/")
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
            string dir = files[0];


            if (File.Exists(dir))
            {
                mediaPlayer.playIndependent(dir);
            }
            else
            {
                mediaPlayer.loadPlaylist(dir);
            }
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

            /*
            for testing purpose
            GameChecker gameChecker = new GameChecker();
            GameLauncher gameLauncher = new GameLauncher(gameChecker, server);
            gameLauncher.Show();
            */
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

            string loc = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo parent = new DirectoryInfo(loc).Parent;

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
            string playlist = cmbSyncPlaylist.Text;
            Task.Factory.StartNew(() => Search.Spotify.Synchronise.SpotifyToLocalByName(playlist));
        }

        private void btnSyncExport_Click(object sender, EventArgs e) { Search.Spotify.Synchronise.ExportSyncedPlaylist(); }

        private void keyYoutube_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.youtube = keyYoutube.Text;
        }

        private void keySpotifyID_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.spotify.id = keySpotifyID.Text;
        }

        private void keySpotifySecret_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.spotify.secret = keySpotifySecret.Text;
        }

        private void keyIGDBId_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.igdb.id = keyIGDBId.Text;
        }

        private void keyIGDBSecret_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.igdb.secret = keyIGDBSecret.Text;
        }

        private void keyTMDB_TextChanged(object sender, EventArgs e)
        {
            Settings.APIKeys.tmdb = keyTMDB.Text;
        }

        private void btnManualAcrop_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MP3 Files|*.mp3";

            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            new OptimizeDL().acrop(new FileInfo(ofd.FileName));
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            Classes.Localiser.SetCulture(cmbLanguage.Text, this);
        }

        private void btnSyncLocalToSpotify_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => Search.Spotify.Synchronise.LocalToSpotify());
        }

        private void btnLoadIndependentAsPlaylist_Click(object sender, EventArgs e)
        {
            mediaPlayer.loadFromIndependentSong();
        }
    }
}
