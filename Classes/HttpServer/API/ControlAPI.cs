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
    public class ControlAPI: WebApiController
    {
        [Route(HttpVerbs.Get, "/next")]
        public async Task RNext()
        {
            next();
        }

        public string next()
        {
            PlayerManager.next();
            return null;
        }

        [Route(HttpVerbs.Get, "/last")]
        public async Task RLast()
        {
            last();
        }
        public string last()
        {
            PlayerManager.last();
            return null;
        }

        [Route(HttpVerbs.Get, "/volume/{value}")]
        public async Task<int> RSetVolume(int value)
        {
            return Convert.ToInt32(volume(value));
        }
        public string volume(int value)
        {
            PlayerManager.volume = value;
            return value.ToString();
        }

        [Route(HttpVerbs.Get, "/jump/{value}")]
        public async void RJump(int value)
        {
            await Static.SendStringAsync(HttpContext, jump(value));
        }
        public string jump(int value)
        {
            PlayerManager.mediaPlayer.jumpTo(value);
            return PlayerManager.playerPosition.ToString();
        }

        [Route(HttpVerbs.Get, "/sort/{type}")]
        public async void RSort(string type)
        {
            await Static.SendStringAsync(HttpContext, sort(type));
        }
        public string sort(string type)
        {
            PlayerManager.mediaPlayer.sort(type);
            return PlayerManager.mediaPlayer.playlist[PlayerManager.mediaPlayer.nextIndex].ToString();
        }

        [Route(HttpVerbs.Get, "/block/{blockList}")]
        public async void RBlock(string blockList)
        {
            block(blockList);
        }

        public string block(string blockList)
        {
            try
            {
                blockList = blockList.Replace("\"", "");

                int[] t = JsonConvert.DeserializeObject<int[]>(blockList);
                return block(t);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string block(int[] blockList)
        {
            PlayerManager.mediaPlayer.blockList = new List<int>(blockList);
            return null;
        }

        [Route(HttpVerbs.Get, "/playPause")]
        public async Task RPlayPause(int value)
        {
            await Static.SendStringAsync(HttpContext, playPause());
            PlayerManager.mediaPlayer.jumpTo(50);
        }
        public string playPause()
        {
            PlayerManager.playPause();
            if (PlayerManager.isPlaying)
            {
                //return GetStream("resources/controls/webPlay.png");
                return Static.GetStream("resources/controls/webPlay.png");
            }
            else
            {
                //return GetStream("resources/controls/webPause.png");
                return Static.GetStream("resources/controls/webPause.png");
            }
        }

        [Route(HttpVerbs.Get, "/load/playlist/{index}")]
        public async void RLoadPlaylist(int index)
        {
            await Static.SendStringAsync(HttpContext, loadPlaylist(index));
        }
        public string loadPlaylist(int index)
        {
            return PlayerManager.loadPlaylist(index);
        }

        [Route(HttpVerbs.Get, "/load/{index}&global")]
        public async void RLoadGlobalSong(int index)
        {
            await Static.SendStringAsync(HttpContext, loadGlobalSong(index));
        }
        public string loadGlobalSong(int index)
        {
            var song = DataAPI.globalPlaylistCache[index];
            PlayerManager.mediaPlayer.playIndependent(song, true, startPosition: 300);
            return song.oneLiner;
        }

        [Route(HttpVerbs.Get, "/load/{index}")]
        public async void RLoadSong(int index)
        {
            await Static.SendStringAsync(HttpContext, loadSong(index));
        }
        public string loadSong(int index)
        {
            PlayerManager.load(index);
            return PlayerManager.displayName;
        }

        private struct ILoadSong
        {
            public int index;
            public string scope;
        }

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            int value = 0;
            bool isInt = int.TryParse(msg.endpoint == "jump" ? msg.data.Replace(".", "") : msg.data, out value);

            switch (msg.endpoint)
            {
                case "next":
                    msg.data = next();
                    break;

                case "last":
                    msg.data = last();
                    break;

                case "volume":
                    msg.data = isInt ? volume(value) : null;
                    break;

                case "jump":
                    msg.data = isInt ? jump(value) : null;
                    break;

                case "sort":
                    msg.data = sort(msg.data);
                    break;

                case "block":
                    msg.data = block(msg.data);
                    break;

                case "playPause":
                    msg.data = playPause();
                    break;

                case "load/playlist":
                    msg.data = isInt ? loadPlaylist(value) : null;
                    break;

                case "load":
                    if (!isInt)
                    {
                        try
                        {
                            var t = JsonConvert.DeserializeObject<ILoadSong>(msg.data);

                            if (t.scope == "global")
                            {
                                msg.data = loadGlobalSong(t.index);
                            }
                            else
                            {
                                isInt = true;
                                value = t.index;
                            }
                        }
                        catch
                        {
                            msg.data = null;
                        }
                    }

                    msg.data = isInt ? loadSong(value) : msg.data;
                    break;

                default:
                    msg.data = "404";
                    break;
            }
        }
    }
}
