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
        public ICommand SelectedType { get; }
        public ObservableCollection<SearchItem> SearchItems { get; set; }
        private ObservableRangeCollection<SearchItem> searchItemsDisplay;
        public ObservableRangeCollection<SearchItem> SearchItemsDisplay
        {
            get => searchItemsDisplay;
            set => SetProperty(ref searchItemsDisplay, value);
        }

        private List<SearchItem> entireCollection;
        public List<SearchItem> EntireCollection
        {
            get => entireCollection;
            set => SetProperty(ref entireCollection, value);
        }

        private string type = "ALL";
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }
        public ObservableCollection<string> TypesList { get; set; }

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

            NavToDetail = new Command<SearchItem>(Navigate);
            AddToRealised = new Command<SearchItem>(async (sender) => await Realised(sender));
            AddToRealise = new Command<SearchItem>(async (sender) => await Realise(sender));
            SearchChanged = new Command<string>(async (sender) => await Search(sender));
            LoadMoreItems = new Command(LoadMore);
            Logout = new Command(LogoutUser);
            TypesList = new ObservableCollection<string>() { "ALL", "MOVIES", "TV SERIES", "BOOKS", "GAMES" };


            SelectedType = new Command<string>(async (sender) => await TypeChanged(sender));
        }

        private async Task TypeChanged(string sender)
        {
            Type = sender;
            await ChangeCollectionType(Type.ToLower());
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
                EntireCollection = trending;

                if (Type.ToLower().Equals("all"))
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
                else
                {
                    await ChangeCollectionType(Type.ToLower());
                }
                
            }
            OnPropertyChanged(nameof(Type));
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
                EntireCollection = concat;

                if (Type.ToLower().Equals("all"))
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
                else
                {
                    await ChangeCollectionType(Type.ToLower());
                }
                
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

            if(obj.Type.Equals("books"))
                await Shell.Current.GoToAsync($"detailbook?id={obj.ID}");
            else
                await Shell.Current.GoToAsync($"detail?type={obj.Type}&id={obj.ID}");
        }

        private async Task ChangeCollectionType(string type)
        {
            SearchItemsDisplay.Clear();

            if (type.Equals("all"))
            {
                var itemsNumber = EntireCollection.Count < batchSize ? EntireCollection.Count : batchSize;
                SearchItems.Clear();
                foreach (var x in EntireCollection)
                {
                    SearchItems.Add(x);
                }

                for (int i = 0; i < itemsNumber; i++)
                {
                    SearchItemsDisplay.Add(EntireCollection[i]);
                }
                currentItemsIndex = itemsNumber;
            }
            else
            {
                List<SearchItem> typesCollection = EntireCollection.Select(x => x).Where(x => x.Type.Equals(type)).ToList();
                var itemsNumber = typesCollection.Count < batchSize ? typesCollection.Count : batchSize;

                SearchItems.Clear();
                foreach (var x in typesCollection)
                {
                    SearchItems.Add(x);
                }

                for (int i = 0; i < itemsNumber; i++)
                {
                    SearchItemsDisplay.Add(typesCollection[i]);
                }
                currentItemsIndex = itemsNumber;
            }
        }
    }
}
