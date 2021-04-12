using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace reAudioPlayerML
{
    public class GameChecker
    {
        private string docs;
        private List<string> knownGames;
        private List<string> runningGames = new List<string>();
        Timer tmr = new Timer(1000);

        public event EventHandler<string> GameStarted;

        public GameChecker()
        {
            docs = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\reAudioPlayer";

            var lines = getExes();
            knownGames = new List<string>(lines);
            tmr.Elapsed += Tmr_Elapsed;
            tmr.Enabled = true;

            Task.Factory.StartNew(() => populateJson());
        }

        public string[] getExes()
        {
            List<string> t = new List<string>();
            var items = loadJson();

            foreach (var item in items)
            {
                t.AddRange(item.exe);
            }

            return t.ToArray();
        }

        public void populateJson()
        {
            var items = loadJson();
            Search.Game game = new Search.Game();
            bool changedAtLeastOne = false;
            
            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].igdbId == -1)
                {
                    var t = game.byQuery(items[i].name).Result;
                    items[i].igdbId = (long)t.Id;
                    items[i].cover = t.Cover.Value.Url.Replace("t_thumb", "t_1080p");
                    changedAtLeastOne = true;
                }
            }

            if (changedAtLeastOne)
                saveJson(items.ToArray());
        }

        private static void saveJson(GameObj[] items)
        {
            File.WriteAllText("ressources\\games.json", JsonConvert.SerializeObject(items));
        }

        public static void addGameToJson(string name, string exe)
        {
            var items = loadJson();
            Search.Game game = new Search.Game();
            GameObj item = new GameObj();
            item.name = name;
            item.exe = new List<string>();
            item.exe.Add(exe);
            item.igdbId = -1;

            //var t = game.byQuery(item.name).Result;
            //item.igdbId = (long)t.Id;
            //item.cover = t.Cover.Value.Url.Replace("t_thumb", "t_1080p");
            items.Add(item);
            saveJson(items.ToArray());
        }

        public static List<GameObj> loadJson()
        {
            using (StreamReader r = new StreamReader("ressources\\games.json"))
            {
                string json = r.ReadToEnd();
                List<GameObj> items = JsonConvert.DeserializeObject<List<GameObj>>(json);
                return items;
            }
        }

        public class GameObj
        {
            public string name;
            public string cover;
            public long igdbId;
            public List<string> exe;
        }

        public string getDisplayName(string exeName)
        {
            var items = loadJson();

            exeName = Path.GetFileName(exeName);

            foreach (var item in items)
            {
                if (item.exe.Contains(exeName))
                    return item.name;
            }

            return Path.GetFileNameWithoutExtension(exeName);
        }

        private void Tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task<List<string>>.Factory.StartNew(() => getRunningGames()).ContinueWith((task) =>
            {
                foreach (var runningGame in task.Result)
                {
                    if (!runningGames.Contains(runningGame))
                    {
                        if (GameStarted != null)
                        {
                            GameStarted(new object(), runningGame);

                            lock (runningGames)
                            {
                                runningGames.Add(runningGame);
                            }
                        }
                    }
                }
            });
        }

        public List<string> getKnownGames()
        {
            return knownGames;
        }

        public List<string> getRunningGames()
        {
            List<string> _runningGames = new List<string>();
            
            Process[] processlist = Process.GetProcesses();

            foreach (var game in knownGames)
            {
                Process[] pname = Process.GetProcessesByName(game.Replace(".exe", ""));

                if (pname.Length > 0)
                    _runningGames.Add(game);
            }

            return _runningGames;
        }

        public string getSuggestedPlaylist(Logger logger, string gameName)
        {
            int i = knownGames.IndexOf(gameName);

            List<string> suggestedSongs = new List<string>();
            var songLib = File.ReadAllLines(logger.songLib);
            var suggestedPlaylists = new Dictionary<string, int>();
            
            foreach (var str in logger.gameTrainerTable)
            {
                var s = str.Split(',');
                var gameIdString = s[0];
                var songIdString = s[1];

                try
                {
                    var gameId = Convert.ToInt32(gameIdString);
                    var songId = Convert.ToInt32(songIdString);

                    if (gameId == i)
                    {
                        suggestedSongs.Add(songLib[songId]);
                    }
                } catch { }
            }

            foreach (var suggestedSong in suggestedSongs)
            {
                var p = Path.GetDirectoryName(suggestedSong);

                if (suggestedPlaylists.ContainsKey(p))
                    suggestedPlaylists[p]++;
                else
                    suggestedPlaylists.Add(p, 1);
            }

            if (suggestedPlaylists.Count > 0)
            {
                var max = suggestedPlaylists.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

                return max;
            }
            else
                return "";
        }
    }
}
