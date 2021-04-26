using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand Login { get; }
        public ICommand Register { get; }
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            Login = new Command(async () => await LoginMethod());
        }

        private async Task LoginMethod()
        {

        }

    }
}
