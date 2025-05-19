using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Application.Wallets.Dtos;
using Domain.Exceptions;

namespace Application.Wallets.Commands
{
    public class CreditWalletCommand : IRequest<WalletDto>
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string TransactionId { get; }
        public byte[] RowVersion { get; }

        public CreditWalletCommand(Guid walletId, CreditWalletRequest request)
        {
            WalletId = walletId;
            Amount = request.Amount;
            RowVersion = Convert.FromBase64String(request.RowVersion);
            TransactionId = string.IsNullOrWhiteSpace(request.TransactionId)
                ? Guid.NewGuid().ToString()
                : request.TransactionId;
        }
    }

    public class CreditWalletCommandHandler : IRequestHandler<CreditWalletCommand, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreditWalletCommandHandler(IWalletRepository walletRepository, IWalletTransactionRepository walletTransactionRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletDto> Handle(CreditWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetById(request.WalletId, cancellationToken);
            if (wallet == null) throw new KeyNotFoundException("Wallet not found.");

            if (await _walletTransactionRepository.ExistsAsync(wallet.Id, request.TransactionId))
            {
                throw new NotFoundWalletException();
            }

            wallet.Credit(request.Amount, request.TransactionId);

            wallet.RowVersion = request.RowVersion;
            _walletRepository.Update(wallet);

            var transaction = wallet.Transactions.Last();
            await _walletTransactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new WalletDto
            {
                Id = wallet.Id,
                Balance = wallet.Balance,
                RowVersion = Convert.ToBase64String(wallet.RowVersion),
            };
        }
    }
}
