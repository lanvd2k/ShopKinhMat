using ShopBanDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDo.Controllers
{
    public class ModuleController : Controller
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();

        private const string SessionCart = "SessionCart";
        // GET: Module
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Banner()
        {
            return View();
        }
        public ActionResult Cart()
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            else if (cart == null)
            {
                ViewBag.statusCart = "Giỏ hàng trống";

            }
            return View(list);
        }
        public ActionResult Footer()
        {
            return View();
        }
        public ActionResult Header()
        {
            var list = db.Menus.Where(m => m.Status != 0).ToList();
            return View("Header", list);
        }
        public ActionResult Slider()
        {
            var list = db.Sliders.Where(m => m.Status == 1).ToList();
            return View("Slider", list);
        }
        public ActionResult ListCategory()
        {

            return View("ListCategory", db.Categorys.ToList());
        }
        public ActionResult Quantity()
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            float priceTotol = 0;
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                foreach (var item1 in list)
                { 
                        int temp = item1.quantity;
                        priceTotol += temp;
                }
            }
            ViewBag.CartTotal = priceTotol;
            return View("Quantity", list);
        }
        public ActionResult Post()
        {
            var list = db.Posts.Where(m => m.Status != 0 && m.Type=="post")
                .OrderByDescending(m=>m.Updated_At)
                .Take(3)
                .ToList();
            return View("Post", list);
        }
        public ActionResult CategoryList()
        {
            var list = db.Categorys.Where(m => m.Status != 0 && m.ParentId == 0)
                .ToList();
            return View("CategoryList", list);
        }
        public ActionResult PageList()
        {
            return View("PageList", db.Posts.Where(m=>m.Status!=0 && m.Type=="page").ToList());
        }
        public ActionResult ListCategoryFooter()
        {
            return View("ListCategoryFooter", db.Categorys
                .Where(m => m.Status != 0 && m.ParentId == 0)
                .ToList());
        }
        public ActionResult ListPageFooter()
        {
            return View("ListPageFooter", db.Posts
                .Where(m => m.Status != 0 && m.Type == "page")
                .ToList());
        }
    }
}