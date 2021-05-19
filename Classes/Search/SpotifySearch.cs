using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Phonix;
using RestSharp;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reAudioPlayerML.Search
{
    public class Spotify
    {
        public SpotifyClient client;
        private readonly string clientid = Settings.APIKeys.spotify.id;
        private readonly string clientsecret = Settings.APIKeys.spotify.secret;
        private readonly string redirectUri = "http://reap.ml/callback/";
        public bool ready { get; private set; }

        private ListView listView;
        private readonly ListView syncView;
        private SpotifyLinker spotifyLinker;
        private readonly ContextMenuStrip contextMenu;
        private readonly ContextMenuStrip contextMenuSync;
        public ToolStripComboBox playlists;
        public List<SimplePlaylist> spotifyPlaylists;
        private List<Release> releases;
        private readonly NotifyIcon notifyIcon;
        private readonly MediaPlayer player;
        private readonly Logger logger;

        private TextBox txtSyncIn;
        private TextBox txtSyncOut;
        private ComboBox cmbSyncPlaylist;
        private Label lblSyncProgress;

        List<FullTrack> localPlaylistOnSpotify = new List<FullTrack>();

        private Dictionary<string, object> spotifyCache
        {
            set
            {
                Properties.Settings.Default.spotifyCache = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
            get
            {
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(Properties.Settings.Default.spotifyCache);
            }
        }

        private Dictionary<string, string> spotifyLinkCache
        {
            set
            {
                Properties.Settings.Default.spotifyLinkCache = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
            get
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(Properties.Settings.Default.spotifyLinkCache);
            }
        }

        public Dictionary<string, TrackAudioFeatures> spotifyFeatureCache
        {
            set
            {
                Properties.Settings.Default.spotifyFeatureCache = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
            get
            {
                return JsonConvert.DeserializeObject<Dictionary<string, TrackAudioFeatures>>(Properties.Settings.Default.spotifyFeatureCache);
            }
        }

        private List<PlaylistTrack<IPlayableItem>> syncPlaylist;
        private readonly Dictionary<string, int> selectedIndex = new Dictionary<string, int>();

        public Spotify(ListView listView,
            ListView syncView,
            ContextMenuStrip contextMenu,
            ContextMenuStrip contextMenuSync,
            TextBox txtSyncIn,
            TextBox txtSyncOut,
            ComboBox cmbSyncPlaylist,
            Label lblSyncProgress,
            NotifyIcon notifyIcon,
            MediaPlayer player,
            Logger logger)
        {
            ready = false;

            this.listView = listView;
            this.contextMenu = contextMenu;
            this.contextMenuSync = contextMenuSync;
            this.notifyIcon = notifyIcon;
            this.player = player;
            this.logger = logger;
            this.syncView = syncView;
            this.txtSyncIn = txtSyncIn;
            this.txtSyncOut = txtSyncOut;
            this.cmbSyncPlaylist = cmbSyncPlaylist;
            this.cmbSyncPlaylist = cmbSyncPlaylist;
            this.lblSyncProgress = lblSyncProgress;

            this.listView
                .GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(this.listView, true, null);

            this.syncView
                .GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(this.syncView, true, null);

            listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            syncView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            listView.DoubleClick += Preview_Click;

            selectedIndex.Add(listView.Name, -1);
            selectedIndex.Add(syncView.Name, -1);

            foreach (object item in contextMenuSync.Items)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem button = item as ToolStripMenuItem;
                    button.Click += Button_Click;
                }
            }

            foreach (object item in contextMenu.Items)
            {
                Type type = item.GetType();

                if (item.GetType() == typeof(ToolStripComboBox))
                {

                }
                else if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem button = item as ToolStripMenuItem;
                    button.Click += Button_Click;

                    if (button.Name == "btnAddToPlaylist")
                    {
                        playlists = button.DropDownItems[0] as ToolStripComboBox;
                    }
                }
            }
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView = sender as ListView;

            if (listView.SelectedIndices.Count == 0)
            {
                return;
            }

            if (selectedIndex[listView.Name] >= 0)
            {
                listView.Items[selectedIndex[listView.Name]].BackColor = Color.FromArgb(33, 33, 33); // normal item
            }

            selectedIndex[listView.Name] = listView.SelectedIndices[0];

            listView.Items[selectedIndex[listView.Name]].BackColor = Color.FromArgb(77, 77, 77); // highlighted item
            listView.Items[selectedIndex[listView.Name]].Selected = false;
        }

        private Release getSelectedRelease()
        {
            return releases[selectedIndex[listView.Name]];
        }

        private void Playlists_Click(object sender, EventArgs e)
        {
            Release album = getSelectedRelease();
            List<string> t = new List<string>();
            Paging<SimpleTrack> songs = album.tracks;

            foreach (SimpleTrack song in songs.Items)
            {
                t.Add(song.Uri);
            }

            ToolStripMenuItem parent = sender as ToolStripMenuItem;
            ToolStripComboBox item = parent.DropDownItems[0] as ToolStripComboBox;

            if (item.Text == "")
            {
                return;
            }

            addToPlaylist(t.ToArray(), item.Text);

            MessageBox.Show("Song added!");
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                if ((sender as ToolStripMenuItem).Name == "toolStripMenuItemPreviewSpotify")
                {
                    new SpotifyPreview(player, syncPlaylist[selectedIndex[listView.Name]].Track as FullTrack);
                    return;
                }
                else if ((sender as ToolStripMenuItem).Name == "toolStripMenuItemPreviewLocal")
                {
                    string filename = syncView.Items[selectedIndex[listView.Name]].SubItems[1].Text;

                    if (filename == "N/A")
                    {
                        MessageBox.Show("no local equivalent has been found :/");
                        return;
                    }

                    string displayname = getDisplayName(new FileInfo(filename));
                    player.playIndependent(filename, displayname.Split('-')[1].Trim(), displayname.Split('-')[0].Trim());
                    return;
                }
            }

            Release album = getSelectedRelease();
            Paging<SimpleTrack> songs = album.tracks;
            new SpotifyPreview(player, album.tracks.Items[0]);
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            OpenFileDialog ofd;

            Release album;

            ProcessStartInfo ps = new ProcessStartInfo()
            {
                UseShellExecute = true,
                Verb = "open"
            };

            switch (item.Name)
            {
                case "openOnSpotifyToolStripMenuItem":
                    album = getSelectedRelease();
                    ps.FileName = album.link;
                    Process.Start(ps);
                    break;
                case "searchOnYoutubeToolStripMenuItem":
                    album = getSelectedRelease();
                    ps.FileName = album.youtubeLink;
                    Process.Start(ps);
                    break;
                case "btnAddToPlaylist":
                    Playlists_Click(sender, e);
                    break;
                case "previewToolStripMenuItem":
                    Preview_Click(sender, e);
                    break;
                // sync
                case "toolStripMenuItemPreviewSpotify":
                    Preview_Click(sender, e);
                    break;
                case "toolStripMenuItemPreviewLocal":
                    Preview_Click(sender, e);
                    break;
                case "toolStripMenuItemOpenSpotify":
                    ps.FileName = (syncPlaylist[selectedIndex[listView.Name]].Track as FullTrack).Uri;
                    ps.FileName = "https://open.spotify.com/" + ps.FileName.Replace("spotify:", "").Replace(':', '/');
                    Process.Start(ps);
                    break;
                case "toolStripMenuItemShowInExplorer":
                    ps.FileName = "explorer";
                    ps.Arguments = "/select," + syncView.Items[selectedIndex[listView.Name]].SubItems[1].Text;
                    Process.Start(ps);
                    break;
                case "toolStripMenuItemChangeFile":
                    ofd = new OpenFileDialog();
                    ofd.Title = $"Searching for '{syncView.Items[selectedIndex[listView.Name]].Text}'";
                    ofd.Filter = "MP3 Songs|*.mp3";
                    if (ofd.ShowDialog() == DialogResult.Cancel)
                    {
                        break;
                    }
                    logger.addSongToDB(ofd.FileName);
                    addOrReplace((syncPlaylist[selectedIndex[listView.Name]].Track as FullTrack).Id, ofd.FileName);
                    syncView.Items[selectedIndex[listView.Name]].SubItems[1].Text = ofd.FileName;
                    break;
                case "toolStripMenuItemChangeSpotify":
                    if (string.IsNullOrEmpty((sender as ToolStripMenuItem).DropDownItems["link"].Text))
                    {
                        return;
                    }

                    var t = syncView.Items[selectedIndex[listView.Name]].SubItems[1].Text;

                    var link = (sender as ToolStripMenuItem).DropDownItems["link"].Text.Split("/");
                    var trackId = link[link.Length - 1].Split("?")[0];
                    var slc = spotifyLinkCache;
                    if (slc.ContainsKey(trackId))
                    {
                        slc[trackId] = t;
                    }
                    else
                    {
                        slc.Add(trackId, t);
                    }
                    spotifyLinkCache = slc;
                    var track = client.Tracks.Get(trackId).Result;
                    Search.SpotifyPreview.ClearCachedTrack(syncPlaylist[selectedIndex[listView.Name]].Track as SimpleTrack);
                    syncPlaylist[selectedIndex[listView.Name]].Track = track;
                    syncView.Items[selectedIndex[listView.Name]].SubItems[0].Text = getDisplayName(track);
                    (sender as ToolStripMenuItem).DropDownItems["link"].Text = "";
                    break;
                case "toolStripMenuItemRecommend":
                    Recommender form = new Recommender(this, player, (syncPlaylist[selectedIndex[listView.Name]].Track as FullTrack));
                    form.Show();
                    break;
            }
        }

        public void openOnSpotify(FullTrack track)
        {
            openOnSpotify(track.ExternalUrls.FirstOrDefault().Value);
        }
        public void openOnSpotify(SimpleTrack track)
        {
            openOnSpotify(track.ExternalUrls.FirstOrDefault().Value);
        }
        public void openOnSpotify(string ExternalUrl)
        {
            ProcessStartInfo ps = new ProcessStartInfo()
            {
                UseShellExecute = true,
                Verb = "open",
                FileName = ExternalUrl
            };

            Process.Start(ps);
        }

        public void addToPlaylist(string[] Uris, string playlistName)
        {
            SimplePlaylist playlist = spotifyPlaylists.Find((x) => x.Name == playlistName);
            SnapshotResponse tmp = client.Playlists.AddItems(playlist.Id, new PlaylistAddItemsRequest(Uris)).Result;
        }
        public void addToPlaylist(string Uri, string playlistName)
        {
            addToPlaylist(new string[] { Uri }, playlistName);
        }

        public void syncPlaylistByIndex(int index)
        {
            string i = spotifyPlaylists[index].Id;
            syncPlaylistById(i);
        }

        public void syncPlaylistByName(string name)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string i = spotifyPlaylists.Find(x => x.Name == name).Id;
            try
            {
                syncPlaylistById(i);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            sw.Stop();

            lblSyncProgress.Invoke(new Action(() => lblSyncProgress.Text = $"{syncPlaylist.Count} songs analysed in {sw.Elapsed.ToString()}"));
        }

        public void syncPlaylistById(string id)
        {
            List<string> lib;
            string dir;
            int index = 0; // default mode

            switch (index)
            {
                case 0:
                    lib = new List<string>(File.ReadAllLines(logger.songLib));
                    break;

                case 1:
                    dir = "";

                    txtSyncIn.Invoke(new Action(() =>
                    {
                        dir = txtSyncIn.Text;

                        if (!Directory.Exists(dir))
                        {
                            FolderBrowserDialog fbd = new FolderBrowserDialog();
                            fbd.ShowNewFolderButton = true;

                            if (fbd.ShowDialog() == DialogResult.Cancel)
                                return;

                            dir = txtSyncIn.Text = fbd.SelectedPath;
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

            if (spotifyPlaylists.FindIndex(x => x.Id == id) < 0)
            {
                return;
            }

            syncView.Invoke(new Action(() => syncView.Items.Clear()));

            FullPlaylist tracks = client.Playlists.Get(id).Result;
            IList<PlaylistTrack<IPlayableItem>> test = client.PaginateAll(tracks.Tracks).Result;
            syncPlaylist = test as List<PlaylistTrack<IPlayableItem>>;

            for (int i = 0; i < syncPlaylist.Count; i++)
            {
                FullTrack track = syncPlaylist[i].Track as SpotifyAPI.Web.FullTrack;

                string displayname = getDisplayName(track);

                string localFile = getMatchingLocalSong(ref track, ref lib);

                syncView.Invoke(new Action(() =>
                {
                    lblSyncProgress.Text = $"{i + 1} / {syncPlaylist.Count}";
                    syncView.Items.Add(displayname);
                    syncView.Items[i].SubItems.Add(localFile);

                    syncView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }));
            }
        }

        public TrackAudioFeatures getFeatures(string id)
        {
            Dictionary<string, TrackAudioFeatures> featureCache = spotifyFeatureCache is null ? new Dictionary<string, TrackAudioFeatures>() : spotifyFeatureCache;
            if (!featureCache.ContainsKey(id))
            {
                TrackAudioFeatures res = client.Tracks.GetAudioFeatures(id).Result;
                featureCache.Add(id, res);
                spotifyFeatureCache = featureCache;
            }
            return featureCache[id];
        }

        public TrackAudioFeatures[] getSeveralFeatures(string[] ids)
        {
            Dictionary<string, TrackAudioFeatures> featureCache = spotifyFeatureCache is null ? new Dictionary<string, TrackAudioFeatures>() : spotifyFeatureCache;

            List<string> missingIds = new List<string>();

            foreach (var id in ids)
            {
                if (!featureCache.ContainsKey(id))
                {
                    missingIds.Add(id);
                }
            }

            if (missingIds.Count > 0)
            {
                TracksAudioFeaturesRequest req = new TracksAudioFeaturesRequest(missingIds);
                List<TrackAudioFeatures> res = client.Tracks.GetSeveralAudioFeatures(req).Result.AudioFeatures;

                foreach (TrackAudioFeatures re in res)
                {
                    featureCache.Add(re.Id, re);
                }

                spotifyFeatureCache = featureCache;
            }

            List<TrackAudioFeatures> ret = new List<TrackAudioFeatures>();

            foreach (var id in ids)
            {
                ret.Add(featureCache[id]);
            }

            return ret.ToArray();
        }

        public void exportSyncedPlaylist()
        {
            bool overwrite = false, fetchMetadata = false;

            if (string.IsNullOrEmpty(txtSyncOut.Text))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                if (fbd.ShowDialog() == DialogResult.Cancel)
                {
                    var t = MessageBox.Show("Do you want to apply the metatags to the current files?", "Spotify Sync", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (t == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        overwrite = true;
                    }
                }
                else
                {
                    txtSyncOut.Text = fbd.SelectedPath;
                }
            }

            if (!overwrite)
            {
                TaskDialogPage td = new TaskDialogPage();
                td.Buttons = new TaskDialogButtonCollection() { TaskDialogButton.Yes, TaskDialogButton.No };
                td.Text = "Do you want to fetch metadata from spotify?";
                td.Expander.Text = "Fetches: Artist, Title, Album, BPM, Key, {pop, nrg, loudness, cover, ?}";
                fetchMetadata = TaskDialog.ShowDialog(td) == TaskDialogButton.Yes;
            }
            else
            {
                fetchMetadata = true;
            }

            Dictionary<string, TrackAudioFeatures> featureCache = spotifyFeatureCache is null ? new Dictionary<string, TrackAudioFeatures>() : spotifyFeatureCache;

            // get features ?
            if (fetchMetadata)
            {
                List<string> missingIds = new List<string>();

                foreach (ListViewItem item in syncView.Items)
                {
                    int index = syncView.Items.IndexOf(item);
                    FullTrack ft = syncPlaylist[index].Track as FullTrack;

                    if (ft is not null && !featureCache.ContainsKey(ft.Id))
                    {
                        missingIds.Add(ft.Id);
                    }
                }

                if (missingIds.Count > 0)
                {
                    TracksAudioFeaturesRequest req = new TracksAudioFeaturesRequest(missingIds);
                    List<TrackAudioFeatures> res = client.Tracks.GetSeveralAudioFeatures(req).Result.AudioFeatures;

                    foreach (TrackAudioFeatures re in res)
                    {
                        featureCache.Add(re.Id, re);
                    }

                    spotifyFeatureCache = featureCache;
                }
            }

            // actually exporting
            foreach (ListViewItem item in syncView.Items)
            {
                string file = item.SubItems[1].Text;
                int index = syncView.Items.IndexOf(item);

                if (File.Exists(file))
                {
                    if (!fetchMetadata)
                    {
                        File.Copy(file, Path.Combine(txtSyncOut.Text, Path.GetFileName(file)));
                    }
                    else
                    {
                        string output = overwrite ? file : Path.Combine(txtSyncOut.Text, item.Text + Path.GetExtension(file));
                        
                        if (!overwrite)
                        {
                            File.Copy(file, output, true);
                        }

                        TagLib.File tag = TagLib.File.Create(output);

                        FullTrack ft = syncPlaylist[index].Track as FullTrack;

                        if (ft is not null)
                        {
                            tag.Tag.Comment = new SpotifyComment(featureCache[ft.Id], ft.Popularity, ft.Album.ReleaseDate).ToString();
                            tag.Tag.Title = ft.Name;
                            tag.Tag.Performers = ft.Artists.Select(x => x.Name).ToArray();
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

            MessageBox.Show("Done");
        }

        private class SpotifyComment
        {
            public SpotifyComment(TrackAudioFeatures features, int popularity, string releaseDate)
            {
                energy = (int)Math.Round(features.Energy * 100);
                danceability = (int)Math.Round(features.Danceability * 100);
                happiness = (int)Math.Round(features.Valence * 100);
                loudness = (int)Math.Round(features.Loudness * 100);
                accousticness = (int)Math.Round(features.Acousticness * 100);
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
                return JsonConvert.DeserializeObject<SpotifyComment>(comment);
            }

            public int popularity, energy, danceability, happiness, loudness, accousticness, instrumentalness, liveness, speechiness;
            public string key, camelot, releaseDate;
        }

        private void addOrReplace(string id, string file)
        {
            Dictionary<string, string> localCache = spotifyLinkCache is null ? new Dictionary<string, string>() : spotifyLinkCache;

            if (localCache.ContainsKey(id))
            {
                localCache[id] = file;
                spotifyLinkCache = localCache;
                return;
            }

            localCache.Add(id, file);
            spotifyLinkCache = localCache;
        }

        public void synchroniseLocalToSpotify()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {

                string dir = "";

                txtSyncIn.Invoke(new Action(() =>
                {
                    if (!Directory.Exists(txtSyncIn.Text))
                    {
                        FolderBrowserDialog fbd = new FolderBrowserDialog();

                        if (fbd.ShowDialog() == DialogResult.Cancel)
                        {
                            return;
                        }

                        txtSyncIn.Text = fbd.SelectedPath;
                    }

                    dir = txtSyncIn.Text;
                }));

                var files = Directory.GetFiles(dir, "*.mp3");

                syncPlaylist = new List<PlaylistTrack<IPlayableItem>>();

                syncView.Invoke(new Action(() =>
                {
                    syncView.Items.Clear();
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
                    syncView.Invoke(new Action(() =>
                    {
                        lblSyncProgress.Text = $"{i + 1} / {files.Length}";
                        syncView.Items.Add(getDisplayName(syncPlaylist[i].Track as FullTrack));
                        syncView.Items[i].SubItems.Add(files[i]);

                        syncView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    }));
                }

                sw.Stop();

                lblSyncProgress.Invoke(new Action(() => lblSyncProgress.Text = $"{syncPlaylist.Count} songs analysed in {sw.Elapsed.ToString()}"));
            }
            catch (APITooManyRequestsException)
            {
                lblSyncProgress.Invoke(new Action(() => lblSyncProgress.Text = "Rate Limit Exceeded"));
            }
        }

        private List<FullTrack> getAllTracks(List<string> trackIds)
        {
            List<FullTrack> ret = new List<FullTrack>();

            while (trackIds.Count > 0)
            {
                ret.AddRange(client.Tracks.GetSeveral(new TracksRequest(trackIds.Take(50).ToList())).Result.Tracks);
                trackIds.RemoveRange(0, trackIds.Count > 50 ? 50 : trackIds.Count);
            }

            return ret;
        }

        private FullTrack getMatchingSpotifySong(string song)
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
                var search = client.Search.Item(new SearchRequest(SearchRequest.Types.Track, q)).Result.Tracks;
                var item = search.Items.FirstOrDefault();

                if (item is null)
                {
                    q = Path.GetFileNameWithoutExtension(song).Replace(" & ", " ").Replace(" - ", " ").Replace(" x ", " ").Replace("(Extended Mix)", "");
                    search = client.Search.Item(new SearchRequest(SearchRequest.Types.Track, q)).Result.Tracks;
                    item = search.Items.FirstOrDefault();
                }

                return item;
            }
            catch (SpotifyAPI.Web.APITooManyRequestsException)
            {
                lblSyncProgress.Invoke(new Action(() => lblSyncProgress.Text = "Rate Limit Exceeded"));
            }

            return null;
        }

        private string getMatchingLocalSong(ref FullTrack track, ref List<string> lib)
        {
            Dictionary<string, string> localCache = spotifyLinkCache is null ? new Dictionary<string, string>() : spotifyLinkCache;

            if (localCache.ContainsKey(track.Id))
            {
                string file = localCache[track.Id];

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

        private double artistMatch(FullTrack track, string dispname)
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

        private string getDisplayName(FileInfo file)
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

        private string getDisplayName(FullTrack track)
        {
            if (track is null)
            {
                return "N/A";
            }

            string name = track.Artists.FirstOrDefault().Name;

            for (int i = 1; i < track.Artists.Count; i++)
            {
                name += $", {track.Artists[i].Name}";
            }

            name += $" - {track.Name}";

            return name;
        }

        public class Release
        {
            public string link;
            public string artist;
            public string album;
            public string oneliner;
            public string youtubeLink;
            public string date;
            public string uri;
            public string id;
            public Paging<SimpleTrack> tracks;
        }

        private void getPlaylists()
        {
            spotifyPlaylists = client.Playlists.CurrentUsers().Result.Items;

            string myId = client.UserProfile.Current().Result.Id;

            foreach(SimplePlaylist item in spotifyPlaylists)
            {
                if (item.Owner.Id == myId)
                {
                    playlists.Items.Add(item.Name);
                    cmbSyncPlaylist.Invoke(new Action(() =>
                    {
                        cmbSyncPlaylist.Items.Add(item.Name);
                    }));
                }
            }
        }

        private SimpleAlbum getLatestRelease(string artistId)
        {
            List<string> dates = new List<string>();

            List<SimpleAlbum> albums = client.Artists.GetAlbums(artistId).Result.Items;

            foreach (SimpleAlbum album in albums)
            {
                dates.Add(album.ReleaseDate);
            }

            return albums[dates.IndexOf(dates.Max())];
        }

        private void initClient(string accessToken)
        {
            try
            {
                FollowOfCurrentUserRequest request = new FollowOfCurrentUserRequest { Limit = 50 };
                client = new SpotifyClient(accessToken);
                FollowedArtistsResponse followedArtists = client.Follow.OfCurrentUser().Result;
                List<FullArtist> follows = (List<FullArtist>)client.PaginateAll(followedArtists.Artists, next => next.Artists).Result;

                getPlaylists();

                releases = new List<Release>();
                
                foreach (FullArtist follow in follows)
                {
                    SimpleAlbum latestRelease = getLatestRelease(follow.Id);
                    Release release = new Release();

                    release.artist = follow.Name;
                    release.album = latestRelease.Name;
                    release.date = latestRelease.ReleaseDate;
                    release.link = "https://open.spotify.com/" + latestRelease.Uri.Replace("spotify:", "").Replace(':', '/');
                    release.oneliner = $"{release.artist} - {release.album}";
                    release.youtubeLink = $"https://music.youtube.com/search?q={release.oneliner}";
                    release.uri = latestRelease.Uri;
                    release.id = latestRelease.Id;
                    release.tracks = client.Albums.GetTracks(latestRelease.Id).Result;

                    releases.Add(release);
                }

                releases.Sort(CompareReleases); // sort all releases (show newest songs on top)
                notifyIfNeeded(releases);

                listView.Invoke(new Action(() =>
                {
                    foreach (Release release in releases)
                    {
                        listView.Items.Add(release.artist);

                        int i = listView.Items.Count - 1;

                        listView.Items[i].SubItems.Add(release.album);
                        listView.Items[i].SubItems.Add(release.date);
                    }

                    listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }));
                ready = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ready = false;
            }
        }

        private void notifyIfNeeded(List<Release> currentReleases)
        {
            List<Release> oldReleases = JsonConvert.DeserializeObject<List<Release>>(Properties.Settings.Default.spotifyReleasesOld);

            if (oldReleases is null)
            {
                notify(currentReleases);
                return;
            }

            List<Release> notifications = currentReleases.Except(oldReleases).ToList();

            if (notifications.Count <= 0)
            {
                return;
            }

            notify(notifications, currentReleases);
        }

        private void notify(List<Release> notifications, List<Release> currentReleases = null)
        {
            currentReleases = currentReleases is null ? notifications : currentReleases;

            string tipContent = "";

            if (notifications.Count > 1)
            {
                string highlight = notifications[0].artist;

                for (int i = 1; i < notifications.Count; i++)
                {
                    if (i >= 3)
                    {
                        highlight += ", ...";
                        break;
                    }

                    highlight += $", {notifications[i].artist}";
                }

                tipContent = $"{notifications.Count} new releases from: {highlight}";
            }
            else
            {
                tipContent = $"New Release: {notifications[0].oneliner}";
            }

            notifyIcon.ShowBalloonTip(2000, "Spotify Release Alert", tipContent, ToolTipIcon.Info);

            Properties.Settings.Default.spotifyReleasesOld = JsonConvert.SerializeObject(currentReleases);
            Properties.Settings.Default.Save();

            Debug.WriteLine("notify!");
        }

        private static int CompareReleases(Release r1, Release r2)
        {
            return r2.date.CompareTo(r1.date);
        }

        public void authoriseUser(string scope = "user-follow-read playlist-modify-public")
        {
            string url = $"https://accounts.spotify.com/authorize?client_id={clientid}&response_type=code&redirect_uri={redirectUri}&scope={scope}";

            spotifyLinker = new SpotifyLinker(new Uri(url));
            spotifyLinker.onNavigateComplete += handleAuthorisationResponse;
        }

        private async void handleAuthorisationResponse(object sender, Uri url)
        {
            if (url.Host.Contains("accounts.spotify.com"))
            {
                spotifyLinker.Show();
                return;
            }

            Task.Factory.StartNew(new Action(() => handleAuthorisationComplete(url.AbsoluteUri)));

            spotifyLinker.Close();
        }

        private async void handleAuthorisationComplete(string uri)
        {
            string code = parseRedirect(uri);

            RestClient client = new RestClient("https://accounts.spotify.com/");
            RestRequest request = new RestRequest("/api/token", Method.POST);

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", clientid);
            request.AddParameter("client_secret", clientsecret);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            IRestResponse response = client.Post(request);
            string content = response.Content; // Raw content as string

            try
            {
                JObject json = JObject.Parse(content);
                string accessToken = json["access_token"].ToString();
                initClient(accessToken);
            } catch { }
        }

        private string parseRedirect(string location)
        {
            return location.Replace("http://reap.ml/callback/?code=", "");
        }

        [Obsolete("GetAccessToken is deprecated, kept here to check if completely redundant")]
        private string GetAccessToken()
        {
            string url5 = "https://accounts.spotify.com/api/token";

            string encode_clientid_clientsecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", clientid, clientsecret)));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url5);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("Authorization: Basic " + encode_clientid_clientsecret);

            string request = ("grant_type=client_credentials");
            byte[] req_bytes = Encoding.ASCII.GetBytes(request);
            webRequest.ContentLength = req_bytes.Length;

            Stream strm = webRequest.GetRequestStream();
            strm.Write(req_bytes, 0, req_bytes.Length);
            strm.Close();

            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
            string json = "";
            using (Stream respStr = resp.GetResponseStream())
            {
                using (StreamReader rdr = new StreamReader(respStr, Encoding.UTF8))
                {
                    //should get back a string i can then turn to json and parse for accesstoken
                    json = rdr.ReadToEnd();
                    rdr.Close();
                }
            }

            return json.Split('"')[3];
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
