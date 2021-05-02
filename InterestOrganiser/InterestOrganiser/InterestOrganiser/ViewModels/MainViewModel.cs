using InterestOrganiser.Models;
using InterestOrganiser.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Xamarin.CommunityToolkit.ObjectModel;

namespace InterestOrganiser.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand Logout { get; }
        public ICommand NavToDetail { get; }
        public ICommand AddToRealised { get; }
        public ICommand AddToRealise { get; }
        public ICommand SearchChanged { get; }
        public ICommand TrendingList { get; }
        public ICommand LoadMoreItems { get; }
        public List<TestItem> ItemList { get; set; }
        public ObservableCollection<SearchItem> SearchItems { get; set; }
        private ObservableRangeCollection<SearchItem> searchItemsDisplay;
        public ObservableRangeCollection<SearchItem> SearchItemsDisplay
        {
            get => searchItemsDisplay;
            set => SetProperty(ref searchItemsDisplay, value);
        }

        private IMovieDB movieDB;
        private IFirebase firebase;
        private IBookApi bookApi;

        private int batchSize = 5;
        private int currentItemsIndex = 0;

        public MainViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();
            firebase = DependencyService.Get<IFirebase>();
            bookApi = DependencyService.Get<IBookApi>();

            SearchItems = new ObservableCollection<SearchItem>();
            SearchItemsDisplay = new ObservableRangeCollection<SearchItem>();

            TrendingList = new Command(async () => await TrendingItems());
            Task.Run(() => TrendingItems());

            ItemList = new List<TestItem>()
            {
                new TestItem(){Name = "Item 1", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 2", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 3", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 4", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 5", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"}
            };

            NavToDetail = new Command<SearchItem>(Navigate);
            AddToRealised = new Command<SearchItem>(async (sender) => await Realised(sender));
            AddToRealise = new Command<SearchItem>(async (sender) => await Realise(sender));
            SearchChanged = new Command<string>(async (sender) => await Search(sender));
            LoadMoreItems = new Command(LoadMore);
            Logout = new Command(LogoutUser);
        }

        private void LoadMore()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if(currentItemsIndex < SearchItems.Count())
            {
                SearchItemsDisplay.AddRange(
                    SearchItems.Skip(currentItemsIndex).Take(batchSize)
                    );

                currentItemsIndex += batchSize;
            }

            IsBusy = false;
        }

        private async Task TrendingItems()
        {
            IsBusy = true;
            List<SearchItem> trending = await movieDB.TrendingList("all", "week");
            if (trending != null && trending.Any())
            {
                SearchItems.Clear();
                foreach (SearchItem item in trending)
                    SearchItems.Add(item);

                SearchItemsDisplay.Clear();
                var itemsNumber = trending.Count < batchSize ? trending.Count : batchSize;
                for (int i = 0; i < itemsNumber; i++)
                {
                    SearchItemsDisplay.Add(trending[i]);
                }
                currentItemsIndex = itemsNumber;
            }
            IsBusy = false;
        }

        private void LogoutUser()
        {
            var logout = firebase.SignOut();
            if (logout)
            {
                Shell.Current.GoToAsync("//login");
            }
        }

        private async Task Search(string text)
        {
            if (String.IsNullOrEmpty(text))
                return;

            IsBusy = true;
            List<SearchItem> movies = await movieDB.SearchMovie(text);
            List<SearchItem> tv = await movieDB.SearchTV(text);
            List<SearchItem> books = await bookApi.SearchBooks(text);

            List<SearchItem> concat = movies.Concat(tv).Concat(books).OrderBy(x => x.Title).ToList();

            if (concat.Any())
            {
                SearchItems.Clear();
                foreach (SearchItem item in concat)
                    SearchItems.Add(item);

                SearchItemsDisplay.Clear();
                var itemsNumber = movies.Count < batchSize ? movies.Count : batchSize;
                for (int i = 0; i < itemsNumber; i++)
                {
                    SearchItemsDisplay.Add(movies[i]);
                }
                currentItemsIndex = itemsNumber;
            }
            IsBusy = false;
        }


        private async Task Realise(SearchItem obj)
        {
            
        }

        private async Task Realised(SearchItem obj)
        {
            
        }

        private async void Navigate(SearchItem obj)
        {
            if (obj == null)
                return;

            if(obj.Type.Equals("book"))
                await Shell.Current.GoToAsync($"detailbook?id={obj.ID}");
            else
                await Shell.Current.GoToAsync($"detail?type={obj.Type}&id={obj.ID}");
        }
    }
}
