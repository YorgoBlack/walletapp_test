using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Wallet.Data.Models
{
    public class AppUser : IdentityUser
    {
        public IEnumerable<UserWallet> Wallets { get; set; }
    }
}
