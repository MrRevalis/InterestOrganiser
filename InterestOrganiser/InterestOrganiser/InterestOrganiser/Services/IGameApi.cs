using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public interface IGameApi
    {
        Task<List<SearchItem>> SearchGames(string title);
        Task GetGameDetails(string ID);
    }
}