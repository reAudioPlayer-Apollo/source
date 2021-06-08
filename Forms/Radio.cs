using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class Radio : Form
    {
        Logger logger;
        private readonly System.Windows.Media.MediaPlayer player = new System.Windows.Media.MediaPlayer();

        public Dictionary<string, List<string>> programmes = new Dictionary<string, List<string>>();
        string currentProgramme;
        DateTime programmeLoaded;
        DateTime programmeEnd;
        GameChecker gameChecker = new GameChecker();
        bool skipProgramme = false;
        string forceProgramme = null;

        public string displayname { get { return $"{lblTitle.Text} ({lblProgramme.Text})"; } }

        public int volume
        {
            get
            {
                return prgVolume.Value;
            }

            set
            {
                prgVolume.Invoke(new Action(() =>
                {
                    prgVolume.Value = value;
                }));
            }
        }

        public void playPause()
        {
            player.Dispatcher.Invoke(new Action(() =>
            {
                player.IsMuted = !player.IsMuted;
            }));
        }

        public bool isPlaying
        {
            get
            {
                bool ret = false;

                player.Dispatcher.Invoke(new Action(() =>
                {
                    ret = !player.IsMuted;
                }));

                return ret;
            }
        }

        public Radio(Logger l)
        {
            logger = l;
            InitializeComponent();
            player.MediaEnded += Player_MediaEnded;
            player.MediaOpened += Player_MediaOpened;
            Debug.WriteLine("Session Created");
            this.Show();
            gameChecker.GameStarted += GameChecker_GameStarted;
        }

        private void GameChecker_GameStarted(object sender, string e)
        {
            var suggestedPlaylist = gameChecker.getSuggestedPlaylist(logger, e);
            reAudioPlayer.ShowBalloonTip(2000, "Game Launch Detected!", $"You are playing '{e}', do you want to listen to '{suggestedPlaylist}'", ToolTipIcon.Info);

            suggestedPlaylist = suggestedPlaylist.Split('\\')[suggestedPlaylist.Split('\\').Length - 1];
            Debug.WriteLine(suggestedPlaylist);

            if (programmes.Count == 0 || suggestedPlaylist != currentProgramme && (currentProgramme == null || programmes.ContainsKey(currentProgramme)))
            {
                skipProgramme = true;
                forceProgramme = suggestedPlaylist;
            }
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            player.Play();
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            programmes[currentProgramme].RemoveAt(0);
            loadNextSong();
        }

        private void Radio_Load(object sender, EventArgs e)
        {
            loadNextSong();
        }

        private void loadNextSong()
        {
            if (currentProgramme == null || !programmes.ContainsKey(currentProgramme) || DateTime.Now > programmeEnd || skipProgramme)
            {
                loadNextProgramme();
            }

            if (programmes.Count > 0)
            {
                if (!programmes.ContainsKey(currentProgramme is null ? "" : currentProgramme))
                {
                    return;
                }

                string song = programmes[currentProgramme][0];
                try
                {
                    TagLib.File tag = TagLib.File.Create(song);
                    lblArtist.Text = tag.Tag.FirstPerformer is null ? "N/A" : tag.Tag.FirstPerformer.ToUpper();
                    lblTitle.Text = tag.Tag.Title is null ? "N/A" : tag.Tag.Title.ToUpper();
                    pictureBox1.Image = MediaPlayer.GetCover(tag);
                }
                catch
                {
                    lblArtist.Text = "N/A";
                    lblTitle.Text = "N/A";
                    pictureBox1.Image = null;
                }
                player.Open(new Uri(song));
            }
        }

        public void loadNextProgramme()
        {
            skipProgramme = false;

            if (currentProgramme != null && programmes.ContainsKey(currentProgramme))
            {
                programmes.Remove( currentProgramme );
            }

            if (programmes.Count == 0)
            {
                Task.Factory.StartNew(() => createSession()).ContinueWith((task) =>
                {
                    lblProgramme.Invoke(new Action(() => {
                        Random randd = new Random();
                        
                        programmeLoaded = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                        programmeEnd = programmeLoaded.AddHours(1);

                        currentProgramme = programmes.ElementAt(randd.Next(programmes.Count)).Key;

                        if (forceProgramme != null)
                        {
                            currentProgramme = forceProgramme;
                            forceProgramme = null;
                            lblProgramme.Text = "★ " + currentProgramme;
                        }
                        else
                        {
                            lblProgramme.Text = currentProgramme;
                        }
                        lblProgramme.Text = lblProgramme.Text.ToUpper();
                        lblTime.Text = $"NOW PLAYING | {programmeLoaded.ToString("htt")} - {programmeEnd.ToString("htt")}";
                        
                        string song = programmes[currentProgramme][0];
                        TagLib.File tag = TagLib.File.Create(song);
                        lblArtist.Text = tag.Tag.FirstPerformer.ToUpper();
                        lblTitle.Text = tag.Tag.Title.ToUpper();
                        pictureBox1.Image = MediaPlayer.GetCover(tag);
                        player.Open(new Uri(song));
                    }));
                });

                return;
            }

            Random rand = new Random();

            programmeLoaded = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            programmeEnd = programmeLoaded.AddHours(1);

            currentProgramme = programmes.ElementAt(rand.Next(programmes.Count)).Key;

            if (forceProgramme != null)
            {
                lblProgramme.Invoke(new Action(() =>
                {
                    currentProgramme = forceProgramme;
                    forceProgramme = null;
                    lblProgramme.Text = "★ " + currentProgramme;
                }));
            }
            else
            {
                lblProgramme.Invoke(new Action(() =>
                {
                    lblProgramme.Text = currentProgramme;
                }));
            }
            lblTime.Text = $"NOW PLAYING | {programmeLoaded.ToString("htt")} - {programmeEnd.ToString("htt")}";
        }

        public List<string> readPlaylists()
        {
            var playlists = File.ReadAllLines(logger.playlistLib);

            for (var i = 0; i < playlists.Length; i++)
            {
                playlists[i] = new DirectoryInfo(playlists[i]).FullName;
            }

            return new List<string>(playlists);
        }

        public List<string> readSongs(string pl)
        {
            List<string> x = new List<string>();

            DirectoryInfo d = new DirectoryInfo(pl);

            logger.addPlaylistToDB(pl);

            foreach (FileInfo file in d.GetFiles("*.*"))
            {
                if (FileManager.isSupported(file.FullName))
                    x.Add(file.FullName);
            }

            return x;
        }

        private void createSession()
        {
            var playlists = readPlaylists();

            while (playlists.Count > 0)
            {
                Random r = new Random();
                int i = r.Next(playlists.Count);
                string playlist = playlists[i];
                lock (playlists)
                {
                    playlists.RemoveAt(i);
                }

                var songs = readSongs(playlist);

                if (songs.Count > 0)
                {
                    int hourInSec = 60 * 60;
                    int duration = 0;

                    var programme = new List<string>();

                    while (duration < hourInSec)
                    {
                        r = new Random();
                        if (songs.Count > 1)
                            i = r.Next(songs.Count - 1);
                        else if (songs.Count == 0)
                            songs = readSongs(playlist);
                        else
                            i = 0;

                        string song = songs[i];
                        songs.RemoveAt(i);

                        programme.Add(song);

                        System.Windows.Media.MediaPlayer m = new System.Windows.Media.MediaPlayer();
                        m.Open(new Uri(song));
                        while (!m.NaturalDuration.HasTimeSpan) ;
                        duration += (int)m.NaturalDuration.TimeSpan.TotalSeconds;
                        m.Close();
                    }

                    lock (programmes)
                    {
                        string key = new FileInfo(playlist).Name;

                        if (!programmes.ContainsKey(key))
                        {
                            programmes.Add(key, programme);
                        }
                    }
                }
            }
        }

        private void Radio_FormClosed(object sender, FormClosedEventArgs e)
        {
            player.Stop();
        }

        bool spinnerDirUp = true;

        private void tmrSpinner_Tick(object sender, EventArgs e)
        {
            if (metroProgressSpinner1.Value >= 80)
                spinnerDirUp = false;
            if (metroProgressSpinner1.Value <= 0)
                spinnerDirUp = true;

            if (spinnerDirUp)
            {
                metroProgressSpinner1.Value += 1;
            }
            else
            {
                metroProgressSpinner1.Value -= 1;
            }
        }

        public void next()
        {
            loadNextProgramme();
            loadNextSong();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            next();
        }

        private void prgVolume_ValueChanged(object sender, EventArgs e)
        {
            player.Volume = prgVolume.Value / 100.0;
        }
    }
}
