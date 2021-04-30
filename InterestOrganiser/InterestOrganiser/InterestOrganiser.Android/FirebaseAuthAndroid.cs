using Firebase.Auth;
using InterestOrganiser.Droid;
using InterestOrganiser.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly : Dependency(typeof(FirebaseAuthAndroid))]
namespace InterestOrganiser.Droid
{
    public class FirebaseAuthAndroid : IFirebase
    {
        public async Task<string> CreateAccount(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = user.User.Uid;
                return token;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "Close");
                return String.Empty;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "Close");
                return String.Empty;
            }
        }

        public bool IsSignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> Login(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = user.User.Uid;
                return token;
            }
            catch(FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "Close");
                return String.Empty;
            }
            catch(FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                await App.Current.MainPage.DisplayAlert("Error", e.Message, "Close");
                return String.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}