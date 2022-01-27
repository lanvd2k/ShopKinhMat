namespace ShopBanDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _99 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "address", c => c.String());
            AlterColumn("dbo.Users", "fullname", c => c.String());
            AlterColumn("dbo.Users", "username", c => c.String());
            AlterColumn("dbo.Users", "password", c => c.String());
            AlterColumn("dbo.Users", "email", c => c.String());
            AlterColumn("dbo.Users", "gender", c => c.String());
            AlterColumn("dbo.Users", "phone", c => c.String());
            AlterColumn("dbo.Users", "created_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "created_by", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "updated_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "updated_by", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "status", c => c.Int());
            AlterColumn("dbo.Users", "updated_by", c => c.Int());
            AlterColumn("dbo.Users", "updated_at", c => c.DateTime());
            AlterColumn("dbo.Users", "created_by", c => c.Int());
            AlterColumn("dbo.Users", "created_at", c => c.DateTime());
            AlterColumn("dbo.Users", "phone", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "gender", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "password", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "username", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "fullname", c => c.String(nullable: false));
            DropColumn("dbo.Users", "address");
        }
    }
}
