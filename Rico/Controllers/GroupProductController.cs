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

namespace Rico.Controllers
{
    public class GroupProductController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        
        #region Index
        public ActionResult Index(string sortOrder, string sortName, int? trang, string sortSDate, 
         string currentGroupProductNameFilter, string groupproduct, 
         string currentCatGroupProductFilter, string catgroupproduct, 
         string currentGroupProductAmount)
        {
            if ((Request.Cookies["Username"] != null) )
            {     
                    if (Request.HttpMethod == "GET")
                    {
                        groupproduct = currentGroupProductNameFilter;
                        catgroupproduct = currentCatGroupProductFilter;
                        if (Session["GroupProductAmount"] != null)
                        {
                            currentGroupProductAmount = Session["GroupProductAmount"].ToString();
                            Session["GroupProductAmount"] = null;
                        }
                    }
                    else
                    {
                        trang = 1;
                    }

                    ViewBag.CurrentGroupProductNameFilter = groupproduct;
                    ViewBag.CurrentCatGroupProductFilter = catgroupproduct;
                    ViewBag.CurrentGroupProductAmount = currentGroupProductAmount;                  
                   
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
                    ViewBag.CurrentSortOrder = Session["sortOrder"].ToString();
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

                //var all = db.GroupNews.OrderByDescending(g => g.Ord).ToList();
                //var all = db.GroupProducts.OrderBy(g => g.Level).ToList();
                List<GroupProduct> lst = new List<GroupProduct>();
                lst= db.GroupProducts.OrderBy(g => g.Level).ToList();
                #region[Tìm kiếm]

                // Tìm theo tên sản phẩm
                if (!String.IsNullOrEmpty(groupproduct))
                {
                    if (groupproduct != "Tên Danh Mục sản phẩm")
                    {
                        lst = lst.Where(p => p.Name.ToUpper().Contains(groupproduct.ToUpper())).OrderByDescending(p => p.Id).ToList();
                        //all = all.Where(p => p.Name.ToUpper().Contains(groupproduct.ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                }

                //Tìm theo Danh Mục sản phẩm
                if (!String.IsNullOrEmpty(catgroupproduct))
                {
                    int catid = Int32.Parse(catgroupproduct);
                    //var parent = db.GroupProducts.Where(pa => pa.Id == catid).SingleOrDefault();
                                   
                    lst = db.GroupProducts.Where(p => p.ParentId==catid).OrderByDescending(p => p.Ord).ToList();
                    //all = all.Where(p => p.Id == catid).OrderByDescending(p => p.Id).ToList();
                }
               
                if (Session["GroupProductName"] != null)
                {
                    lst = lst.Where(p => p.Name.ToUpper().Contains(Session["GroupProductName"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                if (Session["Cat"] != null)
                {
                    if (Session["GroupProductName"] != null)
                    {
                        lst = lst.Where(p => p.ParentId == int.Parse(Session["Cat"].ToString()) && p.Name.ToUpper().Contains(Session["GroupProductName"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                    //else
                    //{
                    //    lst = lst.Where(p => p.ParentId == int.Parse(Session["Cat"].ToString())).OrderByDescending(p => p.Id).ToList();
                    //}
                    Session["Cat"] = null;
                }
                if (Session["GroupProductName"] != null) { Session["GroupProductName"] = null; }

                #endregion

                #region Order
               
                switch (sortOrder)
                {
                    case "ord desc":
                        lst = db.GroupProducts.OrderByDescending(a => a.Ord).ToList();
                        break;
                    case "ord asc":
                        lst = db.GroupProducts.OrderBy(a => a.Ord).ToList();
                        break;
                    default:
                        break;
                }

                switch (sortName)
                {
                    case "name desc":
                        lst = db.GroupProducts.OrderByDescending(a => a.Name).ToList();
                        break;
                    case "name asc":
                        lst = db.GroupProducts.OrderBy(a => a.Name).ToList();
                        break;
                    default:
                        break;
                }

                switch (sortSDate)
                {
                    case "sdate desc":
                        lst = db.GroupProducts.OrderByDescending(a => a.SDate).ToList();
                        break;
                    case "sdate asc":
                        lst = db.GroupProducts.OrderBy(a => a.SDate).ToList();
                        break;
                    default:
                        break;
                }

               
                #endregion Order

                int pageSize = 10;
                if (currentGroupProductAmount != null && currentGroupProductAmount != "")
                {
                    pageSize = Convert.ToInt32(currentGroupProductAmount);
                }
               
             int pageNumber = (trang ?? 1);

                #region[Last Page]
                //begin [get last page]
                if (trang != null)
            {
                ViewBag.mPage = (int)trang;
            }
            else
            {
                ViewBag.mPage = 1;
            }


            int lastPage = lst.Count / pageSize;
            if (lst.Count % pageSize > 0)
            {
                lastPage++;
            }
            ViewBag.LastPage = lastPage;

            ViewBag.PageSize = pageSize;
                //end [get last page]
                #endregion

                #region[view drop search]
                var cat = db.GroupProducts.Where(n => n.Active == true).OrderBy(n=>n.Level).ToList();                         
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
                        items.Add(new SelectListItem { Text = i + " Danh Mục sản phẩm / trang", Value = i + "", Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = i + " Danh Mục sản phẩm / trang", Value = i + "" });
                    }
                }
                ViewBag.ddlGroupProductAmount = items;
                #endregion
                ViewBag.TotalGroupProduct = db.GroupProducts.Count();


                #region PagedList
                PagedList<GroupProduct> gp = (PagedList<GroupProduct>)lst.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_Data", gp);
                #endregion

                return View(gp);
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
            if ((Request.Cookies["Username"] != null) )
            { 
            var gp = db.GroupProducts.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
            if (gp.Count == 0)
            {
                    gp.Add(new GroupProduct { Level = "", Name = "" });
            }

            foreach (var item in gp)
            {
                item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
            }

            for (int i = 0; i < gp.Count; i++)
            {
                ViewBag.GroupProduct = new SelectList(gp, "Id", "Name");
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
        public ActionResult Create(FormCollection collection, GroupProduct catego)
        {
            if ((Request.Cookies["Username"] != null) )
            { 
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupProduct"] != "0")
                {
                    if (collection["GroupProduct"] != "")
                    {
                        parentId = Int32.Parse(collection["GroupProduct"]);
                        string parentLevel = db.GroupProducts.Where(g => g.Id == parentId).SingleOrDefault().Level;
                        level = parentLevel + "00000";
                    }
                }
                catego.Level = level;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Detail = collection["Detail"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Image = collection["Image"];
                catego.SDate = System.DateTime.Now;
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.ParentId = parentId;
                db.sp_GroupProduct_Insert(catego.Name, catego.Detail, catego.Title, catego.Description, catego.Keyword, catego.Tag, catego.Image,
                    catego.Level, catego.SDate, catego.Index, catego.Priority, catego.Active, catego.Ord, catego.ParentId);
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

        #region[CreateSub]
        public ActionResult CreateSub(int id)
        {
            if ((Request.Cookies["Username"] != null))
            {
                var GroupProducts = db.GroupProducts.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
                //var GroupNews = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
                foreach (var item in GroupProducts)
                {
                    item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
                }

                for (int i = 0; i < GroupProducts.Count; i++)
                {
                    //if (GroupNews[i].Level.Length == 5)
                    //{
                    //    lstgr.Add(new SelectListItem { Text = StringClass.ShowNameLevel(GroupNews[i].Name, GroupNews[i].Level), Value = GroupNews[i].Level.ToString() });
                    //}
                    //else
                    //{
                    //    lstgr.Add(new SelectListItem { Text = StringClass.ShowNameLevel(GroupNews[i].Name, GroupNews[i].Level), Value = GroupNews[i].Level.ToString() });
                    //}
                    ViewBag.GroupProduct = new SelectList(GroupProducts, "Id", "Name", id);
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
        public ActionResult CreateSub(FormCollection collection, GroupProduct catego)
        {
            if ((Request.Cookies["Username"] != null))
            {
                // Lấy dữ liệu từ view
                int parentId = 0;
                string level = "00000";
                if (collection["GroupProduct"] != "")
                {
                    parentId = Int32.Parse(collection["GroupProduct"]);
                    string parentLevel = db.GroupProducts.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    level = parentLevel + "00000";
                }
                catego.Level = level;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.SDate = System.DateTime.Now;
                catego.Detail = collection["Detail"];
                catego.Image = collection["Image"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.ParentId = parentId;
                db.sp_GroupProduct_Insert(catego.Name, catego.Detail, catego.Title, catego.Description, catego.Keyword, catego.Tag, catego.Image,
                    catego.Level, catego.SDate, catego.Index, catego.Priority, catego.Active, catego.Ord, catego.ParentId);
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
            if ((Request.Cookies["Username"] != null) )
            { 
            var edit = db.GroupProducts.Find(id);
                var all = db.GroupProducts.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();

                foreach (var item in all)
                {
                    item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
                }
                //if (GroupNew.Level.Length > 5)
                //{
                //    string t = GroupNew.Level.Substring(0, GroupNew.Level.Length - 5);
                //    ViewBag.GroupNews = new SelectList(GroupNews, "Level", "Name", t);
                //}
                //else
                //{
                //    ViewBag.GroupNews = new SelectList(GroupNews, "Level", "Name");
                //}
                if (edit.Level.Length > 5)
                {
                    string t = edit.Level.Substring(0, edit.Level.Length - 5);
                    ViewBag.GroupProduct = new SelectList(all, "Level", "Name", t);
                }
                else
                {
                    ViewBag.GroupProduct = new SelectList(all, "Level", "Name");
                }

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
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {
                var catego = db.GroupProducts.Find(id);
                //catego.Name = collection["Name"];
                //m.Name = m.Name.Replace(".", "");
                //m.Ord = Convert.ToInt32(collection["Ord"]);
                //string le = collection["GroupProduct"];
                //m.Level = le + "00000";
                //m.Title = collection["Title"];
                //m.Description = collection["Description"];
                //m.Keyword = collection["Keyword"];
                //m.Image = collection["Image"];
                //m.Tag = Rico.Models.StringClass.NameToTag(collection["Name"]);
                ////m.Priority = (collection["Priority"] == "false") ? false : true;
                //m.Index = (collection["Index"] == "false") ? false : true;
                //m.Active = (collection["Active"] == "false") ? false : true;
                //db.sp_GroupProduct_Update(m.Id, m.Name, m.Title, m.Description, m.Keyword, m.Image, m.Tag, m.Level, m.Ord, m.Active, m.Index);
                // Lấy dữ liệu từ view
                              
                if (collection["GroupProduct"] != "")
                {
                    //int parentId = 0;
                    //string level = "00000";
                    string le = collection["GroupProduct"];
                    catego.Level = le + "00000";
                    var ch = db.GroupProducts.Where(v => v.Level == le).Single();
                    //int parentId = Int32.Parse(collection["GroupProduct"]);
                    //string parentLevel = db.GroupProducts.Where(g => g.Id == parentId).SingleOrDefault().Level;
                    //string level = parentLevel + "00000";
                    //catego.Level = level;
                    catego.ParentId = ch.Id;
                }
                else
                {
                    int parentId = 0;
                    catego.ParentId = parentId;
                }

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
                catego.Detail = collection["Detail"];
                catego.Image = collection["Image"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                db.sp_GroupProduct_Update(catego.Id, catego.Name, catego.Detail, catego.Title, catego.Description, catego.Keyword, catego.Tag, catego.Image,
                    catego.Level, catego.SDate, catego.Index, catego.Priority, catego.Active, catego.Ord, catego.ParentId);
                //catego.SDate = DateTime.Now;

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
        #region Delete
        public ActionResult Delete(int id, int trang, int kichco)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                //var del = (from categaa in db.GroupNews where categaa.Id == id select categaa).Single();
                var del = db.GroupProducts.First(p => p.Id == id);
                ViewBag.Id = del.Id;
                db.GroupProducts.Remove(del);
                db.SaveChanges();

                List<GroupProduct> gps = db.GroupProducts.ToList();

                if ((gps.Count % kichco) == 0)
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

            List<GroupProduct> gpp = db.GroupProducts.ToList();
            int lastpage = gpp.Count / kichco;
            if (gpp.Count % kichco > 0)
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
                                var Del = (from del in db.GroupProducts where del.Id == id select del).SingleOrDefault();
                                //if (Del.SpTon == 0)
                                //{
                                db.GroupProducts.Remove(Del);
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

                else if (collect["ddlGroupProductAmount"] != null)
                {
                    if (collect["ddlGroupProductAmount"].Length > 0)
                    {
                        Session["GroupProductAmount"] = collect["ddlGroupProductAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else if (collect["btnSearch"] != null)
                {
                    if (collect["GroupProductName"].Length > 0)
                    {
                        Session["GroupProductName"] = collect["GroupProductName"];
                    }
                    return RedirectToAction("Index", new { trang = trang });
                }
                else
                {
                    string Namesearch = collect["GroupProductName"];
                    string cat = collect["Cat"];
                    if (Namesearch.Length > 0)
                    {
                        Session["GroupProductName"] = Namesearch;
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
                    //        var Up = db.GroupProducts.Where(e => e.Id == id).FirstOrDefault();

                    //        if (Up != null)
                    //        {
                    //            if (!collect["Ord" + id].Equals(""))
                    //            {
                    //                Up.Ord = int.Parse(collect["Ord" + id]);
                    //            }

                    //            if (!collect["Active" + id].Equals(""))
                    //            {
                    //                bool Active = (collect["Active" + id] == "false") ? false : true;
                    //                Up.Active = Active;
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
                            var Del = (from emp in db.GroupProducts where emp.Id == id select emp).SingleOrDefault();
                            db.GroupProducts.Remove(Del);
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
            var gp = db.GroupProducts.Find(id);
            if (gp != null)
            {
                if (ord != null)
                {
                    gp.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                else if (name != null)
                {
                    gp.Name = name;
                    results = "Tên Danh Mục sản phẩm đã được thay đổi.";
                }
                else if(tag != null)
                {
                    gp.Tag = tag;
                    results = "Tag của Danh Mục sản phẩm đã được thay đổi.";
                }
            }
            db.Entry(gp).State = EntityState.Modified;
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
                var gp = db.GroupProducts.Find(id);
                if (gp != null)
                {
                    gp.Index = gp.Index == true ? false : true;
                }
                db.Entry(gp).State = EntityState.Modified;
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
        #region ChangeActive
        [HttpPost]
        public ActionResult ChangePriority(int id)
        {
            if ((Request.Cookies["Username"] != null) )
            {
                var gp = db.GroupProducts.Find(id);
                if (gp != null)
                {
                    gp.Priority = gp.Priority == true ? false : true;
                }
                db.Entry(gp).State = EntityState.Modified;
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
                var gp = db.GroupProducts.Find(id);
                if (gp != null)
                {
                    gp.Active = gp.Active == true ? false : true;
                }
                db.Entry(gp).State = EntityState.Modified;
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
        public ActionResult SortByName(string sortName, string currentGroupProductAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentGroupProductAmount != null)
                {
                    Session["GroupProductAmount"] = currentGroupProductAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region Sort by Ord
        [HttpPost]
        public ActionResult SortByOrd(string sortOrder, string currentGroupProductAmount)
        {

            if (sortOrder != null)
            {
                Session["sortOrder"] = sortOrder;
                if (currentGroupProductAmount != null)
                {
                    Session["GroupProductAmount"] = currentGroupProductAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region Sort by SDate
        [HttpPost]
        public ActionResult SortBySDate(string sortSDate, string currentGroupProductAmount)
        {

            if (sortSDate != null)
            {
                Session["sortSDate"] = sortSDate;
                if (currentGroupProductAmount != null)
                {
                    Session["GroupProductAmount"] = currentGroupProductAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region[Ajax LoadGroupProductAmount]
        // AJAX: /Product/LoadGroupProductAmount
        [HttpPost]
        public ActionResult LoadGroupProductAmount(string GroupProductAmount, string sortName)
        {

            if (GroupProductAmount != null)
            {
                Session["GroupProductAmount"] = GroupProductAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

    }
}
