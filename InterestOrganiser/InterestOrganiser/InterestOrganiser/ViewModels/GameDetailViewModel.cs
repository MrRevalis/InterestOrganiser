using InterestOrganiser.Models.GameDetail;
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
    public class GameDetailViewModel : BaseViewModel
    {
        private IGameApi gameApi;

        public ICommand AppearingCommand { get; private set; }

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

        public GameDetailViewModel()
        {
            gameApi = DependencyService.Get<IGameApi>();

            AppearingCommand = new Command(async () => await OnAppearing());
        }

        private async Task OnAppearing()
        {
            IsBusy = true;

            if (!String.IsNullOrEmpty(ID))
            {
                GameDetail tempObject = await gameApi.GetGameDetails(ID);
                if(tempObject != null)
                {
                    Game = tempObject;
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
