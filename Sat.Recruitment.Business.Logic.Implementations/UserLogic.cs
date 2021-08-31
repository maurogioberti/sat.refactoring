using Sat.Recruitment.Business.Logic.Abstractions;
using Sat.Recruitment.ResourceAccess.Entities;
using Sat.Recruitment.ResourceAccess.FileManager;
using Sat.Recruitment.ResourceAccess.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
            var errors = "";
            
            ValidateErrors(user.Name, user.Email, user.Address, user.Phone, ref errors);

            if (!string.IsNullOrEmpty(errors))
                return new ValidationResponse()
                {
                    Success = false,
                    Message= errors
                };

            switch (user.UserType)
            {
                case "Normal":
                {
                    if (user.Money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.12);
                        //If new user is normal and has more than USD100
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }
                    if (user.Money < 100)
                    {
                        if (user.Money > 10)
                        {
                            var percentage = Convert.ToDecimal(0.8);
                            var gif = user.Money * percentage;
                            user.Money = user.Money + gif;
                        }
                    }

                    break;
                }
                case "SuperUser":
                {
                    if (user.Money > 100)
                    {
                        var percentage = Convert.ToDecimal(0.20);
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }

                    break;
                }
                case "Premium":
                {
                    if (user.Money > 100)
                    {
                        var gif = user.Money * 2;
                        user.Money = user.Money + gif;
                    }

                    break;
                }
            }


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
            try
            {
                var isDuplicated = false;
                foreach (var u in _users)
                {
                    if (u.Email == user.Email || u.Phone == user.Phone)
                    {
                        isDuplicated = true;
                    }
                    else if (u.Name == user.Name && u.Address == user.Address)
                    {
                        isDuplicated = true;
                        throw new Exception("User is duplicated");
                    }
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");

                    return new ValidationResponse()
                    {
                        Success = true,
                        Message = "User Created"
                    };
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");

                    return new ValidationResponse()
                    {
                        Success = false,
                        Message = "The user is duplicated"
                    };
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");
                return new ValidationResponse()
                {
                    Success = false,
                    Message = "The user is duplicated"
                };
            }
        }

        //Validate errors
        private void ValidateErrors(string name, string email, string address, string phone, ref string errors)
        {
            if (name == null)
                //Validate if Name is null
                errors = "The name is required";
            if (email == null)
                //Validate if Email is null
                errors = errors + " The email is required";
            if (address == null)
                //Validate if Address is null
                errors = errors + " The address is required";
            if (phone == null)
                //Validate if Phone is null
                errors = errors + " The phone is required";
        }
    }
}