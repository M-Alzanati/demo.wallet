using System;
using Domain.Enums;

namespace Application.Wallets.Dtos
{
    public class WalletTransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public TransactionType Type { get; set; }
        public string RowVersion { get; set; }
    }
}
