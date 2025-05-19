using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Application.Wallets.Dtos;
using Domain.Exceptions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Wallets.Commands
{
    public class CreditWalletCommand : ConcurrencyCommand, IRequest<WalletDto>
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string TransactionId { get; }

        public CreditWalletCommand(Guid walletId, CreditWalletRequest request)
        {
            WalletId = walletId;
            Amount = request.Amount;
            RowVersion = request.RowVersion ?? throw new ArgumentNullException(nameof(request.RowVersion));
            TransactionId = string.IsNullOrWhiteSpace(request.TransactionId)
                ? Guid.NewGuid().ToString()
                : request.TransactionId;
        }
    }

    public class CreditWalletCommandHandler : IRequestHandler<CreditWalletCommand, WalletDto>
    {
        private readonly IWalletTransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreditWalletCommandHandler(
            IWalletTransactionRepository transactionRepository,
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WalletDto> Handle(CreditWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetById(request.WalletId, cancellationToken);
            if (wallet == null)
                throw new KeyNotFoundException("Wallet not found.");

            _walletRepository.AttachRowVersion(wallet, request.GetRowVersionBytes());

            var creditTransaction = new WalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                Amount = request.Amount,
                RemainingAmount = request.Amount,
                Type = TransactionType.Credit,
                Status = TransactionStatus.Available,
                Timestamp = DateTime.UtcNow
            };

            _transactionRepository.Add(creditTransaction);
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
