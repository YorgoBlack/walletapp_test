using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Wallet.Data;
using Wallet.Data.Models;

namespace Wallet.Service
{

    public class CurrencyService : ICurrencyService
    {
        volatile bool busy;
        readonly TimeSpan updateInterval;
        Dictionary<string, float> currenciesRates;
        DateTime lastUploadTime;
        CurrencyApiProvider _currencyApiProvider;

        public CurrencyService(CurrencyApiProvider currencyApiProvider)
        {
            busy = false;
            currenciesRates = new Dictionary<string, float>();
            _currencyApiProvider = currencyApiProvider;
            lastUploadTime = DateTime.MinValue;
            updateInterval = TimeSpan.FromSeconds(30);
        }
        // return True if currency is known for us 
        public bool CheckForCurrencyExist(string currency)
        {
            updateCurrencies();
            if (string.IsNullOrWhiteSpace(currency)) return false;
            return currenciesRates.ContainsKey(currency.ToUpper());
        }

        public bool TryGetCurrencyExchangeRate(string currencyFrom, string currencyTo, out float rate)
        {
            rate = 0;
            if (string.IsNullOrWhiteSpace(currencyFrom) || string.IsNullOrWhiteSpace(currencyTo)) return false;
            updateCurrencies();
            currencyFrom = currencyFrom.ToUpper();
            currencyTo = currencyTo.ToUpper();
            var f = currenciesRates.ContainsKey(currencyFrom) ? currenciesRates[currencyFrom] : 0;
            var t = currenciesRates.ContainsKey(currencyTo) ? currenciesRates[currencyTo] : 0;
            if (f == 0 || t == 0) return false;
            rate = t / f;
            return true;
        }

        void updateCurrencies()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (busy)
            {
                Thread.Sleep(50);
                if (sw.Elapsed > TimeSpan.FromMinutes(1))
                {
                    return;
                }
            }
            try
            {
                busy = true;
                if (DateTime.Now + updateInterval > lastUploadTime)
                {
                    var current = _currencyApiProvider.GetCurrenciesRates();
                    if( current != null )
                    {
                        currenciesRates = current;
                        lastUploadTime = DateTime.Now;
                    }
                    else
                    {
                        currenciesRates.Clear();
                        lastUploadTime = DateTime.MinValue;
                    }
                }
            }
            catch 
            {
                lastUploadTime = DateTime.MinValue;
            }
            finally
            {
                busy = false;
            }
        }
    }
}
