using System;
using System.Collections.Generic;
using System.Text;
using Sat.Recruitment.Business.Logic.Abstractions;

namespace Sat.Recruitment.Business.Logic.Implementations
{
    public class NormalUserGiftCalculator : IUserGiftCalculator
    {
        private const decimal PercentageMoreThanHundred = 0.12m;
        private const decimal PercentageLessThanHundredAndMoreThanTen = 0.8m;

        public decimal Calculate(decimal money)
        {
            if (money > 100)
            {
                var gif = money * PercentageMoreThanHundred;
                money = money + gif;
            }

            if (money < 100 && money > 10)
            {
                var gif = money * PercentageLessThanHundredAndMoreThanTen;
                money = money + gif;
            }

            return money;
        }
    }
}
