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

namespace Rico.Controllers.PictureView
{
    public class PictureViewController : Controller
    {
        DieutriungthuEntities data = new DieutriungthuEntities();
        #region[Danh sách]
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
            int pageSize = 12;
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

            var gp = data.GroupPictures.Where(r => r.Active == true && r.Tag == tag).SingleOrDefault();
            //var menu = data.Menus.Where(m => m.Active == true && m.Tag == tag).SingleOrDefault();
            if (gp != null)
            {
                //string rootMenuLevel = menu.Level;
                //string menuName = menu.Name;
                //string menuLink = menu.Link;
                string gpName = gp.Name;
                string gpTag = gp.Tag;
                ViewBag.GpName = gpName;
                ViewBag.GpTag = gpTag;
                int gid = gp.Id;
                //string str = "";

                //List<News> list = new List<News>();
                var list = data.GroupPictures.Where(n => n.Active == true && n.ParentId == gid).OrderBy(n => n.SDate).ToList();
                //list = list.OrderByDescending(l => l.SDate).Take(10).ToList();       

                #region[Title, Keyword, Deskription]
                if (gp.Title.Length > 0) { ViewBag.tit = gp.Title; } else { ViewBag.tit = gp.Name; }
                if (gp.Description.Length > 0) { ViewBag.des = gp.Description; } else { ViewBag.des = gp.Name; }
                if (gp.Keyword.Length > 0) { ViewBag.key = gp.Keyword; } else { ViewBag.key = gp.Name; }
                #endregion



                //ViewBag.Str = str;
                //ViewBag.MenuName = menuName;
                //ViewBag.MenuLink = menuLink;
                return View(list.ToPagedList(pageNumber, pageSize));                     
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region[2]
        public ActionResult List2(int? trang, string tag)
        {
            if (Request.HttpMethod == "GET")
            {
            }
            else
            {
                trang = 1;
            }

            #region[Phân trang]
            int pageSize = 12;
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

            var gp = data.GroupPictures.Where(r => r.Active == true && r.Tag == tag).SingleOrDefault();
            var menu = data.Menus.Where(m => m.Active == true && m.Tag == tag).SingleOrDefault();
            if (gp != null)
            {
                //string rootMenuLevel = menu.Level;
                string menuName = menu.Name;
                string menuLink = menu.Link;
                int gid = gp.Id;
                string str = "";

                //List<News> list = new List<News>();
                var list = data.GroupPictures.Where(n => n.Active == true && n.ParentId == gid).OrderBy(n => n.SDate).ToList();
                //list = list.OrderByDescending(l => l.SDate).Take(10).ToList();       

                #region[Title, Keyword, Deskription]
                if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
                if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
                if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
                #endregion

                //if(list==null)
                //{
                //    return RedirectToAction("Detail2", "PictureView", new { gpid = gp.Id });
                //}
                //else                    
                //{
                for (int i = 0; i < list.Count; i++)
                //for (int i = 0; i < list.ToPagedList(pageNumber, pageSize).Count; i++)
                {
                    int parId = list[i].Id;
                    var pi = data.Pictures.Where(m => m.IdCategory == parId).ToList();
                    if (pi == null)
                    {
                        var dcn = data.Pictures.Where(l => l.Name == "Đang cập nhật").Single();
                        str += "<div class=\"col-sx-12 col-sm-4 album-wrapper\"><a href=\"/anh/" + list[i].Tag + "\"><img class=\"img-responsive img-thumbnail\" src=\"" + dcn.Image + "\" alt=\"" + list[i].Name + "\" /></a></div>";
                    }
                    else
                    {
                        for (int k = 0; k < pi.Count; k++)
                        {
                            if (k == 0)
                            {
                                str += "<div class=\"col-sx-12 col-sm-4 album-wrapper\"><a href=\"/anh/" + list[i].Tag + "\"><img class=\"img-responsive img-thumbnail\" src=\"" + pi[k].Image + "\" alt=\"" + list[i].Name + "\" /></a></div>";
                            }
                        }
                    }
                }
                ViewBag.Str = str;
                ViewBag.MenuName = menuName;
                ViewBag.MenuLink = menuLink;
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region[Detail 2]
        public ActionResult Detail2(int? trang, int gpid)
        {
            if (Request.HttpMethod == "GET")
            {
            }
            else
            {
                trang = 1;
            }
            var root = data.GroupPictures.Where(s => s.Active == true && s.Id == gpid).Single();

            #region[Phân trang]
            int pageSize = 12;
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
            
            if (root != null)
            {
                string gpTag = root.Tag;
                var menu = data.Menus.Where(m => m.Active == true && m.Tag == gpTag).SingleOrDefault();
                string menuName = menu.Name;
                string menuTag = menu.Tag;
                //int gid = root.Id;
                //int parentid = gp.ParentId;

                //var parentgp = data.GroupPictures.Where(tt => tt.Active == true && tt.Id == parentid).Single();
                //string prName = parentgp.Name;
                //string prTag = parentgp.Tag;

                #region[Title, Keyword, Deskription]
                if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
                if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
                if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
                #endregion

                var list = data.Pictures.Where(n => n.Active == true && n.IdCategory == gpid).OrderBy(n => n.SDate).ToList();

                ViewBag.MenuName = menuName;
                ViewBag.MenuTag = menuTag;
                //ViewBag.PrName = prName;
                //ViewBag.PrTag = prTag;
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region[Detail]
        public ActionResult Detail(int? trang, string tag)
        {
            if (Request.HttpMethod == "GET")
            {
            }
            else
            {
                trang = 1;
            }

            #region[Phân trang]
            int pageSize = 12;
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

            var gp = data.GroupPictures.Where(r => r.Active == true && r.Tag == tag).SingleOrDefault();
            


            //string str = "";
            #region[Title, Keyword, Deskription]
            if (gp.Title.Length > 0) { ViewBag.tit = gp.Title; } else { ViewBag.tit = gp.Name; }
            if (gp.Description.Length > 0) { ViewBag.des = gp.Description; } else { ViewBag.des = gp.Name; }
            if (gp.Keyword.Length > 0) { ViewBag.key = gp.Keyword; } else { ViewBag.key = gp.Name; }
            #endregion
            if (gp != null && gp.Level.Length > 5 || gp.Level.Length==5)
            {
                //var menu = data.Menus.Where(m => m.Active == true && m.Tag == tag).SingleOrDefault();
                //string menuName = menu.Name;
                //string menuLink = menu.Link;
                string gpName = gp.Name;
                string gpTag = gp.Tag;
                ViewBag.GpName = gpName;
                ViewBag.GpTag = gpTag;

                //int parentid = gp.ParentId;
                int gid = gp.Id;
                //var parentgp = data.GroupPictures.Where(tt => tt.Active == true && tt.Id == parentid).Single();
                //string prName = parentgp.Name;
                //string prTag = parentgp.Tag;
                //ViewBag.PrName = prName;
                //ViewBag.PrTag = prTag;
                var list = data.Pictures.Where(n => n.Active == true && n.IdCategory == gid).OrderBy(n => n.SDate).ToList();

                //ViewBag.MenuName = menuName;
                //ViewBag.MenuLink = menuLink;
   
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            //else if(gp.Level.Length==5)
            //{
            //    int gid = gp.Id;
            //    var list = data.Pictures.Where(n => n.Active == true && n.IdCategory == gid).OrderBy(n => n.SDate).ToList();
            //    return View(list);
            //}
            else
            {
                return View();
            }
           
        }
        #endregion

        #region[Detail]
        public ActionResult ViewDetail(string tag, string item)
        {
           

            var gp = data.GroupPictures.Where(r => r.Active == true && r.Tag == tag).SingleOrDefault();
            var p = data.Pictures.Where(i => i.Active == true && i.Tag == item).Single();


            //string str = "";
            #region[Title, Keyword, Deskription]
            if (gp.Title.Length > 0) { ViewBag.tit = gp.Title; } else { ViewBag.tit = gp.Name; }
            if (gp.Description.Length > 0) { ViewBag.des = gp.Description; } else { ViewBag.des = gp.Name; }
            if (gp.Keyword.Length > 0) { ViewBag.key = gp.Keyword; } else { ViewBag.key = gp.Name; }
            #endregion
            if (gp != null && gp.Level.Length > 5 || gp.Level.Length == 5)
            {
                //var menu = data.Menus.Where(m => m.Active == true && m.Tag == tag).SingleOrDefault();
                //string menuName = menu.Name;
                //string menuLink = menu.Link;
                string gpName = gp.Name;
                string gpTag = gp.Tag;
                ViewBag.GpName = gpName;
                ViewBag.GpTag = gpTag;

                //int parentid = gp.ParentId;
                //int gid = gp.Id;
                //var parentgp = data.GroupPictures.Where(tt => tt.Active == true && tt.Id == parentid).Single();
                //string prName = parentgp.Name;
                //string prTag = parentgp.Tag;
                //ViewBag.PrName = prName;
                //ViewBag.PrTag = prTag;
                //var list = data.Pictures.Where(n => n.Active == true && n.IdCategory == gid).ToList();

                //ViewBag.MenuName = menuName;
                //ViewBag.MenuLink = menuLink;

                return View(p);
            }
            //else if(gp.Level.Length==5)
            //{
            //    int gid = gp.Id;
            //    var list = data.Pictures.Where(n => n.Active == true && n.IdCategory == gid).OrderBy(n => n.SDate).ToList();
            //    return View(list);
            //}
            else
            {
                return View();
            }

        }
        #endregion

        #region[404]
        public ActionResult Updating()
        {
            var t = data.Pictures.Where(p => p.Name == "DCN").SingleOrDefault();
            string imgSrc = t.Image;
            string imgAlt = t.Name;
            ViewBag.ImageSource = imgSrc;
            ViewBag.ImageAlt = imgAlt;
            return View();
        }
        #endregion

    }
}
