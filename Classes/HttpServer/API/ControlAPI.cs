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
        public string next()
        {
            PlayerManager.next();
            return null;
        }

        public string last()
        {
            PlayerManager.last();
            return null;
        }

        public string volume(int value)
        {
            PlayerManager.volume = value;
            return value.ToString();
        }

        public string jump(int value)
        {
            PlayerManager.mediaPlayer.jumpTo(value);
            return PlayerManager.playerPosition.ToString();
        }

        public string sort(string type)
        {
            PlayerManager.mediaPlayer.sort(type);
            return PlayerManager.mediaPlayer.playlist[PlayerManager.mediaPlayer.nextIndex].ToString();
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

        public string playPause()
        {
            PlayerManager.playPause();
            if (PlayerManager.isPlaying)
            {
                //return GetStream("resources/controls/webPlay.png");
                return Static.GetAsBase64("resources/controls/webPlay.png");
            }
            else
            {
                //return GetStream("resources/controls/webPause.png");
                return Static.GetAsBase64("resources/controls/webPause.png");
            }
        }

        public string loadAutoPlaylist(string name)
        {
            try
            {
                return PlayerManager.loadPlaylist(PlaylistManager.AutoPlaylists.FetchAutoPlaylist(name));

                /*PlaylistManager.AutoPlaylists.SpecialPlaylists playlist = (PlaylistManager.AutoPlaylists.SpecialPlaylists)Enum.Parse(typeof(PlaylistManager.AutoPlaylists.SpecialPlaylists), name, true);
                return PlayerManager.loadPlaylist(
                    PlaylistManager.AutoPlaylists.getSpecialPlaylists(playlist)
                    .FirstOrDefault()
                    ?.songs
                    ?.Select(x => x.location)
                    ?.ToArray());*/
            }
            catch
            {
                return "Playlist Not Found";
            }
        }

        public string loadPlaylist(string query)
        {
            int val;

            if (int.TryParse(query, out val))
            {
                return loadPlaylist(val);
            }

            var res = PlaylistManager.FindDetailedPlaylist(query);
            var name = res.Value.autoplaylists.Concat(res.Value.customplaylists).ToArray()[res.Key].name;

            var x = res.Key >= res.Value.autoplaylists.Count ? PlayerManager.loadPlaylist(res.Key - res.Value.autoplaylists.Count) : loadAutoPlaylist(name);

            return name;
        }

        public string loadPlaylist(int index)
        {
            return PlayerManager.loadPlaylist(index);
        }

        public string loadGlobalSong(int index)
        {
            var song = DataAPI.globalPlaylistCache[index];
            PlayerManager.mediaPlayer.playIndependent(new MediaPlayer.Song(song), true, startPosition: 300);
            return $"{song.artist} - {song.title}";
        }

        public string loadSong(string query)
        {
            int val;

            if (int.TryParse(query, out val))
            {
                return loadSong(val);
            }

            var index = PlayerManager.mediaPlayer.playlist
                .FindIndex(x => query.Split(' ')
                    .Where(y => x.keywords
                        .ToLower()
                        .Contains(y
                            .ToLower()))
                    .Count() == query.Split(' ').Length);

            if (index < 0)
            {
                return "404";
            }

            return loadSong(index);
        }

        public string loadSong(int index)
        {
            PlayerManager.load(index);
            return PlayerManager.displayName;
        }

        /* ROUTES */

        [Route(HttpVerbs.Get, "/next")]
        public async Task RNext()
        {
            next();
        }

        [Route(HttpVerbs.Get, "/last")]
        public async Task RLast()
        {
            last();
        }

        [Route(HttpVerbs.Get, "/volume/{value}")]
        public async Task<int> RSetVolume(int value)
        {
            return Convert.ToInt32(volume(value));
        }

        [Route(HttpVerbs.Get, "/jump/{value}")]
        public async void RJump(int value)
        {
            await Static.SendStringAsync(HttpContext, jump(value));
        }

        [Route(HttpVerbs.Get, "/sort/{type}")]
        public async void RSort(string type)
        {
            await Static.SendStringAsync(HttpContext, sort(type));
        }

        [Route(HttpVerbs.Get, "/block/{blockList}")]
        public async void RBlock(string blockList)
        {
            block(blockList);
        }

        [Route(HttpVerbs.Get, "/playPause")]
        public async Task RPlayPause(int value)
        {
            await Static.SendStringAsync(HttpContext, playPause());
            PlayerManager.mediaPlayer.jumpTo(50);
        }

        [Route(HttpVerbs.Get, "/load/global/{index}")]
        public async void RLoadGlobalSong(int index)
        {
            await Static.SendStringAsync(HttpContext, loadGlobalSong(index));
        }

        [Route(HttpVerbs.Get, "/load/{query}")]
        public async void RLoadSong(string query)
        {
            await Static.SendStringAsync(HttpContext, loadSong(query));
        }

        [Route(HttpVerbs.Get, "/load/playlist/{query}")]
        public async void RLoadPlaylist(string query)
        {
            await Static.SendStringAsync(HttpContext, loadPlaylist(query));
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
                    msg.data = isInt ? loadPlaylist(value) : loadAutoPlaylist(msg.data);
                    break;

                case "load/global":
                    msg.data = isInt ? loadGlobalSong(value) : null;
                    break;

                case "load":
                    msg.data = isInt ? loadSong(value) : loadSong(msg.data);
                    break;

                default:
                    msg.data = "404";
                    break;
            }
        }
    }
}
