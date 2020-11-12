using System;
using System.Collections.Generic;
using System.Text;
using Wallet.Data.Models;

namespace Wallet.Data
{
    public interface IUserService
    {
        AppUser CreateUser(string username);
        AppUser FindByName(string username);
        IEnumerable<string> GetAll();
    }
}
