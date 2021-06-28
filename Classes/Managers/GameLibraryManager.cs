using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public static class GameLibraryManager
    {
        static GameChecker gameChecker;
        static HttpServer.HttpWebServer webServer;
        static int activeScannings = 0;
        static FolderBrowserDialog fbd;
        public static string steamLocation;
        static Dictionary<string, GameStartInfo> installedGames = new Dictionary<string, GameStartInfo>();

        public static string getInstalledGamesAsJSON()
        {
            var items = GameChecker.loadJson();
            List<GameChecker.GameObj> returnItems = new List<GameChecker.GameObj>();

            foreach (var item in items)
            {
                if (installedGames.ContainsKey(item.name))
                    returnItems.Add(item);
            }

            string v = JsonConvert.SerializeObject(returnItems);
            return v;
        }

        public class GameStartInfo
        {
            public string location;
            public Platform platform;

            public enum Platform
            {
                EpicGames, Steam, Origin, Uplay, Unknown
            }
        }

        public static void Initialise(GameChecker gc, HttpServer.HttpWebServer hs)
        {
            gameChecker = gc;
            webServer = hs;

            loadFromCache();
            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                scanAndAppend(drive.RootDirectory.FullName);
            }
        }

        public static void loadFromCache()
        {
            var cachedGames = cache;

            foreach (var game in cachedGames)
            {
                if (File.Exists(game.Key))
                {
                    var name = gameChecker.getDisplayName(game.Key);

                    lock (installedGames)
                    {
                        if (!installedGames.ContainsKey(name))
                        {
                            GameStartInfo t = new GameStartInfo();
                            t.location = game.Key;

                            if (game.Key.Contains("steamapps"))
                                t.platform = GameStartInfo.Platform.Steam;
                            else
                                t.platform = GameStartInfo.Platform.Unknown;

                            installedGames.Add(name, t);
                        }
                    }
                }
            }
        }

        public static Dictionary<string, string> cache
        {
            get
            {
                var t = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.gameCache);
                return t is null ? new Dictionary<string, string>() : t;
            }
            set
            {
                Properties.Settings.Default.gameCache = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
        }

        private static void scanAndAppend(string directory)
        {
            activeScannings++;

            Logger.Log($"scanning {directory}", "GameLibraryManager");

            Task.Factory.StartNew(() => getGamesInFolder(directory)).ContinueWith(games =>
            {
                foreach (var game in games.Result)
                {
                    var name = gameChecker.getDisplayName(game);

                    lock (installedGames)
                    {
                        if (!installedGames.ContainsKey(name))
                        {
                            GameStartInfo t = new GameStartInfo();
                            t.location = game;
                            if (game.Contains("steamapps"))
                                t.platform = GameStartInfo.Platform.Steam;
                            else
                                t.platform = GameStartInfo.Platform.Unknown;

                            installedGames.Add(name, t);
                        }
                    }
                }

                activeScannings--;

                Logger.Log($"scanned {directory} ({activeScannings} remaining)", "GameLibraryManager");

                if (activeScannings == 0)
                {
                    lock (installedGames)
                    {
                        Logger.Log($"Game Library has been scanned and updated, {installedGames.Count} games are installed", "GameLibraryManager");

                        Dictionary<string, string> tmp = new Dictionary<string, string>();
                        foreach (var game in installedGames)
                        {
                            tmp.Add(game.Value.location, game.Key);
                        }
                        cache = tmp;
                    }
                }
            }).ConfigureAwait(true);
        }

        static string[] getExesInFolder(string folder)
        {
            try
            {
                return Directory.GetFiles(folder, "*.exe", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException)
            {
                if (folder.Contains("System Volume Information"))
                    return new string[0];

                List<string> exes = new List<string>();
                Debug.WriteLine("UnauthorizedAccessException, " + folder);
                try
                {
                    var dirs = Directory.GetDirectories(folder);

                    foreach (var dir in dirs)
                    {
                        if (Directory.Exists(dir))
                            exes.AddRange(getExesInFolder(dir));
                    }

                    return exes.ToArray();
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.WriteLine("Inner UnauthorizedAccessException");
                    return new string[0];
                }
            }
            catch (IOException)
            {
                return new string[0];
            }
        }

        public static bool launchGameByIGDBId(int igdbId, string source = "local")
        {
            if (source is null)
                return false;

            var games = GameChecker.loadJson();
            var t = games.Where(x => x.igdbId == igdbId).FirstOrDefault();

            bool isBlocked = HttpServer.HttpWebServer.users[source];

            if (t == null || isBlocked)
                return false;

            launchGameByName(t.name, source, true);
            return true;
        }

        public static void launchGameByName(string name, string source = "local", bool fromWeb = false)
        {
            if (fromWeb)
            {
                TaskDialogPage t = new TaskDialogPage();
                t.Verification.Text = "block user if declined";
                t.Buttons.Add(TaskDialogButton.Yes);
                t.Buttons.Add(TaskDialogButton.No);
                t.DefaultButton = TaskDialogButton.Yes;
                t.Caption = "Game Launch Requested";
                t.Heading = name;
                t.Text = $"Do you want to launch {name} as requested?";
                t.Icon = new TaskDialogIcon(SystemIcons.Question);

                if (TaskDialog.ShowDialog(t) == TaskDialogButton.No)
                {
                    if (t.Verification.Checked)
                    {
                        HttpServer.HttpWebServer.users[source] = true;
                    }

                    return;
                }
            }

            if (installedGames[name].platform == GameStartInfo.Platform.Steam)
            {
                if (steamLocation == null || !File.Exists(steamLocation))
                {
                    MessageBox.Show("This game requires steam, but steam has not been found yet. Please try again later.", "Steam Not Found Yet!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Process.Start(steamLocation);
            }

            string game = installedGames[name].location;

            Process.Start(game);
        }

        static List<string> getGamesInFolder(string folder)
        {
            List<string> returner = new List<string>();
            var exes = getExesInFolder(folder);
            var games = gameChecker.getKnownGames();

            foreach (var exe in exes)
            {
                if (games.Contains(Path.GetFileName(exe)))
                    returner.Add(exe);
                else if (Path.GetFileName(exe) == "steam.exe")
                    steamLocation = exe;
            }

            return returner;
        }
    }
}
