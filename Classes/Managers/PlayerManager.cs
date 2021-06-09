using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML
{
    public static class PlayerManager
    {
        public static MediaPlayer mediaPlayer;
        public static Logger logger;
        static RevealedStream revealedStream;
        static Radio radio;
        static string revealedLink = RevealedStream.defaultLink;
        public static HttpServer.Modules.WebSocket webSocket;
        private static Image _cover;
        public static Image cover
        {
            get
            {
                if (activePlayer == ActivePlayer.RevealedStream)
                {
                    //revealedStream.setAsCover();
                }

                return _cover;
            }
            set
            {
                _cover = value;
                Debug.WriteLine("Broadcast");
                webSocket?.broadCastCover();
            }
        }

        public static ActivePlayer activePlayer
        {
            get
            {
                if (!(revealedStream is null))
                {
                    return ActivePlayer.RevealedStream;
                }
                else if (!(radio is null))
                {
                    return ActivePlayer.ApolloOnAir;
                }
                else
                {
                    return ActivePlayer.Playlist;
                }
            }
        }

        public static string displayName
        {
            get
            {
                switch (activePlayer)
                {
                    case ActivePlayer.ApolloOnAir:
                        return radio.displayname;

                    case ActivePlayer.RevealedStream:
                        return "Revealed Radio";

                    case ActivePlayer.Playlist:
                    default:
                        return mediaPlayer.upNow.oneLiner;
                }
            }
        }

        public static PlayerPosition playerPosition
        {
            get
            {
                switch (activePlayer)
                {
                    case ActivePlayer.ApolloOnAir:
                        return new PlayerPosition();

                    case ActivePlayer.RevealedStream:
                        return new PlayerPosition();

                    case ActivePlayer.Playlist:
                    default:
                        PlayerPosition t = null;
                        mediaPlayer.trackBar.Invoke(new Action(() =>
                        {
                            t = new PlayerPosition(mediaPlayer.player.Position, mediaPlayer.player.NaturalDuration.TimeSpan);
                        }));
                        return t;
                }
            }
        }

        public class PlayerPosition
        {
            public string remainingTime;
            public string duration;
            public string absolutePosition;
            public string relativePosition;

            public PlayerPosition() { }

            public PlayerPosition(TimeSpan absolutePosition, TimeSpan duration)
            {
                this.duration = duration.ToString("mm':'ss");
                this.absolutePosition = absolutePosition.ToString("mm':'ss");
                remainingTime = (duration - absolutePosition).ToString("mm':'ss");
                relativePosition = Math.Round((absolutePosition.TotalSeconds / duration.TotalSeconds) * 100, 1).ToString() + "%";
            }

            public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public static int volume
        {
            get
            {
                switch (activePlayer)
                {
                    case ActivePlayer.ApolloOnAir:
                        return radio.volume;

                    case ActivePlayer.RevealedStream:
                        return 50;

                    case ActivePlayer.Playlist:
                    default:
                        return mediaPlayer.volume;
                }
            }

            set
            {
                webSocket.broadCastVolume(value);

                switch (activePlayer)
                {
                    case ActivePlayer.ApolloOnAir:
                        radio.volume = value;
                        return;

                    case ActivePlayer.RevealedStream:
                        return;

                    case ActivePlayer.Playlist:
                    default:
                        mediaPlayer.volume = value;
                        return;
                }
            }
        }

        public static bool isPlaying
        {
            get
            {
                switch (activePlayer)
                {
                    case ActivePlayer.ApolloOnAir:
                        return radio.isPlaying;

                    case ActivePlayer.RevealedStream:
                        return true;

                    case ActivePlayer.Playlist:
                    default:
                        return mediaPlayer.isPlaying;
                }
            }
        }

        public static Color accentColour
        {
            get
            {
                switch (activePlayer)
                {
                    case ActivePlayer.ApolloOnAir:
                        return Color.White;

                    case ActivePlayer.RevealedStream:
                        return Color.White;

                    case ActivePlayer.Playlist:
                    default:
                        return mediaPlayer.accentColour;
                }
            }
        }

        public static void load(int index)
        {
            switch (activePlayer)
            {
                case ActivePlayer.ApolloOnAir:
                    //  nothing
                    return;

                case ActivePlayer.RevealedStream:
                    // nothing
                    return;

                case ActivePlayer.Playlist:
                default:
                    mediaPlayer.loadSong(index);
                    return;
            }
        }

        public static PlaylistManager.FullPlaylist loadPlaylistVirtually(int index)
        {
            var playlists = File.ReadAllLines(logger.playlistLib);
            var songList = MediaPlayer.GetPlaylist(playlists[index], logger);
            return loadPlaylistVirtually(songList);
        }

        public static PlaylistManager.FullPlaylist loadPlaylistVirtually(List<MediaPlayer.Song> songList)
        {
            return PlaylistManager.FullPlaylist.FromSongList(songList);
        }

        public static string loadPlaylist(int index)
        {
            switch (activePlayer)
            {
                case ActivePlayer.ApolloOnAir:
                    return "Streaming Apollo On Air";

                case ActivePlayer.RevealedStream:
                    return "Streaming Revealed Radio";

                case ActivePlayer.Playlist:
                default:
                    var playlists = File.ReadAllLines(logger.playlistLib);
                    mediaPlayer.loadPlaylist(playlists[index]);
                    return new DirectoryInfo(playlists[index]).Name;
            }
        }

        public static void next()
        {
            switch (activePlayer)
            {
                case ActivePlayer.ApolloOnAir:
                    radio.next();
                    return;

                case ActivePlayer.RevealedStream:
                    // nothing
                    return;

                case ActivePlayer.Playlist:
                default:
                    mediaPlayer.next();
                    return;
            }
        }

        public static void last()
        {
            switch (activePlayer)
            {
                case ActivePlayer.ApolloOnAir:
                    // nothing
                    return;

                case ActivePlayer.RevealedStream:
                    // nothing
                    return;

                case ActivePlayer.Playlist:
                default:
                    mediaPlayer.last();
                    return;
            }
        }

        public static void playPause()
        {
            switch (activePlayer)
            {
                case ActivePlayer.RevealedStream:
                    // pause
                    return;

                case ActivePlayer.ApolloOnAir:
                    radio.playPause();
                    return;

                case ActivePlayer.Playlist:
                default:
                    mediaPlayer.playPause();
                    return;
            }
        }

        public enum ActivePlayer
        {
            Playlist, RevealedStream, ApolloOnAir
        }

        private static void RevealedStream_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            revealedStream = null;
        }

        private static void Radio_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            radio = null;
        }

        public static void launchApolloOnAir()
        {
            mediaPlayer.pause();

            if (!(revealedStream is null))
            {
                revealedLink = revealedStream.getLink();
                revealedStream.Close();
            }

            radio = new Radio(logger);
            radio.FormClosed += Radio_FormClosed;
            radio.Show();
        }

        public static void launchRevealedRadio()
        {
            mediaPlayer.pause();

            if (!(radio is null))
                radio.Close();

            revealedStream = new RevealedStream(revealedLink);
            revealedStream.FormClosed += RevealedStream_FormClosed;
            revealedStream.Show();
        }

        // resumes Playlist music play!!
        public static void resumeMusic()
        {
            if (!(revealedStream is null))
            {
                revealedLink = revealedStream.getLink();
                revealedStream.Close();
            }

            if (!(radio is null))
                radio.Close();
        }

        public static string getRadioProgrammes()
        {
            List<RadioProgramme> programmes = new List<RadioProgramme>();

            if (!(radio is null))
            {
                var programmeLoaded = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                var programmeEnd = programmeLoaded.AddHours(1);

                foreach (var programme in radio.programmes)
                {
                    RadioProgramme x;
                    var t = PlaylistManager.getDetailedPlaylist(Path.GetDirectoryName( programme.Value[0] ));
                    x.description = t.description += " songs, including:";
                    x.songs = t.tags;
                    x.name = t.name;
                    x.time = programmeLoaded.ToString("h tt") + " - " + programmeEnd.ToString("h tt");
                    programmes.Add(x);
                    programmeLoaded = programmeEnd;
                    programmeEnd = programmeLoaded.AddHours(1);
                }
            }

            return JsonConvert.SerializeObject(programmes);
        }

        private struct RadioProgramme
        {
            public string name;
            public string description;
            public string[] songs;
            public string time;
        }
    }
}
