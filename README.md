# demo.wallet

* What things would you add/change to make this production ready? 

* Domain-Driven Design we should introduce aggregates & domain events
- Use value objects for Amount, TransactionId, etc.
- CQRS/Event Sourcing	we should separate read/write models
- Reintroduce event sourcing with snapshotting support if needed
- Unit of Workensure a robust implementation to manage transactions explicitly
- Optimistic Concurrency already used with RowVersion — keep it and add retry policy.
- Auditing with add a transaction history table (if not event sourcing), store metadata like timestamps, IPs, user agents.
- Authentication & Authorization with integrate OAuth2/JWT and authorize wallets per-user.
- Input Validation use FluentValidation or DataAnnotations. Prevent invalid requests.
- Rate Limiting	prevent abuse via a middleware
- Use SQL indexing, partitioning, and possibly redis cache for hot wallets.
- Background Processing	Offload slow operations using Hangfire, Quartz.
- Logging	Serilog/NLog + structured logs. Include TransactionId, WalletId, etc.
- Health Checks	Expose /health endpoint for readiness/liveness probes.
