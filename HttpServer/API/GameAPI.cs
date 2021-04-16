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
    class GameAPI: WebApiController
    {
        [Route(HttpVerbs.Get, "/validate-user/{user}")]
        public async Task validateUser(string user)
        {
            if (user == "null" || !HttpWebServer.users.ContainsKey(user))
            {
                user = RandomString(6);
                HttpWebServer.users.Add(user, false);
            }

            await Static.SendStringAsync(HttpContext, user);
        }

        [Route(HttpVerbs.Get, "/library")]
        public async Task getGames()
        {
            await Static.SendStringAsync(HttpContext, GameLibraryManager.getInstalledGamesAsJSON());
        }

        [Route(HttpVerbs.Get, "/launch/{id}&{user}")]
        public async Task launchGame(int id, string user)
        {
            bool success = GameLibraryManager.launchGameByIGDBId(id, user);

            if (success)
            {
                await Static.SendStringAsync(HttpContext, "OK");
            }
            else
            {
                await Static.SendStringAsync(HttpContext, "You have been temporarily blocked!");
            }
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
