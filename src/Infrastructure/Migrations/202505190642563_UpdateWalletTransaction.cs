namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWalletTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WalletTransactions", "IsProcessed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WalletTransactions", "IsProcessed");
        }
    }
}
