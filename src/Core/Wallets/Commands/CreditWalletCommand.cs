using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Application.DTOs;

namespace Application.Wallets.Commands
{
    public class CreditWalletCommand : IRequest
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string TransactionId { get; }

        public CreditWalletCommand(Guid walletId, CreditWalletRequest request)
        {
            WalletId = walletId;
            Amount = request.Amount;
            TransactionId = string.IsNullOrWhiteSpace(request.TransactionId)
                ? Guid.NewGuid().ToString()
                : request.TransactionId;
        }
    }

    public class CreditWalletCommandHandler : IRequestHandler<CreditWalletCommand>
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

        public async Task Handle(CreditWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetById(request.WalletId, cancellationToken);
            if (wallet == null) throw new KeyNotFoundException("Wallet not found.");

            if (await _walletTransactionRepository.ExistsAsync(wallet.Id, request.TransactionId))
            {
                return;
            }

            wallet.Credit(request.Amount, request.TransactionId);

            _walletRepository.Update(wallet);

            // Save transaction record explicitly via repository or rely on EF cascade save
            var transaction = wallet.Transactions.Last();
            await _walletTransactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
