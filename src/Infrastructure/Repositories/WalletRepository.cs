using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using Domain.Base;
using System.Data.Entity;

namespace Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _context;

        public WalletRepository(WalletContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetById(Guid id, CancellationToken cancellationToken) => await _context.Wallets.FindAsync(cancellationToken, id);

        public Guid Add(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            return wallet.Id;
        }

        public void Update(Wallet wallet)
        {
            _context.Entry(wallet).OriginalValues["RowVersion"] = wallet.RowVersion;
            _context.Wallets.Attach(wallet);
            _context.Entry(wallet).State = EntityState.Modified;
        }

        public bool Exists(Guid id) => _context.Wallets.Any(w => w.Id == id);

        public async Task<PagedResult<Wallet>> GetWalletsAsync(int pageNumber, int pageSize, decimal? minBalance = null, decimal? maxBalance = null)
        {
            var query = _context.Wallets.AsQueryable();

            if (minBalance.HasValue)
                query = query.Where(w => w.Balance >= minBalance.Value);

            if (maxBalance.HasValue)
                query = query.Where(w => w.Balance <= maxBalance.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(w => w.Id) // or any other sorting logic
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Wallet>(items, totalCount);
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
