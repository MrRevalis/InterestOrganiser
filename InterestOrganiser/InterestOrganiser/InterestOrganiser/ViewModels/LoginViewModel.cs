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

        private string email = "dominikr26@interia.pl";
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        private string password = "123456";
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public LoginViewModel()
        {
            Login = new Command(async () => await LoginMethod());
            Register = new Command(async () => await Shell.Current.GoToAsync("//login/registration"));
        }

        private async Task LoginMethod()
        {
            if(!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password) && !IsNotConnected)
            {
                IsBusy = true;
                var token = await FirebaseAuth.Login(Email, Password);
                if(token != String.Empty)
                {
                    await Shell.Current.GoToAsync("//main");
                    Email = "";
                    Password = "";
                }
                IsBusy = false;
            }
        }
    }
}
