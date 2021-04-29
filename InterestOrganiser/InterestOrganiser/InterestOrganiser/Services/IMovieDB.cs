using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public interface IMovieDB
    {
        Task<List<SearchItem>> SearchMovie(string title);
        Task<List<SearchItem>> SearchTV(string title);
        Task<DetailItem> MovieDetail(int ID);
    }
}
