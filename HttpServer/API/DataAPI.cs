using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        [Route(HttpVerbs.Get, "/playlists")]
        public async Task RGetPlaylists()
        {
            await Static.SendStringAsync(HttpContext, playlists());
        }

        public string playlists()
        {
            return JsonConvert.SerializeObject(PlaylistManager.getDetailedPlaylists());
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

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            switch (msg.endpoint)
            {
                case "displayname":
                    msg.data = displayname();
                    break;

                case "playlists":
                    msg.data = playlists();
                    break;

                case "volume":
                    msg.data = volume();
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

                default:
                    msg.data = "404";
                    break;
            }
        }
    }
}
