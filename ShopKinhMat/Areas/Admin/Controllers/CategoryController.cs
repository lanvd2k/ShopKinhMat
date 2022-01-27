using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopBanDo.Models;

namespace ShopBanDo.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();

        // GET: Admin/Category
        public ActionResult Index()
        {
            var list = db.Categorys.Where(m => m.Status != 0).ToList();

            ViewBag.demrac = db.Categorys.Where(m => m.Status == 0).Count();

            foreach (var row in list)
            {
                var temp_link = db.Links
                    .Where(m => m.Type == "category" && m.TableId == row.Id);
                if (temp_link.Count() > 0)
                {
                    var row_link = temp_link.First();
                    row_link.Name = row.Name;
                    row_link.Slug = row.Slug;
                    db.Entry(row_link).State = EntityState.Modified;
                }
                else
                {
                    var row_link = new modelLink();
                    row_link.Name = row.Name;
                    row_link.Slug = row.Slug;
                    row_link.Type = "category";
                    row_link.TableId = row.Id;
                    db.Links.Add(row_link);
                }
            }
            db.SaveChanges();
            return View(list);
            //return View(db.Categorys.ToList());
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelCategory modelCategory = db.Categorys.Find(id);
            if (modelCategory == null)
            {
                return HttpNotFound();
            }
            return View(modelCategory);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.List = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(modelCategory modelCategory)
        {
            if (ModelState.IsValid)
            {
                if (modelCategory.ParentId == null)
                {
                    modelCategory.ParentId = 0;
                }

                String slug = XString.ToAscii(modelCategory.Name);
                modelCategory.Slug = slug;
                modelCategory.Created_at = DateTime.Now;
                modelCategory.Updated_at = DateTime.Now;
                modelCategory.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelCategory.Created_by = int.Parse(Session["Admin_ID"].ToString());
                db.Categorys.Add(modelCategory);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.List = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);

            return View(modelCategory);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.List = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelCategory modelCategory = db.Categorys.Find(id);
            if (modelCategory == null)
            {
                return HttpNotFound();
            }
            return View(modelCategory);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(modelCategory modelCategory)
        {
            if (ModelState.IsValid)
            {
                if (modelCategory.ParentId == null)
                {
                    modelCategory.ParentId = 0;
                }
                String slug = XString.ToAscii(modelCategory.Name);
                modelCategory.Slug = slug;

                modelCategory.Updated_at = DateTime.Now;
                modelCategory.Updated_by = int.Parse(Session["Admin_ID"].ToString()); 
                modelCategory.Created_by = int.Parse(Session["Admin_ID"].ToString()); 
                db.Categorys.Add(modelCategory);
                // db.SaveChanges();

                db.Entry(modelCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.List = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Categorys.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View(modelCategory);
        }
        public ActionResult delTrash(int? id)
        {
            modelCategory modelCategory = db.Categorys.Find(id);
            if (modelCategory == null)
            {
                return RedirectToAction("Index");
            }

            modelCategory.Status = 0;

            modelCategory.Updated_at = DateTime.Now;
            modelCategory.Created_by = int.Parse(Session["Admin_ID"].ToString()); 
            modelCategory.Updated_at = DateTime.Now;
            modelCategory.Updated_by = int.Parse(Session["Admin_ID"].ToString()); 
            modelCategory.Updated_by = int.Parse(Session["Admin_ID"].ToString()); 

            db.Entry(modelCategory).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Ném vào thùng rác thành công !" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        public ActionResult Undo(int? id)
        {
            modelCategory modelCategory = db.Categorys.Find(id);
            if (modelCategory == null)
            {

                return RedirectToAction("Trash");
            }
            modelCategory.Status = 2;

            modelCategory.Updated_at = DateTime.Now;
            modelCategory.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelCategory.Updated_at = DateTime.Now;
            modelCategory.Updated_by = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelCategory).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công !" + " ID = " + id, "success");
            return RedirectToAction("Trash");

        }
        //trash
        public ActionResult Trash()
        {
            var list = db.Categorys.Where(m => m.Status == 0).ToList();

            return View(list);
        }
        //doi trang thai
        public ActionResult Status(int? id)
        {
            modelCategory modelCategory = db.Categorys.Find(id);
            if (modelCategory == null)
            {

                return RedirectToAction("Index");
            }
            modelCategory.Status = (modelCategory.Status == 1) ? 2 : 1;

            modelCategory.Updated_at = DateTime.Now;
            modelCategory.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelCategory.Updated_at = DateTime.Now;
            modelCategory.Updated_by = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelCategory).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công!" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelCategory modelCategory = db.Categorys.Find(id);
            if (modelCategory == null)
            {
                return HttpNotFound();
            }
            return View(modelCategory);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelCategory modelCategory = db.Categorys.Find(id);
            db.Categorys.Remove(modelCategory);
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
