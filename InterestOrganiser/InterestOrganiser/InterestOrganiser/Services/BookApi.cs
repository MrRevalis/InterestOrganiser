using Google.Apis.Books.v1;
using Google.Apis.Services;
using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace InterestOrganiser.Services
{
    public class BookApi : IBookApi
    {
        private const string api = "AIzaSyDqoFKd6m9VgUBQygz88pX-wHEJN5TuHcQ";
        private BooksService booksService;
        public BookApi()
        {
            booksService = new BooksService(new BaseClientService.Initializer()
            {
                ApiKey = api,
                ApplicationName = this.GetType().ToString()
            });
        }

        public async Task<BookDetail> GetBook(string id)
        {
            var res = await booksService.Volumes.Get(id).ExecuteAsync();
            if(res != null)
            {
                return new BookDetail
                {
                    ID = res.Id,
                    Book = res.VolumeInfo
                };
            }
            return null;
        }

        public async Task<List<SearchItem>> SearchBooks(string title)
        {
            var list = booksService.Volumes.List(title);
            var res = await list.ExecuteAsync();
            if(res != null)
            {
                var books = res.Items.Select(x => new SearchItem
                {
                    ID = x.Id,
                    Type = "books",
                    Title = x.VolumeInfo.Title,
                    Background = x.VolumeInfo.ImageLinks != null ? x.VolumeInfo.ImageLinks.Thumbnail : ""
                }).ToList();

                return books;
            }
            return new List<SearchItem>();
        }
    }
}
