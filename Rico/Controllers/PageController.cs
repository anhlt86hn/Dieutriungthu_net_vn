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
    public class PageController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();

        public ActionResult Index(string tag)
        {
            var menu = db.Menus.Where(n => n.Active == true && n.Tag==tag).Single();
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

        //public ActionResult Album()
        //{
        //    var menu = db.Menus.Where(n => n.Active == true && n.Name == "Album ảnh"&&n.Tag=="album-anh").Single();

        //    #region[Title, Keyword, Deskription]
        //    if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
        //    if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
        //    if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
        //    #endregion

        //    string str = "";
        //    var album = db.Pictures.Where(p => p.Active == true && p.Position == 3).OrderBy(p => p.Ord).ToList();
        //    for (int z = 0; z < album.Count; z++)
        //    {
        //        if (z % 3 == 0)
        //        {
        //            str += "<tr>";
        //        }
        //        str += "<td><div style=\"padding-bottom:10px;padding-right:3px;\">";
        //        str += "<div align=\"center\"><span style=\"color:red\"><strong>" + album[z].Name + "</strong></span></div>";
        //        str += "<div><a href=\"" + album[z].Image + "\" class=\"lightbox\" rel=\"gallery\" title=\"" + album[z].Name + "\">";
        //        str += "<img width=\"150px\" height=\"122\" src=\"" + album[z].Image + "\" title=\"" + album[z].Name + "\" border=\"0\"></a></div></div></div></td>";
        //        if (z % 3 == 2)
        //        {
        //            str += "</str>";
        //        }
        //    }
        //    ViewBag.AlbumAnh = str;

        //    return View();
        //}

        //#region[Nhataitro]
        //public ActionResult NhaTaiTro(int? trang)
        //{
        //    if (Request.HttpMethod == "GET")
        //    {
        //    }
        //    else
        //    {
        //        page = 1;
        //    }

        //    #region[Phân trang]
        //    int pageSize = 15;
        //    int pageNumber = (page ?? 1);

        //    // Thiết lập phân trang
        //    PagedListRenderOptions ship = new PagedListRenderOptions();

        //    ship.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToLastPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToNextPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToIndividualPages = true;
        //    ship.DisplayPageCountAndCurrentLocation = false;
        //    ship.MaximumPageNumbersToDisplay = 5;
        //    ship.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
        //    ship.EllipsesFormat = "&#8230;";
        //    ship.LinkToFirstPageFormat = "Trang đầu";
        //    ship.LinkToPreviousPageFormat = "«";
        //    ship.LinkToIndividualPageFormat = "{0}";
        //    ship.LinkToNextPageFormat = "»";
        //    ship.LinkToLastPageFormat = "Trang cuối";
        //    ship.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
        //    ship.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
        //    ship.FunctionToDisplayEachPageNumber = null;
        //    ship.ClassToApplyToFirstListItemInPager = null;
        //    ship.ClassToApplyToLastListItemInPager = null;
        //    ship.ContainerDivClasses = new[] { "pagination-container" };
        //    ship.UlElementClasses = new[] { "pagination" };
        //    ship.LiElementClasses = Enumerable.Empty<string>();

        //    ViewBag.ship = ship;
        //    #endregion

        //    var menu = db.Menus.Where(m => m.Active == true && m.Name == "Những tấm lòng vàng" && m.Tag=="nhung-tam-long-vang").SingleOrDefault();

        //    //string rootMenuLevel = menu.Level;
        //    string menuName = menu.Name;
        //    string menuLink = menu.Link;


        //    //List<News> list = new List<News>();
        //    var list = db.NhaTaiTroes.Where(n => n.Active == true).OrderBy(n => n.SDate).ToList();
        //    //list = list.OrderByDescending(l => l.SDate).Take(10).ToList();       

        //    #region[Title, Keyword, Deskription]
        //    if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
        //    if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
        //    if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
        //    #endregion

        //    ViewBag.MenuName = menuName;
        //    ViewBag.MenuLink = menuLink;
        //    return View(list.ToPagedList(pageNumber, pageSize));

        //}
        //#endregion


        //#region[Chi tiet nha tai tro]
        //public ActionResult NhaTaiTroDetail(string tag)
        //{
        //    var ntt = (from n in db.NhaTaiTroes where n.Link == tag select n).SingleOrDefault();

        //    string nttName = ntt.Name;
        //    string nttLink = ntt.Link;
        //    string nttDetail = ntt.Detail;
        //    string nttAddress = ntt.Address;
        //    string nttTitle = ntt.Title;
        //    string nttDescription = ntt.Description;
        //    string nttKeyword = ntt.Keyword;

        //    var menu = db.Menus.Where(m => m.Active == true && m.Name == "Những tấm lòng vàng" && m.Tag == "nhung-tam-long-vang").SingleOrDefault();
        //    string menuName = menu.Name;
        //    ViewBag.MenuName = menuName;
        //    ViewBag.NTTName = nttName;
        //    ViewBag.NTTLink = nttLink;
        //    ViewBag.NTTDetail = nttDetail;
        //    ViewBag.NTTAddress = nttAddress;


        //    var list = db.NhaTaiTroes.Where(l => l.Active == true).OrderByDescending(l => l.Ord).Take(5).ToList();
        //    list.Remove(ntt);

        //    #region[Title, Keyword, Deskription]

        //    if (ntt.Title != null && ntt.Title.Length > 0) { ViewBag.tit = ntt.Title; } else { ViewBag.tit = ntt.Name; }
        //    if (ntt.Description != null && ntt.Description.Length > 0) { ViewBag.des = ntt.Description; } else { ViewBag.des = ntt.Name; }
        //    if (ntt.Keyword != null && ntt.Keyword.Length > 0) { ViewBag.key = ntt.Keyword; } else { ViewBag.key = ntt.Name; }
        //    #endregion
        //    return View(list);
        //}
        //#endregion


        //#region[Video]
        //public ActionResult Video(int? page)
        //{
        //    if (Request.HttpMethod == "GET")
        //    {
        //    }
        //    else
        //    {
        //        page = 1;
        //    }

        //    #region[Phân trang]
        //    int pageSize = 15;
        //    int pageNumber = (page ?? 1);

        //    // Thiết lập phân trang
        //    PagedListRenderOptions ship = new PagedListRenderOptions();

        //    ship.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToLastPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToNextPage = PagedListDisplayMode.Always;
        //    ship.DisplayLinkToIndividualPages = true;
        //    ship.DisplayPageCountAndCurrentLocation = false;
        //    ship.MaximumPageNumbersToDisplay = 5;
        //    ship.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
        //    ship.EllipsesFormat = "&#8230;";
        //    ship.LinkToFirstPageFormat = "Trang đầu";
        //    ship.LinkToPreviousPageFormat = "«";
        //    ship.LinkToIndividualPageFormat = "{0}";
        //    ship.LinkToNextPageFormat = "»";
        //    ship.LinkToLastPageFormat = "Trang cuối";
        //    ship.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
        //    ship.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
        //    ship.FunctionToDisplayEachPageNumber = null;
        //    ship.ClassToApplyToFirstListItemInPager = null;
        //    ship.ClassToApplyToLastListItemInPager = null;
        //    ship.ContainerDivClasses = new[] { "pagination-container" };
        //    ship.UlElementClasses = new[] { "pagination" };
        //    ship.LiElementClasses = Enumerable.Empty<string>();

        //    ViewBag.ship = ship;
        //    #endregion

        //    var menu = db.Menus.Where(m => m.Active == true && m.Name == "Phim phóng sự" && m.Tag == "phim-phong-su").SingleOrDefault();

        //    //string rootMenuLevel = menu.Level;
        //    string menuName = menu.Name;
        //    string menuLink = menu.Link;


        //    //List<News> list = new List<News>();
        //    var list = db.Videos.Where(n => n.Active == true).OrderBy(n => n.SDate).ToList();
        //    //list = list.OrderByDescending(l => l.SDate).Take(10).ToList();       

        //    #region[Title, Keyword, Deskription]
        //    if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
        //    if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
        //    if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
        //    #endregion

        //    ViewBag.MenuName = menuName;
        //    ViewBag.MenuLink = menuLink;
        //    return View(list.ToPagedList(pageNumber, pageSize));

        //}
        //#endregion

    }
}
