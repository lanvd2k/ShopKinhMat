using ShopBanDo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDo.Areas.Admin.Controllers
{
    
    public class OrderController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var list = db.Orders.Where(m => m.Status != 0).OrderByDescending(m => m.Id).ToList();
            return View(list);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.customer = db.Orders.Where(m => m.Id == id).First();
            var lisst = db.Orderdetails.Where(m => m.OrderId == id).ToList();
            return View("Orderdetail", lisst);
        }
        //status
        public ActionResult Status(int id)
        {
            modelOrder morder = db.Orders.Find(id);
            morder.Status = (morder.Status == 1) ? 2 : 1;
            morder.updated_at = DateTime.Now;
            morder.updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Thay đổi trang thái thành công", "success");
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult Trash()
        {
            var list = db.Orders.Where(m => m.Status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            modelOrder morder = db.Orders.Find(id);
            morder.Status = 0;
            morder.updated_at = DateTime.Now;
            morder.updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
        public ActionResult Retrash(int id)
        {
            modelOrder morder = db.Orders.Find(id);
            morder.Status = 2;
            morder.updated_at = DateTime.Now;
            morder.updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(morder).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Khôi phục thành công", "success");
            return RedirectToAction("trash");
        }
        public ActionResult deleteTrash(int id)
        {
            modelOrder morder = db.Orders.Find(id);
            db.Orders.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Đã xóa vĩnh viễn 1 Đơn hàng", "success");
            return RedirectToAction("trash");
        }
    }
}