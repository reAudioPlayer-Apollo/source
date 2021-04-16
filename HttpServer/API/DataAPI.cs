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
    class DataAPI: WebApiController
    {
        [Route(HttpVerbs.Get, "/displayname")]
        public async Task getVersion()
        {
            await Static.SendStringAsync(HttpContext, PlayerManager.displayName);
        }

        [Route(HttpVerbs.Get, "/playlists")]
        public async Task getPlaylists()
        {
            await Static.SendStringAsync(HttpContext,
                JsonConvert.SerializeObject(PlaylistManager.getDetailedPlaylists()));
        }

        [Route(HttpVerbs.Get, "/volume")]
        public async Task getVolume()
        {
            await Static.SendStringAsync(HttpContext, PlayerManager.volume.ToString());
        }

        [Route(HttpVerbs.Get, "/cover")]
        public async Task getCover()
        {
            await Static.SendStringAsync(HttpContext, Static.GetStream(PlayerManager.cover), "images/jpeg");
        }

        [Route(HttpVerbs.Get, "/radioProgramme")]
        public async Task getProgramme()
        {
            var programmes = PlayerManager.getRadioProgrammes();
            await Static.SendStringAsync(HttpContext, programmes);
        }

        [Route(HttpVerbs.Get, "/accentColour")]
        public async Task getAccentColour()
        {
            await Static.SendStringAsync(HttpContext, ColorTranslator.ToHtml(PlayerManager.accentColour));
        }
    }
}
