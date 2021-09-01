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
        private readonly List<User> _usersCreatedList = new List<User>();

        private IFileManager _fileManager;

        public UserLogic()
        {
        }

        /// <summary>
        /// Only for Unit Test Porpouses, FileManager must by mocked
        /// </summary>
        /// <param name="fileManager"></param>
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
            foreach (var u in _usersCreatedList)
            {
                if ((u.Name == user.Name && u.Address == user.Address) || u.Email == user.Email || u.Phone == user.Phone)
                {
                    return true;
                }
            }

            return false;
        }

        private void ReadUsersList(User user)
        {
            var reader = FileManager.ReadFile("/Files/Users.txt");

            user.Email = NormalizeEmail(user.Email);

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLineAsync().Result;
                if (line != null)
                {
                    User userReaded = MappedUserFromFile(line);
                    _usersCreatedList.Add(userReaded);
                }
            }

            reader.Close();
        }

        private static User MappedUserFromFile(string? line)
        {
            User userReaded = new User
            {
                Name = line.Split(',')[0],
                Email = line.Split(',')[1],
                Phone = line.Split(',')[2],
                Address = line.Split(',')[3],
                UserType = line.Split(',')[4],
                Money = decimal.Parse(line.Split(',')[5]),
            };
            return userReaded;
        }

        private string NormalizeEmail(string email)
        {
            var aux = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            email = string.Join("@", new string[] { aux[0], aux[1] });

            return email;
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