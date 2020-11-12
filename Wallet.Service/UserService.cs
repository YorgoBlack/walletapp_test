using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wallet.Data;
using Wallet.Data.Models;

namespace Wallet.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public AppUser CreateUser(string username)
        {
            if( !string.IsNullOrWhiteSpace(username) && FindByName(username) == null )
            {
                var user = new AppUser() { UserName = username };
                _context.Add( user );
                _context.SaveChanges();
                return user;
            }
            return null;
        }

        public AppUser FindByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null;
            return _context.Users.FirstOrDefault(x => x.UserName == userName);
        }

        public IEnumerable<string> GetAll()
        {
            return _context.Users.Select(x => x.UserName);
        }
    }
}
