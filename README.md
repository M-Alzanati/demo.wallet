# demo.wallet

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
