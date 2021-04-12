using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace reAudioPlayerML.Search
{
    public class SpotifyPreview
    {
        MediaPlayer player;
        SimpleTrack preview;
        string filename;

        public SpotifyPreview(MediaPlayer mediaPlayer)
        {
            player = mediaPlayer;
        }

        public SpotifyPreview(MediaPlayer mediaPlayer, SimpleTrack track)
        {
            player = mediaPlayer;
            downloadPreview(track);
            playPreview();
        }

        public void playPreview()
        {
            if (!File.Exists(filename))
            {
                return;
            }

            var artists = preview.Artists.Select(s => s.Name);
            player.playIndependent(filename, preview.Name, string.Join(" x ", artists));
        }

        public void downloadPreview(SimpleTrack track)
        {
            filename = AppContext.BaseDirectory + "spotify\\" + track.Name + ".mp3";

            preview = track;

            if (File.Exists(filename))
                return;

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(track.PreviewUrl + ".mp3", filename);
                }
            }
            catch (System.Net.WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    MessageBox.Show("Spotify messed up");
                }
            }
        }
    }
}
