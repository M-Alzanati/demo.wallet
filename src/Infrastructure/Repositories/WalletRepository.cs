using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using System.Data.Entity;
using Common;

namespace Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly WalletContext _context;

        public WalletRepository(WalletContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetById(Guid id, CancellationToken cancellationToken) => 
            await _context.Wallets.Include(e => e.Transactions).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

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
                query = query.Where(w => w.GetAvailableBalance() >= minBalance.Value);

            if (maxBalance.HasValue)
                query = query.Where(w => w.GetAvailableBalance() <= maxBalance.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(w => w.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Transactions)
                .ToListAsync();

            return new PagedResult<Wallet>(items, totalCount);
        }

        public void AttachRowVersion(Wallet wallet, byte[] rowVersion)
        {
            _context.Entry(wallet).OriginalValues["RowVersion"] = rowVersion;
        }
    }
}
