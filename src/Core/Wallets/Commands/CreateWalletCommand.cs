using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Wallets.Commands
{
    public class CreateWalletCommand : IRequest<Guid>
    {
    }

    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Guid>
    {
        private readonly IWalletRepository _walletRepository;

        public CreateWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = Wallet.Create();
            _walletRepository.Add(wallet);
            await _walletRepository.SaveChanges(cancellationToken);
            return wallet.Id;
        }
    }

}
