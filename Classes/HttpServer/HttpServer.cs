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

namespace reAudioPlayerML.HttpServer
{
    public class HttpWebServer
    {
        WebServer eserver;
        MediaPlayer mediaPlayer;
        MetroFramework.Controls.MetroTrackBar prgVolume;
        //public GameLauncher gameLauncher;
        public static Dictionary<string, bool> users = new Dictionary<string, bool>();


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

        string getIconStream(string path)
        {
            var icon = Icon.ExtractAssociatedIcon(path);
            //byte[] bytes;
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

                .WithLocalSessionManager()

                .WithWebApi("/api/control", m => m
                    .WithController<API.ControlAPI>())
                .WithWebApi("/api/data", m => m
                    .WithController<API.DataAPI>())
                .WithWebApi("/api/games", m => m
                    .WithController<API.GameAPI>())
                .WithWebApi("/api/youtube", m => m
                    .WithController<API.YoutubeAPI>())
                .WithWebApi("/api", m => m
                    .WithController<API.GeneralAPI>())

                .WithModule(new Modules.WebSocket("/ws"))

                .WithStaticFolder("/", "ressources/www/", true)
                .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

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
    }
}