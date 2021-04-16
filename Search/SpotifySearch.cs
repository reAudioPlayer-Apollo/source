using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SpotifyAPI.Web;
using System.Collections.Specialized;
using System.Diagnostics;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json;

namespace reAudioPlayerML.Search
{
    class Spotify
    {
        public SpotifyClient client;
        string clientid = "18c4212a7c2d4682a6415d3d7fed5762";
        string clientsecret = "877e4e6faa504f12b9d3fb74c8671f88";
        string redirectUri = "http://reap.ml/callback/";
        public bool ready { get; private set; }
        ListView listView;
        SpotifyLinker spotifyLinker;
        ContextMenuStrip contextMenu;
        ToolStripComboBox playlists;
        List<SimplePlaylist> spotifyPlaylists;
        List<Release> releases;
        NotifyIcon notifyIcon;
        MediaPlayer player;

        int selectedIndex = -1;

        public Spotify(ListView lView, ContextMenuStrip ctxMenu, NotifyIcon notify, MediaPlayer mediaPlayer)
        {
            //var token = GetAccessToken();

            //client = new SpotifyClient(token);
            ready = false;
            listView = lView;
            contextMenu = ctxMenu;
            notifyIcon = notify;
            player = mediaPlayer;

            listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            lView.DoubleClick += Preview_Click;

            foreach(var item in contextMenu.Items)
            {
                var type = item.GetType();

                if (item.GetType() == typeof(ToolStripComboBox))
                {
                    
                }
                else if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    var button = item as ToolStripMenuItem;
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
            if (listView.SelectedIndices.Count == 0)
                return;

            if (selectedIndex >= 0)
                listView.Items[selectedIndex].BackColor = Color.FromArgb(33, 33, 33); // normal item
            
            selectedIndex = listView.SelectedIndices[0];

            listView.Items[selectedIndex].BackColor = Color.FromArgb(77, 77, 77); // highlighted item
            listView.Items[selectedIndex].Selected = false;
        }

        private Release getSelectedRelease()
        {
            return releases[selectedIndex];
        }

        private void Playlists_Click(object sender, EventArgs e)
        {
            var album = getSelectedRelease();
            var t = new List<string>();
            var songs = album.tracks;

            foreach (var song in songs.Items)
            {
                t.Add(song.Uri);
            }

            ToolStripMenuItem parent = sender as ToolStripMenuItem;
            ToolStripComboBox item = parent.DropDownItems[0] as ToolStripComboBox;

            if (item.Text == "")
                return;

            var playlist = spotifyPlaylists.Find((x) => x.Name == item.Text);
            var tmp = client.Playlists.AddItems(playlist.Id, new PlaylistAddItemsRequest(t)).Result;

            MessageBox.Show("Song added!");
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            var album = getSelectedRelease();
            var songs = album.tracks;
            SpotifyPreview preview = new SpotifyPreview(player, album.tracks.Items[0]);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            var album = getSelectedRelease();

            switch (item.Name)
            {
                case "openOnSpotifyToolStripMenuItem":
                    Process.Start(album.link);
                    break;
                case "searchOnYoutubeToolStripMenuItem":
                    Process.Start(album.youtubeLink);
                    break;
                case "btnAddToPlaylist":
                    Playlists_Click(sender, e);
                    break;
                case "previewToolStripMenuItem":
                    Preview_Click(sender, e);
                    break;
            }
        }

        struct Release
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

        public void getPlaylists()
        {
            spotifyPlaylists = client.Playlists.CurrentUsers().Result.Items;

            var myId = client.UserProfile.Current().Result.Id;

            foreach(var item in spotifyPlaylists)
            {
                if (item.Owner.Id == myId)
                {
                    playlists.Items.Add(item.Name);
                }
            }
        }

        private SimpleAlbum getLatestRelease(string artistId)
        {
            List<string> dates = new List<string>();

            var albums = client.Artists.GetAlbums(artistId).Result.Items;

            foreach (var album in albums)
            {
                dates.Add(album.ReleaseDate);
            }

            return albums[dates.IndexOf(dates.Max())];
        }

        public void initClient(string accessToken)
        {
            try
            {
                var request = new FollowOfCurrentUserRequest { Limit = 50 };
                client = new SpotifyClient(accessToken);
                var followedArtists = client.Follow.OfCurrentUser().Result;
                List<FullArtist> follows = (List<FullArtist>)client.PaginateAll(followedArtists.Artists, next => next.Artists).Result;

                getPlaylists();

                releases = new List<Release>();
                
                foreach (var follow in follows)
                {
                    var latestRelease = getLatestRelease(follow.Id);
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

                        var i = listView.Items.Count - 1;

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

        void notifyIfNeeded(List<Release> currentReleases)
        {
            var oldReleases = JsonConvert.DeserializeObject<List<Release>>(Properties.Settings.Default.spotifyReleasesOld);

            if (oldReleases is null)
            {
                notify(currentReleases);
                return;
            }

            var notifications = currentReleases.Except(oldReleases).ToList();

            if (notifications.Count <= 0)
                return;

            notify(notifications, currentReleases);
        }

        void notify(List<Release> notifications, List<Release> currentReleases = null)
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

        public void authorizeUser(string scope = "user-follow-read playlist-modify-public")
        {
            string url5 = $"https://accounts.spotify.com/authorize?client_id={clientid}&response_type=code&redirect_uri={redirectUri}&scope={scope}";

            //"https://accounts.spotify.com/authorize?client_id=5fe01282e44241328a84e7c5cc169165&response_type=code&redirect_uri=https%3A%2F%2Fexample.com%2Fcallback&scope=user-read-private%20user-read-email&state=34fFs29kd09"

            spotifyLinker = new SpotifyLinker(new Uri(url5));
            spotifyLinker.onNavigateComplete += webBrowserNavigated;
        }

        private async void webBrowserNavigated(object sender, Uri url)
        {
            if (url.Host.Contains("accounts.spotify.com"))
            {
                spotifyLinker.Show();
                return;
            }

            Task.Factory.StartNew(new Action(() => WebBrowser_Navigated(url.AbsoluteUri)));

            spotifyLinker.Close();
        }

        private async void WebBrowser_Navigated(string uri)
        {
            var code = parseRedirect(uri);

            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUri },
                { "client_id", clientid },
                { "client_secret", clientsecret },
                { "Content-Type", "application/json"}
            };

            var client = new RestClient("https://accounts.spotify.com/");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            var request = new RestRequest("/api/token", Method.POST);

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", clientid);
            request.AddParameter("client_secret", clientsecret);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var response = client.Post(request);
            var content = response.Content; // Raw content as string

            try
            {
                JObject json = JObject.Parse(content);
                string accessToken = json["access_token"].ToString();
                initClient(accessToken);
            } catch { }
        }

        string parseRedirect(string location)
        {
            return location.Replace("http://reap.ml/callback/?code=", "");
        }

        public string GetAccessToken()
        {
            string url5 = "https://accounts.spotify.com/api/token";

            //request to get the access token
            var encode_clientid_clientsecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", clientid, clientsecret)));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url5);

            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("Authorization: Basic " + encode_clientid_clientsecret);

            var request = ("grant_type=client_credentials");
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
    }
}
