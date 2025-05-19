using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Wallets.Queries
{
    public class GetWalletByIdQuery : IRequest<WalletDto>
    {
        public Guid Id { get; }
        public GetWalletByIdQuery(Guid id) => Id = id;
    }

    // Application/Wallets/Queries/GetWalletByIdQueryHandler.cs
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
                Balance = wallet.Balance
            });
        }
    }
}
