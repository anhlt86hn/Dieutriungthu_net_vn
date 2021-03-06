﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Rico.Models;
using Rico.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;

namespace Rico.Controllers.Admin.Pictures
{
    public class NewsController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region Index
        public ActionResult Index(string sortOrder, string sortName, string sortGroup, string sortSDate, int? trang, string currentNewsCodeFilter, string currentNewsNameFilter, string Newsname, string currentGroupNewsFilter, string GroupNews, string currentNewsAmount)
        {

            if (Request.HttpMethod == "GET")
            {
                Newsname = currentNewsNameFilter;
                GroupNews = currentGroupNewsFilter;
                if (Session["NewsAmount"] != null)
                {
                    currentNewsAmount = Session["NewsAmount"].ToString();
                    Session["NewsAmount"] = null;
                }
            }
            else
            {
                trang = 1;
            }

            ViewBag.CurrentNewsNameFilter = Newsname;
            ViewBag.currentGroupNewsFilter = GroupNews;
            ViewBag.CurrentNewsAmount = currentNewsAmount;
           
            if (sortName != "")
            {
                ViewBag.CurrentSortName = sortName;
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            }
           
            if (Session["sortName"] != null)
            {
                sortName = Session["sortName"].ToString();
                ViewBag.CurrentSortName = Session["sortName"].ToString();
                Session["sortName"] = null;
            }
            if (sortOrder != "")
            {
                ViewBag.CurrentSortOrder = sortOrder;
                ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";
            }
          
            if (Session["sortOrder"] != null)
            {
                sortOrder = Session["sortOrder"].ToString();
                ViewBag.CurrentSortName = Session["sortOrder"].ToString();
                Session["sortOrder"] = null;
            }

            if (sortSDate != "")
            {
                ViewBag.CurrentSortSDate = sortSDate;
                ViewBag.SortSDateParm = sortSDate == "sdate asc" ? "sdate desc" : "sdate asc";
            }
          
            if (Session["sortSDate"] != null)
            {
                sortSDate = Session["sortSDate"].ToString();
                ViewBag.CurrentSortSDate = Session["sortSDate"].ToString();
                Session["sortSDate"] = null;
            }
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";
            ViewBag.SortSDateParm = sortSDate == "sdate asc" ? "sdate desc" : "sdate asc";
            ViewBag.CurrentSortGroup = sortGroup;
            ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";

            var all = db.sp_News_GetByAll().OrderByDescending(p => p.Id).ToList();           

            #region[Tìm kiếm]
            // Tìm theo tên Tin
            if (!String.IsNullOrEmpty(Newsname))
            {
                if (Newsname != "Tên Tin")
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(Newsname.ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
            }

            // Tìm theo Danh Mục Tin
            if (!String.IsNullOrEmpty(GroupNews))
            {
                int groupid = Int32.Parse(GroupNews);
                all = all.Where(p => p.IdGroup == groupid).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["Namesearch"] != null)
            {
               all = all.Where(p => p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
            }
            if (Session["GroupNews"] != null)
            {
                if (Session["Namesearch"] != null)
                {
                    all = all.Where(p => p.IdGroup == int.Parse(Session["GroupNews"].ToString()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                else
                {
                    all = all.Where(p => p.IdGroup == int.Parse(Session["GroupNews"].ToString())).OrderByDescending(p => p.Id).ToList();
                }
                Session["GroupNews"] = null;
            }
            if (Session["Namesearch"] != null) { Session["Namesearch"] = null; }
            #endregion

            #region[Sắp xếp]
            switch (sortOrder)
            {
                case "ord desc":
                    all = all.OrderByDescending(p => p.Ord).ToList();
                    break;
                case "ord asc":
                    all = all.OrderBy(p => p.Ord).ToList();
                    break;
                default:
                    break;
            }

            switch (sortName)
            {
                case "name desc":
                    all = all.OrderByDescending(p => p.Name).ToList();
                    break;
                case "name asc":
                    all = all.OrderBy(p => p.Name).ToList();
                    break;
                default:
                    break;
            }
            switch (sortSDate)
            {
                case "sdate desc":
                    all = all.OrderByDescending(p => p.SDate).ToList();
                    break;
                case "sdate asc":
                    all = all.OrderBy(p => p.SDate).ToList();
                    break;
                default:
                    break;
            }

            #endregion

            if (Session["DeletePro"] != null)
            {
                var a = Session["DeletePro"].ToString();
                ViewBag.DelErr = "<p class='require'>" + a + "</p>";
                Session["DeletePro"] = null;
            }

            int pageSize = 10;
            if (currentNewsAmount != null && currentNewsAmount != "")
            {
                pageSize = Convert.ToInt32(currentNewsAmount);
            }
            int pageNumber = (trang ?? 1);

            #region[Get Last Page]
            if (trang != null)
            {
                ViewBag.mPage = (int)trang;
            }
            else
            {
                ViewBag.mPage = 1;
            }


            int lastPage = all.Count / pageSize;
            if (all.Count % pageSize > 0)
            {
                lastPage++;
            }
            ViewBag.LastPage = lastPage;

            ViewBag.PageSize = pageSize;
            #endregion

            #region[view drop search]
            var cat = db.GroupNews.Where(n => n.Active == true).OrderBy(g => g.Level).ToList();
            if (cat.Count == 0)
            {
                cat.Add(new GroupNew { Level = "", Name = "" });
            }
            foreach (var item in cat)
            {
                item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.GroupNews = new SelectList(cat, "Id", "Name");
            }
            #endregion

            #region[Số Tin hiển thị trên trang]
            // Số Tin hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 10; i <= 100; i += 10)
            {
                if (i == pageSize)
                {
                    items.Add(new SelectListItem { Text = i + " Tin / trang", Value = i + "", Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = i + " Tin / trang", Value = i + "" });
                }
            }
            ViewBag.ddlNewsAmount = items;
            #endregion

            ViewBag.TotalNews = db.News.Count();

            // Phân trang
            PagedList<sp_News_GetByAll_Result> nn = (PagedList<sp_News_GetByAll_Result>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_Data", nn);

            return View(nn);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {

            //string chuoi = "";
            var cat = db.GroupNews.Where(n => n.Active == true).OrderBy(g => g.Level).ToList();
            if (cat.Count == 0)
            {
                cat.Add(new GroupNew { Level = "", Name = "" });
            }
            foreach (var item in cat)
            {
                item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }
            for (int i = 0; i < cat.Count; i++)
            {
                ViewBag.GroupNews = new SelectList(cat, "Id", "Name");
            }
            return View();
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, News news, HttpPostedFileBase fileImg)
        {
            int idmem = 0;
            if (Session["uId"] != null) { idmem = int.Parse(Session["uId"].ToString()); }
            if (Request.Cookies["Username"] != null)
            {
                news.Name = collection["Name"];
                news.Tag = StringClass.NameToTag(collection["Name"]);
                news.Image = collection["Image"];
                news.Content = collection["Content"];
                news.Detail = collection["Detail"];
                int GroupNews = Convert.ToInt32(collection["GroupNews"]);
                news.IdGroup = GroupNews;
                news.Keyword = collection["Keyword"];
                news.Description = collection["Description"];
                news.Title = collection["Title"];
                news.Ord = Convert.ToInt32(collection["Ord"]);
                news.Index = (collection["Index"] == "false") ? false : true;
                news.Active = (collection["Active"] == "false") ? false : true;
                //news.View = 0;//so luot xem
                news.SDate = System.DateTime.Now;
                //news.MDate = System.DateTime.Now;
                db.sp_News_Insert(news.Name, news.Title, news.Description, news.Keyword, news.Content, news.Detail, 
                    news.Tag, news.Image, news.SDate,
                    news.Index, news.Active, news.Ord, news.IdGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        #endregion
        #region Edit
        public ActionResult Edit(int id)
        {
            //string chuoi = "";
            var Edit = db.News.First(m => m.Id == id);
            var nId = Edit.Id;
            var cat = db.GroupNews.Where(n => n.Active == true).ToList();
            ViewBag.GroupNew = new SelectList(cat, "Id", "Name", Edit.IdGroup);
            ViewBag.NewsId = nId;
            return View(Edit);
        }
     
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                var nn = db.News.Find(id);
                string Name = collection["Name"];
                string Tag = StringClass.NameToTag(collection["Name"]);
                string Image = collection["Image"];
                string Content = collection["Content"];
                string Detail = collection["Detail"];
                int IdGroup = Convert.ToInt32(collection["GroupNew"]);
                string Keyword = collection["Keyword"];
                string Description = collection["Description"];
                string Title = collection["Title"];
                int Ord = Convert.ToInt32(collection["Ord"]);
                bool Index = (collection["Index"] == "false") ? false : true;
                bool Active = (collection["Active"] == "false") ? false : true;
                //int View = 0;//so luot xem
                DateTime SDate = nn.SDate;
                //DateTime SDate = DateTime.Now;
                db.sp_News_Update(id, Name, Title, Description, Keyword, Content, Detail, Tag, Image, SDate, Index, Active,   Ord, IdGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        #endregion


        #region Delete
        public ActionResult Delete(int id, int trang, int kichco)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                var del = db.News.First(p => p.Id == id);
                db.News.Remove(del);
                db.SaveChanges();
                List<News> news = db.News.ToList();
                if ((news.Count % kichco) == 0)
                {
                    if (trang > 1)
                    {
                        trang--;
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index", new { trang = trang });
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        #endregion   
        #region MultiCommand
        public ActionResult MultiCommand(FormCollection collect)
        {
            int m = int.Parse(collect["mPage"]);
            int kichco = int.Parse(collect["PageSize"]);

            List<News> news = db.News.ToList();
            int lastpage = news.Count / kichco;
            if (news.Count % kichco > 0)
            {
                lastpage++;
            }
            if (Request.Cookies["Username"] != null)
            {

                if (collect["btnDeleteAll"] != null)
                {
                    //string str = "";
                    foreach (string key in Request.Form)
                    {
                        var checkbox = "";
                        if (key.Contains("chk"))
                        {
                            if (key.Contains("chkIndex") || key.Contains("chkActive"))
                            {

                            }
                            else
                            {
                                checkbox = Request.Form["" + key];
                                if (checkbox != "false")
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 3));
                                    var Del = (from del in db.News where del.Id == id select del).SingleOrDefault();
                                    db.News.Remove(Del);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (m == 1)
                        {
                            return RedirectToAction("Index");
                        }

                        if (m == lastpage)
                        {
                            m--;
                        }
                    }
                    return RedirectToAction("Index", new { trang = m });
                }
                else if (collect["ddlNewsAmount"] != null)
                {
                    if (collect["ddlNewsAmount"].Length > 0)
                    {
                        Session["ddlNewsAmount"] = collect["ddlNewsAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else
                {
                    string Namesearch = collect["NewsName"];
                    string cat = collect["GroupNews"];
                    if (Namesearch.Length > 0)
                    {
                        Session["Namesearch"] = Namesearch;
                    }
                    if (cat.Length > 0)
                    {
                        Session["GroupNews"] = cat;
                    }
                    return RedirectToAction("Index", new { trang = m });
                }
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }
        #endregion

        #region[Ajax Autocomplete for search]
        // Autocomplete for search productcode 
        [HttpGet]
        public ActionResult NewsAutocomplete(string term)
        {
            var newsCodes = from p in db.News
                               select p.Name;
            string[] items = newsCodes.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            var news = db.News.Find(id);
            if (news != null)
            {
                news.Active = news.Active == true ? false : true;
            }
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(results);
        }
        #endregion

        #region ChangeIndex
        [HttpPost]
        public ActionResult ChangeIndex(int id)
        {
            var news = db.News.Find(id);
            if (news != null)
            {
                news.Index = news.Index == true ? false : true;
            }
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái hiển thị trang chủ đã được thay đổi.";
            return Json(results);
        }
        #endregion

        #region ChangeNews
        [HttpPost]
        public ActionResult UpdateDirect(int id, string name, string active, string ord, string tag)
        {
            var results = "";
            var news = db.News.Find(id);
            if (news != null)
            {
                if (name != null)
                {
                    news.Name = name;
                    results = "Tin đã được thay đổi.";
                }
                if (ord != null)
                {
                    news.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                if (tag != null)
                {
                    news.Tag = tag;
                    results = "Tag đã được thay đổi.";
                }
                //else
                //{
                //    news.Active = news.Active == true ? false : true;
                //    results = "Trạng thái tin đã được thay đổi.";
                //}
            }
            db.Entry(news).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
        #endregion

        #region LoadNewsAmount

        [HttpPost]
        public ActionResult LoadNewsAmount(string newsAmount, string sortName)
        {

            if (newsAmount != null)
            {
                Session["NewsAmount"] = newsAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Sort by Name
        [HttpPost]
        public ActionResult SortByName(string sortName, string currentNewsAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentNewsAmount != null)
                {
                    Session["NewsAmount"] = currentNewsAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Ord
        [HttpPost]
        public ActionResult SortByOrd(string sortOrder, string currentNewsAmount)
        {

            if (sortOrder != null)
            {
                Session["sortOrder"] = sortOrder;
                if (currentNewsAmount != null)
                {
                    Session["NewsAmount"] = currentNewsAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Ord
        [HttpPost]
        public ActionResult SortBySDate(string sortSDate, string currentNewsAmount)
        {

            if (sortSDate != null)
            {
                Session["sortSDate"] = sortSDate;
                if (currentNewsAmount != null)
                {
                    Session["NewsAmount"] = currentNewsAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
