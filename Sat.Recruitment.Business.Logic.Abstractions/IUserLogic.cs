using Sat.Recruitment.ResourceAccess.Entities;
using Sat.Recruitment.ResourceAccess.Responses;

namespace Sat.Recruitment.Business.Logic.Abstractions
{
    public interface IUserLogic
    {
        ValidationResponse CreateUser(User user);
    }
}