using InterestOrganiser.Models;
using InterestOrganiser.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    [QueryProperty("Type", "type")]
    [QueryProperty("ID", "id")]
    public class DetailViewModel : BaseViewModel
    {
        private IMovieDB movieDB;

        public ICommand AppearingCommand { get; private set; }
        public ICommand AddRealised { get; private set; }
        public ICommand AddToRealise { get; private set; }
        public ICommand PlayVideoCommand { get; private set; }

        private string type;
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private string id;
        public string ID
        {
            get => id;
            set => SetProperty(ref id, value);
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

        private DetailItem item;
        public DetailItem Item 
        {
            get => item;
            set => SetProperty(ref item, value);
        }

        private ObservableRangeCollection<Video> videosList;
        public ObservableRangeCollection<Video> VideoList
        {
            get => videosList;
            set => SetProperty(ref videosList, value);
        }


        public DetailViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();

            Item = new DetailItem();
            VideoList = new ObservableRangeCollection<Video>();

            AppearingCommand = new Command(async () => await OnAppearing());
            AddRealised = new Command(async () => await AddRealisedItem());
            AddToRealise = new Command(async () => await AddToRealiseItem());
            PlayVideoCommand = new Command<string>(async (sender) => await PlayVideo(sender));

            ItemRealised = false;
            ItemToRealise = true;
        }

        private async Task PlayVideo(string link)
        {
            if (String.IsNullOrEmpty(link))
                return;

            await Shell.Current.GoToAsync($"video?url={link}");
        }

        private async Task AddToRealiseItem()
        {
            ItemToRealise = !ItemToRealise;
        }

        private async Task AddRealisedItem()
        {
            ItemRealised = !ItemRealised;
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            switch (Type.ToLower())
            {
                case "movies": Item = await movieDB.MovieDetail(ID); break;
                case "tv series": Item = await movieDB.TvDetail(ID); break;
            }

            List<Video> videos = await movieDB.MoviesTrailers(ID, Type.ToLower());
            VideoList.AddRange(videos);

            if (Item == null)
                await Shell.Current.GoToAsync("//main");

            IsBusy = false;
        }

    }
}
