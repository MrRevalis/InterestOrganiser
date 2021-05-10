using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterestOrganiser.Services
{
    public interface IFirebase
    {
        Task<string> Login(string email, string password);
        Task<string> CreateAccount(string email, string password);
        bool SignOut();
        bool IsSignIn();
        string GetUserName();
    }
}