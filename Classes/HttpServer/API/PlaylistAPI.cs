using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.API
{
    class PlaylistAPI: WebApiController
    {
        public int[] parseIntArray(string json)
        {
            try
            {
                json = json.Replace("\"", "");

                return JsonConvert.DeserializeObject<int[]>(json);
            }
            catch
            {
                return new int[0];
            }
        }

        public string createPlaylist(string songlist, bool virtually = false)
        {
            return createPlaylist(parseIntArray(songlist), virtually);
        }

        public string createPlaylist(int[] songlist, bool virtually = false)
        {
            return PlaylistManager.Create(DataAPI.globalPlaylistCache
                .Where(x => songlist.Contains(x.id))
                .Select(x => x.location)
                .ToArray(), virtually);
        }

        public string getVirtual()
        {
            var t = PlaylistManager.virtualPlaylists.Keys.ToArray();
            return JsonConvert.SerializeObject(t);
        }

        [Route(HttpVerbs.Get, "/virtual/create/{songlist}")]
        public async Task RCreateVirtually(string songlist)
        {
            await Static.SendStringAsync(HttpContext, createPlaylist(songlist, true));
        }

        [Route(HttpVerbs.Get, "/create/{songlist}")]
        public async Task RCreate(string songlist)
        {
            await Static.SendStringAsync(HttpContext, createPlaylist(songlist));
        }

        [Route(HttpVerbs.Get, "/virtual")]
        public async Task RVirtual()
        {
            await Static.SendStringAsync(HttpContext, getVirtual());
        }

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            switch (msg.endpoint)
            {
                case "create":
                    msg.data = createPlaylist(msg.data);
                    break;

                case "virtual/create":
                    msg.data = createPlaylist(msg.data, true);
                    break;

                case "virtual":
                    msg.data = getVirtual();
                    break;

                default:
                    msg.data = "404";
                    break;
            }
        }
    }
}
