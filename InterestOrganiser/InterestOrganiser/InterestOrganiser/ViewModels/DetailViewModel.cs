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
    [QueryProperty("Type", "type")]
    [QueryProperty("ID", "id")]
    public class DetailViewModel : BaseViewModel
    {
        private string type;
        public string Type
        {
            get => type;
            set
            {
                SetProperty(ref type, value);
                ItemType();
            }
        }

        private string id;
        public string ID
        {
            get => id;
            set {
                SetProperty(ref id, value);
                Task.Run(() => ExecuteLoadItem());
            }
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

        private string realised;
        public string Realised
        {
            get => realised;
            set => SetProperty(ref realised, value);
        }

        private string realise;
        public string Realise
        {
            get => realise;
            set => SetProperty(ref realise, value);
        }

        private IMovieDB movieDB;
        private IBookApi bookApi;
        public DetailItem Item { get; set; }
        public ICommand Refresh { get; }
        public ICommand AddRealised { get; }
        public ICommand AddToRealise { get; }

        public DetailViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();
            bookApi = DependencyService.Get<IBookApi>();
            Refresh = new Command(async () => await ExecuteLoadItem());

            ItemRealised = false;
            ItemToRealise = true;

            AddRealised = new Command(async () => await AddRealisedItem());
            AddToRealise = new Command(async () => await AddToRealiseItem());

        }

        private async Task AddToRealiseItem()
        {
            ItemToRealise = !ItemToRealise;
        }

        private async Task AddRealisedItem()
        {
            ItemRealised = !ItemRealised;
        }

        private async Task ExecuteLoadItem()
        {
            IsBusy = true;
            DetailItem item = null;
            switch (Type.ToLower())
            {
                case "movie": item = await movieDB.MovieDetail(ID); break;
                case "tv": item = await movieDB.TvDetail(ID); break;
                case "book": item = await bookApi.GetBook(ID);break;
            }

            if(item != null)
            {
                Item = item;
                OnPropertyChanged(nameof(Item));
            }
            else
            {
                await Shell.Current.GoToAsync("//main");
            }

            IsBusy = false;
        }

        private void ItemType()
        {
            switch (Type.ToLower())
            {
                case "movie":
                    Realise = "To watch";
                    Realised = "Watched";
                    break;
                case "tv":
                    Realise = "To watch";
                    Realised = "Watched";
                    break;
                case "book":
                    Realise = "To read";
                    Realised = "Read";
                    break;
            }
        }
    }
}
