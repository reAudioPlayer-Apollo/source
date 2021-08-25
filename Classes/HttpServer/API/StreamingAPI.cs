using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using EmbedIO.WebSockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.API
{
    public class StreamingAPI : WebApiController
    {
        List<IWebSocketContext> subscribers = new List<IWebSocketContext>();

        public void init()
        {
            PlayerManager.mediaPlayer.Skip += MediaPlayer_Skip;
            PlayerManager.mediaPlayer.Play += MediaPlayer_Play;
            PlayerManager.mediaPlayer.Pause += MediaPlayer_Pause;
            PlayerManager.mediaPlayer.Jump += MediaPlayer_Jump;
        }

        private void MediaPlayer_Jump(object sender, int e)
        {
            PlayerManager.webSocket.SendToSubscribers(new Modules.WebSocket.MessageObject("streaming", "jump", e.ToString()), subscribers.ToArray());
        }

        private void MediaPlayer_Pause(object sender, EventArgs e)
        {
            PlayerManager.webSocket.SendToSubscribers(new Modules.WebSocket.MessageObject("streaming", "pause", null), subscribers.ToArray());
        }

        private void MediaPlayer_Play(object sender, EventArgs e)
        {
            PlayerManager.webSocket.SendToSubscribers(new Modules.WebSocket.MessageObject("streaming", "play", null), subscribers.ToArray());
        }

        private void MediaPlayer_Skip(object sender, bool e)
        {
            using (var stream = File.OpenRead(PlayerManager.mediaPlayer.upNow.location))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
                PlayerManager.webSocket.SendToSubscribers(buffer, subscribers.ToArray());
            }
        }

        public byte[] handleWebsocket(ref Modules.WebSocket.MessageObject msg, IWebSocketContext context)
        {
            switch(msg.endpoint)
            {
                case "start":
                    subscribers.Add(context);
                    if (PlayerManager.mediaPlayer.upNow.location is null)
                    {
                        return null;
                    }
                    using (var stream = File.OpenRead(PlayerManager.mediaPlayer.upNow.location))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                        return buffer;
                    }
                case "stop":
                    subscribers.Remove(context);
                    return null;
                default:
                    return null;
            }
        }
    }
}
