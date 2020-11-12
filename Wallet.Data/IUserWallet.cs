using System;
using System.Collections.Generic;
using System.Text;
using Wallet.Data.Models;

namespace Wallet.Data
{
    public interface IUserWallet
    {
        UserWallet CreateWallet(string username, string currency);
        UserWallet GetById(int walletId);
        IEnumerable<UserWallet> GetByUser(string username);
        void Withdraw(int walletId, float sum);
        void Replanish(int walletId, float sum);
        void Exchange(int walletIdFrom, int walletIdTo, float sum);
    }
}
