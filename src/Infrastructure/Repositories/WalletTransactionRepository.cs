using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
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

        public void Add(WalletTransaction transaction)
        {
            _context.WalletTransactions.Add(transaction);
        }

        public void Update(WalletTransaction transaction)
        {
            _context.Entry(transaction).OriginalValues["RowVersion"] = transaction.RowVersion;
            _context.WalletTransactions.Attach(transaction);
            _context.Entry(transaction).State = EntityState.Modified;
        }

        public async Task<List<WalletTransaction>> GetAvailableCreditsAsync(Guid walletId)
        {
            return await _context.WalletTransactions
                .Where(tx => tx.WalletId == walletId
                             && tx.Type == TransactionType.Credit
                             && tx.Status != TransactionStatus.Used)
                .ToListAsync();
        }
    }
}
