using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Wallet.Data
{
    public abstract class CurrencyApiProvider
    {
        protected HttpClient httpClient;
        protected CurrencyApiProvider()
        {
            httpClient = new HttpClient();
        }

        public abstract Dictionary<string, float> GetCurrenciesRates();
    }
}
