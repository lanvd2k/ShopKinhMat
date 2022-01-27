namespace ShopBanDo.Models
{
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
    public class ShopBanDoDbContext : DbContext
    {
        public ShopBanDoDbContext() : base("name=StrC")
        { }
        public virtual DbSet<modelProduct> Products { get; set; }
        public virtual DbSet<modelCategory> Categorys { get; set; }
        public virtual DbSet<modelContact> Contacts { get; set; }
        public virtual DbSet<modelMenu> Menus { get; set; }
        public virtual DbSet<modelOrder> Orders { get; set; }
        public virtual DbSet<modelOrderdetail> Orderdetails { get; set; }
        public virtual DbSet<modelPost> Posts { get; set; }
        public virtual DbSet<modelSlider> Sliders { get; set; }
        public virtual DbSet<modelTopic> Topics { get; set; }
        public virtual DbSet<modelUser> Users { get; set; }
        public virtual DbSet<modelLink> Links { get; set; }
    }
}