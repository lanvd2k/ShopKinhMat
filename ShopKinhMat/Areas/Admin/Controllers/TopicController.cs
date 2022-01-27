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
    public class TopicController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();

        // GET: Admin/Topic
        public ActionResult Index()
        {
            ViewBag.demrac = db.Topics.Where(m => m.Status == 0).Count();
            var list = db.Topics.Where(m => m.Status != 0).ToList();
            foreach (var row in list)
            {
                var temp_link = db.Links
                    .Where(m => m.Type == "topic" && m.TableId == row.Id);
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
                    row_link.Type = "topic";
                    row_link.TableId = row.Id;
                    db.Links.Add(row_link);
                }
            }
            db.SaveChanges();
            return View(list);
        }

        // GET: Admin/Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelTopic modelTopic = db.Topics.Find(id);
            if (modelTopic == null)
            {
                return HttpNotFound();
            }
            return View(modelTopic);
        }

        // GET: Admin/Topic/Create
        public ActionResult Create()
        {
            ViewBag.List = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View();
        }

        // POST: Admin/Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(modelTopic modelTopic)
        {

            if (ModelState.IsValid)
            {
                if (modelTopic.ParentId == null)
                {
                    modelTopic.ParentId = 0;
                }

                String slug = XString.ToAscii(modelTopic.Name);
                modelTopic.Slug = slug;
                modelTopic.Created_at = DateTime.Now;
                modelTopic.Updated_at = DateTime.Now;
                modelTopic.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelTopic.Created_by = int.Parse(Session["Admin_ID"].ToString());
                db.Topics.Add(modelTopic);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.List = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View(modelTopic);
        }

        // GET: Admin/Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.List = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelTopic modelTopic = db.Topics.Find(id);
            if (modelTopic == null)
            {
                return HttpNotFound();
            }
            return View(modelTopic);
        }

        // POST: Admin/Topic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(modelTopic modelTopic)
        {
            if (ModelState.IsValid)
            {
                if (modelTopic.ParentId == null)
                {
                    modelTopic.ParentId = 0;
                }
                String slug = XString.ToAscii(modelTopic.Name);
                modelTopic.Slug = slug;
                modelTopic.Updated_at = DateTime.Now;

                modelTopic.Updated_by = int.Parse(Session["Admin_ID"].ToString());
                modelTopic.Created_by = int.Parse(Session["Admin_ID"].ToString());
                db.Topics.Add(modelTopic);
                // db.SaveChanges();

                db.Entry(modelTopic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.List = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            ViewBag.Orders = new SelectList(db.Topics.Where(m => m.Status != 0).ToList(), "Id", "Name", 0);
            return View(modelTopic);
        }
        //trash
        public ActionResult Trash()
        {
            var list = db.Topics.Where(m => m.Status == 0).ToList();

            return View(list);
        }
        public ActionResult DelTrash(int? id)
        {
            modelTopic modelTopic = db.Topics.Find(id);
            if (modelTopic == null)
            {
                return RedirectToAction("Index");
            }

            modelTopic.Status = 0;

            modelTopic.Updated_at = DateTime.Now;
            modelTopic.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelTopic.Updated_at = DateTime.Now;
            modelTopic.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelTopic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult ReTrash(int? id)
        {
            modelTopic modelTopic = db.Topics.Find(id);
            if (modelTopic == null)
            {

                return RedirectToAction("Trash");
            }
            modelTopic.Status = 2;

            modelTopic.Updated_at = DateTime.Now;
            modelTopic.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelTopic.Updated_at = DateTime.Now;
            modelTopic.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelTopic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Trash");

        }
        //doi trang thai
        public ActionResult Status(int? id)
        {
            modelTopic modelTopic = db.Topics.Find(id);
            if (modelTopic == null)
            {

                return RedirectToAction("Index");
            }
            modelTopic.Status = (modelTopic.Status == 1) ? 2 : 1;

            modelTopic.Updated_at = DateTime.Now;
            modelTopic.Created_by = int.Parse(Session["Admin_ID"].ToString());
            modelTopic.Updated_at = DateTime.Now;
            modelTopic.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelTopic).State = EntityState.Modified;
            db.SaveChanges();
            //Thongbao.set_flash("Thay đổi trạng thái thành công!" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        // GET: Admin/Topic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelTopic modelTopic = db.Topics.Find(id);
            if (modelTopic == null)
            {
                return HttpNotFound();
            }
            return View(modelTopic);
        }

        // POST: Admin/Topic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelTopic modelTopic = db.Topics.Find(id);
            db.Topics.Remove(modelTopic);
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
