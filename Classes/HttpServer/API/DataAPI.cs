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
        [Route(HttpVerbs.Get, "/displayname")]
        public async Task RGetDisplayname()
        {
            await Static.SendStringAsync(HttpContext, displayname());
        }

        public string displayname()
        {
            return PlayerManager.displayName;
        }

        [Route(HttpVerbs.Get, "/playlist")]
        public async Task RGetPlaylist()
        {
            await Static.SendStringAsync(HttpContext, playlist());
        }

        public string playlist()
        {
            return JsonConvert.SerializeObject(PlayerManager.loadPlaylistVirtually(PlayerManager.mediaPlayer.playlist));
        }

        [Route(HttpVerbs.Get, "/playlist/{index}")]
        public async Task RGetPlaylist(int index)
        {
            await Static.SendStringAsync(HttpContext, playlist(index));
        }

        public string playlist(int index)
        {
            return JsonConvert.SerializeObject(PlayerManager.loadPlaylistVirtually(index));
        }

        [Route(HttpVerbs.Get, "/playlists")]
        public async Task RGetPlaylists()
        {
            await Static.SendStringAsync(HttpContext, playlists());
        }

        public string playlists()
        {
            return JsonConvert.SerializeObject(PlaylistManager.getDetailedPlaylists());
        }

        [Route(HttpVerbs.Get, "/search/{query}&{scope}")]
        public async Task RSearch(string query, string scope = null)
        {
            await Static.SendStringAsync(HttpContext, search(query, scope));
        }

        public string search(ISearch req)
        {
            return search(req.query, req.scope);
        }

        public string search(string query, string scope = "playlist")
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<MediaPlayer.Song> pl = scope is not null && scope.ToLower() == "global" ?
                MediaPlayer.GetPlaylist(File.ReadAllLines(PlayerManager.logger.songLib).ToList(), PlayerManager.logger, true, true) :
                PlayerManager.mediaPlayer.playlist;

            if (pl is null)
            {
                return "null";
            }

            query = query is null ? "" : query;

            string ret;

            if (string.IsNullOrWhiteSpace(query))
            {
                ret = MediaPlayer.Song.ToString(pl.ToArray());
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

        [Route(HttpVerbs.Get, "/volume")]
        public async Task RGetVolume()
        {
            await Static.SendStringAsync(HttpContext, volume());
        }

        public string volume()
        {
            return PlayerManager.volume.ToString();
        }

        [Route(HttpVerbs.Get, "/position")]
        public async Task RPosition()
        {
            await Static.SendStringAsync(HttpContext, position());
        }

        public string position()
        {
            return PlayerManager.playerPosition.ToString();
        }

        [Route(HttpVerbs.Get, "/cover")]
        public async Task RGetCover()
        {
            await Static.SendStringAsync(HttpContext, cover());
        }
        public string cover()
        {
            return Static.GetStream(PlayerManager.cover);
        }

        [Route(HttpVerbs.Get, "/radioProgramme")]
        public async Task RGetProgramme()
        {
            await Static.SendStringAsync(HttpContext, radioProgramme());
        }
        public string radioProgramme()
        {
            return PlayerManager.getRadioProgrammes();
        }

        [Route(HttpVerbs.Get, "/accentColour")]
        public async Task getAccentColour()
        {
            await Static.SendStringAsync(HttpContext, accentColour());
        }

        public string accentColour()
        {
            return ColorTranslator.ToHtml(PlayerManager.accentColour);
        }

        public struct ISearch
        {
            public string query;
            public string scope;
            public string type;
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
