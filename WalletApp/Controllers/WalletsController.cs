using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wallet.Data;
using Wallet.Data.Models;

namespace WalletApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IUserWallet _walletService;
        private readonly IUserService _userService;

        public WalletsController(IUserWallet walletService, IUserService userService)
        {
            _walletService = walletService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get(string username)
        {
            var u = _userService.FindByName(username);
            if (u != null)
            {
                var rez = _walletService.GetByUser(username).Select(x => new { x.Id, x.CurrencyName, x.Amount });
                return Ok(rez);
            }
            else
            {
                return BadRequest(username);
            }
        }

        [HttpPost(nameof(Create))]
        public IActionResult Create(string username, string currency)
        {
            var w = _walletService.CreateWallet(username,currency);
            if (w != null)
            {
                return Ok(w);
            }
            else
            {
                return StatusCode(555, "Error creating wallet for " + username + "/" + currency);
            }
        }
        
        [HttpPost(nameof(Withdraw))]
        public IActionResult Withdraw(int id, float sum)
        {
            var wallet = _walletService.GetById(id);
            if( wallet == null )
            {
                return BadRequest("Wallet not found ");
            }
            _walletService.Withdraw(wallet.Id, sum);
            return Ok();
        }

        [HttpPost(nameof(Replanish))]
        public IActionResult Replanish(int id, float sum)
        {
            var wallet = _walletService.GetById(id);
            if (wallet == null)
            {
                return BadRequest("Wallet not found ");
            }
            _walletService.Replanish(wallet.Id, sum);
            return Ok();
        }

        [HttpPost(nameof(Exchange))]
        public IActionResult Exchange(int wallet_from, int wallet_to, float sum)
        {
            if( wallet_from == wallet_to )
            {
                return Ok();
            }
            var wallet_f = _walletService.GetById(wallet_from);
            if (wallet_f == null)
            {
                return BadRequest("Wallet not found " + wallet_from);
            }
            var wallet_t = _walletService.GetById(wallet_to);
            if (wallet_t == null)
            {
                return BadRequest("Wallet not found " + wallet_to);
            }

            _walletService.Exchange(wallet_f.Id, wallet_t.Id, sum);

            return Ok();
        }


    }
}
