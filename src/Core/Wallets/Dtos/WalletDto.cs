using System;

namespace Application.Wallets.Dtos
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public string RowVersion { get; set; }
    }
}
