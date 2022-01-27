namespace ShopBanDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Flag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Flag");
        }
    }
}
