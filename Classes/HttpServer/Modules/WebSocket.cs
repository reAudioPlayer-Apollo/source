using EmbedIO.WebSockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.Modules
{
    /// <summary>
    /// Defines a very simple chat server.
    /// </summary>
    public class WebSocket : WebSocketModule
    {
        public WebSocket(string urlPath)
            : base(urlPath, true)
        {
            PlayerManager.webSocket = this;
        }

        /// <inheritdoc />
        protected override Task OnMessageReceivedAsync(
            IWebSocketContext context,
            byte[] rxBuffer,
            IWebSocketReceiveResult rxResult)
            {
            var sMessage = Encoding.GetString(rxBuffer);
            var jMessage = JsonConvert.DeserializeObject<MessageObject>(sMessage);

            switch (jMessage.apiModule.ToLower())
            {
                case "data":
                    new API.DataAPI().handleWebsocket(ref jMessage);
                    break;

                case "control":
                    new API.ControlAPI().handleWebsocket(ref jMessage);
                    break;

                case "general":
                    new API.GeneralAPI().handleWebsocket(ref jMessage);
                    break;

                case "game":
                    new API.GameAPI().handleWebsocket(ref jMessage);
                    break;

                case "youtube":
                    new API.YoutubeAPI().handleWebsocket(ref jMessage);
                    break;

                case "playlist":
                    new API.PlaylistAPI().handleWebsocket(ref jMessage);
                    break;

                default:
                    jMessage.data = "404";
                    break;
            }
            
            return SendAsync(context, jMessage.ToString());
        }

        public class MessageObject
        {
            public MessageObject(string apiModule, string endpoint, string data)
            {
                this.apiModule = apiModule;
                this.endpoint = endpoint;
                this.data = data;
            }
            public MessageObject() { }

            public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public string apiModule;
            public string endpoint;
            public string data;
        }

        /// <inheritdoc />
        protected override Task OnClientConnectedAsync(IWebSocketContext context)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
        {
            return Task.CompletedTask;
        }
        
        private Task SendToAll(string payload)
        {
            return BroadcastAsync(payload);
        }
        
        public void broadCastDisplayname()
        {
            SendToAll(new MessageObject("data", "displayname", PlayerManager.displayName).ToString()).Wait();
        }

        public void broadCastCover()
        {
            SendToAll(new MessageObject("data", "cover", API.Static.GetAsBase64(PlayerManager.cover)).ToString()).Wait();
        }

        public void broadCastVolume(int value)
        {
            SendToAll(new MessageObject("data", "volume", value.ToString()).ToString()).Wait();
        }
    }
}
