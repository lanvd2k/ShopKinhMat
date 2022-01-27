using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBanDo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
         name: "huy thanh toan online",
         url: "cancel-order",
         defaults: new { controller = "Checkout", action = "cancel_order", id = UrlParameter.Optional }
         );
            routes.MapRoute(
           name: "thanh toan thanh cong",
           url: "confirm-orderPaymentOnline",
           defaults: new { controller = "Checkout", action = "confirm_orderPaymentOnline", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "huy thanh toan online momo",
           url: "cancel-order-momo",
           defaults: new { controller = "Checkout", action = "cancel_order_momo", id = UrlParameter.Optional }
           );

            routes.MapRoute(
           name: "thanh toan thanh cong momo",
           url: "confirm-orderPaymentOnline-momo",
           defaults: new { controller = "Checkout", action = "confirm_orderPaymentOnline_momo", id = UrlParameter.Optional }
           );
            routes.MapRoute(
          name: "Lien he",
          url: "lien-he",
          defaults: new { controller = "Site", action = "Contact", id = UrlParameter.Optional }
          );
            routes.MapRoute(
          name: "Tin tuc",
          url: "tin-tuc",
          defaults: new { controller = "Site", action = "Posts", id = UrlParameter.Optional }
          );
            routes.MapRoute(
          name: "register",
          url: "dang-ky",
          defaults: new { controller = "Auths", action = "register", id = UrlParameter.Optional }
          );
           
            routes.MapRoute(
               name: "Logout",
               url: "logout",
              defaults: new { Controller = "Auths", action = "Logout", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Login",
               url: "login",
              defaults: new { Controller = "Auths", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "thanh-toan",
                url: "thanh-toan",
                defaults: new { controller = "Checkout", action = "Index", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "xoa gio hang",
                url: "xoa-gio-hang",
                defaults: new { controller = "Cart", action = "deleteitem", id = UrlParameter.Optional }
                );
            
            routes.MapRoute(
                name: "them vao gio hang",
                url: "them-sp-giohang",
                defaults: new { controller = "Cart", action = "Additem", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "gio hang",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TatCaSP",
                url: "tat-ca-san-pham",
                defaults: new { controller = "Site", action = "Product", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SiteSlug",
                url: "{slug}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
