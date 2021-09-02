using System;

namespace Sat.Recruitment.ResourceAccess.Entities
{
    public partial class User
    {
        //TODO: Validations must be placed better in a resource file
        public static class Validations
        {
            public static readonly string UserCreated = "User Created";
            public static readonly string UserDuplicated = "The user is duplicated";
            public static readonly string NullValueFieldMask = "The {0} is required";
        }
    }
}
