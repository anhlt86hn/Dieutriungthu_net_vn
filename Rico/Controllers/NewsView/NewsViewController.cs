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

namespace Rico.Controllers.NewsView
{
    public class NewsViewController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region[Danh Mục tin]
        public ActionResult List(int? trang, string tag)
        {
            if (Request.HttpMethod == "GET")
            {
            }
            else
            {
                trang = 1;
            }

            #region[Phân trang]
            int pageSize = 5;
            int pageNumber = (trang ?? 1);

            // Thiết lập phân trang
            PagedListRenderOptions ship = new PagedListRenderOptions();

            ship.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToLastPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToNextPage = PagedListDisplayMode.Always;
            ship.DisplayLinkToIndividualPages = true;
            ship.DisplayPageCountAndCurrentLocation = false;
            ship.MaximumPageNumbersToDisplay = 5;
            ship.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            ship.EllipsesFormat = "&#8230;";
            ship.LinkToFirstPageFormat = "Trang đầu";
            ship.LinkToPreviousPageFormat = "«";
            ship.LinkToIndividualPageFormat = "{0}";
            ship.LinkToNextPageFormat = "»";
            ship.LinkToLastPageFormat = "Trang cuối";
            ship.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ship.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            ship.FunctionToDisplayEachPageNumber = null;
            ship.ClassToApplyToFirstListItemInPager = null;
            ship.ClassToApplyToLastListItemInPager = null;
            ship.ContainerDivClasses = new[] { "pagination-container" };
            ship.UlElementClasses = new[] { "pagination" };
            ship.LiElementClasses = Enumerable.Empty<string>();

            ViewBag.ship = ship;
            #endregion

            var root = db.GroupNews.Where(r => r.Active == true && r.Tag == tag).SingleOrDefault();
            var menu = db.Menus.Where(m => m.Active == true && m.Tag == tag).SingleOrDefault();

            string menuName, menuLink = "";
            menuName = menu.Name;
            menuLink = menu.Link;

            int gid = root.Id;
            string gLevel = root.Level;
            string groupnewsdetail = root.Detail;
            List<News> sum = new List<News>();
            string str = "";
            if (menu.Level.Length == 5)
            {
                var cate = db.GroupNews.Where(l => l.Level.Substring(0, 5) == gLevel && l.Tag != tag).OrderBy(l => l.Ord).ToList();
                for (int i = 0; i < cate.Count; i++)
                {
                    int cId = cate[i].Id;
                    //string cTag = cate[i].Tag;
                    //string cName = cate[i].Name;
                    var list = db.News.Where(n => n.Active == true && n.IdGroup == cId).OrderBy(n => n.Ord).ToList();
                    sum.AddRange(list);
                  
                }
                sum = sum.OrderBy(su => su.SDate).ToList();
                for(int k=0;k< sum.Count;k++)
                {
                    str += "<div class=\"article-blog\"><div class=\"article_img\"><a class=\"link-image\" href=\"/baiviet/" + sum[k].Tag + "\">";
                    str += "<img src=\"" + sum[k].Image + "\"><div class=\"hover\"></div></a></div>";
                    str += "<div class=\"title\"><h2><a href=\"/baiviet/"+ sum[k].Tag+"\">"+ sum[k].Name+"</a></h2></div>";
                    str += "<div class=\"details\">" + sum[k].Detail + "</div><div class=\"clear\"></div></div>";                     
                }
            }
            else if (menu.Level.Length == 10)
            {
                var group = db.GroupNews.Where(gn => gn.Active == true && gn.ParentId == gid).OrderBy(gn => gn.Ord).ToList();
                for(int v=0;v<group.Count;v++)
                {
                    int ggId = group[v].Id;
                    var list = db.News.Where(n => n.Active == true && n.IdGroup == ggId).OrderBy(n => n.Ord).ToList();
                    sum.AddRange(list);
                }
                   
                sum = sum.OrderBy(su => su.SDate).ToList();
                for (int k = 0; k < sum.Count; k++)
                {
                    str += "<div class=\"article-blog\"><div class=\"article_img\"><a class=\"link-image\" href=\"/baiviet/" + sum[k].Tag + "\">";
                    str += "<img src=\"" + sum[k].Image + "\"><div class=\"hover\"></div></a></div>";
                    str += "<div class=\"title\"><h2><a href=\"/baiviet/" + sum[k].Tag + "\">" + sum[k].Name + "</a></h2></div>";
                    str += "<div class=\"details\">" + sum[k].Detail + "</div><div class=\"clear\"></div></div>";
                }
            }
            else if (menu.Level.Length == 15)
            {
                var list = db.News.Where(n => n.Active == true && n.IdGroup == gid).OrderBy(n => n.Ord).ToList();
                for (int v = 0; v < list.Count; v++)
                {
                    sum.AddRange(list);
                }
                sum = sum.OrderBy(su => su.Ord).ToList();
                for (int k = 0; k < sum.Count; k++)
                {
                    str += "<div class=\"article-blog\"><div class=\"article_img\"><a class=\"link-image\" href=\"/baiviet/" + sum[k].Tag + "\">";
                    str += "<img src=\"" + sum[k].Image + "\"><div class=\"hover\"></div></a></div>";
                    str += "<div class=\"title\"><h2><a href=\"/baiviet/" + sum[k].Tag + "\">" + sum[k].Name + "</a></h2></div>";
                    str += "<div class=\"details\">" + sum[k].Detail + "</div><div class=\"clear\"></div></div>";
                }
            }

                #region[Title Home]
                var listconfig = (from p in db.Configs select p).ToList();
            if (listconfig.Count > 0)
            {
                ViewBag.tithome = listconfig[0].Title;               
            }
            listconfig.Clear();
            listconfig = null;
            #endregion
            #region[Title, Keyword, Deskription]
            if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
            if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
            if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
            #endregion

            ViewBag.Str = str;
            ViewBag.MenuName = menuName;
            ViewBag.MenuLink = menuLink;
            ViewBag.CategoryDetail = groupnewsdetail;
            return View(sum.ToPagedList(pageNumber, pageSize));

        }
        #endregion

