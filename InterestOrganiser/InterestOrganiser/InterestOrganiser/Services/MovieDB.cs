using InterestOrganiser.Models;
using InterestOrganiser.Models.Movie;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace InterestOrganiser.Services
{
    public class MovieDB : IMovieDB
    {
        private const string api = "fbfd2d53b7504d595ee9c450e52d4026";
        private const string imageSource = "https://image.tmdb.org/t/p/w500";
        private HttpClient client;
        public MovieDB()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<SearchItem>> SearchMovie(string title)
        {
            HttpResponseMessage response = await client.GetAsync($"search/movie?api_key={api}&query={title}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        Type = "Movie",
                        Title = x.title,
                        Poster = imageSource + x.poster_path,
                        Background = imageSource + x.backdrop_path,
                        Description = x.overview
                    }).ToList();

                    return movies;
                }
            }
            return null;
        }

        public Task<List<SearchItem>> SearchTV(string title)
        {
            throw new NotImplementedException();
        }
    }
}
