using InterestOrganiser.Models;
using InterestOrganiser.Models.MovieTrailers;
using MovieBase.Models.MovieDetail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public interface IMovieDB
    {
        Task<List<SearchItem>> TrendingList(string mediaType, string time);
        Task<List<SearchItem>> SearchMovie(string title);
        Task<List<SearchItem>> SearchTV(string title);
        Task<DetailItem> MovieDetail(string ID);
        Task<DetailItem> TvDetail(string ID);
        Task<List<Genre>> MoviesGenres();
        Task<List<SearchItem>> MoviesList(int ID);
        Task<List<CastDetail>> MovieCast(string ID);
        Task<List<CastDetail>> TvCast(string ID);
        Task<List<SearchItem>> SimilarMovies(string ID);
        Task<List<SearchItem>> SimilarTV(string ID);
        Task<List<Video>> MoviesTrailers(string ID, string type);
        Task<BrowseItem> BrowseMovie(string ID);
        Task<BrowseItem> BrowseTV(string ID);
    }
}
