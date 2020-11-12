using System;
using System.Collections.Generic;
using System.Text;

namespace Wallet.Data.Models
{
    public class UserWallet
    {
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public float Amount { get; set; }
        public virtual AppUser Owner { get; set; }
    }
}
