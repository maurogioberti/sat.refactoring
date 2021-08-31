using Sat.Recruitment.Business.Logic.Abstractions;
using Sat.Recruitment.Business.Logic.Implementations;
using Sat.Recruitment.ResourceAccess.Entities;
using Sat.Recruitment.ResourceAccess.Responses;
using Sat.Recruitment.Services.Abstractions;
using System;

namespace Sat.Recruitment.Services.Implementations
{
    public class UserService : IUserService
    {
        private IUserLogic _userLogic;
        public IUserLogic UserLogic
        {
            get
            {
                if (_userLogic == default)
                    _userLogic = new UserLogic();

                return _userLogic;
            }
        }

        public ValidationResponse CreateUser(User user)
        {
            try
            {
                return UserLogic.CreateUser(user);
            }
            catch (Exception exception)
            {
                //log exception
            }
            
            return null;
        }
    }
}
