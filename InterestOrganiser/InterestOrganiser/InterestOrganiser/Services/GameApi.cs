using InterestOrganiser.Models;
using InterestOrganiser.Models.Game;
using InterestOrganiser.Models.GameDetail;
using InterestOrganiser.Models.GameTrailers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public class GameApi : IGameApi
    {
        private const string api = "96293706af514b2c80049493ba45f55b";
        private HttpClient client;

        public GameApi()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://rawg.io/api/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<BrowseItem> BrowseGame(FirebaseItem item)
        {
            HttpResponseMessage response = await client.GetAsync($"games/{item.ID}?key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                GameDetail game = JsonConvert.DeserializeObject<GameDetail>(content);
                if (game != null)
                {
                    return new BrowseItem
                    {
                        ID = item.ID,
                        Image = game.background_image,
                        Title = game.name,
                        Type = "games",
                        Realised = item.Realised,
                        ToRealise = item.ToRealise
                    };
                }
            }
            return new BrowseItem();
        }


        public async Task<GameDetail> GetGameDetails(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"games/{ID}?key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                GameDetail game = JsonConvert.DeserializeObject<GameDetail>(content);
                if(game != null)
                {
                    return game;
                }
            }
            return null;
        }

        public async Task<List<Trailers>> GetGameTrailers(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"games/{ID}/movies?key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                GameTrailers gameTrailers = JsonConvert.DeserializeObject<GameTrailers>(content);
                if(gameTrailers != null)
                {
                    return gameTrailers.results.Select(x => new Trailers
                    {
                        Name = x.name,
                        Preview = x.preview,
                        Link = x.data.max
                    }).ToList();
                }
            }
            return new List<Trailers>();
        }

        public async Task<List<SearchItem>> SearchGames(string title)
        {
            HttpResponseMessage response = await client.GetAsync($"games?key={api}&search={title}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Game gameResponse = JsonConvert.DeserializeObject<Game>(content);

                List<SearchItem> gameItems = gameResponse.results.Select(x => new SearchItem
                {
                    ID = x.id.ToString(),
                    Title = x.name,
                    Background = x.background_image,
                    Type = "games"
                }).ToList();

                return gameItems;
            }
            return null;
        }
    }
}
