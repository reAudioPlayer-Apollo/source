using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public class MediaPlayer
    {
        private readonly System.Windows.Media.MediaPlayer player = new System.Windows.Media.MediaPlayer();
        public List<Song> playlist { get; private set; }
        private int playlistIndex;
        private int nextIndex;
        private int lastIndex;
        private int playlistCount;
        private readonly System.Windows.Forms.Timer tmrBarManager = new System.Windows.Forms.Timer();
        private readonly PausableTimer tmrSongPlayed;
        private MetroFramework.Controls.MetroTrackBar volumeBar;
        private MetroFramework.Controls.MetroTrackBar trackBar;
        private System.Windows.Forms.PictureBox playPauseImg;
        private System.Windows.Forms.Label lblDone, lblUp, lblUpNowTitle, lblUpNowArtist;
        private readonly Logger logger;
        private System.Windows.Forms.PictureBox imgCover;
        //private List<Image> covers = new List<Image>();
        //private List<Image> backgrounds = new List<Image>();
        //private List<System.Drawing.Color> accentColours = new List<System.Drawing.Color>();
        private readonly NotifyIcon notifyIcon;
        private bool mayCancelLoad = false, cancelLoad = false;
        public RevealedStream revealedStream;

        public Song upNow { get; private set; }

        private readonly AccentColour.Finder finder = new AccentColour.Finder();

        public Color accentColour = new System.Drawing.Color();

        public bool isPlaying
        { get; private set; }

        public struct Song
        {
            public string oneLiner;
            public string secondLiner;
            public string artist;
            public string title;
            public string album;
            public string location;
            public Color accentColour;
            public Image cover;
            public Image background;
        }

        public MediaPlayer(Logger log, NotifyIcon notifyIco)
        {
            logger = log;
            notifyIcon = notifyIco;

            player.MediaEnded += Player_MediaEnded;

            tmrSongPlayed = new PausableTimer(30000, TmrSongPlayed_Tick);

            tmrBarManager.Interval = 100;
            tmrBarManager.Tick += TmrBarMgr_Tick;
            tmrBarManager.Start();
            var t = upNow;
            t.oneLiner = "N/A";
            upNow = t;
        }

        public void playIndependent(string filename, string title, string artist)
        {
            lblUpNowArtist.Text = artist;
            lblUpNowTitle.Text = title;

            player.Open(new Uri(filename));
            play();
        }

        public void linkCover(System.Windows.Forms.PictureBox cover)
        {
            imgCover = cover;
        }

        private async Task<bool> loadCover()
        {
            bool returnv = true;

            lock (playlist)
            {
                imgCover.Invoke(new Action(() =>
                {
                    if (playlist.Count > playlistIndex && playlistIndex >= 0)
                    {
                        imgCover.Image = playlist[playlistIndex].cover;
                        if (playlist[playlistIndex].cover is null)
                        {
                            imgCover.BackgroundImage = null;
                            returnv = false;

                            accentColour = System.Drawing.Color.Black;
                        }
                        else
                        {
                            //playlist[playlistIndex].cover.Save("ressources\\cover.jpg");
                            PlayerManager.cover = playlist[playlistIndex].cover.Clone() as Image;
                            imgCover.BackgroundImage = playlist[playlistIndex].background;

                            accentColour = playlist[playlistIndex].accentColour;
                        }
                    }

                }));
            }

            return returnv;
        }

        private Image getCover(string file)
        {
            try
            {
                TagLib.File tag = TagLib.File.Create(file);
                MemoryStream stream = new MemoryStream(tag.Tag.Pictures[0].Data.Data);
                return Image.FromStream(stream);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void TmrSongPlayed_Tick(object sender, EventArgs e)
        {
            logger.addPlayedSong(playlist[playlistIndex].location);
        }

        private void TmrBarMgr_Tick(object sender, EventArgs e)
        {
            player.Volume = volumeBar.Value / 100.0;

            if (player.NaturalDuration.HasTimeSpan)
            {
                TimeSpan remainingTime = player.NaturalDuration.TimeSpan - player.Position;

                lblDone.Text = player.Position.ToString("mm':'ss");
                lblUp.Text = remainingTime.ToString("'-'mm':'ss");

                trackBar.Value = toTrackBarScale();
            }
            else
            {
                lblDone.Text = "N/A";
                lblUp.Text = "N/A";
                trackBar.Value = 0;
            }
        }

        public void jumpTo(int position)
        {
            player.Position = new TimeSpan(0, 0, 0, 0, fromTrackBarScale(position));
        }

        public void next()
        {
            loadNext();
        }

        public void last()
        {
            loadSong(lastIndex);
        }

        private int toTrackBarScale()
        {
            double totMs = player.NaturalDuration.TimeSpan.TotalMilliseconds;
            double posMs = player.Position.TotalMilliseconds;

            int r = (int)Math.Round((posMs * 1000.0) / totMs);

            if (r > 1000)
                r = 1000;

            return r;
        }

        private int fromTrackBarScale(int position)
        {
            try
            {
                double totMs = player.NaturalDuration.TimeSpan.TotalMilliseconds;

                return (int)Math.Round((position * totMs) / 1000.0);
            }
            catch
            {
                return 0;
            }
        }

        public void linkUpNowLabels(Label title, Label artist)
        {
            lblUpNowTitle = title;
            lblUpNowArtist = artist;
        }

        public void linkVolume(MetroFramework.Controls.MetroTrackBar bar)
        {
            volumeBar = bar;
        }

        public void linkPlayPauseButton(System.Windows.Forms.PictureBox btnPlayPause)
        {
            playPauseImg = btnPlayPause;
        }

        public void linkTrackbar(MetroFramework.Controls.MetroTrackBar bar)
        {
            trackBar = bar;
        }

        public void linkTimeLabels(System.Windows.Forms.Label done, System.Windows.Forms.Label left)
        {
            lblDone = done;
            lblUp = left;
        }

        public void start()
        {
            loadSong(playlist[0].location);
        }

        private Song getSong(string filename)
        {
            Song song = new Song();
            TagLib.File tagfile = TagLib.File.Create(filename);
            song.artist = tagfile.Tag.FirstPerformer;
            song.title = tagfile.Tag.Title;
            song.album = tagfile.Tag.Album;
            song.location = filename;

            song.oneLiner = $"{song.artist} - {song.title}";
            song.secondLiner = $"{song.artist} - {song.album}";

            return song;
        }

        public async void loadSong(string filename, bool autoplay = true)
        {
            try
            {
                player.Dispatcher.Invoke(new Action(() =>
                {
                    IEnumerable<Song> structResults = playlist.Where(a => a.location == filename);
                    playlistIndex = playlist.IndexOf(structResults.First());
                    player.Open(new Uri(filename));
                    upNow = getSong(filename);
                    lblUpNowTitle.Text = upNow.title;
                    lblUpNowArtist.Text = $"{upNow.artist} - [{filename}]";
                }));

                if (autoplay)
                    play();

                try
                {
                    await loadCover();
                }
                catch { }

                tmrSongPlayed.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void loadSong(int index, bool autoplay = true)
        {
            loadSong(playlist[index].location, autoplay);
        }

        private void loadNext()
        {
            if (playlist is null)
            {
                return;
            }

            loadSong(playlist[nextIndex].location);
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            loadNext();
        }

        public int getNextIndex()
        {
            int i = playlistIndex + 1;

            if (i >= playlistCount)
                i = 0;

            nextIndex = i;
            return i;
        }

        public int getLastIndex()
        {
            int i = playlistIndex - 1;

            if (i < 0)
                i = playlistCount - 1;

            lastIndex = i;
            return i;
        }

        public void loadPlaylist(string pl, bool autoplay = false)
        {
            List<string> x = PlaylistManager.getSongPathsAsStrings(pl);

            logger.addPlaylistToDB(pl);

            loadPlaylist(x, autoplay);
        }

        public void loadPlaylist(List<string> pl, bool autoplay = false)
        {
            playlistIndex = 0;
            playlist = new List<Song>();

            foreach (var f in pl)
            {
                playlist.Add(getSong(f));
            }

            playlistCount = playlist.Count;

            cancelLoad = mayCancelLoad;
            mayCancelLoad = true;

            while (cancelLoad) ;

            

            Task.Run(() => loadCovers());
            loadSong(0, autoplay);
        }

        private async Task loadCovers()
        {
            for (int i = 0; i < playlist.Count; i++)
            {
                if (cancelLoad)
                {
                    cancelLoad = false;
                    return;
                }

                Image cover = getCover(playlist[i].location);
                
                lock (playlist)
                {
                    Song ttt = playlist[i];
                    ttt.cover = cover;
                    playlist[i] = ttt;
                };
            }

            Task t = Task.Run(() => loadBackgrounds());

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(2000);
            }

            try
            {
                await loadCover();
            }
            catch { }

            await t;

            mayCancelLoad = false;
            notifyIcon.ShowBalloonTip(2000, "reAudioPlayer", "Playlist loaded!", ToolTipIcon.Info);
        }

        private async Task loadBackgrounds()
        {
            for (int i = 0; i < playlist.Count; i++)
            {
                if (cancelLoad)
                {
                    cancelLoad = false;
                    return;
                }

                if (playlist[i].cover is null)
                {
                    var t = playlist[i];
                    t.accentColour = Color.Black;
                    t.background = null;
                    playlist[i] = t;
                }
                else
                {
                    Color colour = await getAccentColour(playlist[i].cover, 1);

                    Bitmap bm = getBackground(colour);
                    Song ttt = playlist[i];

                    lock (playlist)
                    {
                        ttt.accentColour = colour;
                        ttt.background = bm;
                        playlist[i] = ttt;
                    }
                }
            }
        }

        public void play()
        {
            player.Dispatcher.Invoke(new Action(() =>
            {
                PlayerManager.resumeMusic();

                playPauseImg.Image = Properties.Resources.pause;
                isPlaying = true;
                player.Play();
                getNextIndex();
                getLastIndex();

                try
                {
                    if (revealedStream != null)
                    {
                        revealedLink = revealedStream.getLink();
                        revealedStream.Close();
                    }
                } catch { }

                tmrSongPlayed.Resume();
            }));
        }

        string revealedLink;

        public bool pause()
        {
            bool ret = false;
            player.Dispatcher.Invoke(new Action(() =>
            {
                if (player.CanPause)
                {
                    playPauseImg.Image = Properties.Resources.play;
                    player.Pause();
                    isPlaying = false;

                    tmrSongPlayed.Pause();

                    try
                    {
                        revealedStream = new RevealedStream(revealedLink);
                        revealedStream.Show();
                    } catch { }

                    ret = true;
                }

                ret = false;
            }));

            return ret;
        }

        public void playPause()
        {
            if (isPlaying)
                pause();
            else
                play();
        }

        private Bitmap getBackground(System.Drawing.Color colour)
        {
            return finder.getColourAsShadowBitmap(colour);
        }

        private async Task<System.Drawing.Color> getAccentColour(Image image, int gap = 2)
        {
            AccentColour.PictureAnalyser piccAnalyser = new AccentColour.PictureAnalyser();
            AccentColour.Finder finderr = new AccentColour.Finder();
            Bitmap bitmap = new Bitmap(image);

            await piccAnalyser.GetMostUsedColor(bitmap, gap);
            List<System.Drawing.Color> mColours = piccAnalyser.TenMostUsedColors;
            List<int> aColours = piccAnalyser.TenMostUsedColorIncidences;

            List<int> indices = finderr.sortList(ref mColours, ref aColours);

            //accentColour = mColours[indices[0]];
            return mColours[indices[0]];
        }

        public string getLoadedSong()
        {
            return playlist[playlistIndex].location;
        }
    }
}
