namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Wallets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id, name: "Id");
            
            CreateTable(
                "dbo.WalletTransactions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WalletId = c.Guid(nullable: false),
                        TransactionId = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Timestamp = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        RemainingAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Wallets", t => t.WalletId)
                .Index(t => t.WalletId, name: "WalletId");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WalletTransactions", "WalletId", "dbo.Wallets");
            DropIndex("dbo.WalletTransactions", "WalletId");
            DropIndex("dbo.Wallets", "Id");
            DropTable("dbo.WalletTransactions");
            DropTable("dbo.Wallets");
            DropTable("dbo.Users");
        }
    }
}
