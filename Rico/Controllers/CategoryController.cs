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
    public class CategoryController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();

        #region Index
        public ActionResult Index(string sortOrder, string sortName, int? trang, string currentCategoryNameFilter, string category, string currentGroupProductFilter, string groupproduct, string currentCategoryAmount)
        {
            if ((Request.Cookies["Username"] != null))
            {
                if (Request.HttpMethod == "GET")
                {
                    category = currentCategoryNameFilter;
                    groupproduct = currentGroupProductFilter;
                    if (Session["CategoryAmount"] != null)
                    {
                        currentCategoryAmount = Session["CategoryAmount"].ToString();
                        Session["CategoryAmount"] = null;
                    }
                }
                else
                {
                    trang = 1;
                }

                ViewBag.currentCategoryNameFilter = category;
                ViewBag.currentGroupProductFilter = groupproduct;
                ViewBag.currentCategoryAmount = currentCategoryAmount;

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
               
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
                ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";               

                //var all = db.GroupNews.OrderByDescending(g => g.Ord).ToList();
                //var all = db.Categorys.OrderBy(g => g.Level).ToList();
                List<Category> lst = new List<Category>();
                lst = db.Categories.OrderBy(g => g.Level).ToList();
                #region[Tìm kiếm]

                // Tìm theo tên sản phẩm
                if (!String.IsNullOrEmpty(category))
                {
                    if (category != "Tên Loại sản phẩm")
                    {
                        lst = lst.Where(p => p.Name.ToUpper().Contains(category.ToUpper())).OrderByDescending(p => p.Id).ToList();
                        //all = all.Where(p => p.Name.ToUpper().Contains(Category.ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                }

                //Tìm theo Loại sản phẩm
                if (!String.IsNullOrEmpty(groupproduct))
                {
                    int catid = Int32.Parse(groupproduct);
                    //var parent = db.Categorys.Where(pa => pa.Id == catid).SingleOrDefault();

                    lst = db.Categories.Where(p => p.IdGroupPro == catid).OrderByDescending(p => p.Ord).ToList();
                    //all = all.Where(p => p.Id == catid).OrderByDescending(p => p.Id).ToList();
                }

                if (Session["Namesearch"] != null)
                {
                    lst = lst.Where(p => p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                if (Session["GroupProduct"] != null)
                {
                    if (Session["Namesearch"] != null)
                    {
                        lst = lst.Where(p => p.IdGroupPro == int.Parse(Session["GroupProduct"].ToString()) && p.Name.ToUpper().Contains(Session["Namesearch"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                    else
                    {
                        lst = lst.Where(p => p.IdGroupPro == int.Parse(Session["GroupProduct"].ToString())).OrderByDescending(p => p.Id).ToList();
                    }
                    Session["GroupProduct"] = null;
                }
                if (Session["Namesearch"] != null) { Session["Namesearch"] = null; }

                #endregion

                #region Order

                switch (sortOrder)
                {
                    case "ord desc":
                        lst = db.Categories.OrderByDescending(a => a.Ord).ToList();
                        break;
                    case "ord asc":
                        lst = db.Categories.OrderBy(a => a.Ord).ToList();
                        break;
                    default:
                        break;
                }

                switch (sortName)
                {
                    case "name desc":
                        lst = db.Categories.OrderByDescending(a => a.Name).ToList();
                        break;
                    case "name asc":
                        lst = db.Categories.OrderBy(a => a.Name).ToList();
                        break;
                    default:
                        break;
                }
             
                #endregion Order

                int pageSize = 10;
                if (currentCategoryAmount != null && currentCategoryAmount != "")
                {
                    pageSize = Convert.ToInt32(currentCategoryAmount);
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
                var cat = db.GroupProducts.Where(n => n.Active == true).OrderBy(n => n.Level).ToList();
                for (int i = 0; i < cat.Count; i++)
                {
                    ViewBag.GroupProduct = new SelectList(cat, "Id", "Name");
                }

                #endregion
                #region[Số Loại sản phẩm hiển thị trên trang]
                // Số sản phẩm hiển thị trên trang
                List<SelectListItem> items = new List<SelectListItem>();
                for (int i = 10; i <= 100; i += 10)
                {
                    if (i == pageSize)
                    {
                        items.Add(new SelectListItem { Text = i + " Loại sản phẩm / trang", Value = i + "", Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = i + " Loại sản phẩm / trang", Value = i + "" });
                    }
                }
                ViewBag.ddlCategoryAmount = items;
                #endregion
                ViewBag.TotalCategory = db.Categories.Count();


                #region PagedList
                PagedList<Category> cate = (PagedList<Category>)lst.ToPagedList(pageNumber, pageSize);

                if (Request.IsAjaxRequest())
                    return PartialView("_Data", cate);
                #endregion

                return View(cate);
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

            List<Category> gpp = db.Categories.ToList();
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
                                var Del = (from del in db.Categories where del.Id == id select del).SingleOrDefault();
                                //if (Del.SpTon == 0)
                                //{
                                db.Categories.Remove(Del);
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

                else if (collect["ddlCategoryAmount"] != null)
                {
                    if (collect["ddlCategoryAmount"].Length > 0)
                    {
                        Session["CategoryAmount"] = collect["ddlCategoryAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else if (collect["btnSearch"] != null)
                {
                    if (collect["CategoryName"].Length > 0)
                    {
                        Session["Namesearch"] = collect["CategoryName"];
                    }
                    return RedirectToAction("Index", new { trang = trang });
                }
                else
                {
                    string Namesearch = collect["CategoryName"];
                    string cat = collect["GroupProduct"];
                    if (Namesearch.Length > 0)
                    {
                        Session["Namesearch"] = Namesearch;
                    }
                    if (cat.Length > 0)
                    {
                        Session["GroupProduct"] = cat;
                    }
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.Categories.Where(e => e.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Ord = int.Parse(collect["Ord" + id]);
                                }

                                if (!collect["Active" + id].Equals(""))
                                {
                                    bool Active = (collect["Active" + id] == "false") ? false : true;
                                    Up.Active = Active;
                                }

                                db.Entry(Up).State = EntityState.Modified;
                                db.SaveChanges();
                            }

                        }
                    }
                    return RedirectToAction("Index", new { trang = trang });
                }
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion
        #region[Ajax LoadCategoryAmount]
        // Ajax LoadCategoryAmount
        [HttpPost]
        public ActionResult LoadCategoryAmount(string CategoryAmount, string sortName)
        {

            if (CategoryAmount != null)
            {
                Session["CategoryAmount"] = CategoryAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region[CategoryCreate]
        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] != null))
            {
                var groupproduct = db.GroupProducts.ToList();
                for (int i = 0; i < groupproduct.Count; i++)
                {
                    ViewBag.GroupProduct = new SelectList(groupproduct, "Id", "Name");
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
        public ActionResult Create(FormCollection collection, Category catego)
        {
            if ((Request.Cookies["Username"] != null))
            {
                int parentId = 0;
                string level = "00000";                
                        parentId = Int32.Parse(collection["GroupProduct"]);
                        string parentLevel = db.GroupProducts.Where(g => g.Id == parentId).SingleOrDefault().Level;
                        level = parentLevel + "00000";  
                catego.Level = level;
                catego.Name = collection["Name"];
                catego.Title = collection["Title"];
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Detail = collection["Detail"];
                catego.Ord = Convert.ToInt32(collection["Ord"]);
                catego.Tag = StringClass.NameToTag(collection["Name"]);                
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.IdGroupPro = Convert.ToInt32(collection["GroupProduct"]);
                db.sp_Category_Insert(catego.Name, catego.Detail, catego.Title, catego.Description, catego.Keyword, catego.Tag, 
                    catego.Level, catego.Index, catego.Priority, catego.Active, catego.Ord, catego.IdGroupPro);
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
        #region[CategoryEdit]
        public ActionResult Edit(int id)
        {
            var Edit = db.Categories.First(m => m.Id == id);
            var groupproduct = db.GroupProducts.ToList();
            for (int i = 0; i < groupproduct.Count; i++)
            {
                ViewBag.GroupProduct = new SelectList(groupproduct, "Id", "Name", Edit.IdGroupPro);
            }
            return View(Edit);
        }
        #endregion
        #region[CategoryEdit]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var catego = db.Categories.First(model => model.Id == id);
                var Name = collection["Name"];
                var Detail = collection["Detail"];
                var Ord = collection["Ord"];
                catego.Tag = StringClass.NameToTag(Name);
                catego.Name = Name;
                catego.Ord = Convert.ToInt32(Ord);
                catego.Detail = Detail;
                //if (catego.Level.Length < 10)
                //{
                    catego.IdGroupPro = Convert.ToInt32(collection["GroupProduct"]);
                //}
                //else
                //{
                //    var grp = db.Categories.Where(m => m.Level == catego.Level.Substring(0, 5)).FirstOrDefault();
                //    catego.IdGroupPro = Convert.ToInt32(grp.Level);
                //}
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Title = collection["Title"];

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion
        #region[CategoryAddSub]
        public ActionResult CreateSub(int id)
        {
            if ((Request.Cookies["Username"] != null))
            {
                var cat = db.Categories.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
                //var GroupNews = db.GroupNews.Where(g => g.Active == true).OrderBy(g => g.Level).ToList();
                foreach (var item in cat)
                {
                    item.Name = Rico.Models.StringClass.ShowNameLevel(item.Name, item.Level);
                }

                for (int i = 0; i < cat.Count; i++)
                {
                    //if (GroupNews[i].Level.Length == 5)
                    //{
                    //    lstgr.Add(new SelectListItem { Text = StringClass.ShowNameLevel(GroupNews[i].Name, GroupNews[i].Level), Value = GroupNews[i].Level.ToString() });
                    //}
                    //else
                    //{
                    //    lstgr.Add(new SelectListItem { Text = StringClass.ShowNameLevel(GroupNews[i].Name, GroupNews[i].Level), Value = GroupNews[i].Level.ToString() });
                    //}
                    ViewBag.Category = new SelectList(cat, "Id", "Name", id);
                }
                return View();
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion
        #region[CategoryAddSub]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateSub(FormCollection collection, Category catego)
        {
            if (Request.Cookies["Username"] != null)
            {
                int parentId = 0;
                string level = "00000";
       
                    parentId = Int32.Parse(collection["Category"]);
                    var cate = db.Categories.Where(g => g.Id == parentId).SingleOrDefault();
                    string parentLevel = cate.Level;
                    level = parentLevel + "00000";
   
                catego.Level = level;
                var Name = collection["Name"];
                var Detail = collection["Detail"];
                var Ord = collection["Ord"];
                catego.Tag = StringClass.NameToTag(Name);
                catego.Name = Name;
                catego.Detail = Detail;
                catego.Ord = Convert.ToInt32(Ord);
                catego.IdGroupPro = Convert.ToInt32(cate.IdGroupPro);
                catego.Active = (collection["Active"] == "false") ? false : true;
                catego.Priority = (collection["Priority"] == "false") ? false : true;
                catego.Index = (collection["Index"] == "false") ? false : true;
                catego.Description = collection["Description"];
                catego.Keyword = collection["Keyword"];
                catego.Title = collection["Title"];
                //catego.Level = level + "00000";
                db.sp_Category_Insert(catego.Name, catego.Detail, catego.Title, catego.Description,
                catego.Keyword, catego.Tag, catego.Level, catego.Index, catego.Priority, catego.Active, 
                catego.Ord, catego.IdGroupPro);
                    
                //db.Categories.Add(catego);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion
        #region[CategoryDelete]
        public ActionResult Delete(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var del = (from categaa in db.Categories where categaa.Id == id select categaa).Single();
                db.Categories.Remove(del);
                db.SaveChanges();
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
        public ActionResult UpdateDirect(int id, string ord, string name, string tag)
        {
            var results = "";
            var gp = db.Categories.Find(id);
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
                    results = "Tên Loại sản phẩm đã được thay đổi.";
                }
                else if (tag != null)
                {
                    gp.Tag = tag;
                    results = "Tag của Loại sản phẩm đã được thay đổi.";
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
                var gp = db.Categories.Find(id);
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
            if ((Request.Cookies["Username"] != null))
            {
                var gp = db.Categories.Find(id);
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
                var gp = db.Categories.Find(id);
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
        public ActionResult SortByName(string sortName, string currentCategoryAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentCategoryAmount != null)
                {
                    Session["CategoryAmount"] = currentCategoryAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region Sort by Ord
        [HttpPost]
        public ActionResult SortByOrd(string sortOrder, string currentCategoryAmount)
        {

            if (sortOrder != null)
            {
                Session["sortOrder"] = sortOrder;
                if (currentCategoryAmount != null)
                {
                    Session["CategoryAmount"] = currentCategoryAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
      
  
        #region[MultiDelete]
        public ActionResult MultiDelete()
        {
            if (Request.Cookies["Username"] != null)
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
                            var Del = (from emp in db.Categories where emp.Id == id select emp).SingleOrDefault();
                            db.Categories.Remove(Del);
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



    }
}
