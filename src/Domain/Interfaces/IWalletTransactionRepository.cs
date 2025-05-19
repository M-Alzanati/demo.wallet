using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWalletTransactionRepository
    {
        Task<bool> ExistsAsync(Guid walletId, string transactionId);
        Task AddAsync(WalletTransaction transaction);
        Task<bool?> IsProcessed(Guid walletId, string transactionId);
    }
}
