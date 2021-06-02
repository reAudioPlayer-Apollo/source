using Newtonsoft.Json;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace reAudioPlayerML.Search.Spotify
{
    public class ReleaseRadar
    {
        public bool scanned = false;

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

        private static List<Release> releases;

        public void scan()
        {
            try
            {
                FollowOfCurrentUserRequest request = new FollowOfCurrentUserRequest { Limit = 50 };
                FollowedArtistsResponse followedArtists = Init.Client.Follow.OfCurrentUser().Result;
                List<FullArtist> follows = (List<FullArtist>)Init.Client.PaginateAll(followedArtists.Artists, next => next.Artists).Result;

                UIHandler.releaseView.DoubleClick += onPreviewRequest;
                UIHandler.releaseView.SelectedIndexChanged += ReleaseView_SelectedIndexChanged;
                initContextMenus();
                Wrapper.GetPlaylists();
                Synchronise.Initialise();

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
                    release.tracks = Init.Client.Albums.GetTracks(latestRelease.Id).Result;

                    releases.Add(release);
                }

                releases.Sort((Release r1, Release r2) => r2.date.CompareTo(r1.date)); // sort all releases (show newest songs on top)
                notifyIfNeeded(releases);

                UIHandler.releaseView.Invoke(new Action(() =>
                {
                    foreach (Release release in releases)
                    {
                        UIHandler.releaseView.Items.Add(release.artist);

                        int i = UIHandler.releaseView.Items.Count - 1;

                        UIHandler.releaseView.Items[i].SubItems.Add(release.album);
                        UIHandler.releaseView.Items[i].SubItems.Add(release.date);
                    }

                    UIHandler.releaseView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }));
                scanned = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                scanned = false;
            }
        }

        private int selectedIndex = -1;

        private void ReleaseView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UIHandler.releaseView.SelectedIndices.Count == 0 || selectedIndex >= UIHandler.releaseView.Items.Count)
            {
                return;
            }

            if (selectedIndex >= 0)
            {
                UIHandler.releaseView.Items[selectedIndex].BackColor = Color.FromArgb(33, 33, 33); // normal item
            }

            selectedIndex = UIHandler.releaseView.SelectedIndices[0];

            UIHandler.releaseView.Items[selectedIndex].BackColor = Color.FromArgb(77, 77, 77); // highlighted item
            UIHandler.releaseView.Items[selectedIndex].Selected = false;
        }

        private SimpleAlbum getLatestRelease(string artistId)
        {
            List<string> dates = new List<string>();

            List<SimpleAlbum> albums = Init.Client.Artists.GetAlbums(artistId).Result.Items;

            foreach (SimpleAlbum album in albums)
            {
                dates.Add(album.ReleaseDate);
            }

            return albums[dates.IndexOf(dates.Max())];
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

            UIHandler.notifyIcon.ShowBalloonTip(2000, "Spotify Release Alert", tipContent, ToolTipIcon.Info);

            Properties.Settings.Default.spotifyReleasesOld = JsonConvert.SerializeObject(currentReleases);
            Properties.Settings.Default.Save();

            Debug.WriteLine("notify!");
        }

        public void initContextMenus()
        {
            foreach (object item in UIHandler.ctxRelease.Items)
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

        private void Button_Click(object sender, EventArgs e)
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
                    onPreviewRequest(sender, e);
                    break;
            }
        }

        private Release getSelectedRelease()
        {
            return releases[selectedIndex];
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

            Wrapper.AddToPlaylist(t.ToArray(), item.Text);

            MessageBox.Show("Song added!");
        }

        private void onPreviewRequest(object sender, EventArgs e)
        {
            Release album = getSelectedRelease();
            new SpotifyPreview(album.tracks.Items[0]);
        }
    }
}
