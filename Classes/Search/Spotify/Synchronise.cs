using Newtonsoft.Json;
using Phonix;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace reAudioPlayerML.Search.Spotify
{
    public static class Synchronise
    {
        private static List<PlaylistTrack<IPlayableItem>> syncPlaylist;

        public static void Initialise()
        {
            InitContextMenus();
            UIHandler.syncView.SelectedIndexChanged += SyncView_SelectedIndexChanged;
        }

        private static void SyncView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UIHandler.syncView.SelectedIndices.Count == 0 || selectedIndex >= UIHandler.syncView.Items.Count)
            {
                return;
            }

            if (selectedIndex >= 0)
            {
                UIHandler.syncView.Items[selectedIndex].BackColor = Color.FromArgb(33, 33, 33); // normal item
            }

            selectedIndex = UIHandler.syncView.SelectedIndices[0];

            UIHandler.syncView.Items[selectedIndex].BackColor = Color.FromArgb(77, 77, 77); // highlighted item
            UIHandler.syncView.Items[selectedIndex].Selected = false;
        }

        public static void InitContextMenus()
        {
            foreach (object item in UIHandler.ctxSync.Items)
            {
                Type type = item.GetType();

                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem button = item as ToolStripMenuItem;
                    button.Click += Button_Click;

                    if (button.Name == "btnAddToPlaylist")
                    {
                        UIHandler.ctxSyncPlaylists = button.DropDownItems[0] as ToolStripComboBox;
                    }
                }
            }
        }

        static int selectedIndex = -1;

        private static void Button_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            OpenFileDialog ofd;

            ReleaseRadar.Release album;

            ProcessStartInfo ps = new ProcessStartInfo()
            {
                UseShellExecute = true,
                Verb = "open"
            };

            switch (item.Name)
            {
                case "toolStripMenuItemPreviewSpotify":
                    onPreviewRequest(sender, e);
                    break;
                case "toolStripMenuItemPreviewLocal":
                    onPreviewRequest(sender, e);
                    break;
                case "toolStripMenuItemOpenSpotify":
                    ps.FileName = (syncPlaylist[selectedIndex].Track as FullTrack).Uri;
                    ps.FileName = "https://open.spotify.com/" + ps.FileName.Replace("spotify:", "").Replace(':', '/');
                    Process.Start(ps);
                    break;
                case "toolStripMenuItemShowInExplorer":
                    ps.FileName = "explorer";
                    ps.Arguments = "/select," + UIHandler.syncView.Items[selectedIndex].SubItems[1].Text;
                    Process.Start(ps);
                    break;
                case "toolStripMenuItemChangeFile":
                    ofd = new OpenFileDialog();
                    ofd.Title = $"Searching for '{UIHandler.syncView.Items[selectedIndex].Text}'";
                    ofd.Filter = "MP3 Songs|*.mp3";
                    if (ofd.ShowDialog() == DialogResult.Cancel)
                    {
                        break;
                    }
                    Init.logger.addSongToDB(ofd.FileName);
                    addOrReplace((syncPlaylist[selectedIndex].Track as FullTrack).Id, ofd.FileName);
                    UIHandler.syncView.Items[selectedIndex].SubItems[1].Text = ofd.FileName;
                    break;
                case "toolStripMenuItemChangeSpotify":
                    if (string.IsNullOrEmpty((sender as ToolStripMenuItem).DropDownItems["link"].Text))
                    {
                        return;
                    }

                    var t = UIHandler.syncView.Items[selectedIndex].SubItems[1].Text;

                    var link = (sender as ToolStripMenuItem).DropDownItems["link"].Text.Split("/");
                    var trackId = link[link.Length - 1].Split("?")[0];
                    var slc = spotifyLinkCache;
                    slc.Add(t, trackId);
                    spotifyLinkCache = slc;
                    var track = Init.Client.Tracks.Get(trackId).Result;
                    Search.SpotifyPreview.ClearCachedTrack(syncPlaylist[selectedIndex].Track as SimpleTrack);
                    syncPlaylist[selectedIndex].Track = track;
                    UIHandler.syncView.Items[selectedIndex].SubItems[0].Text = getDisplayName(track);
                    (sender as ToolStripMenuItem).DropDownItems["link"].Text = "";
                    break;
                case "toolStripMenuItemRecommend":
                    Recommender form = new Recommender((syncPlaylist[selectedIndex].Track as FullTrack));
                    form.Show();
                    break;
            }
        }

        private static void onPreviewRequest(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                if ((sender as ToolStripMenuItem).Name == "toolStripMenuItemPreviewSpotify")
                {
                    new SpotifyPreview(syncPlaylist[selectedIndex].Track as FullTrack);
                    return;
                }
                else if ((sender as ToolStripMenuItem).Name == "toolStripMenuItemPreviewLocal")
                {
                    string filename = UIHandler.syncView.Items[selectedIndex].SubItems[1].Text;

                    if (filename == "N/A")
                    {
                        MessageBox.Show("no local equivalent has been found :/");
                        return;
                    }

                    string displayname = getDisplayName(new FileInfo(filename));
                    PlayerManager.mediaPlayer.playIndependent(filename, displayname.Split('-')[1].Trim(), displayname.Split('-')[0].Trim(), 300);
                    return;
                }
            }
        }

        private static Dictionary<string, string> spotifyLinkCache
        {
            set
            {
                Properties.Settings.Default.spotifyLinkCache = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
            get
            {
                var t = JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.spotifyLinkCache);
                
                if (t is null)
                {
                    return new Dictionary<string, string>();
                }

                if (t.Where(x => File.Exists(x.Value)).Count() > 0)
                {
                    var s = t.Where(x => t.Where(y => y.Value == x.Value).Count() == 1);
                    return s.ToDictionary(x => File.Exists(x.Key) ? x.Key : x.Value, x => File.Exists(x.Key) ? x.Value : x.Key);
                }

                return t.Where(x => File.Exists(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public static void LocalToSpotify()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {

                bool takeDir = true;
                string[] files = { };

                UIHandler.txtSyncIn.Invoke(new Action(() =>
                {
                    TaskDialogPage tdp = new TaskDialogPage();

                    var btnFolder = new TaskDialogCommandLinkButton("Folder", "all files from this folder will be synchronised");
                    var btnFiles = new TaskDialogCommandLinkButton("Files", "let me select the one or multiple files to be synchronised");

                    tdp.Caption = "reAudioPlayer Apollo - Spotify Sync";
                    tdp.Text = "How do you want to select your input?";
                    tdp.Buttons.Add(btnFolder);
                    tdp.Buttons.Add(btnFiles);
                    tdp.Buttons.Add(TaskDialogButton.Cancel);
                    tdp.Icon = new TaskDialogIcon(SystemIcons.Question);

                    var mode = TaskDialog.ShowDialog(tdp);

                    if (mode == TaskDialogButton.Cancel)
                    {
                        return;
                    }

                    takeDir = mode == btnFolder;

                    if (takeDir)
                    {
                        if (!Directory.Exists(UIHandler.txtSyncIn.Text))
                        {
                            FolderBrowserDialog fbd = new FolderBrowserDialog();

                            if (fbd.ShowDialog() == DialogResult.Cancel)
                            {
                                return;
                            }

                            UIHandler.txtSyncIn.Text = fbd.SelectedPath;
                            files = Directory.GetFiles(UIHandler.txtSyncIn.Text, "*.mp3");
                        }
                    }
                    else
                    {
                        OpenFileDialog ofd = new OpenFileDialog()
                        {
                            Filter = "mp3 Files (*.mp3)|*.mp3",
                            Multiselect = true
                        };

                        if (ofd.ShowDialog() == DialogResult.Cancel)
                        {
                            return;
                        }

                        files = ofd.FileNames;
                    }
                }));

                if (files.Length == 0)
                {
                    return;
                }

                syncPlaylist = new List<PlaylistTrack<IPlayableItem>>();

                UIHandler.syncView.Invoke(new Action(() =>
                {
                    UIHandler.syncView.Items.Clear();
                }));

                for (int i = 0; i < files.Length; i++)
                {
                    var spotify = getMatchingSpotifySong(files[i]);
                    PlaylistTrack<IPlayableItem> t = new PlaylistTrack<IPlayableItem>();
                    t.Track = spotify;
                    syncPlaylist.Add(t);
                }

                var tracks = getAllTracks(syncPlaylist.Where(x => x.Track is not null && (x.Track as FullTrack).Name is null).Select(x => (x.Track as FullTrack).Id).ToList());
                foreach (var track in tracks)
                {
                    var index = syncPlaylist.FindIndex(x => x.Track is not null && (x.Track as FullTrack).Id == track.Id);
                    syncPlaylist[index] = new PlaylistTrack<IPlayableItem>() { Track = track };
                }

                for (int i = 0; i < files.Length; i++)
                {
                    UIHandler.syncView.Invoke(new Action(() =>
                    {
                        UIHandler.lblSyncProgress.Text = $"{i + 1} / {files.Length}";
                        UIHandler.syncView.Items.Add(getDisplayName(syncPlaylist[i].Track as FullTrack));
                        UIHandler.syncView.Items[i].SubItems.Add(files[i]);

                        UIHandler.syncView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }));
                }

                sw.Stop();

                UIHandler.lblSyncProgress.Invoke(new Action(() => UIHandler.lblSyncProgress.Text = $"{syncPlaylist.Count} songs analysed in {sw.Elapsed.ToString()}"));
            }
            catch (APITooManyRequestsException)
            {
                UIHandler.lblSyncProgress.Invoke(new Action(() => UIHandler.lblSyncProgress.Text = "Rate Limit Exceeded"));
            }
        }

        private static List<FullTrack> getAllTracks(List<string> trackIds)
        {
            List<FullTrack> ret = new List<FullTrack>();

            while (trackIds.Count > 0)
            {
                try
                {
                    ret.AddRange(Init.Client.Tracks.GetSeveral(new TracksRequest(trackIds.Take(50).ToArray())).Result.Tracks);
                } catch { }
                trackIds.RemoveRange(0, trackIds.Count > 50 ? 50 : trackIds.Count);
            }

            return ret;
        }

        public static FullTrack getMatchingSpotifySong(string song)
        {
            try
            {
                Dictionary<string, string> localCache = spotifyLinkCache is null ? new Dictionary<string, string>() : spotifyLinkCache;

                if (localCache.ContainsValue(song))
                {
                    string trackId = localCache.Where(x => x.Value == song).FirstOrDefault().Key;

                    return new FullTrack() { Id = trackId };
                    //return client.Tracks.Get(trackId).Result; // with several
                }

                var tag = TagLib.File.Create(song);
                string q = $"{string.Join(" ", tag.Tag.Performers)} {tag.Tag.Title}";
                var search = Init.Client.Search.Item(new SearchRequest(SearchRequest.Types.Track, q)).Result.Tracks;
                var item = search.Items.FirstOrDefault();

                if (item is null)
                {
                    q = Path.GetFileNameWithoutExtension(song).Replace(" & ", " ").Replace(" - ", " ").Replace(" x ", " ").Replace("(Extended Mix)", "");
                    search = Init.Client.Search.Item(new SearchRequest(SearchRequest.Types.Track, q)).Result.Tracks;
                    item = search.Items.FirstOrDefault();
                }

                return item;
            }
            catch
            {
                UIHandler.lblSyncProgress.Invoke(new Action(() => UIHandler.lblSyncProgress.Text = "Rate Limit Exceeded @ " + Path.GetFileNameWithoutExtension(song)));
            }

            return null;
        }

        private static string getMatchingLocalSong(ref FullTrack track, ref List<string> lib)
        {
            Dictionary<string, string> localCache = spotifyLinkCache is null ? new Dictionary<string, string>() : spotifyLinkCache;
            string trackId = track.Id;

            if (localCache.ContainsValue(trackId))
            {
                string file = localCache.FirstOrDefault(x => x.Value == trackId).Key;

                if (File.Exists(file) && lib.Contains(file))
                {
                    return file;
                }
            }

            string name = getDisplayName(track);

            Dictionary<string, string> localNLibrary = new Dictionary<string, string>();
            string bestNMatch;

            foreach (string st in lib)
            {
                if (File.Exists(st))
                {
                    bool add = localNLibrary.TryAdd(Path.GetFileNameWithoutExtension(st), st);
                }
            }

            bool inN = localNLibrary.TryGetValue(track.Name, out bestNMatch);
            Dictionary<string, string> matches = new Dictionary<string, string>();

            if (inN)
            {
                double accuracy = artistMatch(track, name);

                if (accuracy > 0.5)
                {
                    addOrReplace(track.Id, bestNMatch);
                    return bestNMatch;
                }
            }
            foreach (string s in lib)
            {
                if (File.Exists(s))
                {
                    string dname = getDisplayName(new FileInfo(s));
                    DoubleMetaphone doubleMetaphone = new DoubleMetaphone();
                    if (doubleMetaphone.IsSimilar(new string[] { dname, name }))
                    {
                        matches.TryAdd(s, dname);
                    }
                }
            }

            string bestMatch = "N/A";
            int bestDistance = 999;

            foreach (KeyValuePair<string, string> match in matches)
            {
                int dist = LevenshteinDistance.Compute(match.Value, name);

                if (dist < bestDistance)
                {
                    bestMatch = match.Key;
                    bestDistance = dist;
                }
            }

            addOrReplace(track.Id, bestMatch);

            return bestMatch;
        }

        private static double artistMatch(FullTrack track, string dispname)
        {
            int matches = 0;

            foreach (SimpleArtist artist in track.Artists)
            {
                if (dispname.ToLower().Contains(artist.Name.ToLower()))
                {
                    matches++;
                }
            }

            return matches / track.Artists.Count;
        }

        private static string getDisplayName(FileInfo file)
        {
            TagLib.File tag = TagLib.File.Create(file.FullName);

            string name = tag.Tag.Performers.FirstOrDefault();

            for (int i = 1; i < tag.Tag.Performers.Length; i++)
            {
                name += $", {tag.Tag.Performers[i]}";
            }

            name += $" - {tag.Tag.Title}";

            return name;
        }

        private static string getDisplayName(FullTrack track)
        {
            if (track is null)
            {
                return "N/A";
            }

            string name = "";

            try
            {
                name = track.Artists.FirstOrDefault().Name;
            } catch { }

            for (int i = 1; i < track.Artists.Count; i++)
            {
                name += $", {track.Artists[i].Name}";
            }

            name += $" - {track.Name}";

            return name;
        }

        private static void addOrReplace(string id, string file)
        {
            Dictionary<string, string> localCache = spotifyLinkCache is null ? new Dictionary<string, string>() : spotifyLinkCache;

            if (localCache.ContainsKey(file))
            {
                localCache[file] = id;
                spotifyLinkCache = localCache;
                return;
            }

            localCache.Add(file, id);
            spotifyLinkCache = localCache;
        }

        public static void SpotifyToLocalByIndex(int index)
        {
            string i = Init.Playlists[index].Id;
            SpotifyToLocalById(i);
        }

        private static string synchedPlaylist = "";

        public static void SpotifyToLocalByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            { return; }

            synchedPlaylist = name;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            string i = Init.Playlists.Find(x => x.Name == name)?.Id;
            try
            {
                SpotifyToLocalById(i);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            sw.Stop();

            UIHandler.lblSyncProgress.Invoke(new Action(() => UIHandler.lblSyncProgress.Text = $"{syncPlaylist?.Count} songs analysed in {sw.Elapsed.ToString()}"));
        }

        public static void SpotifyToLocalById(string id)
        {
            List<string> lib;
            string dir;
            int index = 0; // default mode

            switch (index)
            {
                case 0:
                    lib = new List<string>(File.ReadAllLines(Init.logger.songLib));
                    break;

                case 1:
                    dir = "";

                    UIHandler.txtSyncIn.Invoke(new Action(() =>
                    {
                        dir = UIHandler.txtSyncIn.Text;

                        if (!Directory.Exists(dir))
                        {
                            FolderBrowserDialog fbd = new FolderBrowserDialog();
                            fbd.ShowNewFolderButton = true;

                            if (fbd.ShowDialog() == DialogResult.Cancel)
                                return;

                            dir = UIHandler.txtSyncIn.Text = fbd.SelectedPath;
                        }
                    }));

                    if (dir == "")
                    {
                        return;
                    }

                    lib = Directory.GetFiles(dir, "*.mp3").ToList();
                    break;

                default:
                    return;
            }

            if (Init.Playlists.FindIndex(x => x.Id == id) < 0)
            {
                return;
            }

            UIHandler.syncView.Invoke(new Action(() => UIHandler.syncView.Items.Clear()));

            FullPlaylist tracks = Init.Client.Playlists.Get(id).Result;
            IList<PlaylistTrack<IPlayableItem>> test = Init.Client.PaginateAll(tracks.Tracks).Result;
            syncPlaylist = test as List<PlaylistTrack<IPlayableItem>>;

            for (int i = 0; i < syncPlaylist.Count; i++)
            {
                FullTrack track = syncPlaylist[i].Track as SpotifyAPI.Web.FullTrack;

                string displayname = getDisplayName(track);

                string localFile = getMatchingLocalSong(ref track, ref lib);

                UIHandler.syncView.Invoke(new Action(() =>
                {
                    UIHandler.lblSyncProgress.Text = $"{i + 1} / {syncPlaylist.Count}";
                    UIHandler.syncView.Items.Add(displayname);
                    UIHandler.syncView.Items[i].SubItems.Add(localFile);

                    UIHandler.syncView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }));
            }
        }

        public static void ExportSyncedPlaylist()
        {
            bool overwrite = false, fetchMetadata = false;

            TaskDialogPage td = new TaskDialogPage();
            var btnApply = new TaskDialogCommandLinkButton("Apply", "adds spotify metadata to the current local files");
            var btnRenameApply = new TaskDialogCommandLinkButton("Rename & Apply", "adds spotify metadata to the current local files, but also renames it \"[Artist] - [Title]\"");
            var btnCopyApply = new TaskDialogCommandLinkButton("Copy & Apply", "adds spotify metadata to copies of your local files");
            var btnLoad = new TaskDialogCommandLinkButton("Load As Playlist", "loads all songs as a playlist");
            var btnVirtual = new TaskDialogCommandLinkButton("Create Virtual Playlist", "creates a virtual playlist, leaving your files untouched");
            var btnCopy = new TaskDialogCommandLinkButton("Copy To Clipboard", "copies all files to your clipboard");

            td.Text = "How Do you want to export?";
            td.Expander.Text = "Fetches: Artist, Title, Album, BPM, Key, Popularity, Energy, Cover, Loudness, ...";
            td.Buttons = new TaskDialogButtonCollection() { btnApply, btnRenameApply, btnCopyApply, btnLoad, btnVirtual, btnCopy, TaskDialogButton.Cancel };
            td.Verification = new TaskDialogVerificationCheckBox("don't apply metadata");
            var mode = TaskDialog.ShowDialog(td);

            if (mode == TaskDialogButton.Cancel)
            {
                return;
            }

            var files = UIHandler.syncView.Items
                    .Cast<ListViewItem>()
                    .Select(x => x.SubItems[1].Text)
                    .ToArray();

            if (mode == btnLoad)
            {
                PlayerManager.loadPlaylist(files);
                return;
            }

            if (mode == btnCopy)
            {
                var t = new System.Collections.Specialized.StringCollection();
                t.AddRange(files);
                Clipboard.SetFileDropList(t);
                return;
            }

            if (mode == btnVirtual)
            {
                PlaylistManager.Create(files, synchedPlaylist);
                return;
            }

            fetchMetadata = !td.Verification.Checked;
            overwrite = mode == btnApply || mode == btnRenameApply;

            if (string.IsNullOrEmpty(UIHandler.txtSyncOut.Text))
            {
                if (mode == btnCopyApply)
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    fbd.ShowNewFolderButton = true;

                    if (fbd.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        UIHandler.txtSyncOut.Text = fbd.SelectedPath;
                    }
                }
            }

            Dictionary<string, TrackAudioFeatures> featureCache = Wrapper.SpotifyFeatureCache;

            // get features ?
            if (fetchMetadata)
            {
                List<string> missingIds = new List<string>();

                foreach (ListViewItem item in UIHandler.syncView.Items)
                {
                    int index = UIHandler.syncView.Items.IndexOf(item);
                    FullTrack ft = syncPlaylist[index].Track as FullTrack;

                    if (ft is not null && !featureCache.ContainsKey(ft.Id))
                    {
                        missingIds.Add(ft.Id);
                    }
                }

                if (missingIds.Count > 0)
                {
                    while (missingIds.Count > 0)
                    {
                        TracksAudioFeaturesRequest req = new TracksAudioFeaturesRequest(missingIds.Take(100).ToArray());
                        List<TrackAudioFeatures> res = Init.Client.Tracks.GetSeveralAudioFeatures(req).Result.AudioFeatures;

                        foreach (TrackAudioFeatures re in res)
                        {
                            featureCache.TryAdd(re.Id, re);
                        }

                        missingIds.RemoveRange(0, missingIds.Count > 50 ? 50 : missingIds.Count);
                    }

                    Wrapper.SpotifyFeatureCache = featureCache;
                }
            }

            Dictionary<string, string> localCache = spotifyLinkCache is null ? new Dictionary<string, string>() : spotifyLinkCache;

            // actually exporting
            foreach (ListViewItem item in UIHandler.syncView.Items)
            {
                string file = item.SubItems[1].Text;
                int index = UIHandler.syncView.Items.IndexOf(item);

                if (File.Exists(file))
                {
                    if (!fetchMetadata)
                    {
                        File.Copy(file, Path.Combine(UIHandler.txtSyncOut.Text, Path.GetFileName(file)));
                    }
                    else
                    {
                        string noCopyOutput = mode == btnRenameApply ? Path.Combine(Path.GetDirectoryName(file), item.Text + Path.GetExtension(file)) : file;

                        string output = overwrite ?
                            noCopyOutput :                            
                            Path.Combine(UIHandler.txtSyncOut.Text, item.Text + Path.GetExtension(file));

                        FullTrack ft = syncPlaylist[index].Track as FullTrack;

                        if (file != output)
                        {
                            File.Copy(file, output, true);

                            if (overwrite)
                            {
                                File.Delete(file);
                            }

                            item.SubItems[1].Text = output;

                            if (localCache.ContainsKey(file))
                            {
                                localCache.Add(output, localCache[file]);
                                localCache.Remove(file);
                            }
                        }

                        TagLib.File tag = TagLib.File.Create(output);

                        if (ft is not null)
                        {
                            tag.Tag.Comment = new SpotifyComment(featureCache[ft.Id], ft.Popularity, ft.Album.ReleaseDate).ToString();
                            tag.Tag.Title = ft.Name;
                            tag.Tag.Performers = getExportArtist(ft.Artists.Select(x => x.Name).ToArray());
                            tag.Tag.Album = ft.Album.Name;
                            tag.Tag.BeatsPerMinute = (uint)Math.Round(featureCache[ft.Id].Tempo);

                            if (ft.Album.Images.Count > 0) // fetch album cover
                            {
                                WebClient wc = new WebClient();
                                byte[] bytes = wc.DownloadData(ft.Album.Images[0].Url);

                                TagLib.Id3v2.AttachedPictureFrame cover = new TagLib.Id3v2.AttachedPictureFrame
                                {
                                    Type = TagLib.PictureType.FrontCover,
                                    Description = "Cover",
                                    MimeType = System.Net.Mime.MediaTypeNames.Image.Jpeg,
                                    Data = bytes,
                                    TextEncoding = TagLib.StringType.UTF16
                                };

                                tag.Tag.Pictures = new TagLib.IPicture[] { cover };
                            }

                            try
                            {
                                tag.Save();
                            }
                            catch
                            {
                                MessageBox.Show(file + " couldn't be saved, close it and try again...");
                            }
                        }
                    }
                }
            }

            spotifyLinkCache = localCache;

            MessageBox.Show("Done");
        }

        private static string[] getExportArtist(string[] artists)
        {
            return string.IsNullOrWhiteSpace(UIHandler.txtArtistDelimiter.Text) ? artists : new string[] { string.Join(UIHandler.txtArtistDelimiter.Text, artists) };
        }

        public class SpotifyComment
        {
            public SpotifyComment() { }
            public SpotifyComment(TrackAudioFeatures features, int popularity, string releaseDate)
            {
                energy = (int)Math.Round(features.Energy * 100);
                danceability = (int)Math.Round(features.Danceability * 100);
                happiness = (int)Math.Round(features.Valence * 100);
                loudness = (int)Math.Round(features.Loudness * 100);
                acousticness = (int)Math.Round(features.Acousticness * 100);
                instrumentalness = (int)Math.Round(features.Instrumentalness * 100);
                liveness = (int)Math.Round(features.Liveness * 100);
                speechiness = (int)Math.Round(features.Speechiness * 100);
                key = features.Key.ToString();
                this.popularity = popularity;
                this.releaseDate = releaseDate;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public static SpotifyComment FromString(string comment)
            {
                try
                {
                    return comment is null ? new SpotifyComment() : JsonConvert.DeserializeObject<SpotifyComment>(comment);
                }
                catch
                {
                    return new SpotifyComment();
                }
            }

            public int popularity, energy, danceability, happiness, loudness, acousticness, instrumentalness, liveness, speechiness;
            public string key, camelot, releaseDate;
        }

        /* for matchingLocalSong */
        private static class LevenshteinDistance
        {
            public static int Compute(string s, string t)
            {
                if (string.IsNullOrEmpty(s))
                {
                    if (string.IsNullOrEmpty(t))
                    {
                        return 0;
                    }

                    return t.Length;
                }

                if (string.IsNullOrEmpty(t))
                {
                    return s.Length;
                }

                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // initialize the top and right of the table to 0, 1, 2, ...
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                    ;
                }

                for (int j = 1; j <= m; d[0, j] = j++)
                {
                    ;
                }

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= m; j++)
                    {
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                        int min1 = d[i - 1, j] + 1;
                        int min2 = d[i, j - 1] + 1;
                        int min3 = d[i - 1, j - 1] + cost;
                        d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                    }
                }
                return d[n, m];
            }
        }
    }
}
