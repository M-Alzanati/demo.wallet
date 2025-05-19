using MediatR;
using System.Linq;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Interfaces;
using System.Threading;
using Application.DTOs;

namespace Application.Wallets.Queries
{
    public class GetWalletsQuery : IRequest<PagedResult<WalletDto>>
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public decimal? MinBalance { get; }
        public decimal? MaxBalance { get; }

        public GetWalletsQuery(int pageNumber, int pageSize, decimal? minBalance = null, decimal? maxBalance = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            MinBalance = minBalance;
            MaxBalance = maxBalance;
        }
    }

    public class GetWalletsQueryHandler : IRequestHandler<GetWalletsQuery, PagedResult<WalletDto>>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletsQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<PagedResult<WalletDto>> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
        {
            var pagedWallets = await _walletRepository.GetWalletsAsync(
                request.PageNumber,
                request.PageSize,
                request.MinBalance,
                request.MaxBalance);

            var walletDtos = pagedWallets.Items.Select(w => new WalletDto
            {
                Id = w.Id,
                Balance = w.Balance
            }).ToList();

            return new PagedResult<WalletDto>(walletDtos, pagedWallets.TotalCount);
        }
    }
}
