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
    public class UserController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();
        public ActionResult Index()
        {
            ViewBag.countTrash = db.Users.Where(m => m.status == 0).Count();
            return View(db.Users.Where(m => m.status != 0).ToList());
        }
        public ActionResult Trash()
        {
            return View(db.Users.Where(m => m.status == 0).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(modelUser mUser)
        {
            if (ModelState.IsValid)
            {
                String avatar = XString.ToAscii(mUser.fullname);
                mUser.password = XString.ToMD5(mUser.password);
                mUser.created_at = DateTime.Now;
                mUser.created_by = int.Parse(Session["Admin_ID"].ToString());
                mUser.updated_at = DateTime.Now;
                mUser.updated_by = int.Parse(Session["Admin_ID"].ToString());

                var file = Request.Files["Image"];
                if (file != null && file.ContentLength > 0)
                {
                    String filename = avatar + file.FileName.Substring(file.FileName.LastIndexOf("."));
                    mUser.img = filename;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/User"), filename);
                    file.SaveAs(Strpath);
                }

                db.Users.Add(mUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mUser);
        }


        

        // Delete to trash
        public ActionResult ReTrash(int? id)
        {
            modelUser mUser = db.Users.Find(id);
            if (mUser == null)
            {
                Thongbao.set_flash("Không tồn tại User!", "warning");
                return RedirectToAction("Trash", "User");
            }
            mUser.status = 2;

            mUser.updated_at = DateTime.Now;
            mUser.updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(mUser).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công!" + " ID = " + id, "success");
            return RedirectToAction("Trash", "User");
        }

        // GET: Admin/User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelUser mUser = db.Users.Find(id);
            if (mUser == null)
            {
                return HttpNotFound();
            }
            return View(mUser);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelUser mUser = db.Users.Find(id);
            if (mUser == null)
            {
                return HttpNotFound();
            }
            return View(mUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(modelUser mUser)
        {
            if (ModelState.IsValid)
            {

                String avatar = XString.ToAscii(mUser.fullname);
                mUser.password = XString.ToMD5(mUser.password);
                mUser.created_at = DateTime.Now;
                mUser.created_by = int.Parse(Session["Admin_ID"].ToString());
                mUser.updated_at = DateTime.Now;
                mUser.updated_by = int.Parse(Session["Admin_ID"].ToString());

                var file = Request.Files["Image"];
                if (file != null && file.ContentLength > 0)
                {
                    String filename = avatar + file.FileName.Substring(file.FileName.LastIndexOf("."));
                    mUser.img = filename;
                    String Strpath = Path.Combine(Server.MapPath("~/Public/Library/images/User"), filename);
                    file.SaveAs(Strpath);
                }
                db.Entry(mUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mUser);
        }

        // GET: Admin/User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelUser mUser = db.Users.Find(id);
            if (mUser == null)
            {
                return HttpNotFound();
            }
            return View(mUser);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelUser mUser = db.Users.Find(id);
            db.Users.Remove(mUser);
            db.SaveChanges();
            Thongbao.set_flash("Đã xóa hoàn toàn User!", "success");
            return RedirectToAction("Trash");
        }
        public ActionResult Undo(int? id)
        {
            modelUser modelUser = db.Users.Find(id);
            if (modelUser == null)
            {

                return RedirectToAction("Trash");
            }
            modelUser.status = 2;

            modelUser.updated_at = DateTime.Now;
            modelUser.created_by = int.Parse(Session["Admin_ID"].ToString());
            modelUser.updated_at = DateTime.Now;
            modelUser.updated_by = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelUser).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công !" + " ID = " + id, "success");
            return RedirectToAction("Trash");

        }
        public ActionResult DelTrash(int? id)
        {
            modelUser modelUser = db.Users.Find(id);
            if (modelUser == null)
            {

                return RedirectToAction("Index");
            }
            modelUser.status = 0;

            modelUser.updated_at = DateTime.Now;
            modelUser.created_by = int.Parse(Session["Admin_ID"].ToString());
            modelUser.updated_at = DateTime.Now;
            modelUser.updated_by = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelUser).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Xóa vào thùng rác thành công !" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
        public ActionResult Status(int? id)
        {
            modelUser modelUser = db.Users.Find(id);
            if (modelUser == null)
            {

                return RedirectToAction("Index");
            }
            modelUser.status = (modelUser.status == 1) ? 2 : 1;

            modelUser.updated_at = DateTime.Now;
            modelUser.created_by = int.Parse(Session["Admin_ID"].ToString());
            modelUser.updated_at = DateTime.Now;
            modelUser.updated_by = int.Parse(Session["Admin_ID"].ToString());

            db.Entry(modelUser).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Thay đổi trạng thái thành công!" + " ID = " + id, "success");
            return RedirectToAction("Index");

        }
    }
}
