using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using EmbedIO;
using EmbedIO.WebApi;
using EmbedIO.Actions;
using EmbedIO.Routing;
using System.ComponentModel;

namespace reAudioPlayerML
{
    public class HttpWebServer
    {
        WebServer eserver;
        MediaPlayer mediaPlayer;
        MetroFramework.Controls.MetroTrackBar prgVolume;
        //public GameLauncher gameLauncher;
        public static Dictionary<string, bool> users = new Dictionary<string, bool>();

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
            //server = new Anna.HttpServer("http://*:" + port.ToString() + "/");
            //await initEndpoints(logger);

            var url = "http://localhost:8080/";
            eserver = CreateWebServer(url);
            eserver.RunAsync();
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

        // Create and configure our web server.
        private static WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                // First, we will configure our web server by adding Modules.
                .WithLocalSessionManager()
                .WithWebApi("/api", m => m
                    .WithController<APIController>())
                .WithStaticFolder("/", "ressources/www/", true)
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

            // Listen for state changes.
            //server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

            return server;
        }

        private async Task initEndpoints(Logger logger)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("contentType", "images/jpeg");
            Dictionary<string, string> jsonHeaders = new Dictionary<string, string>();
            jsonHeaders.Add("contentType", "application/json");
            /*
            server.GET("/favicon.ico")
                    .Subscribe(ctx => ctx.Respond(getStream("ressources/faviconApollo.png")));

            /* Pages *

            server.GET("/cover")
                .Subscribe(ctx => ctx.Respond(getStream(PlayerManager.cover), statusCode: 200, headers: headers));

            server.GET("/radio")
                .Subscribe(ctx => ctx.Respond(File.ReadAllText("ressources/radio.html")));

            /* now playing data *

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

                /* controls *

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

            /* playlist data *

            server.GET("/data/playlists") // deprecated
                .Subscribe(ctx =>
                {
                    Debug.WriteLine("data/playlists called but deprecated");
                    var playlists = PlaylistManager.getPlaylistNamesAsStrings();
                    ctx.Respond( JsonConvert.SerializeObject(playlists) );
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
                });*/
        }

        public static void RestartAsAdmin(string dir = "")
        {
            MessageBox.Show("restaresasd");
            ShowElevatedProcessTaskDialog();
            /*
            var startInfo = new ProcessStartInfo("reAudioPlayer Apollo.exe") { Verb = "runas", Arguments = dir};
            Process.Start(startInfo);
            Environment.Exit(0);*/
        }

        // tmp
        private static void ShowElevatedProcessTaskDialog()
        {
            var page = new TaskDialogPage()
            {
                Heading = "Settings saved - Service Restart required",
                Text = "The service needs to be restarted to apply the changes.",
                Icon = TaskDialogIcon.ShieldSuccessGreenBar,
                Buttons =
                {
                    TaskDialogButton.Close
                }
            };

            var restartNowButton = new TaskDialogCommandLinkButton("&Restart now");
            page.Buttons.Add(restartNowButton);

            restartNowButton.ShowShieldIcon = true;
            restartNowButton.Click += (s, e) =>
            {
                restartNowButton.AllowCloseDialog = true;
                restartNowButton.Enabled = false;

                // Try to start an elevated cmd.exe.
                var psi = new ProcessStartInfo("cmd.exe", "/k echo Hi, this is an elevated command prompt.")
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };

                try
                {
                    Process.Start(psi)?.Dispose();
                }
                catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
                {
                    // The user canceled the UAC prompt, so don't close the dialog and
                    // re-enable the restart button.
                    restartNowButton.AllowCloseDialog = false;
                    restartNowButton.Enabled = true;
                    return;
                }
            };

            TaskDialog.ShowDialog(page);
        }

        internal class APIController : WebApiController
        {
            [Route(HttpVerbs.Get, "/games/validate-user/{user}")]
            public async Task validateUser(string user)
            {
                if (user == "null" || !users.ContainsKey(user))
                {
                    user = RandomString(6);
                    users.Add(user, false);
                }

                await HttpContext.SendDataAsync(user);
            }

            [Route(HttpVerbs.Get, "/data/games")]
            public async Task getGames()
            {
                await HttpContext.SendDataAsync(GameLibraryManager.getInstalledGamesAsJSON());
            }

            [Route(HttpVerbs.Get, "/games/launch/{id}&{user}")]
            public async Task launchGame(int id, string user)
            {
                bool success = GameLibraryManager.launchGameByIGDBId(id, user);

                if (success)
                {
                    await HttpContext.SendDataAsync("OK");
                }
                else
                {
                    await HttpContext.SendDataAsync("You have been temporarily blocked!");
                }
            }
        }
    }
}