using InterestOrganiser.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand Login { get; }
        public ICommand Register { get; }
        public string Email { get; set; } = "dominikr26@interia.pl";
        public string Password { get; set; } = "123456";

        private IFirebase auth;

        public LoginViewModel()
        {
            Login = new Command(async () => await LoginMethod());
            Register = new Command(async () => await Shell.Current.GoToAsync("//login/registration"));
            auth = DependencyService.Get<IFirebase>();
        }

        private async Task LoginMethod()
        {
            if(!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password))
            {
                IsBusy = true;
                var token = await auth.Login(Email, Password);
                if(token != String.Empty)
                {
                    await Shell.Current.GoToAsync("//main");
                }
                IsBusy = false;
            }
        }
    }
}
