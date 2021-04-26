using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib;

namespace reAudioPlayerML.Search
{
    public class Movie
    {
        TMDbLib.Client.TMDbClient client = new TMDbLib.Client.TMDbClient(Settings.APIKeys.tmdbKey);

        public async void byQuery(string title)
        {
            var multi = await client.SearchMultiAsync(title);

            TMDbLib.Objects.Search.SearchTv first = (TMDbLib.Objects.Search.SearchTv)multi.Results[0];
            var banner = "https://image.tmdb.org/t/p/original" + first.BackdropPath;
            var cover = "https://image.tmdb.org/t/p/original" + first.PosterPath;
        }
    }
}
