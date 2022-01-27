namespace ShopBanDo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Slug = c.String(),
                        ParentId = c.Int(),
                        Orders = c.Int(),
                        Metakey = c.String(),
                        Metadesc = c.String(),
                        Created_at = c.DateTime(),
                        Created_by = c.Int(),
                        Updated_at = c.DateTime(),
                        Updated_by = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Detail = c.String(nullable: false),
                        Created_at = c.DateTime(),
                        Updated_at = c.DateTime(),
                        Updated_by = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Slug = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        TableId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Link = c.String(),
                        Type = c.String(),
                        TableId = c.Int(),
                        ParentId = c.Int(),
                        Position = c.String(),
                        Orders = c.Int(nullable: false),
                        Created_at = c.DateTime(),
                        Created_by = c.Int(),
                        Updated_at = c.DateTime(),
                        Updated_by = c.Int(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        CustemerId = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ExportDate = c.DateTime(),
                        DeliveryAddress = c.String(),
                        DeliveryName = c.String(),
                        DeliveryPhone = c.String(),
                        DeliveryEmail = c.String(),
                        Status = c.Int(),
                        DeliveryPaymentMethod = c.String(),
                        StatusPayment = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Topid = c.Int(nullable: false),
                        Title = c.String(),
                        Slug = c.String(),
                        Detail = c.String(),
                        Img = c.String(),
                        Type = c.String(),
                        MetaKey = c.String(),
                        MetaDesc = c.String(),
                        Created_At = c.DateTime(),
                        Created_By = c.Int(),
                        Updated_At = c.DateTime(),
                        Updated_By = c.Int(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CatId = c.Int(nullable: false),
                        Name = c.String(),
                        Slug = c.String(),
                        Img = c.String(),
                        Detail = c.String(),
                        Number = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Price_sale = c.Int(nullable: false),
                        Metakey = c.String(),
                        Metadesc = c.String(),
                        Created_at = c.DateTime(),
                        Created_by = c.Int(),
                        Updated_at = c.DateTime(),
                        Updated_by = c.Int(),
                        Status = c.Int(nullable: false),
                        Discount = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sliders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Link = c.String(),
                        Position = c.String(),
                        Img = c.String(),
                        Orders = c.Int(),
                        Created_at = c.DateTime(),
                        Created_by = c.Int(),
                        Updated_at = c.DateTime(),
                        Updated_by = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Slug = c.String(),
                        ParentId = c.Int(),
                        Orders = c.Int(),
                        Metakey = c.String(),
                        Metadesc = c.String(),
                        Created_at = c.DateTime(),
                        Created_by = c.Int(),
                        Updated_at = c.DateTime(),
                        Updated_by = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        Phone = c.String(nullable: false),
                        Img = c.String(),
                        Access = c.Int(nullable: false),
                        Created_at = c.DateTime(),
                        Created_by = c.Int(),
                        Updated_at = c.DateTime(),
                        Updated_by = c.Int(),
                        Status = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Topics");
            DropTable("dbo.Sliders");
            DropTable("dbo.Products");
            DropTable("dbo.Posts");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Menus");
            DropTable("dbo.Links");
            DropTable("dbo.Contacts");
            DropTable("dbo.Categorys");
        }
    }
}
