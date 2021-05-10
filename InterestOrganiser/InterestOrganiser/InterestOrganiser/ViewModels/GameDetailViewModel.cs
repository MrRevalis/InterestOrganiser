using InterestOrganiser.Models;
using InterestOrganiser.Models.GameDetail;
using InterestOrganiser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    [QueryProperty("ID", "id")]
    public class GameDetailViewModel : BaseViewModel
    {
        private IGameApi gameApi;

        public ICommand AppearingCommand { get; private set; }
        public ICommand PlayVideoCommand { get; private set; }
        public ICommand AddRealised { get; private set; }
        public ICommand AddToRealise { get; private set; }

        private string id;
        public string ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private GameDetail game;
        public GameDetail Game
        {
            get => game;
            set => SetProperty(ref game, value);
        }

        private ObservableRangeCollection<Trailers> trailersList;
        public ObservableRangeCollection<Trailers> TrailerList
        {
            get => trailersList;
            set => SetProperty(ref trailersList, value);
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

        public GameDetailViewModel()
        {
            gameApi = DependencyService.Get<IGameApi>();
            TrailerList = new ObservableRangeCollection<Trailers>();
            AppearingCommand = new Command(async () => await OnAppearing());
            PlayVideoCommand = new Command<string>(async (sender) => await PlayVideo(sender));
            AddRealised = new Command(async () => await AddRealisedItem());
            AddToRealise = new Command(async () => await AddToRealiseItem());
        }

        private async Task PlayVideo(string link)
        {
            if (String.IsNullOrEmpty(link))
                return;

            await Shell.Current.GoToAsync($"video?url={link}&type=normal");
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

            if (!String.IsNullOrEmpty(ID))
            {
                GameDetail tempObject = await gameApi.GetGameDetails(ID);
                List<Trailers> trailers = await gameApi.GetGameTrailers(ID);
                if(tempObject != null)
                {
                    Game = tempObject;

                    if (trailers.Any())
                    {
                        TrailerList.AddRange(trailers);
                    }

                    ItemDB = await FirebaseDB.CheckItem(FirebaseAuth.GetUserName(), ID);
                    if (ItemDB.ID == null)
                    {
                        ItemDB.ID = ID;
                        ItemDB.Owner = FirebaseAuth.GetUserName();
                        ItemDB.Type = "games";
                    }
                    ItemRealised = ItemDB.Realised;
                    ItemToRealise = ItemDB.ToRealise;

                }
                else
                {
                    await Shell.Current.GoToAsync("//main");
                }

            }
            else
            {
                await Shell.Current.GoToAsync("//main");
            }

            IsBusy = false;
        }
    }
}
