using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rico.Models;
using PagedList;
using PagedList.Mvc;
using System.Collections;
using System.Data;
using System.Data.Entity;
using Rico.ViewModels;

namespace Rico.Controllers.Admin.GroupNews
{
    public class GroupNewController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        
        #region Index
        public ActionResult Index(string sortOrder, string sortName, int? trang,
           string currentGroupNewsNameFilter, string groupnews,
           string currentCatGroupNewsFilter, string catgroupnews, 
           string currentGroupNewsAmount)
        {
            if ((Request.Cookies["Username"] != null) )
            {
                if (Request.HttpMethod == "GET")
                {
                    groupnews = currentGroupNewsNameFilter;
                    catgroupnews = currentCatGroupNewsFilter;
                    if (Session["GroupNewsAmount"] != null)
                    {
                        currentGroupNewsAmount = Session["GroupNewsAmount"].ToString();
                        Session["GroupNewsAmount"] = null;
                    }
                }
                else
                {
                    trang = 1;
                }

                ViewBag.CurrentGroupNewsNameFilter = groupnews;
                ViewBag.CurrentCatGroupNewsFilter = catgroupnews;
                ViewBag.CurrentGroupNewsAmount = currentGroupNewsAmount;
                ViewBag.CurrentSortOrder = sortOrder;
                ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

                // Sort Name (Truyền sortName khi phân trang)
                if (sortName != "")
                {
                    ViewBag.CurrentSortName = sortName;
                    ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
                }
                // Sort Name (Truyền sortName khi sắp xếp)
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
                // Sort Name (Truyền sortName khi sắp xếp)
                if (Session["sortOrder"] != null)
                {
                    sortOrder = Session["sortOrder"].ToString();
                    ViewBag.CurrentSortName = Session["sortOrder"].ToString();
                    Session["sortOrder"] = null;
                }
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
                ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";
                //ViewBag.CurrentSortGroup = sortGroup;
                //ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";
                //var all = db.GroupNews.OrderByDescending(g => g.Ord).ToList();
                var all = db.GroupNews.OrderBy(g => g.Level).ToList();

                #region[Tìm kiếm]

                // Tìm theo tên sản phẩm
                if (!String.IsNullOrEmpty(groupnews))
                {
                    if (groupnews != "Tên Danh Mục tin")
                    {
                        all = all.Where(p => p.Name.ToUpper().Contains(groupnews.ToUpper())).OrderByDescending(p => p.Id).ToList();
                        //all = all.Where(p => p.Name.ToUpper().Contains(groupproduct.ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                }

                //Tìm theo Danh Mục sản phẩm
                if (!String.IsNullOrEmpty(catgroupnews))
                {
                    int catid = Int32.Parse(catgroupnews);
                    //var parent = db.GroupNews.Where(pa => pa.Id == catid).SingleOrDefault();


                    all = db.GroupNews.Where(p => p.ParentId == catid).OrderByDescending(p => p.Ord).ToList();
                    //all = all.Where(p => p.Id == catid).OrderByDescending(p => p.Id).ToList();
                }

                if (Session["GroupNewsName"] != null)
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(Session["GroupNewsName"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                if (Session["Cat"] != null)
                {
                    if (Session["GroupNewsName"] != null)
                    {
                        all = all.Where(p => p.ParentId == int.Parse(Session["Cat"].ToString()) && p.Name.ToUpper().Contains(Session["GroupNewsName"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                    //else
                    //{
                    //    all = all.Where(p => p.ParentId == int.Parse(Session["Cat"].ToString())).OrderByDescending(p => p.Id).ToList();
                    //}
                    Session["Cat"] = null;
                }
                if (Session["GroupNewsName"] != null) { Session["GroupNewsName"] = null; }

                #endregion


                int pageSize = 10;
                if (currentGroupNewsAmount != null && currentGroupNewsAmount != "")
                {
                    pageSize = Convert.ToInt32(currentGroupNewsAmount);
                }
                int pageNumber = (trang ?? 1);
               
                #region GetLastPage
                //begin [get last page]
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
                //end [get last page]
                #endregion

                #region[view drop search]
                var cat = db.GroupNews.Where(n => n.Active == true).OrderBy(n => n.Level).ToList();                
                    for (int i = 0; i < cat.Count; i++)
                    {
                        ViewBag.Cat = new SelectList(cat, "Id", "Name");
                    }         
            #endregion

            #region[Số Danh Mục sản phẩm hiển thị trên trang]
            // Số sản phẩm hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
                for (int i = 10; i <= 100; i += 10)
                {
                    if (i == pageSize)
                    {
                        items.Add(new SelectListItem { Text = i + " Danh Mục tin / trang", Value = i + "", Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = i + " Danh Mục tin / trang", Value = i + "" });
                    }
                }
                ViewBag.ddlGroupNewsAmount = items;
                #endregion
                ViewBag.TotalGroupNews = db.GroupNews.Count();

                #region Order
                //ViewBag.CurrentSortOrder = sortOrder;
                //ViewBag.SortOrderParm = sortOrder == "ordAsc" ? "ordDesc" : "ordAsc";
                //ViewBag.CurrentSortName = sortName;
                //ViewBag.SortNameParm = sortName == "nameAsc" ? "nameDesc" : "nameAsc";

                switch (sortOrder)
                {
                    case "ord desc":
                        all = db.GroupNews.OrderByDescending(a => a.Ord).ToList();
                        break;
                    case "ord asc":
                        all = db.GroupNews.OrderBy(a => a.Ord).ToList();
                        break;
                    default:
                        break;
                }

                switch (sortName)
                {
                    case "name desc":
                        all = db.GroupNews.OrderByDescending(a => a.Name).ToList();
                        break;
                    case "name asc":
                        all = db.GroupNews.OrderBy(a => a.Name).ToList();
                        break;
                    default:
                        break;
                }
                #endregion Order

                #region PagedList
                PagedList<Rico.Models.GroupNew> GroupNews = (PagedList<Rico.Models.GroupNew>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_Data", GroupNews);
                #endregion

                return View(GroupNews);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] != null))
            { 
            var GroupNews = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
            if (GroupNews.Count == 0)
            {
                GroupNews.Add(new GroupNew { Level = "", Name = "" });
            }

            foreach (var item in GroupNews)
            {
                item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }

            for (int i = 0; i < GroupNews.Count; i++)
            {
                ViewBag.GroupNew = new SelectList(GroupNews, "Id", "Name");
            }
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
            return View();
        }
   
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, GroupNew catego)
        {
            if ((Request.Cookies["Username"] != null))
            { 
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupNew"] != "0")
                {
                    if (collection["GroupNew"] != "")
                    {
                        parentId = Int32.Parse(collection["GroupNew"]);
                        string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
                        level = parentLevel + "00000";
                    }

                }
                catego.Level = level;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Image = collection["Image"];
                catego.Detail = collection["Detail"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.ParentId = parentId;
                db.sp_GroupNews_Insert(catego.Name, catego.Title, catego.Description, catego.Keyword, catego.Detail,
                    catego.Tag, catego.Image, catego.Level, catego.Index, catego.Priority, catego.Active, catego.Ord, catego.ParentId);
                //db.Entry(catego).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

        public ActionResult CreateSub(int id)
        {
            if ((Request.Cookies["Username"] != null) )
            {
                //var GroupNews = db.GroupNews.OrderBy(g => g.Level).ToList();
                var GroupNews = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
                foreach (var item in GroupNews)
                {
                    item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
                }

                for (int i = 0; i < GroupNews.Count; i++)
                {
                    //if (GroupNews[i].Level.Length == 5)
                    //{
                    //    lstgr.Add(new SelectListItem { Text = StringClass.ShowNameLevel(GroupNews[i].Name, GroupNews[i].Level), Value = GroupNews[i].Level.ToString() });
                    //}
                    //else
                    //{
                    //    lstgr.Add(new SelectListItem { Text = StringClass.ShowNameLevel(GroupNews[i].Name, GroupNews[i].Level), Value = GroupNews[i].Level.ToString() });
                    //}
                    ViewBag.GroupNew = new SelectList(GroupNews, "Id", "Name", id);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateSub(FormCollection collection, GroupNew catego)
        {
            if ((Request.Cookies["Username"] != null))
            {
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupNew"] != "")
                {
                    parentId = Int32.Parse(collection["GroupNew"]);
                    string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    level = parentLevel + "00000";
                }
                catego.Level = level;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Image = collection["Image"];
                catego.Detail = collection["Detail"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.ParentId = parentId;
                db.sp_GroupNews_Insert(catego.Name, catego.Title, catego.Description, catego.Keyword, catego.Detail,
                    catego.Tag, catego.Image, catego.Level, catego.Index, catego.Priority, catego.Active, catego.Ord, catego.ParentId);
                //db.Entry(catego).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            if ((Request.Cookies["Username"] != null))
            { 
            var edit = db.GroupNews.Find(id);
                //int idGn = GroupNew.Id;
                //ViewBag.Id = idGn;
                //var GroupNews = db.GroupNews.OrderBy(g => g.Level).ToList();
                var all = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();

                foreach (var item in all)
            {
                item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);

            }
                if (edit.Level.Length > 5)
                {
                    string t = edit.Level.Substring(0, edit.Level.Length - 5);
                    ViewBag.GroupNew = new SelectList(all, "Level", "Name", t);
                }
                else
                {
                    ViewBag.GroupNew = new SelectList(all, "Level", "Name");
                }

                //for (int i = 0; i < GroupNews.Count; i++)
                //{
                //    ViewBag.GroupNew = new SelectList(GroupNews, "Id", "Name", GroupNew.Level);
                //}
                return View(edit);
            }
             else
             {
                 return RedirectToAction("Default", "Admin");
             }          
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if ((Request.Cookies["Username"] != null))
            {
                var catego = db.GroupNews.Find(id);
                //var ob = db.GroupNews.Find(id);
                //m.Name = collection["Name"];
                //m.Name = m.Name.Replace(".", "");
                //m.Ord = Convert.ToInt32(collection["Ord"]);
                //string le = collection["GroupPost"];
                //m.Level = le + "00000";
                //m.Title = collection["Title"];
                //m.Description = collection["Description"];
                //m.Keyword = collection["Keyword"];
                //m.Tag = Rico.Models.StringClass.NameToTag(collection["Name"]);
                //m.Priority = (collection["Priority"] == "false") ? false : true;
                //m.Index = (collection["Index"] == "false") ? false : true;
                //m.Active = (collection["Active"] == "false") ? false : true;
                //db.sp_GroupNews_Update(m.Id, m.Name, m.Title, m.Description, m.Keyword, m.Tag, m.Level, m.Ord, m.Active, m.Index, m.Priority);
                // Lấy dữ liệu từ view

                //int parentId = 0;
                //string level = "00000";
                //if (collection["GroupNew"] != "")
                //{
                    //    parentId = Int32.Parse(collection["GroupNew"]);
                    //    string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    //    level = parentLevel + "00000";
                    //}
                    if (collection["GroupNew"] != "")
                {
                    //int parentId = 0;
                    //string level = "00000";
                    string le = collection["GroupNew"];
                    catego.Level = le + "00000";
                    var ch = db.GroupNews.Where(v => v.Level == le).Single();
                    //int parentId = Int32.Parse(collection["GroupPicture"]);
                    //string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    //string level = parentLevel + "00000";
                    //catego.Level = level;
                    catego.ParentId = ch.Id;
                }

                //catego.Level = level;                        
                //if (catego.Level.Length > 5)
                //{
                //    catego.Name = name.Substring(catego.Level.Length - 5, name.Length - (catego.Level.Length - 5));
                //}
                //else
                //{
                catego.Name = collection["Name"];
                catego.Name = catego.Name.Replace(".", "");
                //}
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Image = collection["Image"];
                catego.Detail = collection["Detail"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;

                db.sp_GroupNews_Update(catego.Id, catego.Name, catego.Title, catego.Description, catego.Keyword, catego.Detail,
                    catego.Tag, catego.Image, catego.Level, catego.Index, catego.Priority, catego.Active, catego.Ord, catego.ParentId);
                //db.Entry(catego).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion


        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult Edit(int id, FormCollection collection, Rico.Models.GroupNew m)
        //{
        //    if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
        //    {
        //        var ob = db.GroupNews.Find(id);
        //        m.Name = collection["Name"];
        //        m.Name = m.Name.Replace(".", "");
        //        m.Ord = Convert.ToInt32(collection["Ord"]);
        //        string le = collection["GroupPost"];
        //        m.Level = le + "00000";
        //        m.Title = collection["Title"];
        //        m.Description = collection["Description"];
        //        m.Keyword = collection["Keyword"];
        //        m.Tag = Rico.Models.StringClass.NameToTag(collection["Name"]);
        //        m.Priority = (collection["Priority"] == "false") ? false : true;
        //        m.Index = (collection["Index"] == "false") ? false : true;
        //        m.Active = (collection["Active"] == "false") ? false : true;
        //        db.sp_GroupNews_Update(m.Id, m.Name, m.Title, m.Description, m.Keyword, m.Tag, m.Level, m.Ord, m.Active, m.Index, m.Priority);
        //        // Lấy dữ liệu từ view
        //        //int parentId = 0;
        //        //string level = "00000";
        //        //if (collection["GroupNew"] != "")
        //        //{
        //        //    parentId = Int32.Parse(collection["GroupNew"]);
        //        //    string parentLevel = db.GroupNews.Where(g => g.Id == parentId).SingleOrDefault().Level;
        //        //    level = parentLevel + "00000";
        //        //}
        //        //catego.Level = level;
        //        //catego.Name = collection["Name"];
        //        ////if (catego.Level.Length > 5)
        //        ////{
        //        ////    catego.Name = name.Substring(catego.Level.Length - 5, name.Length - (catego.Level.Length - 5));
        //        ////}
        //        ////else
        //        ////{
        //        ////    catego.Name = name;
        //        ////}
        //        //catego.Title = collection["Title"];
        //        //catego.Description = collection["Description"];
        //        //catego.Keyword = collection["Keyword"];
        //        //catego.Ord = Convert.ToInt32(collection["Ord"]);
        //        //catego.Tag = StringClass.NameToTag(collection["Name"]);
        //        //catego.Priority = (collection["Priority"] == "false") ? false : true;
        //        //catego.Index = (collection["Index"] == "false") ? false : true;
        //        //catego.Active = (collection["Active"] == "false") ? false : true;


        //        //db.Entry(catego).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return RedirectToAction("Default", "Admin");
        //    }
        //}
        //#endregion

        #region Delete
        public ActionResult Delete(int id, int trang, int kichco)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                //var del = (from categaa in db.GroupNews where categaa.Id == id select categaa).Single();
                var del = db.GroupNews.First(p => p.Id == id);
                ViewBag.Id = del.Id;
                //db.sp_GroupNews_Delete(del.Id);
                db.GroupNews.Remove(del);
                db.SaveChanges();

                List<GroupNew> GroupNews = db.GroupNews.ToList();

                if ((GroupNews.Count % kichco) == 0)
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
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region MultiCommand
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect)
        {
            int trang = int.Parse(collect["mPage"]);
            int kichco = int.Parse(collect["PageSize"]);

            List<GroupNew> GroupNews = db.GroupNews.ToList();
            int lastpage = GroupNews.Count / kichco;
            if (GroupNews.Count % kichco > 0)
            {
                lastpage++;
            }
            //int lastPage = int.Parse(collect["LastPage"]);

            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {

                if (collect["btnDeleteAll"] != null)
                {
                    //string str = "";
                    foreach (string key in Request.Form)
                    {
                        var checkbox = "";
                        if (key.Contains("chk"))
                        {
                            checkbox = Request.Form["" + key];
                            if (checkbox != "false")
                            {
                                int id = Convert.ToInt32(key.Remove(0, 3));
                                var Del = (from del in db.GroupNews where del.Id == id select del).SingleOrDefault();
                                //if (Del.SpTon == 0)
                                //{
                                db.GroupNews.Remove(Del);
                                db.SaveChanges();
                                //}
                                //else
                                //{
                                //    str += Del.Name + ",";
                                //    Session["DeletePro"] = "Sản phẩm " + str + "  vẫn còn trong kho! Không được xóa!";
                                //}
                            }
                        }
                    }

                    if (collect["checkAll"] != null)
                    {
                        if (trang == 1)
                        {
                            return RedirectToAction("Index");
                        }

                        if (trang == lastpage)
                        {
                            trang--;
                        }
                    }
                    return RedirectToAction("Index", new { trang = trang });
                }

                else if (collect["ddlGroupNewsAmount"] != null)
                {
                    if (collect["ddlGroupNewsAmount"].Length > 0)
                    {
                        Session["GroupNewsAmount"] = collect["ddlGroupNewsAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else if (collect["btnSearch"] != null)
                {
                    if (collect["GroupNewsName"].Length > 0)
                    {
                        Session["GroupNewsName"] = collect["GroupNewsName"];
                    }
                    return RedirectToAction("Index", new { trang = trang });
                }

                else
                {
                    string Namesearch = collect["GroupNewsName"];
                    string cat = collect["Cat"];
                    if (Namesearch.Length > 0)
                    {
                        Session["GroupNewsName"] = Namesearch;
                    }
                    if (cat.Length > 0)
                    {
                        Session["Cat"] = cat;
                    }
                    //foreach (string key in Request.Form)
                    //{
                    //    if (key.StartsWith("Ord"))
                    //    {
                    //        Int32 id = Convert.ToInt32(key.Remove(0, 3));
                    //        var Up = db.GroupNews.Where(e => e.Id == id).FirstOrDefault();

                    //        if (Up != null)
                    //        {
                    //            if (!collect["Ord" + id].Equals(""))
                    //            {
                    //                Up.Ord = int.Parse(collect["Ord" + id]);
                    //            }

                    //            db.Entry(Up).State = EntityState.Modified;
                    //            db.SaveChanges();
                    //        }

                    //    }
                    //}
                    return RedirectToAction("Index", new { trang = trang });
                }
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region MultiDelete
        public ActionResult MultiDelete()
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                foreach (string key in Request.Form)
                {
                    var checkbox = "";
                    if (key.StartsWith("chk"))
                    {
                        checkbox = Request.Form["" + key];
                        if (checkbox != "false")
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Del = (from emp in db.GroupNews where emp.Id == id select emp).SingleOrDefault();
                            db.GroupNews.Remove(Del);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region UpdateDirect
        [HttpPost]
        public ActionResult UpdateDirect(int id, string ord, string name,string tag)
        {
            var results = "";
            var GroupNew = db.GroupNews.Find(id);
            if (GroupNew != null)
            {
                if (ord != null)
                {
                    GroupNew.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                else if (name != null)
                {
                    GroupNew.Name = name;
                    results = "Tên Danh Mục Tin đã được thay đổi.";
                }
                else if (tag != null)
                {
                    GroupNew.Tag = tag;
                    results = "Tag của Danh Mục Tin đã được thay đổi.";
                }
            }
            db.Entry(GroupNew).State = EntityState.Modified;
            db.SaveChanges();
            return Json(results);
        }
        #endregion

        #region ChangeIndex
        [HttpPost]
        public ActionResult ChangeIndex(int id)
        {
            if ((Request.Cookies["Username"] != null))
            {
                var GroupNew = db.GroupNews.Find(id);
                if (GroupNew != null)
                {
                    GroupNew.Index = GroupNew.Index == true ? false : true;
                }
                db.Entry(GroupNew).State = EntityState.Modified;
                db.SaveChanges();

                var results = "Hiện ở trang chủ đã được thay đổi.";
                return Json(results);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region ChangePriority
        [HttpPost]
        public ActionResult ChangePriority(int id)
        {
            if ((Request.Cookies["Username"] != null))
            {
                var GroupNew = db.GroupNews.Find(id);
                if (GroupNew != null)
                {
                    GroupNew.Priority = GroupNew.Priority == true ? false : true;
                }
                db.Entry(GroupNew).State = EntityState.Modified;
                db.SaveChanges();

                var results = "Ưu tiên đã được thay đổi.";
                return Json(results);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            if ((Request.Cookies["Username"] != null))
            {
                var GroupNew = db.GroupNews.Find(id);
                if (GroupNew != null)
                {
                    GroupNew.Active = GroupNew.Active == true ? false : true;
                }
                db.Entry(GroupNew).State = EntityState.Modified;
                db.SaveChanges();

                var results = "Trạng thái kích hoạt đã được thay đổi.";
                return Json(results);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion


        #region Sort by Name
        [HttpPost]
        public ActionResult SortByName(string sortName, string currentGroupNewsAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentGroupNewsAmount != null)
                {
                    Session["GroupNewsAmount"] = currentGroupNewsAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Ord
        [HttpPost]
        public ActionResult SortByOrd(string sortOrder, string currentGroupNewsAmount)
        {

            if (sortOrder != null)
            {
                Session["sortOrder"] = sortOrder;
                if (currentGroupNewsAmount != null)
                {
                    Session["GroupNewsAmount"] = currentGroupNewsAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region[Ajax LoadGroupNewsAmount]
        // AJAX: /Product/LoadGroupNewsAmount
        [HttpPost]
        public ActionResult LoadGroupNewsAmount(string groupnewsAmount, string sortName)
        {

            if (groupnewsAmount != null)
            {
                Session["GroupNewsAmount"] = groupnewsAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region ShowPosition
        public static List<DropDownList> ShowPosition()
        {
            List<DropDownList> list = new List<DropDownList>();
            list.Add(new DropDownList { value = "0", text = "-- Danh mục tin --" });          
            return list;
        }
        #endregion
    }
}