        #region[Chi tiết Tin]
        public ActionResult Detail(string tag)
        {
            var post = db.News.Where(n => n.Active == true && n.Tag == tag).Single();
            #region[Title, Keyword, Deskription]
            if (post.Title.Length > 0) { ViewBag.tit = post.Title; } else { ViewBag.tit = post.Name; }
            if (post.Description.Length > 0) { ViewBag.des = post.Description; } else { ViewBag.des = post.Name; }
            if (post.Keyword.Length > 0) { ViewBag.key = post.Keyword; } else { ViewBag.key = post.Name; }
            #endregion
            int pId = post.IdGroup;
            var gp = db.GroupNews.Where(g => g.Active == true & g.Id == pId).Single();            
            string gptag = gp.Tag;
            var menugn = db.Menus.Where(m => m.Active == true & m.Tag == gptag).Single();
            string mnname = menugn.Name;ViewBag.MenuName = mnname;
            string mnlink = menugn.Link;ViewBag.MenuLink = mnlink;
            string mntitle = menugn.Title; ViewBag.MenuTitle = mntitle;
            string str = "";
            var list = db.News.Where(m => m.Active == true && m.IdGroup == pId).OrderByDescending(m => m.Ord).ToList();
          
            for (int j=0;j<list.Count;j++)
            {
                str += "<li><h4><a title =\"" + list[j].Title + "\" href =\"/baiviet/" + list[j].Tag + "\">" + list[j].Name + "</a></h4>";
                str += "<div class=\"pic\"><a title =\"" + list[j].Title + "\" href =\"/baiviet/" + list[j].Tag + "\">";
                str += "<img src=\"" + list[j].Image + "\" alt=\"" + list[j].Title + "\" /></a></div>";
                str += "<div class=\"desc\">" + list[j].Content + "</div></li>";         
            }
            ViewBag.Str = str;            
            return View(post);
        }
        #endregion


        #region[404]
        public ActionResult Updating()
        {
            var t = db.Pictures.Where(p => p.Name == "DCN").SingleOrDefault();
            string imgSrc = t.Image;
            string imgAlt = t.Name;
            ViewBag.ImageSource = imgSrc;
            ViewBag.ImageAlt = imgAlt;
            return View();
        }
        #endregion

    }
}
