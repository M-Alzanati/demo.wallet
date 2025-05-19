using System;
using MediatR;
using System.Threading.Tasks;
using Domain.Interfaces;
using System.Threading;
using Application.Wallets.Dtos;

namespace Application.Wallets.Queries
{
    public class GetWalletsRowVersionQuery : IRequest<WalletDto>
    {
        public Guid Id { get; }

        public GetWalletsRowVersionQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetWalletsRowVersionQueryHandler : IRequestHandler<GetWalletsRowVersionQuery, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletsRowVersionQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletDto> Handle(GetWalletsRowVersionQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetById(request.Id, cancellationToken);

            return new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                RowVersion = Convert.ToBase64String(wallet.RowVersion),
            };
        }
    }
}
