using System;
using System.Collections.Generic;
using System.Text;

namespace Sat.Recruitment.Business.Logic.Abstractions
{
    public interface IUserGiftCalculator
    {
        decimal Calculate(decimal money);
    }
}
