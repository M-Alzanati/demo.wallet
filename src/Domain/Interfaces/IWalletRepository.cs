using Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace Domain.Interfaces
{
    public interface IWalletRepository : IRepository
    {
        Task<Wallet> GetById(Guid id, CancellationToken cancellationToken);
        Guid Add(Wallet wallet);
        void Update(Wallet wallet);
        bool Exists(Guid id);
        Task<PagedResult<Wallet>> GetWalletsAsync(int pageNumber, int pageSize, decimal? minBalance = null, decimal? maxBalance = null);
        void AttachRowVersion(Wallet wallet, byte[] rowVersion);
    }
}
