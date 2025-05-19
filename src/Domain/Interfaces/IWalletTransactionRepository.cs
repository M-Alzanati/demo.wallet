using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWalletTransactionRepository
    {
        Task<bool> ExistsAsync(Guid walletId, string transactionId);
        void Add(WalletTransaction transaction);
        void Update(WalletTransaction transaction);
        Task<List<WalletTransaction>> GetAvailableCreditsAsync(Guid walletId);
    }
}
