using Newtonsoft.Json;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace reAudioPlayerML.Search.Spotify
{
    public static class Wrapper
    {
        public static Dictionary<string, TrackAudioFeatures> SpotifyFeatureCache
        {
            set
            {
                Properties.Settings.Default.spotifyFeatureCache = JsonConvert.SerializeObject(value);
                Properties.Settings.Default.Save();
            }
            get
            {
                var t = JsonConvert.DeserializeObject<Dictionary<string, TrackAudioFeatures>>(Properties.Settings.Default.spotifyFeatureCache);
                return t is null ? new Dictionary<string, TrackAudioFeatures>() : t;
            }
        }

        public static void OpenOnSpotify(FullTrack track)
        {
            OpenOnSpotify(track.ExternalUrls.FirstOrDefault().Value);
        }
        public static void OpenOnSpotify(SimpleTrack track)
        {
            OpenOnSpotify(track.ExternalUrls.FirstOrDefault().Value);
        }
        public static void OpenOnSpotify(string ExternalUrl)
        {
            ProcessStartInfo ps = new ProcessStartInfo()
            {
                UseShellExecute = true,
                Verb = "open",
                FileName = ExternalUrl
            };

            Process.Start(ps);
        }

        public static void AddToPlaylist(string[] Uris, SimplePlaylist playlist)
        {
            SnapshotResponse tmp = Init.Client.Playlists.AddItems(playlist.Id, new PlaylistAddItemsRequest(Uris)).Result;
        }

        public static void AddToPlaylist(string Uri, SimplePlaylist playlist)
        {
            AddToPlaylist(new string[] { Uri }, playlist);
        }

        public static void AddToPlaylist(string[] Uris, string playlistName)
        {
            SimplePlaylist playlist = Init.Playlists.Find((x) => x.Name == playlistName);
            AddToPlaylist(Uris, playlist);
        }

        public static void AddToPlaylist(string Uri, string playlistName)
        {
            AddToPlaylist(new string[] { Uri }, playlistName);
        }


        public static TrackAudioFeatures GetFeatures(string id)
        {
            Dictionary<string, TrackAudioFeatures> featureCache = SpotifyFeatureCache is null ? new Dictionary<string, TrackAudioFeatures>() : SpotifyFeatureCache;
            if (!featureCache.ContainsKey(id))
            {
                TrackAudioFeatures res = Init.Client.Tracks.GetAudioFeatures(id).Result;
                featureCache.Add(id, res);
                SpotifyFeatureCache = featureCache;
            }
            return featureCache[id];
        }

        public static TrackAudioFeatures[] GetSeveralFeatures(string[] ids)
        {
            Dictionary<string, TrackAudioFeatures> featureCache = SpotifyFeatureCache is null ? new Dictionary<string, TrackAudioFeatures>() : SpotifyFeatureCache;

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
                List<TrackAudioFeatures> res = Init.Client.Tracks.GetSeveralAudioFeatures(req).Result.AudioFeatures;

                foreach (TrackAudioFeatures re in res)
                {
                    featureCache.Add(re.Id, re);
                }

                SpotifyFeatureCache = featureCache;
            }

            List<TrackAudioFeatures> ret = new List<TrackAudioFeatures>();

            foreach (var id in ids)
            {
                ret.Add(featureCache[id]);
            }

            return ret.ToArray();
        }


        public static void GetPlaylists()
        {
            Init.Playlists = Init.Client.Playlists.CurrentUsers().Result.Items;

            Init.UserId = Init.Client.UserProfile.Current().Result.Id;

            foreach (SimplePlaylist item in Init.Playlists)
            {
                if (item.Owner.Id == Init.UserId)
                {
                    UIHandler.ctxSyncPlaylists.Items.Add(item.Name);
                    UIHandler.mainPlaylists.Items.Add(item.Name);
                    UIHandler.cmbSyncPlaylist.Invoke(new Action(() =>
                    {
                        UIHandler.cmbSyncPlaylist.Items.Add(item.Name);
                    }));
                }
            }
        }
    }
}
