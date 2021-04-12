﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static HttpWebServer webServer;
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

            return JsonConvert.SerializeObject(returnItems);
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

        public static void Initialise(GameChecker gc, HttpWebServer hs)
        {
            gameChecker = gc;
            webServer = hs;

            var drives = DriveInfo.GetDrives();

            foreach (var drive in drives)
            {
                scanAndAppend(drive.RootDirectory.FullName);
            }
        }

        private static void scanAndAppend(string directory)
        {
            activeScannings++;

            Task.Factory.StartNew(() => getGamesInFolder(directory)).ContinueWith(games =>
            {
                lock (games)
                {
                    foreach (var game in games.Result)
                    {
                        var name = gameChecker.getDisplayName(game);

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
        }

        public static bool launchGameByIGDBId(int igdbId, string source = "local")
        {
            var games = GameChecker.loadJson();
            var t = games.Where(x => x.igdbId == igdbId).FirstOrDefault();

            bool isBlocked = webServer.users[source];

            if (t == null || isBlocked)
                return false;

            launchGameByName(t.name, source, true);
            return true;
        }

        public static void launchGameByName(string name, string source = "local", bool fromWeb = false)
        {
            if (fromWeb)
            {
                if (MessageBox.Show($"It was requested to launch {name}. Do you want to allow this?", "Game Launch Requested", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    if (MessageBox.Show("Do you want to block this user for this session?", "Prevent Spam?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        webServer.users[source] = true;
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
