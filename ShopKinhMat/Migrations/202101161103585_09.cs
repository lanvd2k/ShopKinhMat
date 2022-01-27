namespace ShopBanDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _09 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "priceSale", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "priceSale");
        }
    }
}
