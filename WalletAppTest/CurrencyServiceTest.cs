using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Net.Http;
using Wallet.Data;
using Wallet.Service;
using WalletApp;
using WalletApp.Controllers;

namespace WalletAppTest
{
    [TestClass]
    public class CurrencyServiceTest
    {
        readonly string ConnectionStr = "Server=(localdb)\\MSSQLLocalDB;Database=Wallet.Dev;Integrated Security=True";
        ServiceProvider serviceProvider;

        public CurrencyServiceTest()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionStr));

            services.AddSingleton<CurrencyApiProvider, EuropaCurrencyProvider>();
            services.AddSingleton<ICurrencyService, CurrencyService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserWallet, WalletService>();

            serviceProvider = services.BuildServiceProvider();
        }
        [TestMethod]
        public void Scenario_CreateUser()
        {
            var users = new UsersController(serviceProvider.GetService<IUserService>());
            string vasya = "vasya";
            var rez = users.Create(vasya);
            Assert.IsTrue(rez is Microsoft.AspNetCore.Mvc.OkResult, 
                (rez as Microsoft.AspNetCore.Mvc.ObjectResult).Value.ToString() );
        }

        // etc 
    }
}
