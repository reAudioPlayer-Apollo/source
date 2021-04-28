using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace reAudioPlayerML.HttpServer.API
{
    internal class YoutubeAPI : WebApiController
    {
        [Route(HttpVerbs.Get, "/download/{link}&{output}")]
        public async Task RDownload(string link, string output)
        {
            await Static.SendStringAsync(HttpContext, download(link, output));
        }
        [Route(HttpVerbs.Get, "/download/{link}")]
        public async Task RDownload(string link)
        {
            var output = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            await Static.SendStringAsync(HttpContext, download(link, output));
        }
        public string download(DownloadRequest drObj)
        {
            return download(drObj.link, drObj.output);
        }
        public string download(string link, string output)
        {
            if (link == "undefined")
            {
                return null;
            }
            Static.syncer.createAndDownload(link, output, noPlaylist: link.Contains("music.youtube.com"));
            return output;
        }

        [Route(HttpVerbs.Get, "/sync/{link}&{output}")]
        public async Task RSync(string link, string output)
        {
            await Static.SendStringAsync(HttpContext, sync(link, output));
        }
        [Route(HttpVerbs.Get, "/sync/{link}")]
        public async Task RSync(string link)
        {
            var output = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            await Static.SendStringAsync(HttpContext, sync(link, output));
        }
        public string sync(DownloadRequest drObj)
        {
            return sync(drObj.link, drObj.output);
        }
        public string sync(string link, string output)
        {
            if (link == "undefined")
            {
                return null;
            }
            Static.syncer.createAndDownload(link, output, false, true);
            return output;
        }

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            DownloadRequest dRObj;

            switch (msg.endpoint)
            {
                case "download":
                    dRObj = JsonConvert.DeserializeObject<DownloadRequest>(msg.data);
                    dRObj.output = dRObj.output is null ? Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) : dRObj.output;
                    msg.data = download(dRObj);
                    break;

                case "sync":
                    dRObj = JsonConvert.DeserializeObject<DownloadRequest>(msg.data);
                    dRObj.output = dRObj.output is null ? Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) : dRObj.output;
                    msg.data = download(dRObj);
                    break;

                default:
                    msg.data = "404";
                    break;
            }
        }

        public class DownloadRequest
        {
            public string link;
            public string output;
            public bool noPlaylist;
            public bool sync;
        }
    }
}
