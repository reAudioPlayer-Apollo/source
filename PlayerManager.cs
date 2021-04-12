using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public static Image cover;

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
