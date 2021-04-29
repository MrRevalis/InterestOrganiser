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
            set => SetProperty(ref type, value);
        }

        private int id;
        public int ID
        {
            get => id;
            set {
                SetProperty(ref id, value);
                Task.Run(() => ExecuteLoadItem());
            }
        }
        private IMovieDB movieDB;

        public DetailItem Item { get; set; }
        public ICommand Refresh { get; }

        public DetailViewModel()
        {
            movieDB = DependencyService.Get<IMovieDB>();
            Refresh = new Command(async () => await ExecuteLoadItem());
        }

        private async Task ExecuteLoadItem()
        {
            IsBusy = true;

            DetailItem item = await movieDB.MovieDetail(ID);
            if(item != null)
            {
                Item = item;
                OnPropertyChanged(nameof(Item));
            }

            IsBusy = false;
        }
    }
}
