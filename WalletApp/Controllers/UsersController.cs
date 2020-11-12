using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Data;
using Wallet.Data.Models;

namespace WalletApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet(nameof(Find))]
        public IActionResult Find(string username)
        {
            var user = _userService.FindByName(username);
            if( user != null )
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("User not found + " + username);
            }
            
        }

        [HttpPost(nameof(Create))]
        public IActionResult Create(string username)
        {
            var user = _userService.CreateUser(username);
            if (user != null )
            {
                
                return Ok();
            }
            else
            {
                return StatusCode(555, "Error creating user " + username);
            }
        }
    }
}
