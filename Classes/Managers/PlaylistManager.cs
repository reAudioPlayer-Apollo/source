using Newtonsoft.Json;
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
    public static class PlaylistManager
    {
        public static Logger logger;

        public static string Create(string[] songs, bool virtually = false)
        {
            return virtually ? createVirtually(songs) : createPhysically(songs);
        }

        public static string Create(string[] songs, string name)
        {
            return createVirtually(songs, name);
        }

        public static Dictionary<string, string[]> virtualPlaylists
        {
            get
            {
                var t = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(Properties.Settings.Default.virtualPlaylists);
                return t is null ? new Dictionary<string, string[]>() : t;
            }
            set
            {
                Properties.Settings.Default.virtualPlaylists = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
        }

        private static string createVirtually(string[] songs, string name = "Virtual Playlist")
        {
            var pls = virtualPlaylists;

            if (pls.ContainsKey(name))
            {
                pls[name] = songs;
            }
            else
            {
                pls.Add(name, songs);
            }

            virtualPlaylists = pls;

            return name;
        }

        private static string createPhysically(string[] songs)
        {
            string ret = "";

            Logger.txtLogger.Invoke(new Action(() =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.Cancel)
                {
                    ret = "[Aborted]";
                }
                else
                {
                    foreach (var song in songs)
                    {
                        File.Copy(song, Path.Combine(fbd.SelectedPath, Path.GetFileName(song)));
                    }

                    ret = fbd.SelectedPath;
                }
            }));

            return ret;
        }

        public static List<string> getPlaylistPathsAsStrings()
        {
            return new List<string>( File.ReadAllLines(logger.playlistLib) );
        }

        public static List<string> getPlaylistNamesAsStrings()
        {
            var playlists = getPlaylistPathsAsStrings();

            for (var i = 0; i < playlists.Count; i++)
            {
                playlists[i] = new DirectoryInfo(playlists[i]).Name;
            }

            return playlists;
        }

        public static List<FileInfo> getSongPathsAsFileInfos(string[] playlist)
        {
            List<FileInfo> x = new List<FileInfo>();

            foreach (string file in playlist)
            {
                if (FileManager.isSupported(file))
                    x.Add(new FileInfo(file));
            }

            return x;
        }

        public static List<FileInfo> getSongPathsAsFileInfos(string playlist)
        {
            List<FileInfo> x = new List<FileInfo>();
            DirectoryInfo d = new DirectoryInfo(playlist);

            foreach (FileInfo file in d.GetFiles("*.*"))
            {
                if (FileManager.isSupported(file.FullName))
                    x.Add(file);
            }

            return x;
        }

        public static List<string> getSongPathsAsStrings(string playlist)
        {
            List<string> x = new List<string>();
            DirectoryInfo d = new DirectoryInfo(playlist);

            foreach (FileInfo file in d.GetFiles("*.*"))
            {
                if (FileManager.isSupported(file.FullName))
                    x.Add(file.FullName);
            }

            return x;
        }

        public static List<string> getTagsOfPlaylist(string playlist)
        {
            var pl = getSongPathsAsFileInfos(playlist);
            
            return getTagsOfPlaylist(pl.ToArray());
        }

        public static List<string> getTagsOfPlaylist(FileInfo[] pl)
        {
            List<string> tags = new List<string>();
            var fis = new List<FileInfo>(pl.OrderByDescending(f => f.LastWriteTime).Take(4));

            foreach (var fi in fis)
            {
                TagLib.File tag = TagLib.File.Create(fi.FullName);

                if (tag.Tag.Title is null)
                    tags.Add(Path.GetFileNameWithoutExtension(fi.FullName));
                else
                    tags.Add(tag.Tag.Title);
            }

            return tags;
        }

        public static string GetLastWriteDateOfPlaylists(string playlist)
        {
            var songs = getSongPathsAsFileInfos(playlist);
            if (songs.Count == 0)
            {
                var ret = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToString("d MM yyyy");
                return ret;
            }
            FileInfo file = songs.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
            return file.LastWriteTime.ToString("d MMM yyyy");
        }

        public static string GetLastWriteDateOfPlaylists(FileInfo[] playlist)
        {
            if (playlist.Length == 0)
            {
                var ret = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToString("d MM yyyy");
                return ret;
            }
            FileInfo file = playlist.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
            return file.LastWriteTime.ToString("d MMM yyyy");
        }

        public class DetailedPlaylist
        {
            public string name { get; set; }
            public string description { get; set; }
            public string date { get; set; }
            public string[] tags { get; set; }

            public static explicit operator DetailedPlaylist(FullPlaylist bar)
            {
                return new DetailedPlaylist { name = bar.name, description = bar.description, date = bar.date, tags = bar.tags };
            }
        }

        public class FullPlaylist
        {
            public string name { get; set; }
            public string description { get; set; }
            public string date { get; set; }
            public string[] tags { get; set; }

            public MediaPlayer.Song[] songs;

            public static implicit operator FullPlaylist(DetailedPlaylist bar)
            {
                return new FullPlaylist { name = bar.name, description = bar.description, date = bar.date, tags = bar.tags};
            }

            public static FullPlaylist FromSongList(List<MediaPlayer.Song> songs)
            {
                if (songs is null || songs.Count <= 0)
                {
                    return null;
                }

                FullPlaylist t = GetDetailedPlaylist(Path.GetDirectoryName(songs.FirstOrDefault().location));
                t.songs = songs.ToArray();
                return t;
            }
        }

        public struct AllDetailedPlaylists
        {
            public List<DetailedPlaylist> autoplaylists;
            public List<DetailedPlaylist> customplaylists;
        }

        public static KeyValuePair<int, AllDetailedPlaylists> FindDetailedPlaylist(string query)
        {
            var apls = getDetailedPlaylists();
            var pls = apls.autoplaylists.Concat(apls.customplaylists).ToList();
            var index = pls.FindIndex(x => x.name.ToLower().Contains(query.ToLower()));
            return new KeyValuePair<int, AllDetailedPlaylists>(index, apls);
        }

        public static AllDetailedPlaylists getDetailedPlaylists()
        {
            AutoPlaylists.updateSpecialPlaylists();

            AllDetailedPlaylists allDetailedPlaylists = new AllDetailedPlaylists();
            allDetailedPlaylists.customplaylists = new List<DetailedPlaylist>();
            allDetailedPlaylists.autoplaylists = new List<DetailedPlaylist>();

            var s = AutoPlaylists.getSpecialPlaylists();
            foreach (var cs in s)
            {
                allDetailedPlaylists.autoplaylists.Add((DetailedPlaylist)cs);
            }

            var pls = getPlaylistPathsAsStrings();

            foreach (var pl in pls) // customplaylists
            {
                var t = GetDetailedPlaylist(pl);
                t.description += " songs, hand picked by you";
                allDetailedPlaylists.customplaylists.Add(t);
            }

            var vpls = virtualPlaylists;

            foreach (var vpl in vpls)
            {
                var t = GetDetailedPlaylist(vpl);
                t.name = "♺ " + t.name;
                t.description += " songs, hand picked by you, stored virtually";
                allDetailedPlaylists.customplaylists.Add(t);
            }

            return allDetailedPlaylists;
        }

        public static DetailedPlaylist GetDetailedPlaylist(KeyValuePair<string, string[]> playlist)
        {
            var fis = playlist.Value.Select(x => new FileInfo(x)).ToArray();
            DetailedPlaylist t = new DetailedPlaylist();
            t.tags = getTagsOfPlaylist(fis).ToArray();
            t.description = fis.Length.ToString();
            t.name = playlist.Key;
            t.date = GetLastWriteDateOfPlaylists(fis);
            return t;
        }

        public static DetailedPlaylist GetDetailedPlaylist(string playlist)
        {
            DetailedPlaylist t = new DetailedPlaylist();
            t.tags = getTagsOfPlaylist(playlist).ToArray();
            t.description = getSongPathsAsStrings(playlist).Count.ToString();
            t.name = new DirectoryInfo(playlist).Name;
            t.date = GetLastWriteDateOfPlaylists(playlist);
            return t;
        }

        public static class AutoPlaylists
        {
            public static List<FileInfo> recentsFI;
            public static List<FileInfo> favouritesFI;

            public static List<FullPlaylist> getSpecialPlaylists(SpecialPlaylists playlists = SpecialPlaylists.All)
            {
                updateSpecialPlaylists();
                List<FullPlaylist> list = new List<FullPlaylist>();

                if (playlists == SpecialPlaylists.Breaking || playlists == SpecialPlaylists.All)
                {
                    FullPlaylist t = new FullPlaylist();
                    t.tags = getTagsOfPlaylist(recentsFI.ToArray()).ToArray();
                    t.name = "Breaking";
                    t.description = "your 20 newest tracks";
                    t.date = DateTime.Now.ToString("d MMM yyyy");
                    t.songs = MediaPlayer.GetPlaylist(recentsFI.Select(x => x.FullName).ToList(), logger).ToArray();
                    list.Add(t);
                }

                if (playlists == SpecialPlaylists.Favourites || playlists == SpecialPlaylists.All)
                {
                    FullPlaylist t = new FullPlaylist();
                    t.tags = getTagsOfPlaylist(favouritesFI.ToArray()).ToArray();
                    t.name = "Favourites";
                    t.description = "your 20 favourite tracks";
                    t.date = DateTime.Now.ToString("d MMM yyyy");
                    t.songs = MediaPlayer.GetPlaylist(favouritesFI.Select(x => x.FullName).ToList(), logger).ToArray();
                    list.Add(t);
                }

                return list;
            }

            public static void updateSpecialPlaylists(SpecialPlaylists playlists = SpecialPlaylists.All)
            {
                var allPlaylists = getPlaylistPathsAsStrings();

                if (playlists == SpecialPlaylists.Breaking || playlists == SpecialPlaylists.All)
                {
                    List<FileInfo> infos = new List<FileInfo>();

                    foreach (string playlist in allPlaylists)
                    {
                        infos.AddRange(getSongPathsAsFileInfos(playlist));
                    }

                    recentsFI = new List<FileInfo>(infos.OrderByDescending(f => f.LastWriteTime).Take(20));
                }

                if (playlists == SpecialPlaylists.Favourites || playlists == SpecialPlaylists.All)
                {
                    var playCounts = Logger.GetPlayCountCache();

                    var playlist = playCounts
                        .OrderByDescending(x => x.Value)
                        .Take(20)
                        .Select(x => x.Key)
                        .ToArray();

                    favouritesFI = getSongPathsAsFileInfos(playlist);
                }
            }

            public enum SpecialPlaylists
            {
                Favourites, Breaking, All
            }
        }
    }
}
