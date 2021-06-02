using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML
{
    public static class PlaylistManager
    {
        public static Logger logger;

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

        public static string getLastWriteDateOfPlaylists(string playlist)
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

        public struct DetailedPlaylist
        {
            public string name;
            public string description;
            public string date;
            public string[] tags;
        }

        public struct AllDetailedPlaylists
        {
            public List<DetailedPlaylist> autoplaylists;
            public List<DetailedPlaylist> customplaylists;
        }

        public static AllDetailedPlaylists getDetailedPlaylists()
        {
            AutoPlaylists.updateSpecialPlaylists();

            AllDetailedPlaylists allDetailedPlaylists = new AllDetailedPlaylists();
            allDetailedPlaylists.customplaylists = new List<DetailedPlaylist>();
            allDetailedPlaylists.autoplaylists = new List<DetailedPlaylist>();

            allDetailedPlaylists.autoplaylists.AddRange(AutoPlaylists.getSpecialPlaylists());
            var pls = getPlaylistPathsAsStrings();

            foreach (var pl in pls) // customplaylists
            {
                var t = getDetailedPlaylist(pl);
                t.description += " songs, handpicked by you";
                allDetailedPlaylists.customplaylists.Add(t);
            }

            return allDetailedPlaylists;
        }

        public static DetailedPlaylist getDetailedPlaylist(string playlist)
        {
            DetailedPlaylist t = new DetailedPlaylist();
            t.tags = getTagsOfPlaylist(playlist).ToArray();
            t.description = getSongPathsAsStrings(playlist).Count.ToString();
            t.name = new DirectoryInfo(playlist).Name;
            t.date = getLastWriteDateOfPlaylists(playlist);
            return t;
        }

        public static class AutoPlaylists
        {
            public static List<FileInfo> recentsFI;

            public static List<DetailedPlaylist> getSpecialPlaylists(SpecialPlaylists playlists = SpecialPlaylists.All)
            {
                List<DetailedPlaylist> list = new List<DetailedPlaylist>();

                if (playlists == SpecialPlaylists.Recents || playlists == SpecialPlaylists.All)
                {
                    DetailedPlaylist t = new DetailedPlaylist();
                    t.tags = getTagsOfPlaylist(recentsFI.ToArray()).ToArray();
                    t.name = "Breaking";
                    t.description = "your 20 newest tracks";
                    t.date = DateTime.Now.ToString("d MMM yyyy");
                    list.Add(t);
                }

                return list;
            }

            public static void updateSpecialPlaylists(SpecialPlaylists playlists = SpecialPlaylists.All)
            {
                var allPlaylists = getPlaylistPathsAsStrings();

                if (playlists == SpecialPlaylists.Recents || playlists == SpecialPlaylists.All)
                {
                    List<FileInfo> infos = new List<FileInfo>();

                    foreach (string playlist in allPlaylists)
                    {
                        infos.AddRange(getSongPathsAsFileInfos(playlist));
                    }

                    recentsFI = new List<FileInfo>(infos.OrderByDescending(f => f.LastWriteTime).Take(20));
                }
            }

            public enum SpecialPlaylists
            {
                Favourites, Recents, All
            }
        }
    }
}
