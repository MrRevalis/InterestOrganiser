using InterestOrganiser.Extensions;
using InterestOrganiser.Models;
using InterestOrganiser.Services;
using MovieBase.Models.MovieDetail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private IMovieDB movieDB;
        private string lastItemID;
        private string typeItem;
        #region Command
        public ICommand AppearingCommand { get; private set; }
        public ICommand SearchPageCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }
        public ICommand ChangeTypeCommand { get; private set; }
        public ICommand SwipeCommand { get; private set; }
        public ICommand ChangePageCommand { get; private set; }
        #endregion
        #region Properties
        private ObservableRangeCollection<SearchItem> trendingList;
        public ObservableRangeCollection<SearchItem> TrendingList
        {
            get => trendingList;
            set => SetProperty(ref trendingList, value);
        }

        private List<string> types;
        public List<string> Types
        {
            get => types;
            set => SetProperty(ref types, value);
        }

        private ObservableRangeCollection<SearchItem> actualMovies;
        public ObservableRangeCollection<SearchItem> ActualMovies
        {
            get => actualMovies;
            set => SetProperty(ref actualMovies, value);
        }
        
        private string currentItemType;
        public string CurrentItemType
        {
            get => currentItemType;
            set
            {
                SetProperty(ref currentItemType, value);
                OnPropertyChanged(nameof(CurrentItemType));
            }
        }

        private Dictionary<string, List<SearchItem>> moviesCollection;
        public Dictionary<string, List<SearchItem>> MoviesCollection
        {
            get => moviesCollection;
            set => SetProperty(ref moviesCollection, value);
        }

        #endregion

        public MainViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();

            TrendingList = new ObservableRangeCollection<SearchItem>();
            Types = new List<string>() { "Action", "Adventure", "Animation", "Comedy", "Crime", "Documentary", "Drama", "Family", "Fantasy", "History", "Horror", "Music", "Mystery", "Romance", "Science Fiction", "TV Movie", "Thriller", "War", "Western" };
            MoviesCollection = new Dictionary<string, List<SearchItem>>();
            ActualMovies = new ObservableRangeCollection<SearchItem>();

            AppearingCommand = new Command(async (sender) => await OnAppearing());
            ChangePageCommand = new Command<SearchItem>(async (sender) => await ChangePage(sender));

            SearchPageCommand = new Command(GoToSearchPage);
            SignOutCommand = new Command(SignOut);
            ChangeTypeCommand = new Command<string>(ChangeType);
            SwipeCommand = new Command<int>(Swipe);
        }

        private void Swipe(int value)
        {
            int actualIndex = Types.IndexOf(CurrentItemType);
            int newIndex = (actualIndex + value) % Types.Count;
            int correctedIndex = newIndex < 0 ? Types.Count - 1 : newIndex;

            ChangeType(Types[correctedIndex]);
        }

        private void ChangeType(string type)
        {
            if (MoviesCollection.Any())
            {
                CurrentItemType = type;
                ActualMovies.Clear();
                ActualMovies.AddRange(MoviesCollection.FirstOrDefault(x => x.Key == CurrentItemType).Value);
            }
        }

        private async void SignOut()
        {
            bool signOut = FirebaseAuth.SignOut();

            if (signOut)
            {
                await Shell.Current.GoToAsync($"//login");
            }
        }

        private async void GoToSearchPage()
        {
            await Shell.Current.GoToAsync($"//main/search?type={typeItem}&id={lastItemID}");
        }
        private async Task ChangePage(SearchItem item)
        {
            if (item == null)
                return;

            if (item.Type.Equals("books"))
                await Shell.Current.GoToAsync($"detailbook?id={item.ID}");
            else
            {
                lastItemID = item.ID;
                typeItem = item.Type;
                await Shell.Current.GoToAsync($"detail?type={item.Type}&id={item.ID}");
            }

        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            List<SearchItem> trending = await movieDB.TrendingList("all", "week");
            if (trending != null && trending.Any())
            {
                TrendingList.RemoveAll();

                var amount = trending.Count < 5 ? trending.Count : 5;
                TrendingList.AddRange(trending.Take(amount));
            }

            if (!MoviesCollection.Any())
            {
                await CompleteCarousel();
            }

            Swipe(0);
            IsBusy = false;
        }

        private async Task CompleteCarousel()
        {
            List<Genre> genres = await movieDB.MoviesGenres();
            if(genres != null && genres.Any())
            {
                //Types.AddRange(genres.Select(x => x.name).ToList());
                foreach (Genre item in genres)
                {
                    List<SearchItem> genreList = await movieDB.MoviesList(item.id);
                    MoviesCollection.Add(item.name, genreList);
                }
            }
        }
    }
}
