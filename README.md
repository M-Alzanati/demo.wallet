# demo.wallet

## project structure
WalletService.sln
│
├── 📁 WalletService.Core              → Domain entities, interfaces, enums
│   ├── Entities
│   │   ├── Wallet.cs
│   │   ├── WalletTransaction.cs
|   |   ├── User.cs
│   ├── Interfaces
│   │   ├── IWalletRepository.cs
│   │   ├── IWalletTransactionRepository.cs
│   │   ├── IUserRepository.cs
│   ├── Enums
│   │   ├── TransactionType.cs
│   │   ├── TransactionStatus.cs

├── 📁 WalletService.Application       → CQRS (commands, queries), DTOs, behavior
│   ├── Wallets
│   │   ├── Commands
│   │   │   ├── CreditWalletCommand.cs
│   │   │   ├── DebitWalletCommand.cs
│   │   │   ├── CreateWalletCommand.cs
│   │   ├── Queries
│   │   │   ├── GetWalletByIdQuery.cs
│   ├── Users
│   │   ├── Queries
│   │   │   ├── ValidateUserQuery.cs
│   ├── DTOs
│   │   ├── WalletDto.cs
│   │   ├── WalletTransactionDto.cs
│   │   ├── UserDto.cs
│   ├── Behaviors
│   │   ├── ConcurrencyRetryBehavior.cs
│   ├── DependencyInjection
│       ├── ApplicationModule.cs

├── 📁 WalletService.Infrastructure    → EF, Repositories, Migrations, Seed
│   ├── Data
│   │   ├── AppDbContext.cs
│   ├── Repositories
│   │   ├── WalletRepository.cs
│   │   ├── WalletTransactionRepository.cs
│   │   ├── UserRepository.cs
|   |   ├── Migrations/
│   ├── DependencyInjection
│       ├── InfrastructureModule.cs

├── 📁 WalletService.Web               → Web API project (.NET 4.8)
│   ├── Controllers
│   │   ├── WalletController.cs
│   │   ├── AuthController.cs
│   ├── Authentication
│   │   ├── BasicAuthFilter.cs
│   ├── DependencyInjection
│   │   ├── AutofacConfig.cs
│   ├── Filters
│   │   ├── GlobalExceptionFilter.cs
│   ├── App_Start
│   │   ├── WebApiConfig.cs
│   │   ├── FilterConfig.cs
│   ├── Global.asax
│   ├── Web.config


## Key Features Included

* Core           ├──	Entities + Interfaces (pure domain logic)
* Application    ├──	CQRS handlers, DTOs, validation, retry logic
* Infrastructure ├──	EF DbContext, Repos, Migrations, DI
* Web	           ├──  Web API, Auth filter, Controllers, DI, Config

## Benefits of This Structure
* Clear separation of concerns
* Aligned with Clean Architecture
* Easily extendable for: Logging, Caching, Messaging, Token-based Auth

## What things would you add/change to make this production ready? 

- Relate debit transactions to the credit transactions they spend.
- Prevent duplicate processing of requests, especially on retries by using some sort of tokens.
- In unit of work enforce consistent transaction boundaries.
- Ensure atomicity between changes to Wallet and WalletTransaction.
- Integrating with messaging systems like RabbitMq or Kafka or any cloud service.
- Audit logging.
- Deploy behind a load balancer.
- Adding Application Gateway.
- Add retries for failed commands, particularly around concurrenc.
- Replace Basic Auth with OAuth2/JWT.
- Rate limiting and anti-fraud logic.
- Setup failure or performance thresholds.
- Validation by using FluentValidation for DTOs.
- API versioning.
- Unit tests for all commands, queries, domain models.
- Separate query model optimized for reads.
