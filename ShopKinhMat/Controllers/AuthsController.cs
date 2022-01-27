using ShopBanDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDo.Controllers
{
    public class AuthsController : Controller
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();
        // GET: Auths
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection field)
        {
            String username = field["user"];
            String password = MyString.ToMD5(field["password"]);
            //String password = field["password"];

            int count_username = db.Users.Where(m => m.status == 1 && (m.username == username || m.email == username) && m.access == 0).Count();
            if (count_username == 0)
            {
                ViewBag.Error = "<span class ='text-danger'>Tài khoản này không tồn tại!!!</span>";
            }
            else
            {
                var user_acount = db.Users
                .Where(m => m.status == 1 && (m.username == username || m.email == username) && m.password == password && m.access==0);

                if (user_acount.Count() == 0)
                {
                    ViewBag.Error = "<span class ='text-danger'>Mật khẩu này không đúng!!!</span>";
                }
                else
                {
                    var user = user_acount.First();

                    Session.Add(Common.CommonConstants.CUSTOMER_SESSION, user);
                    Session["User_Name"] = user.fullname;
                    Session["User_Id"] = user.ID;
                    Response.Redirect("~/");
                }
            }
            return View("Login");
        }
        public ActionResult Logout()
        {

            Common.CommonConstants.CUSTOMER_SESSION = "";
            Session["User_Name"] = null;
            Session["User_Id"] = null;
            Response.Redirect("~/");
            return null;
        }
        public ActionResult register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult register(modelUser muser, FormCollection fc)
        {
            string uname = fc["uname"];
            string fname = fc["fname"];
            string Pass = XString.ToMD5(fc["psw"]);
            string Pass2 = XString.ToMD5(fc["repsw"]);
            if (Pass2 != Pass)
            {
                ViewBag.error = "Mật khẩu không khớp";
                return View("register");
            }
            string email = fc["email"];
            string address = fc["address"];
            string phone = fc["phone"];
            if (ModelState.IsValid)
            {
                var Luser = db.Users.Where(m => m.status == 1 && m.username == uname && m.access == 1);
                if (Luser.Count() > 0)
                {
                    ViewBag.error = "Tên Đăng Nhập đã tồn tại";
                    return View("register");
                }
                else
                {
                    muser.img = "defalt.png";
                    muser.password = Pass;
                    muser.username = uname;
                    muser.fullname = fname;
                    muser.email = email;
                    muser.address = address;
                    muser.phone = phone;
                    muser.gender = "nam";
                    muser.access = 0;
                    muser.created_at = DateTime.Now;
                    muser.updated_at = DateTime.Now;
                    muser.created_by = 1;
                    muser.updated_by = 1;
                    muser.status = 1;
                    db.Users.Add(muser);
                    db.SaveChanges();
                    Message.set_flash("Đăng ký tài khoản thành công ", "success");
                    return View("register");
                }

            }
            Message.set_flash("Đăng ký tài khoản thất bai", "danger");
            return Redirect("~/");
        }
    }
}