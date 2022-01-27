namespace ShopBanDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "updated_at", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "updated_by", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "updated_by");
            DropColumn("dbo.Orders", "updated_at");
        }
    }
}
