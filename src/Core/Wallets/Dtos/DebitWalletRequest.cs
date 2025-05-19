namespace Application.Wallets.Dtos
{
    public class DebitWalletRequest
    {
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public string RowVersion { get; set; }
    }
}
