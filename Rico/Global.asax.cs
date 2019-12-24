using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Rico
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //if (!File.Exists(Server.MapPath("Count_Visited.txt")))
            //    File.WriteAllText(Server.MapPath("Count_Visited.txt"), "0");
            //Application["DaTruyCap"] = int.Parse(File.ReadAllText(Server.MapPath("Count_Visited.txt")));
        }

        protected void Session_Start()
        {
            Application.Lock();
            //if (Application["DangTruyCap"] == null)
            //    Application["DangTruyCap"] = 1;
            //else
            //    Application["DangTruyCap"] = (int)Application["DangTruyCap"] + 1;           
            //Application["DaTruyCap"] = (int)Application["DaTruyCap"] + 1;
            
            ////File.WriteAllText(Server.MapPath("Count_Visited.txt"), Application["DaTruyCap"].ToString());
            ////Application["PageView"] = (int)Application["PageView"] + 1;
            ////Application["Online"] = (int)Application["Online"] + 1;
            Application.UnLock();
        }

        protected void Session_End()
        {
            Application.Lock();
            //Application["DangTruyCap"] = (int)Application["DangTruyCap"] - 1;
            Application.UnLock();
        }
    }
}
