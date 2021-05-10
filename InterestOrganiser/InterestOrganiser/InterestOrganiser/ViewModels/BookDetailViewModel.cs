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
        public ICommand AddRealised { get; private set; }
        public ICommand AddToRealise { get; private set; }

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

        private bool itemRealised;
        public bool ItemRealised
        {
            get => itemRealised;
            set => SetProperty(ref itemRealised, value);
        }

        private bool itemToRealise;
        public bool ItemToRealise
        {
            get => itemToRealise;
            set => SetProperty(ref itemToRealise, value);
        }

        private FirebaseItem itemDB;
        public FirebaseItem ItemDB
        {
            get => itemDB;
            set => SetProperty(ref itemDB, value);
        }

        public BookDetailViewModel()
        {
            bookApi = DependencyService.Get<IBookApi>();

            Book = new BookDetail();

            AppearingCommand = new Command(async () => await OnAppearing());
            AddRealised = new Command(async () => await AddRealisedItem());
            AddToRealise = new Command(async () => await AddToRealiseItem());
        }

        private async Task AddToRealiseItem()
        {
            ItemDB.ToRealise = !ItemDB.ToRealise;
            ItemToRealise = ItemDB.ToRealise;
            await FirebaseDB.UpdateItem(ItemDB);
        }

        private async Task AddRealisedItem()
        {
            ItemDB.Realised = !ItemDB.Realised;
            ItemRealised = ItemDB.Realised;
            await FirebaseDB.UpdateItem(ItemDB);
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            BookDetail response = await bookApi.GetBook(ID);
            if (response != null)
            {
                Book = response;

                ItemDB = await FirebaseDB.CheckItem(FirebaseAuth.GetUserName(), ID);
                if (ItemDB.ID == null)
                {
                    ItemDB.ID = ID;
                    ItemDB.Owner = FirebaseAuth.GetUserName();
                    ItemDB.Type = "books";
                }
                ItemRealised = ItemDB.Realised;
                ItemToRealise = ItemDB.ToRealise;

            }
            else
            {
                await Shell.Current.GoToAsync("//main");
            }

            IsBusy = false;
        }
    }
}
