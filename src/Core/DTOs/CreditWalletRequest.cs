using System;

namespace Application.DTOs
{
    public class CreditWalletRequest
    {
        public decimal Amount { get; set; }

        public string TransactionId;
    }
}
