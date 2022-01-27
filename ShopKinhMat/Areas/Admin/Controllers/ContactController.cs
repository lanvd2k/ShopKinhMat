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
    public class ContactController : BaseController
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();

        public ActionResult Index()
        {
            ViewBag.countTrash = db.Contacts.Where(m => m.Status == 0).Count();
            return View(db.Contacts.Where(m => m.Status == 1).ToList());
        }
        public ActionResult Trash()
        {
            return View(db.Contacts.Where(m => m.Status == 0).ToList());
        }

        public ActionResult Reply(int? id)
        {
            if (id == null)
            {
                Thongbao.set_flash("Không tồn tại liên hệ từ khách hàng!", "danger");
                return RedirectToAction("Index", "Contact");
            }
            modelContact modelContact = db.Contacts.Find(id);
            if (modelContact == null)
            {
                Thongbao.set_flash("Không tồn tại liên hệ từ khách hàng!", "danger");
                return RedirectToAction("Index", "Contact");
            }
            return View(modelContact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Reply(modelContact modelContact)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        modelContact.Status = 2;
        //        modelContact.Updated_at = DateTime.Now;
        //        modelContact.Updated_by = 1;

        //        String content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Views/Mail/D_Mail.html"));
        //        content = content.Replace("{{FullName}}", modelContact.Fullname);
        //        content = content.Replace("{{Reply}}", modelContact.Reply);
        //        content = content.Replace("{{RQ}}", modelContact.Detail);
        //        content = content.Replace("{{AdminName}}", Session["User_Admin"].ToString());
        //        String subject = "Phản hồi liên hệ từ Giadunghiendai.com";
        //        //new MailHelper().SendMail(modelContact.Email, subject, content);

        //        db.Entry(modelContact).State = EntityState.Modified;
        //        db.SaveChanges();
        //        Thongbao.set_flash("Đã trả lời liên hệ!", "success");
        //        return RedirectToAction("Index");
        //    }
        //    return View(modelContact);
        //}
        public ActionResult Reply(modelContact modelContact)
        {
            if (ModelState.IsValid)
            {
                modelContact.Flag = 1;
                modelContact.Updated_at = DateTime.Now;
                modelContact.Updated_by = int.Parse(Session["Admin_ID"].ToString());

                db.Entry(modelContact).State = EntityState.Modified;
                db.SaveChanges();
                Thongbao.set_flash("Đã trả lời liên hệ!", "success");
                return RedirectToAction("Index");
            }
            return View(modelContact);
        }

        public ActionResult DelTrash(int id)
        {
            modelContact modelContact = db.Contacts.Find(id);
            if (modelContact == null)
            {
                Thongbao.set_flash("Không tồn tại!", "warning");
                return RedirectToAction("Index");
            }
            modelContact.Status = 0;
            modelContact.Updated_at = DateTime.Now;
            modelContact.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelContact).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Ném thành công vào thùng rác!" + " ID = " + id, "success");
            return RedirectToAction("Index");
        }

        public ActionResult ReTrash(int? id)
        {
            modelContact modelContact = db.Contacts.Find(id);
            if (modelContact == null)
            {
                Thongbao.set_flash("Không tồn tại danh mục!", "warning");
                return RedirectToAction("Trash", "Contact");
            }
            modelContact.Status = 1;
            modelContact.Updated_at = DateTime.Now;
            modelContact.Updated_by = int.Parse(Session["Admin_ID"].ToString());
            db.Entry(modelContact).State = EntityState.Modified;
            db.SaveChanges();
            Thongbao.set_flash("Khôi phục thành công!" + " ID = " + id, "success");
            return RedirectToAction("Trash", "Contact");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                Thongbao.set_flash("Không tồn tại!", "warning");
                return RedirectToAction("Index");
            }
            modelContact modelContact = db.Contacts.Find(id);
            if (modelContact == null)
            {
                Thongbao.set_flash("Không tồn tại!", "warning");
                return RedirectToAction("Index");
            }
            return View(modelContact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelContact modelContact = db.Contacts.Find(id);
            db.Contacts.Remove(modelContact);
            db.SaveChanges();
            Thongbao.set_flash("Đã xóa vĩnh viễn liên hệ!", "danger");
            return RedirectToAction("Index");
        }
    }
}
