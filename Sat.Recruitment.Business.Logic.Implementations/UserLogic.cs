using Sat.Recruitment.Business.Logic.Abstractions;
using Sat.Recruitment.ResourceAccess.Entities;
using Sat.Recruitment.ResourceAccess.FileManager;
using Sat.Recruitment.ResourceAccess.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sat.Recruitment.Business.Logic.Implementations
{
    public class UserLogic : IUserLogic
    {
        private readonly List<User> _users = new List<User>();

        private IFileManager _fileManager;

        public UserLogic()
        {
        }

        public UserLogic(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        public IFileManager FileManager
        {
            get
            {
                if (_fileManager == default)
                    _fileManager = new FileManager();

                return _fileManager;
            }
        }
        public ValidationResponse CreateUser(User user)
        {
            if (HasValidationErrors(user.Name, user.Email, user.Address, user.Phone, out string message))
                return new ValidationResponse(false, message);

            IUserGiftCalculator userGiftCalculator = GiftCalculatorFactory(user);
            user.Money = userGiftCalculator.Calculate(user.Money);

            ReadUsersList(user);

            return IsUserDuplicated(user)
                ? new ValidationResponse(false, User.Validations.UserDuplicated)
                : new ValidationResponse(true, User.Validations.UserCreated);
        }

        private bool IsUserDuplicated(User user)
        {
            try
            {
                foreach (var u in _users)
                {
                    if ((u.Name == user.Name && u.Address == user.Address) || u.Email == user.Email || u.Phone == user.Phone)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        private void ReadUsersList(User user)
        {
            var reader = FileManager.ReadFile("/Files/Users.txt");

            //Normalize email
            var aux = user.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            user.Email = string.Join("@", new string[] { aux[0], aux[1] });

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                User userReaded = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                _users.Add(userReaded);
            }

            reader.Close();
        }

        private IUserGiftCalculator GiftCalculatorFactory(User user)
        {
            IUserGiftCalculator userGiftCalculator = null;

            if(user.UserType == UserType.Normal)
            {
                userGiftCalculator = new NormalUserGiftCalculator();
            }

            if (user.UserType == UserType.SuperUser)
            {
                userGiftCalculator = new SuperUserGiftCalculator();
            }

            if (user.UserType == UserType.Premium)
            {
                userGiftCalculator = new PremiumUserGiftCalculator();
            }

            return userGiftCalculator;
        }

        //Validate errors
        private bool HasValidationErrors(string name, string email, string address, string phone, out string errors)
        {
            StringBuilder validationErrors = new StringBuilder();

            if (name == null)
                validationErrors.Append(string.Format(User.Validations.NullValueFieldMask, "name"));
            if (email == null)
                validationErrors.Append(string.Format(User.Validations.NullValueFieldMask, "email"));
            if (address == null)
                validationErrors.Append(string.Format(User.Validations.NullValueFieldMask, "address"));
            if (phone == null)
                validationErrors.Append(string.Format(User.Validations.NullValueFieldMask, "phone"));

            errors = validationErrors.ToString();

            return validationErrors.Length > 0;
        }
    }
}