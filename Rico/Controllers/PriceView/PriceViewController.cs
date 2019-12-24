using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.Linq;
using Rico.Models;
using PagedList;
using PagedList.Mvc;

namespace Rico.Controllers.PriceView
{
    public class PriceViewController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region[Danh sách]
        public ActionResult List(string tag)
        {                        
            var menu = db.Menus.Where(m => m.Active == true && m.Name == "Báo giá").SingleOrDefault();
            var price = db.Prices.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList(); 

            string menuName, menuLink = "";
            menuName = menu.Name;
            menuLink = menu.Link;                      
            #region[Title, Keyword, Deskription]
            if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
            if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
            if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
            #endregion
            ViewBag.MenuName = menuName;
            ViewBag.MenuLink = menuLink;
            return View(price);
        }
        #endregion

        #region[Danh sách]
        public ActionResult Detail()
        {
            var menu = db.Menus.Where(m => m.Active == true && m.Name == "Báo giá").SingleOrDefault();
            var price = db.Prices.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string str = "";
            str += "<ul class=\"lst-lsb\">";
            for(int i=0;i<price.Count;i++)
            {
                str += "<li><a href=\"/bao-gia/chi-tiet#" + price[i].Tag + "\">" + price[i].Name + "</a></li>";
            }
            str += "</ul>";
            string menuName, menuLink = "";
            menuName = menu.Name;
            menuLink = menu.Link;
            #region[Title, Keyword, Deskription]
            if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
            if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
            if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
            #endregion
            ViewBag.MenuName = menuName;
            ViewBag.MenuLink = menuLink;
            ViewBag.Str = str;
            return View(price);
        }
        #endregion


        #region[404]
        public ActionResult Updating()
        {
            var t = db.Pictures.Where(p => p.Name == "Đang cập nhật").SingleOrDefault();
            string imgSrc = t.Image;
            string imgAlt = t.Name;
            ViewBag.ImageSource = imgSrc;
            ViewBag.ImageAlt = imgAlt;
            return View();
        }
        #endregion

    }
}
