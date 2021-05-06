using InterestOrganiser.Models;
using InterestOrganiser.Models.Game;
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
        private const string ApiKey = "96293706af514b2c80049493ba45f55b";
        private HttpClient Client;

        public GameApi()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://rawg.io/api/");
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task GetGameDetails(string ID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SearchItem>> SearchGames(string title)
        {
            HttpResponseMessage response = await Client.GetAsync($"games?key={ApiKey}&search={title}");
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
