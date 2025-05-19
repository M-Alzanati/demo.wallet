namespace Application.Wallets.Dtos
{
    public class CreditWalletRequest
    {
        public decimal Amount { get; set; }

        public string TransactionId;

        public string RowVersion { get; set; }
    }
}
