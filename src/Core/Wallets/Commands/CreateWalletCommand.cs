using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Application.Wallets.Dtos;

namespace Application.Wallets.Commands
{
    public class CreateWalletCommand : IRequest<WalletDto>
    {
    }

    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWalletCommandHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletDto> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = Wallet.Create();
            _walletRepository.Add(wallet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new WalletDto()
            {
                Id = wallet.Id,
                AvailableBalance = wallet.GetAvailableBalance(),
                RowVersion = Convert.ToBase64String(wallet.RowVersion),
            };
        }
    }
}
