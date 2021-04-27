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

        public static string youtube
        {
            set
            {
                keys.youtube = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.youtube;
            }
        }

        public static class spotify
        {
            public static string id
            {
                set
                {
                    keys.spotify.id = value;
                    Properties.Settings.Default.APIKeys = keys.ToString();
                    Properties.Settings.Default.Save();
                }
                get
                {
                    load();
                    return keys.spotify.id;
                }
            }

            public static string secret
            {
                set
                {
                    keys.spotify.secret = value;
                    Properties.Settings.Default.APIKeys = keys.ToString();
                    Properties.Settings.Default.Save();
                }
                get
                {
                    load();
                    return keys.spotify.secret;
                }
            }

            public static bool isSet
            {
                get
                {
                    return (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(secret));
                }
            }
        }

        public static string tmdb
        {
            set
            {
                keys.tmdb = value;
                Properties.Settings.Default.APIKeys = keys.ToString();
                Properties.Settings.Default.Save();
            }
            get
            {
                load();
                return keys.tmdb;
            }
        }

        public static class igdb
        {
            public static string id
            {
                set
                {
                    keys.igdb.id = value;
                    Properties.Settings.Default.APIKeys = keys.ToString();
                    Properties.Settings.Default.Save();
                }
                get
                {
                    load();
                    return keys.igdb.id;
                }
            }

            public static string secret
            {
                set
                {
                    keys.igdb.secret = value;
                    Properties.Settings.Default.APIKeys = keys.ToString();
                    Properties.Settings.Default.Save();
                }
                get
                {
                    load();
                    return keys.igdb.secret;
                }
            }
        }

        private class PairedKey
        {
            public string secret;
            public string id;
        }

        private class IAPIKeys
        {
            public string youtube;
            public PairedKey spotify = new PairedKey();
            public PairedKey igdb = new PairedKey();
            public string tmdb;

            public IAPIKeys() { } // for desirialise action

            public IAPIKeys(string setting)
            {
                IAPIKeys t = setting is null ? null : JsonConvert.DeserializeObject<IAPIKeys>(setting);

                youtube = t is null ? "" : t.youtube;
                spotify.id = t is null ? "" : t.spotify.id;
                spotify.secret = t is null ? "" : t.spotify.secret;
                tmdb = t is null ? "" : t.tmdb;
                igdb.id = t is null ? "" : t.igdb.id;
                igdb.secret = t is null ? "" : t.igdb.secret;
            }

            public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
