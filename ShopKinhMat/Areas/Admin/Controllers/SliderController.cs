using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBanDo.Models;

namespace ShopBanDo.Areas.Admin.Controllers
{
    public class SliderController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();

        // GET: Admin/Slider
        public ActionResult Index()
        {
            return View(db.Sliders.Where(m => m.Status != 0).ToList());
        }

        // GET: Admin/Slider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelSlider modelSlider = db.Sliders.Find(id);
            if (modelSlider == null)
            {
                return HttpNotFound();
            }
            return View(modelSlider);
        }

        // GET: Admin/Slider/Create
        public ActionResult Create()
        {
            ViewBag.Orders = new SelectList(db.Sliders.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Slider/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(modelSlider modelSlider)
        {
            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(modelSlider.Name);
                String slug = strSlug;
                modelSlider.Link = slug;
                modelSlider.Position = "slideshow";
                modelSlider.Created_at = DateTime.Now;
                modelSlider.Updated_at = DateTime.Now;
                modelSlider.Created_by = int.Parse(Session["Admin_ID"].ToString());
                modelSlider.Updated_by = int.Parse(Session["Admin_ID"].ToString());

                var f = Request.Files["Img"];

                if (f != null & f.ContentLength > 0)
                {
                    String fileName = strSlug + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    modelSlider.Img = fileName;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/Slider"), fileName);
                    f.SaveAs(Strpath);
                }
                ViewBag.Orders = new SelectList(db.Sliders.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
                db.Sliders.Add(modelSlider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelSlider);
        }

        // GET: Admin/Slider/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.Orders = new SelectList(db.Sliders.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            modelSlider modelSlider = db.Sliders.Find(id);
            if (modelSlider == null)
            {
                return HttpNotFound();
            }
            return View(modelSlider);
        }

        // POST: Admin/Slider/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(modelSlider modelSlider)
        {
            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(modelSlider.Name);
                String slug = strSlug;
                modelSlider.Link = slug;
                modelSlider.Position = "slideshow";
                modelSlider.Updated_at = DateTime.Now;

                modelSlider.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelSlider.Created_by = int.Parse(Session["Admin_ID"].ToString());

                ////Upload file
                var f = Request.Files["Img"];

                if (f != null & f.ContentLength > 0)
                {
                    String fileName = strSlug + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    modelSlider.Img = fileName;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/Slider"), fileName);
                    f.SaveAs(Strpath);
                }
                db.Entry(modelSlider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Orders = new SelectList(db.Sliders.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View(modelSlider);
        }
        public ActionResult Trash()
        {
            var list = db.Sliders.Where(m => m.Status == 0).ToList();

            return View(list);
        }
        public ActionResult delTrash(int? id)
        {
            modelSlider modelSlider = db.Sliders.Find(id);
            if (modelSlider == null)
            {
                return RedirectToAction("Index");
            }
            modelSlider.Status = 0;

            modelSlider.Updated_at = DateTime.Now;

            modelSlider.Updated_at = DateTime.Now;
            modelSlider.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelSlider).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Đẩy vào thùng rác thành công !" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        public ActionResult Undo(int? id)
        {
            modelSlider modelSlider = db.Sliders.Find(id);
            if (modelSlider == null)
            {
                return RedirectToAction("Trash");
            }
            modelSlider.Status = 2;

            modelSlider.Updated_at = DateTime.Now;
            modelSlider.Updated_at = DateTime.Now;
            modelSlider.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelSlider).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công !" + " ID = " + id, "success");
            return RedirectToAction("Trash");

        }

        //doi trang thai
        public ActionResult Status(int? id)
        {
            modelSlider modelSlider = db.Sliders.Find(id);
            if (modelSlider == null)
            {

                return RedirectToAction("Index");
            }
            modelSlider.Status = (modelSlider.Status == 1) ? 2 : 1;

            modelSlider.Updated_at = DateTime.Now;
            modelSlider.Updated_at = DateTime.Now;
            modelSlider.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelSlider).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công !" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        // GET: Admin/Slider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelSlider modelSlider = db.Sliders.Find(id);
            if (modelSlider == null)
            {
                return HttpNotFound();
            }
            return View(modelSlider);
        }

        // POST: Admin/Slider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelSlider modelSlider = db.Sliders.Find(id);
            db.Sliders.Remove(modelSlider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
