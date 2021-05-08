using InterestOrganiser.Models;
using InterestOrganiser.Models.Game;
using InterestOrganiser.Models.GameDetail;
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
