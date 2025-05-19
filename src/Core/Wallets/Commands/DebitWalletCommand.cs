using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Wallets.Dtos;
using Domain.Enums;
using System.Linq;

namespace Application.Wallets.Commands
{
    public class DebitWalletCommand : ConcurrencyCommand, IRequest<WalletDto>
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }

        public DebitWalletCommand(Guid walletId, DebitWalletRequest request)
        {
            WalletId = walletId;
            Amount = request.Amount;
            RowVersion = request.RowVersion ?? throw new ArgumentNullException(nameof(request.RowVersion));
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
            var wallet = await _walletRepository.GetById(request.WalletId, cancellationToken);
            if (wallet == null)
                throw new KeyNotFoundException("Wallet not found.");

            var credits = await _transactionRepository
                .GetAvailableCreditsAsync(request.WalletId);

            decimal totalAvailable = credits.Sum(c => c.RemainingAmount);

            if (totalAvailable < request.Amount)
                throw new InvalidOperationException("Insufficient available funds.");

            _walletRepository.AttachRowVersion(wallet, request.GetRowVersionBytes());

            decimal remainingToDebit = request.Amount;
            var now = DateTime.UtcNow;

            foreach (var credit in credits.OrderBy(c => c.Timestamp))
            {
                if (remainingToDebit == 0)
                    break;

                var consume = Math.Min(credit.RemainingAmount, remainingToDebit);
                credit.RemainingAmount -= consume;
                remainingToDebit -= consume;

                credit.Status = credit.RemainingAmount == 0
                    ? TransactionStatus.Used
                    : TransactionStatus.PartiallyUsed;

                _transactionRepository.Update(credit);
            }

            var debitTx = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = request.WalletId,
                Amount = -request.Amount,
                Type = TransactionType.Debit,
                Status = TransactionStatus.Used,
                RemainingAmount = 0,
                Timestamp = now
            };

            _transactionRepository.Add(debitTx);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new WalletDto
            {
                Id = wallet.Id,
                AvailableBalance = wallet.GetAvailableBalance(),
                RowVersion = Convert.ToBase64String(wallet.RowVersion),
                Transactions = wallet.Transactions
                    .Select(t => new WalletTransactionDto
                    {
                        Id = t.Id,
                        Amount = t.Amount,
                        RowVersion = Convert.ToBase64String(t.RowVersion),
                        Type = t.Type,
                        CreatedAt = t.Timestamp
                    }).ToList()
            };
        }
    }
}
