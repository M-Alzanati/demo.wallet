using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly WalletContext _context;

        public WalletTransactionRepository(WalletContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Guid walletId, string transactionId)
        {
            return await _context.WalletTransactions
                .AnyAsync(t => t.WalletId == walletId && t.TransactionId == transactionId);
        }

        public async Task AddAsync(WalletTransaction transaction)
        {
            await Task.FromResult(_context.WalletTransactions.Add(transaction));
        }

        public async Task<bool?> IsProcessed(Guid walletId, string transactionId)
        {
            var transaction = await _context.WalletTransactions
                .OrderByDescending(e => e.CreatedAt)
                .FirstOrDefaultAsync(t => t.WalletId == walletId && t.TransactionId == transactionId);
            return transaction?.IsProcessed;
        }
    }
}
