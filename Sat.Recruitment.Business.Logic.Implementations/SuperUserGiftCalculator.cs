using System;
using System.Collections.Generic;
using System.Text;
using Sat.Recruitment.Business.Logic.Abstractions;

namespace Sat.Recruitment.Business.Logic.Implementations
{
    public class SuperUserGiftCalculator : IUserGiftCalculator
    {
        public decimal Calculate(decimal money)
        {
            if (money > 100)
            {
                var percentage = Convert.ToDecimal(0.20);
                var gif = money * percentage;
                money = money + gif;
            }

            return money;
        }
    }
}
