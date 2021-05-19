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
        SimpleTrack previewS;
        FullTrack previewF;
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

        public SpotifyPreview(MediaPlayer mediaPlayer, FullTrack track)
        {
            player = mediaPlayer;
            downloadPreview(track);
            playPreview();
        }

        public static void ClearCachedTrack(SimpleTrack track)
        {
            if (track is null)
            {
                return;
            }

            var filename = AppContext.BaseDirectory + "spotify\\" + track.Name + ".mp3";
            
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        public void playPreview()
        {
            if (!File.Exists(filename))
            {
                return;
            }

            if (previewS is null)
            {
                var preview = previewF;
                var artists = preview.Artists.Select(s => s.Name);
                player.playIndependent(filename, preview.Name, string.Join(" x ", artists));
            }
            else
            {
                var preview = previewS;
                var artists = preview.Artists.Select(s => s.Name);
                player.playIndependent(filename, preview.Name, string.Join(" x ", artists));
            }
        }

        public void downloadPreview(SimpleTrack track)
        {
            downloadPreview(track.Name, track.PreviewUrl);
            previewS = track;
        }

        public void downloadPreview(FullTrack track)
        {
            downloadPreview(track.Name, track?.PreviewUrl);
            previewF = track;
        }

        public void downloadPreview(string name, string preview)
        {
            if (preview is null)
            {
                MessageBox.Show("Spotify messed up");
                return;
            }

            filename = AppContext.BaseDirectory + "spotify\\" + name + ".mp3";

            if (File.Exists(filename))
                return;

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(preview + ".mp3", filename);
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
