using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Wallet.Data;
using Wallet.Data.Models;

namespace Wallet.Service
{
    public class WalletService : IUserWallet
    {
        private readonly AppDbContext _context;
        private readonly ICurrencyService _currencyService;
        private readonly IUserService _userService;

        public WalletService(AppDbContext context, IUserService userService, ICurrencyService currencyService)
        {
            _context = context;
            _currencyService = currencyService;
            _userService = userService;
        }

        public UserWallet CreateWallet(string username, string currency)
        {
            var user = _userService.FindByName(username);
            if( user != null && _currencyService.CheckForCurrencyExist(currency) )
            {
                currency = currency.ToUpper();
                var w = _context.UserWallets.FirstOrDefault(x => x.Owner.Id == user.Id && x.CurrencyName == currency);
                if (w == null)
                {
                    w = new UserWallet() { Owner = user, Amount = 0, CurrencyName = currency };
                    _context.Add(w);
                    _context.SaveChanges();
                }
                return w;
            }
            return null;
        }

        public void Exchange(int walletIdFrom, int walletIdTo, float sum)
        {
            if (walletIdFrom == walletIdTo) return;

            var walletFrom = GetById(walletIdFrom);
            if( walletFrom != null && walletFrom.Amount >= sum)
            {
                var walletTo = GetById(walletIdTo);
                if( walletTo != null )
                {
                    if( _currencyService.TryGetCurrencyExchangeRate(walletFrom.CurrencyName, walletTo.CurrencyName, out float rate) )
                    {
                        walletFrom.Amount -= sum;
                        walletTo.Amount += rate * sum;
                        _context.Update(walletFrom);
                        _context.Update(walletTo);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public UserWallet GetById(int walletId)
        {
            return _context.UserWallets.FirstOrDefault(w => w.Id == walletId);
        }

        public IEnumerable<UserWallet> GetByUser(string username)
        {
            return _context.UserWallets.Where(w => w.Owner.UserName == username);
        }

        public void Replanish(int walletId, float sum)
        {
            var wallet = GetById(walletId);
            if( wallet != null )
            {
                UpdateAmount(wallet, sum);
            }
        }

        public void Withdraw(int walletId, float sum)
        {
            var wallet = GetById(walletId);
            if (wallet != null && wallet.Amount >= sum )
            {
                UpdateAmount(wallet, -sum);
            }
        }
        private void UpdateAmount(UserWallet wallet, float sum)
        {
            wallet.Amount += sum;
            _context.Update(wallet);
            _context.SaveChanges();
        }
    }
}
