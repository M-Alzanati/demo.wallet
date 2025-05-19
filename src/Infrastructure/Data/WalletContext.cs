using System.Data.Entity;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class WalletContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        public WalletContext() : base("DefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>()
                .HasKey(w => w.Id)
                .Property(w => w.RowVersion);

            modelBuilder.Entity<WalletTransaction>()
                .HasRequired(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
