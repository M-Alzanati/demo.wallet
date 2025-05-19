using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Entities
{
    public class Wallet
    {
        public static Wallet Create()
        {
            return new Wallet
            {
                Id = Guid.NewGuid(),
                Balance = 0
            };
        }

        [Index(nameof(Id))]
        public Guid Id { get; set; }
        public decimal Balance { get; private set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();

        public void Credit(decimal amount, string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId)) throw new ArgumentException("TransactionId is required.");
            if (HasTransaction(transactionId)) return;

            if (amount <= 0) throw new ArgumentException("Amount must be positive.");

            Balance += amount;

            Transactions.Add(new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = this.Id,
                TransactionId = transactionId,
                Amount = amount,
                CreatedAt = DateTime.UtcNow
            });
        }

        public void Debit(decimal amount, string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId)) throw new ArgumentException("TransactionId is required.");
            if (HasTransaction(transactionId)) return;

            if (amount <= 0) throw new ArgumentException("Amount must be positive.");
            if (amount > Balance) throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;

            Transactions.Add(new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = this.Id,
                TransactionId = transactionId,
                Amount = -amount,
                CreatedAt = DateTime.UtcNow
            });
        }

        private bool HasTransaction(string transactionId)
        {
            return Transactions.Any(t => t.TransactionId == transactionId);
        }
    }
}
