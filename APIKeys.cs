using Newtonsoft.Json;

namespace reAudioPlayerML.Settings
{
    public static class APIKeys
    {
        private static IAPIKeys keys;

        private static void load()
        {
            if (keys is null)
            {
                keys = new IAPIKeys(Properties.Settings.Default.APIKeys);
            }
        }

        public static string youtubeKey
        {
            set
            {
                keys.youtubeKey = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.youtubeKey;
            }
        }

        public static string spotifyId
        {
            set
            {
                keys.spotifyId = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.spotifyId;
            }
        }

        public static string spotifySecret
        {
            set
            {
                keys.spotifySecret = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.spotifySecret;
            }
        }

        public static string tmdbKey
        {
            set
            {
                keys.tmdbKey = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.tmdbKey;
            }
        }

        public static string igdbId
        {
            set
            {
                keys.igdbId = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.igdbId;
            }
        }

        public static string igdbSecret
        {
            set
            {
                keys.igdbSecret = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.igdbSecret;
            }
        }

        private class IAPIKeys
        {
            public string youtubeKey;
            public string spotifyId;
            public string spotifySecret;
            public string tmdbKey;
            public string igdbId;
            public string igdbSecret;

            public IAPIKeys() { } // for desirialise action

            public IAPIKeys(string setting)
            {
                IAPIKeys t = setting is null ? null : JsonConvert.DeserializeObject<IAPIKeys>(setting);

                youtubeKey = t is null ? "" : t.youtubeKey;
                spotifyId = t is null ? "" : t.spotifyId;
                spotifySecret = t is null ? "" : t.spotifySecret;
                tmdbKey = t is null ? "" : t.tmdbKey;
                igdbId = t is null ? "" : t.igdbId;
                igdbSecret = t is null ? "" : t.igdbSecret;
            }

            public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
