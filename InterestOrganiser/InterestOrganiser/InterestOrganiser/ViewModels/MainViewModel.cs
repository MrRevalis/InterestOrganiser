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

namespace InterestOrganiser.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand Logout { get; }
        public ICommand NavToDetail { get; }
        public ICommand AddToRealised { get; }
        public ICommand AddToRealise { get; }
        public ICommand SearchChanged { get; }
        public List<TestItem> ItemList { get; set; }
        public ObservableCollection<SearchItem> SearchItems { get; set; }

        private IMovieDB movieDB;
        private IFirebase firebase;

        public MainViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();
            firebase = DependencyService.Get<IFirebase>();
            SearchItems = new ObservableCollection<SearchItem>();

            ItemList = new List<TestItem>()
            {
                new TestItem(){Name = "Item 1", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 2", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 3", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 4", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 5", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"}
            };

            NavToDetail = new Command<SearchItem>(Navigate);
            AddToRealised = new Command<SearchItem>(Realised);
            AddToRealise = new Command<SearchItem>(Realise);

            SearchChanged = new Command<string>(async (sender) => await Search(sender));

            Logout = new Command(LogoutUser);
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

            List<SearchItem> movies = await movieDB.SearchMovie(text);
            if (movies != null && movies.Any())
            {
                SearchItems.Clear();
                foreach (SearchItem item in movies)
                    SearchItems.Add(item);
            }
        }

        private async void Realise(SearchItem obj)
        {
            
        }

        private async void Realised(SearchItem obj)
        {
            
        }

        private async void Navigate(SearchItem obj)
        {
            if (obj == null)
                return;

            await Shell.Current.GoToAsync($"detail?type={obj.Type}&id={obj.ID}");
        }
    }
}
