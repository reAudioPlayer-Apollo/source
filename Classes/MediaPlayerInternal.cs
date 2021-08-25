using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public partial class MediaPlayer
    {
        private int playlistIndex;
        
        private int lastIndex
        {
            get
            {
                int j = playlistIndex - 1;

                for (int i = 0; i < playlist.Count; i++) // only run once
                {
                    if (j >= 0 && !blockList.Contains(j))
                        continue;

                    if (j < 0)
                        j = playlist.Count - 1;
                    else
                        j--;
                }

                return j;
            }
        }

        private static Dictionary<string, string> accentColourCache
        {
            get
            {
                string cache = Properties.Settings.Default.accentColourCache;
                if (cache is not null)
                {
                    var t = JsonConvert.DeserializeObject<Dictionary<string, string>>(cache);
                    return t is null ? new Dictionary<string, string>() : t;
                }
                else
                {
                    return new Dictionary<string, string>();
                }
            }
            set
            {
                Properties.Settings.Default.accentColourCache = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
        }

        private readonly System.Windows.Forms.Timer tmrBarManager = new System.Windows.Forms.Timer();
        private readonly PausableTimer tmrSongPlayed;
        private MetroFramework.Controls.MetroTrackBar volumeBar;
        private System.Windows.Forms.PictureBox playPauseImg;
        private System.Windows.Forms.Label lblDone, lblUp, lblUpNowTitle, lblUpNowArtist;
        private readonly Logger logger;
        private System.Windows.Forms.PictureBox imgCover;
        private static NotifyIcon notifyIcon;
        private bool mayCancelLoad = false, cancelLoad = false;

        private readonly AccentColour.Finder finder = new AccentColour.Finder();


        private static string ip;

        private string independentSong = null;

        private bool loadCover()
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
                            if (PlayerManager.cover is null)
                            {
                                PlayerManager.cover = playlist[playlistIndex].cover.Clone() as Image;
                            }
                            else
                            {
                                lock (PlayerManager.cover)
                                {
                                    PlayerManager.cover = playlist[playlistIndex].cover.Clone() as Image;
                                }
                            }

                            if (playlist[playlistIndex].background is null)
                            {
                                playlist[playlistIndex].background = getBackground(playlist[playlistIndex].accentColour);
                            }

                            imgCover.BackgroundImage = playlist[playlistIndex].background;
                            accentColour = playlist[playlistIndex].accentColour;
                        }
                    }

                }));
            }

            return returnv;
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

        private int toTrackBarScale(double posMs)
        {
            double totMs = player.NaturalDuration.TimeSpan.TotalMilliseconds;

            int r = (int)Math.Round((posMs * 1000.0) / totMs);

            if (r > 1000)
                r = 1000;

            return r;
        }

        private int toTrackBarScale()
        {
            return toTrackBarScale(player.Position.TotalMilliseconds);
        }

        private void loadNext(bool manually = true)
        {
            if (playlist is null)
            {
                return;
            }

            loadSong(playlist[nextIndex].location);

            if (Next is not null)
            {
                Next(new object(), manually);
            }

            if (Skip is not null)
            {
                Skip(new object(), manually);
            }
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            loadNext(false);
        }

        private async Task loadCovers()
        {
            var files = playlist.Select(x => x.location).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                if (cancelLoad)
                {
                    cancelLoad = false;
                    return;
                }

                Image cover = GetCover(playlist[playlist.FindIndex(x => x.location == files[i])].location);

                lock (playlist)
                {
                    Song ttt = playlist[playlist.FindIndex(x => x.location == files[i])];
                    ttt.cover = cover;
                    playlist[playlist.FindIndex(x => x.location == files[i])] = ttt;
                };
            }

            Task t = Task.Run(() => loadBackgrounds());

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(2000);
            }

            try
            {
                loadCover();
            }
            catch { }

            await t;

            mayCancelLoad = false;
            notifyIcon?.ShowBalloonTip(2000, "reAudioPlayer", "Playlist loaded!", ToolTipIcon.Info);
        }

        private async Task updateAccentColours(Song[] songs, int startIndex = 0)
        {
            /*Task t = null;

            if (songs.Length > (startIndex + 10))
            {
                t = Task.Factory.StartNew(() => updateAccentColours(songs, startIndex + 10)).Unwrap();
            }
            else
            {
                t = Task.CompletedTask;
            }*/

            var files = songs.Select(x => x.location).ToArray();
            var cache = accentColourCache;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = startIndex; i < songs.Length; i++)
            {
                Color colour = await getAccentColour(songs[i].cover, 1);
                int index = playlist.FindIndex(x => x.location == files[i]);

                cache[files[index]] = ColorTranslator.ToHtml(colour);
                accentColourCache = cache;

                if (index >= 0)
                {
                    Bitmap bm = getBackground(colour);

                    lock (playlist)
                    {
                        playlist[index].accentColour = colour;
                        playlist[index].background = bm;
                    }
                }

                Logger.Log($"updating accent colours ({i + 1}/{songs.Length}) ({Math.Round((double)sw.ElapsedMilliseconds / 1000, 2)} s)", "MediaPlayer.updateAccentColours");
            }
            sw.Stop();

            //await t;
            //Logger.Log($"{startIndex} finished", "startindex_temp");
        }

        private async Task loadBackgrounds()
        {
            var files = playlist.Select(x => x.location).ToArray();
            var cache = accentColourCache;

            for (int i = 0; i < files.Length; i++)
            {
                if (cancelLoad)
                {
                    cancelLoad = false;
                    return;
                }

                if (playlist[playlist.FindIndex(x => x.location == files[i])].cover is null)
                {
                    var t = playlist[playlist.FindIndex(x => x.location == files[i])];
                    t.accentColour = cache.ContainsKey(t.location) ? ColorTranslator.FromHtml(cache[t.location]) : Color.Black;
                    t.background = null;
                    playlist[playlist.FindIndex(x => x.location == files[i])] = t;
                }
                else
                {
                    Song ttt = playlist[playlist.FindIndex(x => x.location == files[i])];
                    Color colour = await getAccentColour(ttt.cover, 1);

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Bitmap bm = getBackground(colour);
                    sw.Stop();
                    Debug.WriteLine(sw.Elapsed);

                    cache[ttt.location] = ColorTranslator.ToHtml(colour);
                    accentColourCache = cache;

                    lock (playlist)
                    {
                        ttt.accentColour = colour;
                        ttt.background = bm;
                        playlist[playlist.FindIndex(x => x.location == files[i])] = ttt;
                    }
                }
            }
        }

        string revealedLink;

        private Bitmap getBackground(System.Drawing.Color colour)
        {
            return finder.getColourAsShadowBitmap(colour);
        }

        private static async Task<System.Drawing.Color> getAccentColour(Image image, int gap = 2)
        {
            if (image is null)
            {
                return Color.Black;
            }

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
    }
}
