using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class MediaPlayer
    {
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
            t = t is null ? new Song() : t;
            t.oneLiner = "N/A";
            upNow = t;
        }

        public int nextIndex
        {
            get
            {
                int j = playlistIndex + 1;

                for (int i = 0; i < playlist.Count; i++) // only run once
                {
                    if (j < playlist.Count && !blockList.Contains(j))
                        continue;

                    if (j >= playlist.Count)
                        j = 0;
                    else
                        j++;
                }

                return j;
            }
        }
        public readonly System.Windows.Media.MediaPlayer player = new System.Windows.Media.MediaPlayer();
        public MetroFramework.Controls.MetroTrackBar trackBar;
        public List<Song> playlist { get; private set; }
        public Color accentColour = Color.White;
        public RevealedStream revealedStream;
        public List<int> blockList = new List<int>();
        public Song upNow { get; private set; }

        public int volume
        {
            get
            {
                return volumeBar.Value;
            }
            set
            {
                volumeBar.Invoke(new Action(() =>
                {
                    volumeBar.Value = value;
                }));
            }
        }

        public bool isPlaying
        { get; private set; }

        /// <summary>
        /// on play (either new song or resume)
        /// </summary>
        public event EventHandler Play;

        /// <summary>
        /// on pause
        /// </summary>
        public event EventHandler Pause;

        /// <summary>
        /// on next song in playlist loaded, manually or automatically
        /// true: manually, false: automatically
        /// </summary>
        public event EventHandler<bool> Next;

        /// <summary>
        /// on last song in playlist loaded, can only happen manually
        /// </summary>
        public event EventHandler Last;

        /// <summary>
        /// on last OR next song loaded, manually or automatically
        /// true: manually, false: automatically
        /// </summary>
        public event EventHandler<bool> Skip;

        /// <summary>
        /// position change
        /// int: new position in 1/1000
        /// </summary>
        public event EventHandler<int> Jump;

        public class BasicSong
        {
            public string artist;
            public string title;
            public string location;
            public string coverUri;
            public int id;

            public static BasicSong[] ConvertFromSongList(Song[] bar)
            {
                List<BasicSong> l = new List<BasicSong>();

                foreach (var s in bar)
                {
                    l.Add(new BasicSong() { artist = s.artist, title = s.title, location = s.location, coverUri = s.coverUri, id = s.id });
                }

                return l.ToArray();
            }

            public static BasicSong[] ConvertFromSongList(SimpleSong[] bar)
            {
                List<BasicSong> l = new List<BasicSong>();

                foreach (var s in bar)
                {
                    l.Add(new BasicSong() { artist = s.artist, title = s.title, location = s.location, coverUri = s.coverUri, id = s.id });
                }

                return l.ToArray();
            }
        }

        public class SimpleSong : BasicSong
        {
            public string oneLiner;
            public string secondLiner;
            public string album;
            public Color accentColour;
            public int index;
            public Search.Spotify.Synchronise.SpotifyComment info;
            public int playCount
            {
                get
                {
                    return Logger.GetPlayCount(location);
                }
            }

            public DateTime creationTime
            {
                get
                {
                    return new FileInfo(location).CreationTime;
                }
            }

            public string keywords
            {
                get
                {
                    return $"{title} {artist} {album} pop:{info.popularity} nrg:{info.energy} dnc:{info.danceability} " +
                        $"hap:{info.happiness} loud:{info.loudness} acc:{info.accousticness} inst:{info.instrumentalness} " +
                        $"live:{info.liveness} spe:{info.speechiness} key:{info.key} dat:{info.releaseDate}";
                }
            }

            public SimpleSong() { }
            public SimpleSong(Song song, string specialCoverUriSuffix = "")
            {
                oneLiner = song.oneLiner;
                secondLiner = song.secondLiner;
                artist = song.artist;
                title = song.title;
                album = song.album;
                location = song.location;
                accentColour = song.accentColour;
                index = song.index;
                info = song.info;
                id = song.id;

                if (song.cover is not null || specialCoverUriSuffix == "&global")
                {
                    if (ip is null)
                    {
                        ip = QRCodeForWebClient.GetPhysicalIPAdress();
                    }
                    coverUri = $"http://{ip}:8080/api/data/cover/{id}{specialCoverUriSuffix}"; ;
                }
            }

            public static string ToString(Song song)
            {
                return JsonConvert.SerializeObject(new SimpleSong(song));
            }

            public static string ToString(SimpleSong song)
            {
                return JsonConvert.SerializeObject(song);
            }

            public static SimpleSong[] ConvertList(Song[] songs, string specialCoverUriSuffix = "")
            {
                List<SimpleSong> ret = new List<SimpleSong>();

                foreach (var song in songs)
                {
                    ret.Add(new SimpleSong(song, specialCoverUriSuffix));
                }

                return ret.ToArray();
            }

            public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Song : SimpleSong
        {
            public Image cover;
            public Image background;
            public AutoRating autoRating;
            private SpotifyAPI.Web.FullTrack _cachedSpotifyEqual;

            public SpotifyAPI.Web.FullTrack spotifyEqual
            {
                get
                {
                    if (_cachedSpotifyEqual is null)
                    {
                        _cachedSpotifyEqual = Search.Spotify.Synchronise.getMatchingSpotifySong(location);
                    }

                    return _cachedSpotifyEqual;
                }
            }

            public Song() { }

            public Song(BasicSong song)
            {
                if (song is null)
                {
                    return;
                }

                artist = song.artist;
                title = song.title;
                id = song.id;
                location = song.location;
                coverUri = song.coverUri;
            }

            public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public static string ToString(Song[] songs, bool asSimpleSong = true, string specialCoverUriSuffix = "")
            {
                if (asSimpleSong)
                {
                    return JsonConvert.SerializeObject(ConvertList(songs, specialCoverUriSuffix));
                }
                else
                {
                    return JsonConvert.SerializeObject(songs);
                }
            }

            public byte[] getCoverBytes(bool updateCover = false)
            {
                if (updateCover)
                {
                    cover = GetCover(location);
                }

                if (cover is null)
                {
                    return new byte[0];
                }

                MemoryStream m = new MemoryStream();
                cover.Save(m, cover.RawFormat);
                return m.ToArray();
            }

            public string getBase64Uri()
            {
                using (MemoryStream m = new MemoryStream())
                {
                    try
                    {
                        cover.Save(m, cover.RawFormat);
                        return "data:image/"
                                + cover.RawFormat.ToString()
                                + ";base64,"
                                + Convert.ToBase64String(m.ToArray()) + "\"";
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }

        public enum OrderBy
        {
            Artist,
            Title,
            Album,
            ReleaseDate,
            CreationDate,
            Filename,
            Popularity,
            Energy,
            Danceability,
            Happiness,
            Loudness,
            Accousticness,
            Instrumentalness,
            Liveness,
            Speechiness,
            Key,
            PlayCount
        }

        /// <summary>
        /// sorts the playlist
        /// </summary>
        /// <param name="orderBy">order as string (case ignored)</param>
        public void sort(string orderBy)
        {
            try
            {
                OrderBy eOrderBy = (OrderBy)Enum.Parse(typeof(OrderBy), orderBy, true);
                sort(eOrderBy);
            }
            catch { }
        }

        /// <inheritdoc cref="sort(string)"/>
        /// <param name="orderBy">order</param>
        public void sort(OrderBy orderBy = OrderBy.Artist)
        {
            switch (orderBy)
            {
                case OrderBy.Artist:
                    playlist = playlist.OrderBy(x => x.artist).ToList();
                    break;

                case OrderBy.Title:
                    playlist = playlist.OrderBy(x => x.title).ToList();
                    break;

                case OrderBy.Album:
                    playlist = playlist.OrderBy(x => x.album).ToList();
                    break;

                case OrderBy.ReleaseDate:
                    playlist = playlist.OrderByDescending(x => x.info.releaseDate).ToList();
                    break;

                case OrderBy.CreationDate:
                    playlist = playlist.OrderByDescending(x => x.creationTime).ToList();
                    break;

                case OrderBy.Filename:
                    playlist = playlist.OrderBy(x => Path.GetFileNameWithoutExtension(x.location)).ToList();
                    break;

                case OrderBy.Popularity:
                    playlist = playlist.OrderByDescending(x => x.info.popularity).ToList();
                    break;

                case OrderBy.Energy:
                    playlist = playlist.OrderByDescending(x => x.info.energy).ToList();
                    break;

                case OrderBy.Danceability:
                    playlist = playlist.OrderByDescending(x => x.info.danceability).ToList();
                    break;

                case OrderBy.Happiness:
                    playlist = playlist.OrderByDescending(x => x.info.happiness).ToList();
                    break;

                case OrderBy.Loudness:
                    playlist = playlist.OrderByDescending(x => x.info.loudness).ToList();
                    break;

                case OrderBy.Accousticness:
                    playlist = playlist.OrderByDescending(x => x.info.accousticness).ToList();
                    break;

                case OrderBy.Liveness:
                    playlist = playlist.OrderByDescending(x => x.info.liveness).ToList();
                    break;

                case OrderBy.Speechiness:
                    playlist = playlist.OrderByDescending(x => x.info.speechiness).ToList();
                    break;

                case OrderBy.Key:
                    playlist = playlist.OrderBy(x => x.info.key).ToList();
                    break;

                case OrderBy.PlayCount:
                    playlist = playlist.OrderByDescending(x => x.playCount).ToList();
                    break;
            }

            for (int i = 0; i < playlist.Count; i++)
            {
                playlist[i].index = i;
            }
        }

        /// <summary>Plays a single song right now, playlist will resume afterwards</summary>
        /// <param name="song">the song to be played</param>
        /// <param name="startPosition">start position in 1/1000, useful for previews</param>
        public void playIndependent(BasicSong song, int startPosition = 0)
        {
            playIndependent(song.location, song.title, song.artist, startPosition);
        }

        /// <inheritdoc cref="playIndependent(BasicSong, int)"/>
        public void playIndependent(SimpleSong song, int startPosition = 0)
        {
            playIndependent(song.location, song.title, song.artist, startPosition);
        }

        /// <inheritdoc cref="playIndependent(SimpleSong, int)"/>
        /// <param name="updateCover">if true, the cover will be loaded or updated</param>
        public void playIndependent(Song song, bool updateCover = false, int startPosition = 0)
        {
            song.cover = updateCover ? GetCover(song.location) : song.cover;
            playIndependent(song.location, song.title, song.artist, song.cover, song.accentColour, startPosition);
        }

        /// <inheritdoc cref="playIndependent(SimpleSong, int)"/>
        /// <param name="filename">the file to be played</param>
        /// <param name="fetchInfo">if true, the cover will be loaded or updated</param>
        public void playIndependent(string filename, bool fetchInfo = true, int startPosition = 0)
        {
            var song = GetSong(filename, getCover: fetchInfo);
            playIndependent(song, startPosition: startPosition);
        }

        /// <inheritdoc cref="playIndependent(SimpleSong, int)"/>
        /// <param name="artist">artist name (to be displayed)</param>
        /// <param name="title">title (to be displayed)</param>
        public void playIndependent(string filename, string title, string artist, int startPosition = 0)
        {
            playIndependent(filename, title, artist, null, null, startPosition);
        }

        /// <inheritdoc cref="playIndependent(SimpleSong, int)"/>
        /// <param name="cover">cover (to be displayed)</param>
        /// <param name="accentColour">accent colour (to be fetched and displayed as background)</param>
        public void playIndependent(string filename, string title, string artist, Image cover, Color accentColour, int startPosition = 0)
        {
            this.accentColour = accentColour;
            playIndependent(filename, title, artist, cover, getBackground(accentColour), startPosition);
        }

        /// <inheritdoc cref="playIndependent(SimpleSong, int)"/>
        /// <param name="background">background (to be displayed)</param>
        public void playIndependent(string filename, string title, string artist, Image cover, Image background, int startPosition = 0)
        {
            lblUpNowArtist.Invoke(new Action(() =>
            {
                lblUpNowArtist.Text = artist;
                lblUpNowTitle.Text = title;
                PlayerManager.webSocket?.broadCastDisplayname();

                imgCover.Image = cover;
                imgCover.BackgroundImage = background;
                PlayerManager.cover = cover is null ? null : cover.Clone() as Image;

                independentSong = filename;
                player.Open(new Uri(filename));
                play();

                if (startPosition > 0) { jumpTo(startPosition); };
            }));
        }

        /// <summary>
        /// loads the playlist associated with the independently loaded (currently playing) song
        /// </summary>
        /// <remarks>Load an independent song with <see cref="playIndependent(Song, bool, int)"/> and load its associated playlist</remarks>
        public void loadFromIndependentSong()
        {
            if (independentSong is not null && File.Exists(independentSong))
            {
                int prg = trackBar.Value;
                var s = independentSong;
                loadPlaylist(Path.GetDirectoryName(independentSong));
                loadSong(s);
                jumpTo(prg);
            }
        }

        /// <summary>jumps to any song position
        /// <example>
        /// <code>
        /// jumpTo(300); // jump to 30%
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="position">target position in 1/1000</param>
        public void jumpTo(int position)
        {
            trackBar.Invoke(new Action(() =>
            {
                player.Position = new TimeSpan(0, 0, 0, 0, fromTrackBarScale(position));
            }));

            if (Jump is not null)
            {
                Jump(new object(), position);
            }
        }

        /// <inheritdoc cref="jumpTo(int)"/>
        /// <param name="position">target position as timespan</param>
        public void jumpTo(TimeSpan position)
        {
            trackBar.Invoke(new Action(() =>
            {
                player.Position = position;
            }));

            if (Jump is not null)
            {
                Jump(new object(), toTrackBarScale(position.TotalMilliseconds));
            }
        }

        public void next()
        {
            loadNext();
        }

        public void last()
        {
            if (playlist is null)
            {
                return;
            }

            if (Last is not null)
            {
                Last(new object(), new EventArgs());
            }

            if (Skip is not null)
            {
                Skip(new object(), true);
            }
            loadSong(lastIndex);
        }

        /// <summary>
        /// attempt to get the song position in ms from a trackbar value (1/1000)
        /// </summary>
        /// <param name="position">position in 1/1000</param>
        /// <returns></returns>
        public int fromTrackBarScale(int position)
        {
            if (playlist is not null && playlist.Count <= 0)
            {
                return 0;
            }

            double totMs = 0;

            try
            {
                trackBar.Invoke(new Action(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (player.NaturalDuration.HasTimeSpan)
                        {
                            totMs = player.NaturalDuration.TimeSpan.TotalMilliseconds;
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                }));

                return (int)Math.Round((position * totMs) / 1000.0);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// loads the very first song
        /// </summary>
        public void start()
        {
            loadSong(playlist[0].location);
        }

        /// <summary>
        /// loads a song from the current playlist
        /// </summary>
        /// <param name="index">playlist index that shall be played</param>
        public void loadSong(int index, bool autoplay = true)
        {
            if (playlist is not null && index >= playlist.Count)
            {
                return;
            }

            loadSong(playlist[index].location, autoplay);
        }

        /// <summary>
        /// loads a song from the current playlist
        /// </summary>
        /// <param name="filename">file from playlist that shall be played</param>
        public async void loadSong(string filename, bool autoplay = true)
        {
            try
            {
                player.Dispatcher.Invoke(new Action(() =>
                {
                    IEnumerable<Song> structResults = playlist.Where(a => a.location == filename);
                    playlistIndex = playlist.IndexOf(structResults.First());
                    independentSong = null;
                    player.Open(new Uri(filename));
                    upNow = playlist[playlistIndex];
                    lblUpNowTitle.Text = upNow.title;
                    lblUpNowArtist.Text = $"{upNow.artist} - [{filename}]";
                    PlayerManager.webSocket?.broadCastDisplayname();

                    if (playlist[playlistIndex].autoRating is null)
                    {
                        playlist[playlistIndex].autoRating = new AutoRating(this, player, upNow.location);
                    }

                    playlist[playlistIndex].autoRating.stats.active = true;
                }));

                _ = Task.Delay(30 * 1000).ContinueWith((t) =>
                  {
                      if (upNow.location == filename)
                      {
                          logger.addSongPlayed(filename);
                      }
                  });

                if (autoplay)
                    play();

                try
                {
                    loadCover();
                }
                catch { }

                tmrSongPlayed.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
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
            playlist = GetPlaylist(pl, logger, true);
            blockList = new List<int>();

            Task.Run(() => updateAccentColours(playlist?.ToArray()));
            loadSong(0, autoplay);
        }

        public void playPause()
        {
            if (isPlaying)
                pause();
            else
                play();
        }

        public void play()
        {
            player.Dispatcher.Invoke(new Action(() =>
            {
                PlayerManager.resumeMusic();

                playPauseImg.Image = Properties.Resources.pause;
                isPlaying = true;
                player.Play();

                try
                {
                    if (revealedStream != null)
                    {
                        revealedLink = revealedStream.getLink();
                        revealedStream.Close();
                    }
                }
                catch { }

                tmrSongPlayed.Resume();
            }));

            if (Play is not null)
            {
                Play(new object(), new EventArgs());
            }
        }

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
                    }
                    catch { }

                    ret = true;
                }

                ret = false;
            }));

            if (Pause is not null)
            {
                Pause(new object(), new EventArgs());
            }

            return ret;
        }

        public string getLoadedSong()
        {
            return playlist[playlistIndex].location;
        }

        /* STATIC */

        /// <summary>
        /// Gets all songs from a given directory
        /// </summary>
        /// <param name="pl">directory to load</param>
        /// <param name="logger">logger object (for databases)</param>
        /// <returns></returns>
        public static List<Song> GetPlaylist(string pl, Logger logger)
        {
            List<string> x = PlaylistManager.getSongPathsAsStrings(pl);

            logger.addPlaylistToDB(pl);

            return GetPlaylist(x, logger);
        }

        /// <inheritdoc cref="GetPlaylist(string, Logger)"/>
        /// <param name="pl">playlist as all file locations</param>
        /// <param name="getCover">if true, the cover will be fetched (slightly slower)</param>
        /// <param name="getAccentColour">if true, the accentColour will be calculated
        /// (rapid mode, still makes the function much slower)</param>
        /// <returns></returns>
        public static List<Song> GetPlaylist(List<string> pl, Logger logger, bool getCover = false, bool getAccentColour = false)
        {
            if (pl is null)
            {
                return null;
            }

            List<Song> t = new List<Song>();

            foreach (var f in pl)
            {
                logger.addSongToDB(f);
                t.Add(GetSong(f, getCover, getAccentColour));
                //t[t.Count - 1].autoRating = new AutoRating();
                t[t.Count - 1].index = pl.IndexOf(f);
            }

            return t;
        }

        public static Image GetCover(TagLib.File file)
        {
            try
            {
                MemoryStream stream = new MemoryStream(file.Tag.Pictures[0].Data.Data);
                return Image.FromStream(stream);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Image GetCover(string file)
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

        /// <summary>
        /// gets a song
        /// </summary>
        /// <param name="filename">song file (ideally an mp3)</param>
        /// <param name="getCover">if true, the cover will be fetched (slightly slower)</param>
        /// <param name="getAccentColour">if true, the accentColour will be calculated
        /// (rapid mode, still makes the function much slower)</param>
        /// <param name="accentColourCache">Cached accentColours. If getAccentColour is false, the function attempts
        /// to get the accentColour from a customisable cache.
        /// if null, static cache will be used.</param>
        /// <returns></returns>
        public static Song GetSong(string filename, bool getCover = false, bool getAccentColour = false, Dictionary<string, string> accentColourCache = null)
        {
            if (!File.Exists(filename))
            {
                return new Song();
            }

            if (accentColourCache is null)
            {
                accentColourCache = MediaPlayer.accentColourCache;
            }

            Song song = new Song();
            TagLib.File tagfile = TagLib.File.Create(filename);
            song.artist = tagfile.Tag.FirstPerformer;
            song.title = tagfile.Tag.Title is null ? Path.GetFileNameWithoutExtension(filename) : tagfile.Tag.Title;
            song.album = tagfile.Tag.Album;
            song.location = filename;
            song.info = Search.Spotify.Synchronise.SpotifyComment.FromString(tagfile.Tag.Comment);

            song.oneLiner = $"{song.artist} - {song.title}";
            song.secondLiner = string.IsNullOrWhiteSpace(song.album) ? song.artist : $"{song.artist} - {song.album}";

            song.accentColour = accentColourCache.ContainsKey(song.location) ? ColorTranslator.FromHtml(accentColourCache[song.location]) : Color.Black;

            song.id = PlayerManager.logger.getSongIdByLocation(song.location);

            if (getCover || getAccentColour)
            {
                song.cover = GetCover(tagfile);

                if (getAccentColour && song.cover is not null)
                {
                    song.accentColour = MediaPlayer.getAccentColour(song.cover, gap: 8).Result;
                }
            }

            return song;
        }

        /* LINKS */

        public void linkCover(System.Windows.Forms.PictureBox cover)
        {
            imgCover = cover;
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
    }
}
