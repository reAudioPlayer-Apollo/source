using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML
{
    public class Logger
    {
        private string docs;

        public string songLib { get; private set; }
        public string playlistLib { get; private set; }
        public string songTrainer { get; private set; }
        private string gameTrainer;
        private string moveTrainer;

        public List<string> songTrainerTable { get; private set; } = new List<string>();
        public List<string> gameTrainerTable { get; private set; }  = new List<string>();
        private List<string> moveTrainerTable = new List<string>();
        public Dictionary<string, string> moveTrainerDictionary = new Dictionary<string, string>();

        private string songTrainerFirst, songTrainerSecond;
        //GameChecker gamechecker = new GameChecker();

        public Logger()
        {
            docs = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\reAudioPlayer";
            songLib = Path.Combine(docs, "SMLib.list"); // SONG ML Lib
            playlistLib = Path.Combine(docs, "PLMLib.list"); // PLAYLIST ML Lib
            songTrainer = Path.Combine(docs, "SMLT.csv"); // SONG ML Table / Trainer
            gameTrainer = Path.Combine(docs, "GMLT.csv"); // GAME ML Table / Trainer
            moveTrainer = Path.Combine(docs, "MVMLT.csv"); // MOVE ML Table / Trainer

            if (!Directory.Exists(docs))
                Directory.CreateDirectory(docs);

            if (!File.Exists(songLib))
                File.Create(songLib);

            if (!File.Exists(playlistLib))
                File.Create(playlistLib);

            if (!File.Exists(moveTrainer))
                File.WriteAllText(moveTrainer, "oldDir|newDir");
            else
            {
                moveTrainerTable = new List<string>(File.ReadAllLines(moveTrainer));
                loadMoveDictionary();
            }

            if (!File.Exists(songTrainer))
                File.WriteAllText(songTrainer, "1st,2nd");
            else
            {
                songTrainerTable = new List<string>(File.ReadLines(songTrainer));
            }

            if (!File.Exists(gameTrainer))
                File.WriteAllText(gameTrainer, "GameId,Song");
            else
            {
                gameTrainerTable = new List<string>(File.ReadLines(gameTrainer));
            }

            updateSongLib();
        }

        public string getSongLocationById(int id)
        {
            return File.ReadAllLines(songLib)[id];
        }

        public int getSongIdByLocation(string location)
        {
            return File.ReadAllLines(songLib).ToList().FindIndex(x => x == location);
        }

        private void updateSongLib()
        {
            var songs = new List<string>(File.ReadAllLines(songLib));
            var newSongs = new List<string>(songs);

            foreach (var song in songs)
            {
                if (!File.Exists(song))
                {
                    newSongs.Remove(song);
                }
            }

            File.WriteAllLines(songLib, newSongs);
        }

        public void addFileMove(string oldDir, string newDir)
        {
            if (moveTrainerTable.Count == 0)
                moveTrainerTable = new List<string>(File.ReadAllLines(moveTrainer));

            string line = oldDir + "|" + newDir;

            if (moveTrainerTable.Contains(line))
                moveTrainerTable.Remove(line);

            moveTrainerTable.Add(line);
            File.WriteAllLines(moveTrainer, moveTrainerTable);
            loadMoveDictionary();
        }

        private void loadMoveDictionary()
        {
            for (int i = 1; i < moveTrainerTable.Count; i++)
            {
                var split = moveTrainerTable[i].Split('|');

                if (!moveTrainerDictionary.ContainsKey(split[0]))
                    moveTrainerDictionary.Add(split[0], split[1]);
                else
                    moveTrainerDictionary[split[0]] = split[1];
            }
        }

        public void addPlayedSong(string filename)
        {
            addSongToDB(filename);
            writeSongOrderModel(filename);
            addGamePlayedModel(filename);
        }

        public void addGamePlayedModel(string filename)
        {/*
            foreach (var game in gamechecker.getRunningGames())
            {
                if (game != "")
                {
                    var index = gamechecker.getKnownGames().IndexOf(game);

                    gameTrainerTable.Add(getGameTrainerLine(index, filename));
                    File.WriteAllLines(gameTrainer, gameTrainerTable);
                }
            }*/
        }

        private void writeSongOrderModel(string filename)
        {
            if (songTrainerFirst == null)
            {
                songTrainerFirst = filename;
                return;
            }
            else if (songTrainerFirst == filename)
                return;
            else
                songTrainerSecond = filename;

            var csv = getSongTrainerLine();

            if (!songTrainerTable.Contains(csv))
            {
                songTrainerTable.Add(csv);
                File.WriteAllLines(songTrainer, songTrainerTable);
            }

            songTrainerFirst = songTrainerSecond;
        }

        private string getGameTrainerLine(int gameIndex, string filename)
        {
            var i1 = gameIndex;
            var i2 = getSongIndex(filename);

            return i1.ToString() + "," + i2.ToString();
        }

        private string getSongTrainerLine()
        {
            var i1 = getSongIndex(songTrainerFirst);
            var i2 = getSongIndex(songTrainerSecond);

            return i1.ToString() + "," + i2.ToString();
        }

        public void addPlaylistToDB(string playlistName)
        {
            var lib = new List<string>(File.ReadAllLines(playlistLib));

            if (!lib.Contains(playlistName))
            {
                lib.Add(playlistName);
                File.WriteAllLines(playlistLib, lib);
            }
        }

        private int getSongIndex(string filename)
        {
            var lib = new List<string>(File.ReadAllLines(songLib));

            return lib.IndexOf(filename);
        }

        public void addSongToDB(string filename)
        {
            var lib = new List<string>(File.ReadAllLines(songLib));

            if (!lib.Contains(filename))
            {
                lib.Add(filename);
                File.WriteAllLines(songLib, lib);
            }
        }
    }
}
