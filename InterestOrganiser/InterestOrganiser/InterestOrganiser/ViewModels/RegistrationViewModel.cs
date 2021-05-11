using InterestOrganiser.Services;
using InterestOrganiser.Validation;
using InterestOrganiser.ValidationRules;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterestOrganiser.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        public ICommand Return { get; }
        public ICommand Register { get; }
        public ValidatableObject<string> Email { get; set; }
        public ValidatablePair<string> Password { get; set; }

        public RegistrationViewModel()
        {
            Return = new Command(async () => await Shell.Current.GoToAsync("//login"));
            Register = new Command(async () => await Registration());
            Email = new ValidatableObject<string>();
            Password = new ValidatablePair<string>();

            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Invalid Email" });

            Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            Password.Item1.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "Password between 6-20 characters", MinimunLength = 6, MaximunLength = 20 });
            Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm password required" });
            Password.Validations.Add(new PasswordsMatchRule<string> { ValidationMessage = "Password and confirm password don't match" });
        }

        private async Task Registration()
        {
            if (ValidateFields() && !IsNotConnected)
            {
                var user = await FirebaseAuth.CreateAccount(Email.Value, Password.Item1.Value);
                if(user != String.Empty)
                {
                    var signOut = FirebaseAuth.SignOut();
                    if (signOut)
                    {
                        await Shell.Current.GoToAsync("//login");
                    }
                }
            }
        }

        private bool ValidateFields()
        {
            bool emailValid = Email.Validate();
            bool passwordValid = Password.Validate();

            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));

            return emailValid && passwordValid;
        }
    }
}
