using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reAudioPlayerML.Search.Spotify
{
    public static class Init
    {
        public static SpotifyClient Client;
        public static List<SimplePlaylist> Playlists;
        private static SpotifyLinker spotifyLinker;

        public static Logger logger;

        public static string UserId;

        private readonly static string clientid = Settings.APIKeys.spotify.id;
        private readonly static string clientsecret = Settings.APIKeys.spotify.secret;
        private readonly static string redirectUri = "http://reap.ml/callback/";

        public static void AuthoriseUser(string scope = "user-follow-read playlist-modify-public")
        {
            string url = $"https://accounts.spotify.com/authorize?client_id={clientid}&response_type=code&redirect_uri={redirectUri}&scope={scope}";

            spotifyLinker = new SpotifyLinker(new Uri(url));
            spotifyLinker.onNavigateComplete += handleAuthorisationResponse;
        }

        private static async void handleAuthorisationResponse(object sender, Uri url)
        {
            if (url.Host.Contains("accounts.spotify.com"))
            {
                spotifyLinker.Show();
                return;
            }

            Task.Factory.StartNew(new Action(() => handleAuthorisationComplete(url.AbsoluteUri)));

            spotifyLinker.Close();
        }

        private static async void handleAuthorisationComplete(string uri)
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
                Client = new SpotifyClient(accessToken);
                new ReleaseRadar().scan();
            }
            catch { }
        }

        private static string parseRedirect(string location)
        {
            return location.Replace("http://reap.ml/callback/?code=", "");
        }
    }
}
