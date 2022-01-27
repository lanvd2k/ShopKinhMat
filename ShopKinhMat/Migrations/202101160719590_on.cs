namespace ShopBanDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class on : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Reply", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Reply");
        }
    }
}
