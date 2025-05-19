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
                .HasKey(w => w.Id)
                .Property(w => w.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<WalletTransaction>()
                .HasRequired(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id)
                .Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }
    }
}
