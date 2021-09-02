using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Sat.Recruitment.Business.Logic.Implementations;
using Sat.Recruitment.ResourceAccess.Entities;
using Sat.Recruitment.ResourceAccess.Responses;
using Sat.Recruitment.Services.Abstractions;

namespace Sat.Recruitment.Api.Controllers
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

        [HttpPost]
        [Route("/create-user")]
        public async Task<ValidationResponse> CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            
            var user = new User
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = decimal.Parse(money)
            };

            return _userService.CreateUser(user);
        }

    }

}
