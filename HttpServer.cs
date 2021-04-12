using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;
using Anna;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace reAudioPlayerML
{
    public class HttpWebServer
    {
        HttpServer server;
        MediaPlayer mediaPlayer;
        MetroFramework.Controls.MetroTrackBar prgVolume;
        //public GameLauncher gameLauncher;
        public Dictionary<string, bool> users = new Dictionary<string, bool>();

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public HttpWebServer(MediaPlayer media, Logger logger, MetroFramework.Controls.MetroTrackBar volumeBar, string[] args, bool forceServer = true)
        {
            mediaPlayer = media;
            prgVolume = volumeBar;

            try
            {
                init(logger).Wait();
            }
            catch
            {
                if (!forceServer)
                    return;

                if (args.Length > 0)
                    RestartAsAdmin('"' + args[0] + '"');
                else
                    RestartAsAdmin();
            }
        }

        private async Task init(Logger logger, int port = 8080)
        {
            server = new Anna.HttpServer("http://*:" + port.ToString() + "/");
            await initEndpoints(logger);
        }

        string getStream(Image image)
        {
            if (image is null)
                return "";

            try
            {
                Image i = image.Clone() as Image;

                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, i.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
            catch
            {
                return "";
            }
        }

        string getStream(string path)
        {
            using (Image image = Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        string getIconStream(string path)
        {
            var icon = Icon.ExtractAssociatedIcon(path);
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return Convert.ToBase64String( ms.ToArray() );
            }
        }

        private async Task initEndpoints(Logger logger)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("contentType", "images/jpeg");
            Dictionary<string, string> jsonHeaders = new Dictionary<string, string>();
            jsonHeaders.Add("contentType", "application/json");

            server.GET("/favicon.ico")
                    .Subscribe(ctx => ctx.Respond(getStream("ressources/faviconApollo.png")));

            /* Pages */

            server.GET("/")
                .Subscribe(ctx => ctx.Respond(File.ReadAllText("ressources/landingPage.html")));

            server.GET("/control")
                .Subscribe(ctx => ctx.Respond(File.ReadAllText("ressources/coverSwipe.html")));

            server.GET("/playlists")
                .Subscribe(ctx => ctx.Respond(File.ReadAllText("ressources/playlistStack.html")));

            server.GET("/cover")
                .Subscribe(ctx => ctx.Respond(getStream(PlayerManager.cover), statusCode: 200, headers: headers));

            server.GET("/radio")
                .Subscribe(ctx => ctx.Respond(File.ReadAllText("ressources/radio.html")));

            server.GET("/games")
                .Subscribe(ctx => ctx.Respond(File.ReadAllText("ressources/games.html")));

            server.GET("/games/validate-user/{user}")
                .Subscribe(ctx =>
                {
                    string user = ctx.Request.UriArguments.user;

                    if (user == "null" || !users.ContainsKey(user))
                    {
                        user = RandomString(6);
                        users.Add(user, false);
                    }

                    ctx.Respond(user);
                });

            /* now playing data */

            server.GET("/version")
                    .Subscribe(ctx => ctx.Respond("reAudioPlayer Apollo"));

            server.GET("/accent")
            .Subscribe(ctx => ctx.Respond(ColorTranslator.ToHtml( mediaPlayer.accentColour )));

            server.GET("/displayname")
                .Subscribe(ctx => ctx.Respond(PlayerManager.displayName)); // TODO

            server.GET("/get/volume")
                .Subscribe(ctx =>
                {
                    ctx.Respond(prgVolume.Value.ToString());
                    });

                /* controls */

            server.GET("/control/playPause")
                .Subscribe(ctx =>
                {
                    PlayerManager.playPause();
                    if (PlayerManager.isPlaying)
                    {
                        ctx.Respond(getStream("ressources/controls/webPlay.png"));
                    }
                    else
                    {
                        ctx.Respond(getStream("ressources/controls/webPause.png"));
                    }
                });

            server.GET("/control/next")
                .Subscribe(ctx =>
                {
                    PlayerManager.next();
                    ctx.Respond("OK");
                });

            server.GET("/control/last")
                .Subscribe(ctx =>
                {
                    PlayerManager.last();
                    ctx.Respond("OK");
                });

            server.GET("/control/load/playlist/{index}")
                .Subscribe(ctx =>
                {
                    var playlists = File.ReadAllLines(logger.playlistLib);
                    int index = Convert.ToInt32(ctx.Request.UriArguments.index);
                    mediaPlayer.loadPlaylist(playlists[index]);
                    ctx.Respond("OK");
                });

            server.GET("/control/load/{index}")
                .Subscribe(ctx =>
                {
                    mediaPlayer.loadSong(Convert.ToInt32(ctx.Request.UriArguments.index));
                    ctx.Respond("OK");
                });

            server.GET("/control/volume/{value}")
                .Subscribe(ctx =>
                {
                    prgVolume.Invoke(new Action(() =>
                    {
                        prgVolume.Value = Convert.ToInt32(ctx.Request.UriArguments.value);
                    }));
                    ctx.Respond("OK");
                });

            server.GET("/games/launch/{igdbId}&{user}")
                .Subscribe(ctx =>
                {
                    bool success = GameLibraryManager.launchGameByIGDBId(Convert.ToInt32(ctx.Request.UriArguments.igdbId), ctx.Request.UriArguments.user);
                    if (success)
                    {
                        ctx.Respond("OK");
                    }
                    else
                    {
                        ctx.Respond("You have been temporarily blocked!", 401);
                    }
                });

            /* playlist data */

            server.GET("/data/playlists") // deprecated
                .Subscribe(ctx =>
                {
                    Debug.WriteLine("data/playlists called but deprecated");
                    var playlists = PlaylistManager.getPlaylistNamesAsStrings();
                    ctx.Respond( JsonConvert.SerializeObject(playlists) );
                });

            server.GET("/data/games")
                .Subscribe(ctx =>
                {
                    ctx.Respond(GameLibraryManager.getInstalledGamesAsJSON());
                });

            server.GET("data/radio")
                .Subscribe(ctx =>
                {
                    var programmes = PlayerManager.getRadioProgrammes();

                    ctx.Respond(programmes);
                });

            server.GET("v2/data/playlists")
                .Subscribe(ctx =>
                {
                    ctx.Respond(JsonConvert.SerializeObject(PlaylistManager.getDetailedPlaylists()));
                });

            server.GET("data/playlist")
                .Subscribe(ctx =>
                {
                    List<Dictionary<string, string>> playlist = new List<Dictionary<string, string>>();
                    Dictionary<string, string> temp = new Dictionary<string, string>();

                    var songs = mediaPlayer.playlist;

                    if (songs is null)
                    {
                        ctx.Respond(statusCode: 404);
                        return;
                    }

                    foreach (var song in songs)
                    {
                        temp = new Dictionary<string, string>();
                        temp.Add("Number", songs.IndexOf(song).ToString());
                        temp.Add("Title", song.title);
                        temp.Add("Artist", song.artist);
                        temp.Add("Album", song.album);
                        temp.Add("Duration", "N/A");
                        playlist.Add(temp);
                    }

                    ctx.Respond(JsonConvert.SerializeObject(playlist));
                });
        }

        public static void RestartAsAdmin(string dir = "")
        {
            var startInfo = new ProcessStartInfo("reAudioPlayer Apollo.exe") { Verb = "runas", Arguments = dir};
            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}