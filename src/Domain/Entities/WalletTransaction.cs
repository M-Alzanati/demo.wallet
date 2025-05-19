using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; set; }

        [Index(nameof(WalletId))]
        public Guid WalletId { get; set; }

        public string TransactionId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }

        public TransactionType Type { get; set; }

        public TransactionStatus Status { get; set; }

        public decimal RemainingAmount { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Wallet Wallet { get; set; }
    }
}
