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
    public class PageController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();

        // GET: Admin/Page
        public ActionResult Index()
        {
            ViewBag.demrac = db.Posts.Where(m => m.Status == 0 && m.Type == "page").OrderBy(m => m.Created_At).OrderByDescending(m => m.Created_At).Count();

            var list = db.Posts.Where(m => m.Status != 0 && m.Type == "page").ToList();
            foreach (var row in list)
            {
                var temp_link = db.Links
                    .Where(m => m.Type == "page" && m.TableId == row.Id);

                if (temp_link.Count() > 0)
                {
                    var row_link = temp_link.First();
                    row_link.Name = row.Title;
                    row_link.Slug = row.Slug;
                    db.Entry(row_link).State = EntityState.Modified;
                }
                else
                {
                    var row_link = new modelLink();

                    row_link.Name = row.Title;
                    row_link.Slug = row.Slug;
                    row_link.Type = "page";
                    row_link.TableId = row.Id;
                    db.Links.Add(row_link);
                }
            }
            db.SaveChanges();
            return View(list);
        }

        // GET: Admin/Page/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelPost modelPost = db.Posts.Find(id);
            if (modelPost == null)
            {
                return HttpNotFound();
            }
            return View(modelPost);
        }

        // GET: Admin/Page/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Admin/Page/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(modelPost modelPost)
        {
            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(modelPost.Title);
                String slug = strSlug;
                modelPost.Slug = slug;
                modelPost.Type = "page";
                modelPost.Created_At = DateTime.Now;
                modelPost.Updated_At = DateTime.Now;
                modelPost.Created_By = int.Parse(Session["Admin_ID"].ToString());
                modelPost.Updated_By = int.Parse(Session["Admin_ID"].ToString());

                var f = Request.Files["Img"];

                if (f != null & f.ContentLength > 0)
                {
                    String fileName = strSlug + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    modelPost.Img = fileName;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/page"), fileName);
                    f.SaveAs(Strpath);
                }
                db.Posts.Add(modelPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelPost);
        }

        // GET: Admin/Page/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelPost modelPost = db.Posts.Find(id);
            if (modelPost == null)
            {
                return HttpNotFound();
            }
            return View(modelPost);
        }

        // POST: Admin/Page/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(modelPost modelPost)
        {
            if (ModelState.IsValid)
            {
                String strSlug = XString.ToAscii(modelPost.Title);
                String slug = strSlug;
                modelPost.Slug = slug;
                modelPost.Type = "page";
                modelPost.Updated_At = DateTime.Now;

                modelPost.Updated_By = int.Parse(Session["Admin_ID"].ToString());
                modelPost.Created_By = int.Parse(Session["Admin_ID"].ToString());

                ////Upload file
                var f = Request.Files["Img"];

                if (f != null & f.ContentLength > 0)
                {
                    String fileName = strSlug + f.FileName.Substring(f.FileName.LastIndexOf("."));
                    modelPost.Img = fileName;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/page"), fileName);
                    f.SaveAs(Strpath);
                }
                db.Entry(modelPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelPost);
        }
        public ActionResult delTrash(int? id)
        {
            modelPost modelPost = db.Posts.Find(id);
            if (modelPost == null)
            {
                return RedirectToAction("Index");
            }

            modelPost.Status = 0;

            modelPost.Updated_At = DateTime.Now;
            modelPost.Created_By = int.Parse(Session["Admin_ID"].ToString());
            modelPost.Updated_At = DateTime.Now;
            modelPost.Updated_By = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelPost).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Undo(int? id)
        {
            modelPost modelPost = db.Posts.Find(id);
            if (modelPost == null)
            {

                return RedirectToAction("Trash");
            }
            modelPost.Status = 2;

            modelPost.Updated_At = DateTime.Now;
            modelPost.Created_By = int.Parse(Session["Admin_ID"].ToString());
            modelPost.Updated_At = DateTime.Now;
            modelPost.Updated_By = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelPost).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công !" + " ID = " + id, "success");
            return RedirectToAction("Trash");

        }
        //trash
        public ActionResult Trash()
        {
            var list = db.Posts.Where(m => m.Status == 0).ToList();

            return View(list);
        }
        //doi trang thai
        public ActionResult Status(int? id)
        {
            modelPost modelPost = db.Posts.Find(id);
            if (modelPost == null)
            {

                return RedirectToAction("Index");
            }
            modelPost.Status = (modelPost.Status == 1) ? 2 : 1;

            modelPost.Updated_At = DateTime.Now;
            modelPost.Created_By = int.Parse(Session["Admin_ID"].ToString());
            modelPost.Updated_At = DateTime.Now;
            modelPost.Updated_By = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelPost).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công!" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        // GET: Admin/Page/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelPost modelPost = db.Posts.Find(id);
            if (modelPost == null)
            {
                return HttpNotFound();
            }
            return View(modelPost);
        }

        // POST: Admin/Page/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelPost modelPost = db.Posts.Find(id);
            db.Posts.Remove(modelPost);
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
