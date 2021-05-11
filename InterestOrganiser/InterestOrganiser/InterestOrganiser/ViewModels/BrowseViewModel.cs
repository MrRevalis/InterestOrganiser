using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using InterestOrganiser.Services;

namespace InterestOrganiser.ViewModels
{
    public class BrowseViewModel : BaseViewModel
    {

        private IMovieDB movieDB;
        private IBookApi bookApi;
        private IGameApi gameApi;

        public ICommand ChangeTypeCommand { get; private set; }
        public ICommand AppearingCommand { get; private set; }
        public ICommand ChangePageCommand { get; private set; }
        public ICommand AddRealised { get; private set; }
        public ICommand AddToRealise { get; private set; }
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

        private List<FirebaseItem> firebaseItems;
        public List<FirebaseItem> FirebaseItems
        {
            get => firebaseItems;
            set => SetProperty(ref firebaseItems, value);
        }

        private ObservableRangeCollection<BrowseItem> items;
        public ObservableRangeCollection<BrowseItem> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public BrowseViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();
            bookApi = DependencyService.Get<IBookApi>();
            gameApi = DependencyService.Get<IGameApi>();

            ChangeTypeCommand = new Command<string>(async (sender) => await ChangeType(sender));
            AppearingCommand = new Command(async () => await OnAppearing());
            ChangePageCommand = new Command<BrowseItem>(async (sender) => await ChangePage(sender));
            AddRealised = new Command<BrowseItem>(async (sender) => await AddRealisedItem(sender));
            AddToRealise = new Command<BrowseItem>(async (sender) => await AddToRealiseItem(sender));

            Items = new ObservableRangeCollection<BrowseItem>();
        }

        private async Task AddToRealiseItem(BrowseItem item)
        {
            var index = Items.IndexOf(item);
            Items[index].ToRealise = !item.ToRealise;
            FirebaseItem firebaseItem = new FirebaseItem() { ID = item.ID, Owner = FirebaseAuth.GetUserName(), Realised = item.Realised, ToRealise = item.ToRealise, Type = item.Type };
            await FirebaseDB.UpdateItem(firebaseItem);
            await RefreshList();
        }

        private async Task AddRealisedItem(BrowseItem item)
        {
            var index = Items.IndexOf(item);
            Items[index].ToRealise = !item.ToRealise;
            FirebaseItem firebaseItem = new FirebaseItem() { ID = item.ID, Owner = FirebaseAuth.GetUserName(), Realised = item.Realised, ToRealise = item.ToRealise, Type = item.Type };
            await FirebaseDB.UpdateItem(firebaseItem);
            await RefreshList();
        }


        private async Task ChangePage(BrowseItem item)
        {
            if (item == null)
                return;

            switch (item.Type)
            {
                case "books": await Shell.Current.GoToAsync($"detailbook?id={item.ID}"); break;
                case "games": await Shell.Current.GoToAsync($"detailgame?id={item.ID}"); break;
                case "movies": await Shell.Current.GoToAsync($"detail?type={item.Type}&id={item.ID}"); break;
                case "tv series": await Shell.Current.GoToAsync($"detail?type={item.Type}&id={item.ID}"); break;
                default:
                    await Shell.Current.GoToAsync("//main/browse"); break;
            }
        }

        private async Task OnAppearing()
        {
            IsBusy = true;
            FirebaseItems = await FirebaseDB.GetItemsForUser(FirebaseAuth.GetUserName());
            await ChangeType("Favourite");
            IsBusy = false;
        }

        private async Task ChangeType(string type)
        {
            IsBusy = true;
            CurrentItemType = type;
            Items.Clear();
            if (CurrentItemType == "Favourite")
            {
                var selectedList = FirebaseItems.Where(x => x.ToRealise == true).Select(y => y).ToList();
                await CreateList(selectedList);
            }
            else
            {
                var selectedList = FirebaseItems.Where(x => x.Realised == true).Select(y => y).ToList();
                await CreateList(selectedList);
            }

            IsBusy = false;
        }

        private async Task CreateList(List<FirebaseItem> list)
        {
            List<BrowseItem> newList = new List<BrowseItem>();
            foreach(var x in list)
            {
                switch (x.Type)
                {
                    case "movies":
                        var movie = await movieDB.BrowseMovie(x.ID);
                        movie.ToRealise = x.ToRealise;
                        movie.Realised = x.Realised;
                        newList.Add(movie);
                        break;
                    case "tv series":
                        var tv = await movieDB.BrowseTV(x.ID);
                        tv.ToRealise = x.ToRealise;
                        tv.Realised = x.Realised;
                        newList.Add(tv);
                        break;
                    case "games":
                        var game = await gameApi.BrowseGame(x.ID);
                        game.ToRealise = x.ToRealise;
                        game.Realised = x.Realised;
                        newList.Add(game);
                        break;
                    case "books":
                        var book = await bookApi.BrowseBook(x.ID);
                        book.ToRealise = x.ToRealise;
                        book.Realised = x.Realised;
                        newList.Add(book);
                        break;
                }
            }
            Items.AddRange(newList);
        }

        private async Task RefreshList()
        {
            if (CurrentItemType == "Favourite")
            {
                var refreshItems = Items.Where(x => x.ToRealise == true).Select(y => y).ToList();
                Items.Clear();
                Items.AddRange(refreshItems);
            }
            else
            {
                var refreshItems = Items.Where(x => x.Realised == true).Select(y => y).ToList();
                Items.Clear();
                Items.AddRange(refreshItems);
            }
        }
    }
}