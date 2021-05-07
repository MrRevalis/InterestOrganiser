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
        private IBookApi bookApi;

        public ICommand AppearingCommand { get; }

        private string id;
        public string ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private BookDetail book;
        public BookDetail Book
        {
            get => book;
            set => SetProperty(ref book, value);
        }

        public BookDetailViewModel()
        {
            bookApi = DependencyService.Get<IBookApi>();

            Book = new BookDetail();

            AppearingCommand = new Command(async () => await OnAppearing());
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            BookDetail response = await bookApi.GetBook(ID);
            if (response != null)
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
