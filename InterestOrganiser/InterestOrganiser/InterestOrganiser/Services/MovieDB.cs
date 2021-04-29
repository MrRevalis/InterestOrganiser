﻿using InterestOrganiser.Models;
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

        public async Task<DetailItem> MovieDetail(int ID)
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
                        Release = Convert.ToDateTime(movieResponse.release_date).ToShortDateString(),
                        Poster = imageSource + movieResponse.poster_path,
                        Background = imageSource + movieResponse.backdrop_path,
                        Genres = genres,
                        Overview = movieResponse.overview
                    };
                }
            }

            return null;
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
                        ID = x.id,
                        Type = "Movie",
                        Title = x.title,
                        Background = imageSource + x.backdrop_path,
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


        private string TimeConverter(int time)
        {
            TimeSpan newTime = TimeSpan.FromMinutes(time);
            return $"{newTime.Hours}h {newTime.Minutes % 60}m";
        }
    }
}