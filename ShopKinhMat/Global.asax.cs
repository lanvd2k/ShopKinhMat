using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBanDo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            Session["cart"] = "";
            Session["User_Id"] = " 1 ";
            //Admin
            Session["Admin_ID"] = null;
            Session["Admin_Name"] = null;
           
            //User
            Session["User_Name"] = null;
            Session["User_Id"] = null;
            
            Session["Thongbao"] = "";
            Session["Message"] = "";
            Session["OrderId"] = "";
        }
    }
}
