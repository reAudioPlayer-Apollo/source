using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IGDB;
using IGDB.Models;

namespace reAudioPlayerML.Search
{
    class Game
    {
        IGDBClient igdb = new IGDBClient(
          // Found in Twitch Developer portal for your app
          Settings.APIKeys.igdbId,
          Settings.APIKeys.igdbSecret
        );

        public async Task<IGDB.Models.Game> byQuery(string name)
        {
            var games = await igdb.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games,
                query: $"search \"{name}\"; fields *, artworks.url, cover.url, videos.name, videos.video_id; where category = 0;");

            var t = games.Where(x => x.Name.ToLower() == name.ToLower());

            IGDB.Models.Game game = t.FirstOrDefault();

            if (game is null)
                game = games.First();

            return game;
        }

        public async Task<IGDB.Models.Game> byId(int id)
        {
            var games = await igdb.QueryAsync<IGDB.Models.Game>(IGDBClient.Endpoints.Games,
                query: $"fields *, artworks.url, cover.url, videos.name, videos.video_id; where category = 0 & id={id};");
            var game = games.First();

            return game;
        }
    }
}
