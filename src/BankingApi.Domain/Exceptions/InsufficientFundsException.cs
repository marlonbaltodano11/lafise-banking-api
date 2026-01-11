using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApi.Domain.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(decimal attemptedAmount, decimal balance) 
            : base($"Cannot withdraw {attemptedAmount:C}. Available balance is {balance:C}.")
        { 
        }
    }
}
