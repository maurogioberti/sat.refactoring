using System;
using System.Collections.Generic;
using System.Text;
using Sat.Recruitment.Business.Logic.Abstractions;

namespace Sat.Recruitment.Business.Logic.Implementations
{
    public class PremiumUserGiftCalculator : IUserGiftCalculator
    {
        public decimal Calculate(decimal money)
        {
            if (money > 100)
            {
                var gif = money * 2;
                money = money + gif;
            }

            return money;
        }
    }
}
