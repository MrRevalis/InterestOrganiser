using InterestOrganiser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

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

        public MainViewModel()
        {
            ItemList = new List<TestItem>()
            {
                new TestItem(){Name = "Item 1", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 2", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 3", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 4", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"},
                new TestItem(){Name = "Item 5", Image = "https://cdn.statically.io/img/c4.wallpaperflare.com/wallpaper/65/969/298/danmachi-is-it-wrong-to-try-to-pick-up-girls-in-a-dungeon-bell-cranel-hestia-danmachi-liliruca-arde-hd-wallpaper-preview.jpg"}
            };

            NavToDetail = new Command<TestItem>(Navigate);
            AddToRealised = new Command<TestItem>(Realised);
            AddToRealise = new Command<TestItem>(Realise);

            SearchChanged = new Command(async (sender) => await Search(sender));
        }

        private async Task Search(object sender)
        {
            string text = sender as string;
        }

        private async void Realise(TestItem obj)
        {
            
        }

        private async void Realised(TestItem obj)
        {
            
        }

        private async void Navigate(TestItem obj)
        {
            if (obj == null)
                return;

            await Shell.Current.GoToAsync($"detail?name={obj.Name}");
        }
    }
}
