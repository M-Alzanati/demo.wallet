using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Domain.Enums;

namespace Domain.Entities
{
    public class Wallet
    {
        [Index(nameof(Id))]
        public Guid Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();

        public static Wallet Create()
        {
            return new Wallet
            {
                Id = Guid.NewGuid(),
            };
        }

        public decimal GetAvailableBalance()
        {
            return Transactions
                       .Where(t => t.Type == TransactionType.Credit)
                       .Sum(t => t.Amount)
                   +
                   Transactions
                       .Where(t => t.Type == TransactionType.Debit)
                       .Sum(t => t.Amount);
        }
    }
}
