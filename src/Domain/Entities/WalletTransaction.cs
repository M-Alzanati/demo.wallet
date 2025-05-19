using System;

namespace Domain.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; set; }
        public Guid WalletId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsProcessed { get; set; }

        public virtual Wallet Wallet { get; set; }
    }
}
