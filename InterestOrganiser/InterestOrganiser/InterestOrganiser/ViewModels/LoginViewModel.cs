using Firebase.Auth;
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
        public string Email { get; set; }
        public string Password { get; set; }

        private const string FirebaseAPI = "AIzaSyAAz_2uNo58Qzq8T7J1t6_-3dNMXfBOFrI";
        public LoginViewModel()
        {
            Login = new Command(async () => await LoginMethod());
            Register = new Command(async () => await Shell.Current.GoToAsync("//login/registration"));
        }

        private async Task LoginMethod()
        {
            if(!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password))
            {
                IsBusy = true;
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseAPI));
                try
                {
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                    var content = await auth.GetFreshAuthAsync();
                    var serializedContent = JsonConvert.SerializeObject(content);
                    Preferences.Set("TokenFirebase", serializedContent);
                    await Shell.Current.GoToAsync("//main");
                }
                catch (Exception e)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", e.Message, "OK");
                }
                IsBusy = false;
            }
        }
    }
}
