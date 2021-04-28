using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.API
{
    internal class GameAPI : WebApiController
    {
        [Route(HttpVerbs.Get, "/validate-user/{user}")]
        public async Task RValidateUser(string user)
        {
            await Static.SendStringAsync(HttpContext, validateUser(user));
        }
        public string validateUser(string user)
        {
            if (user == "null" || !HttpWebServer.users.ContainsKey(user))
            {
                user = RandomString(6);
                HttpWebServer.users.Add(user, false);
            }

            return user;
        }

        [Route(HttpVerbs.Get, "/library")]
        public async Task RLibrary()
        {
            await Static.SendStringAsync(HttpContext, library());
        }
        public string library()
        {
            return GameLibraryManager.getInstalledGamesAsJSON();
        }

        [Route(HttpVerbs.Get, "/launch/{id}&{user}")]
        public async Task RLaunch(int id, string user)
        {
            await Static.SendStringAsync(HttpContext, launch(id, user));
        }
        public string launch(int id, string user)
        {
            bool success = GameLibraryManager.launchGameByIGDBId(id, user);

            if (success)
            {
                return "OK";
            }
            else
            {
                return "You have been temporarily blocked!";
            }
        }

        private static readonly Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            int value = 0;
            bool isInt = int.TryParse(msg.data, out value);

            switch (msg.endpoint)
            {
                case "validate-user":
                    msg.data = validateUser(msg.data);
                    break;
                case "library":
                    msg.data = library();
                    break;
                case "launch":
                    LaunchRequestObject lRObj = JsonConvert.DeserializeObject<LaunchRequestObject>(msg.data);
                    msg.data = launch(lRObj.id, lRObj.user);
                    break;
                default:
                    msg.data = "404";
                    break;
            }
        }

        private class LaunchRequestObject
        {
            public int id;
            public string user;
        }
    }
}
