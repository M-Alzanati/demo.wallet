# demo.wallet

* What things would you add/change to make this production ready? 

Domain-Driven Design	- Introduce aggregates & domain events
- Use value objects for Amount, TransactionId, etc.
- CQRS/Event Sourcing	- Separate read/write models
- Reintroduce event sourcing with snapshotting support if needed
- Unit of Work	- Ensure a robust implementation to manage transactions explicitly
- Optimistic Concurrency	Already used with RowVersion — keep it and add retry policy.
- Auditing	Add a transaction history table (if not event sourcing), store metadata like timestamps, IPs, user agents.
- Authentication & Authorization	Integrate OAuth2/JWT and authorize wallets per-user.
- Input Validation	Use FluentValidation or DataAnnotations. Prevent invalid requests (e.g. zero/negative funds).
- Rate Limiting	Prevent abuse via a middleware
- Use SQL indexing, partitioning, and possibly Redis cache for hot wallets.
- Background Processing	Offload slow operations using Hangfire, Quartz, or Azure Functions.
- Logging	Serilog/NLog + structured logs. Include TransactionId, WalletId, etc.
- Health Checks	Expose /health endpoint for readiness/liveness probes.
