using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using Rico.Models;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;

namespace Rico.Controllers
{
    public class AboutController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();

        public ActionResult Index()
        {
            var menu = db.Menus.Where(n => n.Active == true && n.Tag=="ve-chung-toi").Single();
            string name = menu.Name;           
            string link = menu.Link;


            #region[Title, Keyword, Deskription]
            if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
            if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
            if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
            #endregion

            ViewBag.MenuName = name;
            ViewBag.MenuLink = link;
            return View(menu);

        } 
    }
}
