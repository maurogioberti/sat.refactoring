using System;

namespace Sat.Recruitment.ResourceAccess.Entities
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public decimal Money { get; set; }

        //TODO: Validations must be placed better in a resource file
        public static class Validations
        {
            public static readonly string UserCreated = "User Created";
            public static readonly string UserDuplicated = "The user is duplicated";
        }
    }
}
