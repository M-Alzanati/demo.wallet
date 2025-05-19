namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wallets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WalletTransactions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WalletId = c.Guid(nullable: false),
                        TransactionId = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Wallets", t => t.WalletId)
                .Index(t => t.WalletId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WalletTransactions", "WalletId", "dbo.Wallets");
            DropIndex("dbo.WalletTransactions", new[] { "WalletId" });
            DropTable("dbo.WalletTransactions");
            DropTable("dbo.Wallets");
        }
    }
}
