using InterestOrganiser.Models;
using InterestOrganiser.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    [QueryProperty("ID", "id")]
    public class BookDetailViewModel : BaseViewModel
    {
        private string id;
        public string ID
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
                Task.Run(() => ExecuteLoadItem());
            }
        }

        private BookDetail book;
        public BookDetail Book
        {
            get => book;
            set => SetProperty(ref book, value);
        }

        public ICommand Refresh { get; }

        private IBookApi bookApi;

        public BookDetailViewModel()
        {
            bookApi = DependencyService.Get<IBookApi>();
            Book = new BookDetail();
        }

        private async Task ExecuteLoadItem()
        {
            IsBusy = true;

            BookDetail response = await bookApi.GetBook(ID);
            if(response != null)
            {
                Book = response;
            }
            else
            {
                await Shell.Current.GoToAsync("//main");
            }

            IsBusy = false;
        }
    }
}
