using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public interface IBookApi
    {
        Task<List<SearchItem>> SearchBooks(string title);
        Task<BookDetail> GetBook(string id);
        Task<BrowseItem> BrowseBook(string id);
    }
}
