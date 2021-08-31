using Sat.Recruitment.ResourceAccess.Entities;
using Sat.Recruitment.ResourceAccess.Responses;

namespace Sat.Recruitment.Services.Abstractions
{
    public interface IUserService
    {
        ValidationResponse CreateUser(User user);
    }
}