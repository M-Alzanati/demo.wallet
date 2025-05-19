using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Wallets.Dtos;

namespace Application.Wallets.Commands
{
    public class DebitWalletCommand : IRequest<WalletDto>
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string TransactionId { get; }
        public byte[] RowVersion { get; }

        public DebitWalletCommand(Guid walletId, DebitWalletRequest request)
        {
            WalletId = walletId;
            Amount = request.Amount;
            RowVersion = Convert.FromBase64String(request.RowVersion);
            TransactionId = request.TransactionId ?? throw new ArgumentNullException(nameof(request.TransactionId));
        }
    }

    public class DebitWalletCommandHandler : IRequestHandler<DebitWalletCommand, WalletDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DebitWalletCommandHandler(
            IWalletRepository walletRepository,
            IWalletTransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletDto> Handle(DebitWalletCommand request, CancellationToken cancellationToken)
        {
            if (!await _transactionRepository.ExistsAsync(request.WalletId, request.TransactionId))
            {
                throw new NotFoundTransactionException("Transaction for wallet not found");
            }

            if (await _transactionRepository.IsProcessed(request.WalletId, request.TransactionId) == true)
            {
                throw new InsufficientFundsException();
            }

            var wallet = await _walletRepository.GetById(request.WalletId, cancellationToken);
            if (wallet == null)
                throw new KeyNotFoundException("Wallet not found.");

            wallet.Debit(request.Amount, request.TransactionId);

            var transaction = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = request.WalletId,
                TransactionId = request.TransactionId,
                CreatedAt = DateTime.UtcNow,
                IsProcessed = true
            };

            _walletRepository.Update(wallet);
            await _transactionRepository.AddAsync(transaction);

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
