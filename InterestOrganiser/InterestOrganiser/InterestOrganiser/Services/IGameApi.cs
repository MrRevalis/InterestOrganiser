using InterestOrganiser.Models;
using InterestOrganiser.Models.GameDetail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public interface IGameApi
    {
        Task<List<SearchItem>> SearchGames(string title);
        Task<GameDetail> GetGameDetails(string ID);
        Task<List<Trailers>> GetGameTrailers(string ID);
        Task<BrowseItem> BrowseGame(FirebaseItem item);
    }
}