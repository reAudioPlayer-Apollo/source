using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.API
{
    public class ControlAPI: WebApiController
    {
        [Route(HttpVerbs.Get, "/next")]
        public async Task next()
        {
            PlayerManager.next();
        }

        [Route(HttpVerbs.Get, "/last")]
        public async Task last()
        {
            PlayerManager.last();
        }

        [Route(HttpVerbs.Get, "/volume/{value}")]
        public async Task setVolume(int value)
        {
            PlayerManager.volume = value;
        }

        [Route(HttpVerbs.Get, "/playPause")]
        public async Task playPause(int value)
        {
            PlayerManager.playPause();
            if (PlayerManager.isPlaying)
            {
                //return GetStream("ressources/controls/webPlay.png");
                await Static.SendStringAsync(HttpContext, Static.GetStream("ressources/controls/webPlay.png"), "images/jpeg");
            }
            else
            {
                //return GetStream("ressources/controls/webPause.png");
                await Static.SendStringAsync(HttpContext, Static.GetStream("ressources/controls/webPause.png"), "images/jpeg");
            }
        }

        [Route(HttpVerbs.Get, "/load/playlist/{index}")]
        public async Task<string> loadPlaylist(int index)
        {
            return PlayerManager.loadPlaylist(index);
        }

        [Route(HttpVerbs.Get, "/load/{index}")]
        public async Task loadSong(int index)
        {
            PlayerManager.load(index);
        }
    }
}
