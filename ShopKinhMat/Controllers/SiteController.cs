using PagedList;
using ShopBanDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDo.Controllers
{
    public class SiteController : Controller
    {
        private ShopBanDoDbContext db = new ShopBanDoDbContext();
        // GET: Site
        public ActionResult Index(String slug = "")
        {
            int pageNumber = 1;
            Session["keywords"] = null;
            if (!string.IsNullOrEmpty(Request.QueryString["page"]))
            {
                pageNumber = int.Parse(Request.QueryString["page"].ToString());
            }
            if (slug == "")
            {
                return this.Home();
            }
            else
            {
                var link = db.Links.Where(m => m.Slug == slug);
                if (link.Count() > 0)
                {
                    var res = link.First();
                    if (res.Type == "page" || res.Type == "post")
                    {
                        return this.PostPage(slug);
                    }
                  
                    else if (res.Type == "topic")
                    {
                        return this.PostTopic(slug, pageNumber);
                    }
                    else if (res.Type == "category")
                    {
                        return this.ProductCategory(slug, pageNumber);
                    }
                }
                else
                {
                    if (db.Products.Where(m => m.Slug == slug && m.Status == 1).Count() > 0)
                    {
                        return this.ProductDetail(slug);
                    }
                    else if (db.Posts.Where(m => m.Slug == slug && m.Type == "post" && m.Status == 1).Count() > 0)
                    {
                        return this.PostDetail(slug);
                    }
                }
                return this.Error(slug);
            }
        }

        public ActionResult ProductDetail(String slug)
        {
            var model = db.Products
                .Where(m => m.Slug == slug && m.Status == 1)
                .First();
            int catid = model.CatId;

            List<int> listcatid = new List<int>();
            listcatid.Add(catid);

            var list2 = db.Categorys
                .Where(m => m.ParentId == catid)
                .Select(m => m.Id)
                .ToList();
            foreach (var id2 in list2)
            {
                listcatid.Add(id2);
                var list3 = db.Categorys
                    .Where(m => m.ParentId == id2)
                    .Select(m => m.Id)
                    .ToList();
                foreach (var id3 in list3)
                {
                    listcatid.Add(id3);
                }
            }
            // danh mục cùng sản phẩm
            ViewBag.listother = db.Products
                .Where(m => m.Status == 1 && listcatid
                .Contains(m.CatId) && m.Id != model.Id)
                .OrderByDescending(m => m.Created_at)
                .Take(12)
                .ToList();
            // sản phẩm mới nhập
            ViewBag.news = db.Products
                .Where(m => m.Status == 1 /*&& listcatid.Contains(m.CatId)*/ && m.Id != model.Id)
                .OrderByDescending(m => m.Created_at).Take(4).ToList();
            return View("ProductDetail", model);
        }
        public ActionResult PostDetail(String slug)
        {
            var model = db.Posts
                 .Where(m => m.Slug == slug && m.Status == 1)
                 .First();
            int topid = model.Topid;

            List<int> listtopid = new List<int>();
            listtopid.Add(topid);

            var list2 = db.Topics
                .Where(m => m.ParentId == topid)
                .Select(m => m.Id)
                .ToList();
            foreach (var id2 in list2)
            {
                listtopid.Add(id2);
                var list3 = db.Topics
                    .Where(m => m.ParentId == id2)
                    .Select(m => m.Id)
                    .ToList();
                foreach (var id3 in list3)
                {
                    listtopid.Add(id3);
                }
            }
            // danh mục cùng bài viết
            ViewBag.listother = db.Posts
                .Where(m => m.Status == 1 && listtopid
                .Contains(m.Topid) && m.Id != model.Id)
                .OrderByDescending(m => m.Created_At)
                .Take(12)
                .ToList();

            return View("PostDetail", model);
        }
        public ActionResult Error(String slug)
        {
            return View("Error");
        }

        public ActionResult PostPage(String slug)
        {
            var item = db.Posts
                .Where(m => m.Slug == slug && m.Type == "post" || m.Type == "page" )
                 .First();
            return View("PostPage", item);
        }

        public ActionResult PostTopic(String slug, int pageNumber)
        {
            var item = db.Posts
                .Where(m => m.Slug == slug && m.Type == "post" || m.Type == "page" )
                 .First();
            return View("PostPage", item);
        }
        public ActionResult ProductCategory(String slug, int pageNumber)
        {
            int pageSize = 8;
            var row_cat = db.Categorys
                .Where(m => m.Slug == slug)
                .First();
            List<int> listcatid = new List<int>();
            listcatid.Add(row_cat.Id);

            var list2 = db.Categorys
                .Where(m => m.ParentId == row_cat.Id)
                .Select(m => m.Id)
                .ToList();
            foreach (var id2 in list2)
            {
                listcatid.Add(id2);
                var list3 = db.Categorys
                    .Where(m => m.ParentId == id2)
                    .Select(m => m.Id)
                    .ToList();
                foreach (var id3 in list3)
                {
                    listcatid.Add(id3);
                }
            }
            var list = db.Products
              .Where(m => m.Status == 1 && listcatid.Contains(m.CatId))
                .OrderByDescending(m => m.Created_at);
            

            ViewBag.Slug = slug;
            ViewBag.CatName = row_cat.Name;
            return View("ProductCategory", list
                .ToPagedList(pageNumber, pageSize));
        }
        //Trang Chủ
        public ActionResult Home()
        {
            var list = db.Categorys
               .Where(m => m.Status == 1 && m.ParentId == 0)
               .Take(8)
               .ToList();
            return View("Home", list);
        }
        public ActionResult Other()
        {
            return View("Other");
        }
        //Sản phẩm trang chủ
        public ActionResult ProductHome(int catid)
        {
            List<int> listcatid = new List<int>();
            listcatid.Add(catid);

            var list2 = db.Categorys
                .Where(m => m.ParentId == catid).Select(m => m.Id)
                .ToList();
            foreach (var id2 in list2)
            {
                listcatid.Add(id2);
                var list3 = db.Categorys
                    .Where(m => m.ParentId == id2)
                    .Select(m => m.Id).ToList();
                foreach (var id3 in list3)
                {
                    listcatid.Add(id3);
                }
            }

            var list = db.Products
                .Where(m => m.Status == 1 && listcatid
                .Contains(m.CatId))
                .Take(4)
                .OrderByDescending(m => m.Created_at);

            return View("_ProductHome", list);
        }
        //Tat ca sp
        public ActionResult Product(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Products.Where(m => m.Status == 1)
                .OrderByDescending(m => m.Created_at)
                .ToPagedList(pageNumber, pageSize);
            return View(list);
        }
        // tìm kiếm sản phẩm
        public ActionResult Search(String key, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var list = db.Products.Where(m => m.Status == 1);
            if (String.IsNullOrEmpty(key.Trim()))
            {
                return RedirectToAction("Index", "Site");

            }
            else
            {
                list = list.Where(m => m.Name.Contains(key)).OrderByDescending(m => m.Created_at);
            }

            Session["keywords"] = key;
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        //Tat ca bai viet
        public ActionResult Post(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Posts.Where(m => m.Status == 1 && m.Type == "post" )
                .OrderByDescending(m => m.Created_At)
                .ToPagedList(pageNumber, pageSize);
            return View(list);
        }
        //Trang Chủ Bài viết Topic
        public ActionResult HomePost() // url tin-tuc
        {
            var list = db.Topics
               .Where(m => m.Status == 1 && m.ParentId == 0)
               .Take(8)
               .ToList();
            return View("_HomePost", list);
        }
        //home sale (show sản phẩm khuyến mãi)
        public ActionResult HomeProductSale()
        {
            var list = db.Products.Where(m => m.Status == 1 && m.Discount == 1)
                .OrderByDescending(m => m.Created_at);
            return View("_HomeProductSale", list);
        }
        //sản phẩm khuyến mãi  (san-pham-khuyen-mai)
        public ActionResult ProductSale(int? page)
        {
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            var list = db.Products.Where(m => m.Status == 1 && m.Discount == 1)
                .OrderByDescending(m => m.Created_at)
                .ToPagedList(pageNumber, pageSize);
            return View(list);
        }
        public ActionResult Posts()
        {
            var list = db.Topics
               .Where(m => m.Status == 1 && m.ParentId == 0)
               .Take(8)
               .ToList();
            return View("Post", list);
        }
        public ActionResult PostHome(int topid)
        {
            List<int> listtopid = new List<int>();
            listtopid.Add(topid);

            var list2 = db.Topics
                .Where(m => m.ParentId == topid).Select(m => m.Id)
                .ToList();
            foreach (var id2 in list2)
            {
                listtopid.Add(id2);
                var list3 = db.Topics
                    .Where(m => m.ParentId == id2)
                    .Select(m => m.Id).ToList();
                foreach (var id3 in list3)
                {
                    listtopid.Add(id3);
                }
            }

            var list = db.Posts
                .Where(m => m.Status == 1 && listtopid
                .Contains(m.Topid)&& m.Type=="post")
                .Take(4)
                .OrderByDescending(m => m.Created_At);

            return View("PostHome", list);
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Contact(modelContact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Created_at = DateTime.Now;
                contact.Updated_at = DateTime.Now;
                contact.Updated_by = 1;
                contact.Status = 1;
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("ThanksContact", "Site");
            }
            return View();
        }
        public ActionResult ThanksContact()
        {
            return View();
        }
    }
}