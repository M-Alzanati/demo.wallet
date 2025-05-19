using Domain.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Application.Wallets.Dtos;

namespace Application.Wallets.Queries
{
    public class GetWalletByIdQuery : IRequest<WalletDto>
    {
        public Guid Id { get; }
        public GetWalletByIdQuery(Guid id) => Id = id;
    }

    public class GetWalletByIdQueryHandler : IRequestHandler<GetWalletByIdQuery, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletByIdQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetById(request.Id, cancellationToken);
            if (wallet == null) return await Task.FromResult<WalletDto>(null);

            return await Task.FromResult(new WalletDto
            {
                Id = wallet.Id,
                AvailableBalance = wallet.GetAvailableBalance(),
                RowVersion = Convert.ToBase64String(wallet.RowVersion),
                Transactions = wallet.Transactions.Select(t => new WalletTransactionDto
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Type = t.Type,
                    CreatedAt = t.Timestamp,
                    RowVersion = Convert.ToBase64String(t.RowVersion)
                }).ToList()
            });
        }
    }
}
