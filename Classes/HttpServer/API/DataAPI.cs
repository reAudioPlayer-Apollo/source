using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.API
{
    class DataAPI : WebApiController
    {
        public struct ISearch
        {
            public string query;
            public string scope;
            public string type;
        }

        public static List<MediaPlayer.BasicSong> globalPlaylistCache = new List<MediaPlayer.BasicSong>();

        /// <summary>
        /// Gets the displayname of the currently playing song
        /// </summary>
        public string displayname()
        {
            return PlayerManager.displayName;
        }

        /// <summary>
        /// Gets the currently loaded playlist
        /// </summary>
        /// <returns>Serialised Playlist (FullPlaylist)</returns>
        public string playlist()
        {
            return JsonConvert.SerializeObject(PlayerManager.loadPlaylistVirtually(PlayerManager.mediaPlayer.playlist));
        }

        /// <summary>
        /// Gets the playlist @ <paramref name="index"/>
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Serialised Playlist (FullPlaylist)</returns>
        public string playlist(int index)
        {
            return JsonConvert.SerializeObject(PlayerManager.loadPlaylistVirtually(index));
        }

        /// <summary>
        /// Gets few details of all playlists
        /// </summary>
        /// <returns>Serialised Playlist (AllDetailedPlaylists)</returns>
        public string playlists()
        {
            return JsonConvert.SerializeObject(PlaylistManager.getDetailedPlaylists());
        }


        public string search(ISearch req)
        {
            return search(req.query, req.scope);
        }

        public string search(string query, string scope = "playlist")
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string specialCoverUriSuffix = "";

            List<MediaPlayer.Song> pl = scope is not null && scope.ToLower() == "global" ?
                MediaPlayer.GetPlaylist(File.ReadAllLines(PlayerManager.logger.songLib).ToList(), PlayerManager.logger) :
                PlayerManager.mediaPlayer.playlist;

            if (scope is not null && scope.ToLower() == "global")
            {
                globalPlaylistCache = MediaPlayer.BasicSong.ConvertFromSongList(pl.ToArray()).ToList();
                specialCoverUriSuffix = "&global";
            }

            if (pl is null)
            {
                return "null";
            }

            query = query is null ? "" : query;

            string ret;

            if (string.IsNullOrWhiteSpace(query))
            {
                ret = MediaPlayer.Song.ToString(pl.ToArray(), specialCoverUriSuffix: specialCoverUriSuffix);
                sw.Stop();
                Debug.WriteLine(sw.Elapsed.ToString());
                return ret;
            }

            var matches = pl.Where(x => x is not null && x.oneLiner.ToLower().Contains(query.ToLower())).ToList();
            ret = MediaPlayer.Song.ToString(matches.ToArray());

            sw.Stop();
            Debug.WriteLine(sw.Elapsed.ToString());
            return ret;
        }

        public byte[] cover(int id, bool global = false)
        {
            var location = PlayerManager.logger.getSongLocationById(id);

            MediaPlayer.Song song = global ?
                new MediaPlayer.Song(globalPlaylistCache.Where(x => x.location == location).FirstOrDefault()) :
                PlayerManager.mediaPlayer.playlist.Where(x => x.location == location).FirstOrDefault();

            if (song is not null && (song.cover is not null || global))
            {
                return song.getCoverBytes(global);
            }

            return null;
        }

        public string volume()
        {
            return PlayerManager.volume.ToString();
        }

        public string position()
        {
            return PlayerManager.playerPosition.ToString();
        }

        public string cover()
        {
            return Static.GetAsBase64(PlayerManager.cover);
        }

        public string radioProgramme()
        {
            return PlayerManager.getRadioProgrammes();
        }

        public string accentColour()
        {
            return ColorTranslator.ToHtml(PlayerManager.accentColour);
        }

        /* ROUTES */

        [Route(HttpVerbs.Get, "/displayname")]
        public async Task RGetDisplayname()
        {
            await Static.SendStringAsync(HttpContext, displayname());
        }

        [Route(HttpVerbs.Get, "/playlist")]
        public async Task RGetPlaylist()
        {
            await Static.SendStringAsync(HttpContext, playlist());
        }

        [Route(HttpVerbs.Get, "/playlist/{index}")]
        public async Task RGetPlaylist(int index)
        {
            await Static.SendStringAsync(HttpContext, playlist(index));
        }

        [Route(HttpVerbs.Get, "/playlists")]
        public async Task RGetPlaylists()
        {
            await Static.SendStringAsync(HttpContext, playlists());
        }

        [Route(HttpVerbs.Get, "/search/{query}&{scope}")]
        public async Task RSearch(string query, string scope = null)
        {
            await Static.SendStringAsync(HttpContext, search(query, scope));
        }

        [Route(HttpVerbs.Get, "/cover/{id}&global")]
        public async Task RCoverGlobal(int id)
        {
            var bytes = cover(id, true);

            if (bytes is not null)
            {
                using (var stream = HttpContext.OpenResponseStream())
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
        }

        [Route(HttpVerbs.Get, "/cover/{id}")]
        public async Task RCover(int id)
        {
            var bytes = cover(id);

            if (bytes is not null)
            {
                using (var stream = HttpContext.OpenResponseStream())
                {
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }
            }
        }

        [Route(HttpVerbs.Get, "/volume")]
        public async Task RGetVolume()
        {
            await Static.SendStringAsync(HttpContext, volume());
        }

        [Route(HttpVerbs.Get, "/position")]
        public async Task RPosition()
        {
            await Static.SendStringAsync(HttpContext, position());
        }

        [Route(HttpVerbs.Get, "/cover")]
        public async Task RGetCover()
        {
            await Static.SendStringAsync(HttpContext, cover());
        }
        
        [Route(HttpVerbs.Get, "/radioProgramme")]
        public async Task RGetProgramme()
        {
            await Static.SendStringAsync(HttpContext, radioProgramme());
        }
        
        [Route(HttpVerbs.Get, "/accentColour")]
        public async Task getAccentColour()
        {
            await Static.SendStringAsync(HttpContext, accentColour());
        }

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            int value = 0;
            bool isInt = int.TryParse(msg.data, out value);

            switch (msg.endpoint)
            {
                case "displayname":
                    msg.data = displayname();
                    break;

                case "playlists":
                    msg.data = playlists();
                    break;

                case "playlist":
                    msg.data = isInt ? playlist(value) : playlist();
                    break;

                case "volume":
                    msg.data = volume();
                    break;

                case "position":
                    msg.data = position();
                    break;

                case "cover":
                    msg.data = cover();
                    break;

                case "radioProgramme":
                    msg.data = radioProgramme();
                    break;

                case "accentColour":
                    msg.data = accentColour();
                    break;

                case "search":
                    msg.data = search(JsonConvert.DeserializeObject<ISearch>(msg.data));
                    break;

                default:
                    msg.data = "404";
                    break;
            }
        }
    }
}
