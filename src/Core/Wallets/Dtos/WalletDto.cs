using System;
using System.Collections.Generic;

namespace Application.Wallets.Dtos
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public decimal AvailableBalance { get; set; }
        public List<WalletTransactionDto> Transactions { get; set; }
        public string RowVersion { get; set; }
    }
}
