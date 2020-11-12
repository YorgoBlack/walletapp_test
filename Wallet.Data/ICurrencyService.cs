using System;
using System.Collections.Generic;
using System.Text;
using Wallet.Data.Models;

namespace Wallet.Data
{
    public interface ICurrencyService
    {
        bool CheckForCurrencyExist(string name);
        bool TryGetCurrencyExchangeRate(string nameFrom, string nameTo, out float rate);
    }
}
