using ShopBanDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDo.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.NewOrder = db.Orders.Where(m => m.Status == 2).Count();
            ViewBag.CountAdmin = db.Users.Where(m => m.status == 1 && m.access!=0).Count();
            ViewBag.CountUser = db.Users.Where(m => m.status == 1 && m.access == 0).Count();
            ViewBag.CountContact = db.Contacts.Where(m => m.Status == 1 && m.Flag==0).Count();
            return View();
        }
    }
}