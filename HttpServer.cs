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
using EmbedIO.WebSockets;

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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                if (!forceServer)
                    return;

                /*if (args.Length > 0)
                    RestartAsAdmin('"' + args[0] + '"');
                else
                    RestartAsAdmin();*/
            }
        }

        private async Task init(Logger logger, int port = 8080)
        {
            var url = "http://*:8080/";
            eserver = CreateWebServer(url);
            eserver.RunAsync();
        }

        static string GetStream(Image image)
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

        static string GetStream(string path)
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
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        // Create and configure our web server.
        private WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))
                .WithCors()
                // First, we will configure our web server by adding Modules.
                .WithLocalSessionManager()

                .WithWebApi("/api/control", m => m
                    .WithController<ControlController>())
                .WithWebApi("/api/data", m => m
                    .WithController<DataController>())
                .WithWebApi("/api/games", m => m
                    .WithController<GameController>())
                .WithWebApi("/api", m => m
                    .WithController<GeneralController>())

                .WithModule(new WebSocketsChatServer("/chat"))

                .WithStaticFolder("/", "ressources/www/", true)
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

            // Listen for state changes.
            //server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

            return server;
        }

        public static void RestartAsAdmin(string dir = "")
        {
            /*
            MessageBox.Show("restaresasd");
            ShowElevatedProcessTaskDialog();
            
            var startInfo = new ProcessStartInfo("reAudioPlayer Apollo.exe") { Verb = "runas", Arguments = dir};
            Process.Start(startInfo);
            Environment.Exit(0);*/
        }

        private class StaticVars
        {
            public static async Task SendStringAsync(IHttpContext context, string content, 
                string contentType = "text/html", Encoding encoding = null)
            {
                encoding = encoding is null ? Encoding.Latin1 : encoding;

                await context.SendStringAsync(content, contentType, encoding);
            }
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

        internal class GeneralController : WebApiController
        {
            [Route(HttpVerbs.Get, "/version")]
            public async Task getVersion(string user)
            {
                await StaticVars.SendStringAsync(HttpContext, "reAudioPlayer Apollo .5");
            }
        }

        internal class DataController : WebApiController
        {
            [Route(HttpVerbs.Get, "/displayname")]
            public async Task getVersion()
            {
                await StaticVars.SendStringAsync(HttpContext, PlayerManager.displayName);
            }

            [Route(HttpVerbs.Get, "/playlists")]
            public async Task getPlaylists()
            {
                await StaticVars.SendStringAsync(HttpContext,
                    JsonConvert.SerializeObject(PlaylistManager.getDetailedPlaylists()));
            }

            [Route(HttpVerbs.Get, "/volume")]
            public async Task getVolume()
            {
                await StaticVars.SendStringAsync(HttpContext, PlayerManager.volume.ToString());
            }

            [Route(HttpVerbs.Get, "/cover")]
            public async Task getCover()
            {
                await StaticVars.SendStringAsync(HttpContext, GetStream(PlayerManager.cover), "images/jpeg");
            }

            [Route(HttpVerbs.Get, "/radioProgramme")]
            public async Task getProgramme()
            {
                var programmes = PlayerManager.getRadioProgrammes();
                await StaticVars.SendStringAsync(HttpContext, programmes);
            }

            [Route(HttpVerbs.Get, "/accentColour")]
            public async Task getAccentColour()
            {
                await StaticVars.SendStringAsync(HttpContext, ColorTranslator.ToHtml(PlayerManager.accentColour));
            }
        }

        internal class ControlController : WebApiController
        {
            [Route(HttpVerbs.Get, "/next")]
            public async Task next()
            {
                PlayerManager.next();
            }

            [Route(HttpVerbs.Get, "/last")]
            public async Task last()
            {
                PlayerManager.last();
            }

            [Route(HttpVerbs.Get, "/volume/{value}")]
            public async Task setVolume(int value)
            {
                PlayerManager.volume = value;
            }

            [Route(HttpVerbs.Get, "/playPause")]
            public async Task playPause(int value)
            {
                PlayerManager.playPause();
                if (PlayerManager.isPlaying)
                {
                    //return GetStream("ressources/controls/webPlay.png");
                    await StaticVars.SendStringAsync(HttpContext, GetStream("ressources/controls/webPlay.png"), "images/jpeg");
                }
                else
                {
                    //return GetStream("ressources/controls/webPause.png");
                    await StaticVars.SendStringAsync(HttpContext, GetStream("ressources/controls/webPause.png"), "images/jpeg");
                }
            }

            [Route(HttpVerbs.Get, "/load/playlist/{index}")]
            public async Task loadPlaylist(int index)
            {
                PlayerManager.loadPlaylist(index);
            }

            [Route(HttpVerbs.Get, "/load/{index}")]
            public async Task loadSong(int index)
            {
                PlayerManager.load(index);
            }
        }

        internal class GameController : WebApiController
        {
            [Route(HttpVerbs.Get, "/validate-user/{user}")]
            public async Task validateUser(string user)
            {
                if (user == "null" || !users.ContainsKey(user))
                {
                    user = RandomString(6);
                    users.Add(user, false);
                }

                await StaticVars.SendStringAsync(HttpContext, user);
            }

            [Route(HttpVerbs.Get, "/library")]
            public async Task getGames()
            {
                await StaticVars.SendStringAsync(HttpContext, GameLibraryManager.getInstalledGamesAsJSON());
            }

            [Route(HttpVerbs.Get, "/launch/{id}&{user}")]
            public async Task launchGame(int id, string user)
            {
                bool success = GameLibraryManager.launchGameByIGDBId(id, user);

                if (success)
                {
                    await StaticVars.SendStringAsync(HttpContext, "OK");
                }
                else
                {
                    await StaticVars.SendStringAsync(HttpContext, "You have been temporarily blocked!");
                }
            }
        }

        /// <summary>
        /// Defines a very simple chat server.
        /// </summary>
        public class WebSocketsChatServer : WebSocketModule
        {
            public WebSocketsChatServer(string urlPath)
                : base(urlPath, true)
            {
                // placeholder
            }

            /// <inheritdoc />
            protected override Task OnMessageReceivedAsync(
                IWebSocketContext context,
                byte[] rxBuffer,
                IWebSocketReceiveResult rxResult)
                => SendToOthersAsync(context, Encoding.GetString(rxBuffer));

            /// <inheritdoc />
            protected override Task OnClientConnectedAsync(IWebSocketContext context)
                => Task.WhenAll(
                    SendAsync(context, "Welcome to the chat room!"),
                    SendToOthersAsync(context, "Someone joined the chat room."));

            /// <inheritdoc />
            protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
                => SendToOthersAsync(context, "Someone left the chat room.");

            private Task SendToOthersAsync(IWebSocketContext context, string payload)
                => BroadcastAsync(payload, c => c != context);
        }
    }
}