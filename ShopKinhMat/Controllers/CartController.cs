using ShopBanDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDo.Controllers
{
    public class CartController : Controller
    {
        // khởi tạo session:
        private const string SessionCart = "SessionCart";
        // GET: Cart
        private ShopBanDoDbContext db = new ShopBanDoDbContext();
        public ActionResult Index()
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
        public RedirectToRouteResult deleteitem(long productID)
        {
            var cart = Session[SessionCart];
            var list = (List<CartItem>)cart;

            CartItem itemXoa = list.FirstOrDefault(m => m.product.Id == productID);
            if (itemXoa != null)
            {
                list.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }

        public ActionResult cart_header()
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            float priceTotol = 0;
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                foreach (var item1 in list)
                {
                    if (item1.product.Price_sale > 0)
                    {
                        int temp = (((int)item1.product.Price) - ((int)item1.product.Price / 100 * (int)item1.product.Price_sale)) * ((int)item1.quantity);

                        priceTotol += temp;
                    }
                    else
                    {
                        int temp = (int)item1.product.Price * (int)item1.quantity;
                        priceTotol += temp;
                    }
                }
            }
            ViewBag.CartTotal = priceTotol;
            return View("_cartHeader", list);
        }
        public RedirectToRouteResult updateitem(long P_SanPhamID, int P_quantity)
        {
            var cart = Session[SessionCart];
            var list = (List<CartItem>)cart;
            CartItem itemSua = list.FirstOrDefault(m => m.product.Id == P_SanPhamID);
            if (itemSua != null)
            {
                itemSua.quantity = P_quantity;
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public JsonResult Additem(long productID, int quantity)
        {
            var item = new CartItem();
            modelProduct product = db.Products.Find(productID);
            var cart = Session[SessionCart];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(m => m.product.Id == productID))
                {
                    int quantity1 = 0;
                    foreach (var item1 in list)
                    {
                        if (item1.product.Id == productID)
                        {
                            item1.quantity += quantity;
                            quantity1 = item1.quantity;
                        }
                    }
                    var cartCount1 = list.Count();
                    return Json(
                        new
                        {
                            cartCount = cartCount1
                        }
                        , JsonRequestBehavior.AllowGet);

                }
                else
                {
                    item.product = product;
                    item.quantity = quantity;
                    list.Add(item);
                    item.countCart = list.Count();
                    var cartCount1 = list.Count();
                    return Json(
                        new
                        {
                            cartCount = cartCount1
                        }
                        , JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                item.product = product;
                item.quantity = quantity;
                item.countCart = 1;
                item.priceTotal = (int)product.Price;
                var list = new List<CartItem>();
                list.Add(item);
                Session[SessionCart] = list;

            }
            return Json(
                new
                {
                    cartCount = 1
                }
               , JsonRequestBehavior.AllowGet);
        }
    }
}