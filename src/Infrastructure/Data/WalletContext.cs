using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class WalletContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        public DbSet<User> Users { get; set; }

        public WalletContext() : base("DefaultConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>()
                .Property(t => t.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Transactions)
                .WithRequired(t => t.Wallet)
                .HasForeignKey(t => t.WalletId);

            modelBuilder.Entity<WalletTransaction>()
                .HasRequired(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WalletTransaction>()
                .Property(t => t.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id)
                .Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }
    }
}
