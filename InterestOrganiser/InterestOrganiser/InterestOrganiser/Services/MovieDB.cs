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
using MovieBase.Models.MovieDetail;
using InterestOrganiser.Models.Cast;

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

        public async Task<List<CastDetail>> MovieCast(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"movie/{ID}/credits?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Credits castResponse = JsonConvert.DeserializeObject<Credits>(content);

                if (castResponse != null)
                {
                    return castResponse.cast.Select(x => new CastDetail
                    {
                        Name = x.name,
                        Image = imageSource + x.profile_path
                    }).ToList();
                }
            }

            return null;
        }

        public async Task<DetailItem> MovieDetail(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"movie/{ID}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                MovieDetail movieResponse = JsonConvert.DeserializeObject<MovieDetail>(content);

                if(movieResponse != null)
                {
                    string genres = String.Join(", ", movieResponse.genres.Select(x => x.name));

                    return new DetailItem()
                    {
                        Title = movieResponse.original_title,
                        VoteAverage = movieResponse.vote_average,
                        Runtime = TimeConverter(movieResponse.runtime),
                        Release = movieResponse.release_date != null ? Convert.ToDateTime(movieResponse.release_date).ToShortDateString() : "",
                        Poster = imageSource + movieResponse.poster_path,
                        Background = imageSource + movieResponse.backdrop_path,
                        Genres = genres,
                        Overview = movieResponse.overview,
                        Cast = await MovieCast(ID),
                        VoteStars = Math.Round((double)movieResponse.vote_average * 2, MidpointRounding.AwayFromZero) / 4
                    };
                }
            }

            return null;
        }

        public async Task<List<Genre>> MoviesGenres()
        {
            HttpResponseMessage response = await client.GetAsync($"genre/movie/list?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                ListGenres genresResponse = JsonConvert.DeserializeObject<ListGenres>(content);

                if (genresResponse != null)
                {
                    return genresResponse.genres;
                }
            }
            return null;
        }

        public async Task<List<SearchItem>> MoviesList(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"discover/movie?api_key={api}&language=en-US&sort_by=popularity.desc&with_genres={ID}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "movies",
                        Title = x.title,
                        Background = imageSource + x.poster_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
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
                        ID = x.id.ToString(),
                        Type = "movies",
                        Title = x.title,
                        Background = imageSource + x.backdrop_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<SearchItem>> SearchTV(string title)
        {
            HttpResponseMessage response = await client.GetAsync($"search/tv?api_key={api}&query={title}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = "tv series",
                        Title = x.name,
                        Background = imageSource + x.backdrop_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<SearchItem>> TrendingList(string mediaType, string time)
        {
            HttpResponseMessage response = await client.GetAsync($"trending/{mediaType}/{time}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Movie movieResponse = JsonConvert.DeserializeObject<Movie>(content);
                if (movieResponse.results.Any())
                {
                    List<SearchItem> movies = movieResponse.results.Select(x => new SearchItem
                    {
                        ID = x.id.ToString(),
                        Type = x.media_type == "movie" ? "movies" : "tv series",
                        Title = x.title ?? x.name,
                        Background = imageSource + x.backdrop_path,
                    }).ToList();

                    return movies;
                }
            }
            return new List<SearchItem>();
        }

        public async Task<List<CastDetail>> TvCast(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"tv/{ID}/credits?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Credits castResponse = JsonConvert.DeserializeObject<Credits>(content);

                if (castResponse != null)
                {
                    return castResponse.cast.Select(x => new CastDetail
                    {
                        Name = x.name,
                        Image = imageSource + x.profile_path
                    }).ToList();
                }
            }

            return null;
        }

        public async Task<DetailItem> TvDetail(string ID)
        {
            HttpResponseMessage response = await client.GetAsync($"tv/{ID}?api_key={api}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                MovieDetail movieResponse = JsonConvert.DeserializeObject<MovieDetail>(content);

                if (movieResponse != null)
                {
                    string genres = String.Join(", ", movieResponse.genres.Select(x => x.name));

                    return new DetailItem()
                    {
                        Title = movieResponse.original_title ?? movieResponse.name,
                        VoteAverage = movieResponse.vote_average,
                        Runtime = TimeConverter(movieResponse.episode_run_time[0]),
                        Release = movieResponse.first_air_date != null ? Convert.ToDateTime(movieResponse.first_air_date).ToShortDateString() : "",
                        Poster = imageSource + movieResponse.poster_path,
                        Background = imageSource + movieResponse.backdrop_path,
                        Genres = genres,
                        Overview = movieResponse.overview,
                        Cast = await TvCast(ID),
                        VoteStars = Math.Round((double)movieResponse.vote_average * 2, MidpointRounding.AwayFromZero) / 4
                    };
                }
            }

            return null;
        }

        private string TimeConverter(int time)
        {
            TimeSpan newTime = TimeSpan.FromMinutes(time);
            if(newTime.Hours == 0)
            {
                return $"{newTime.Minutes % 60}m";
            }
            else
                return $"{newTime.Hours}h {newTime.Minutes % 60}m";
        }
    }
}
