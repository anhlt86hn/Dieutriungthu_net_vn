using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rico.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Objects;
using System.Data;
using System.IO;



namespace Rico.Controllers.Admins.Support
{
    public class SupportController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region[SupportIndex]
        public ActionResult Index(int? trang, string currentSupportName)
        {
            if (Request.HttpMethod == "GET")
            {
                if (Session["Support"] != null)
                {
                    currentSupportName = Session["Support"].ToString();
                    Session["Support"] = null;
                }
            }
            else
            {
                trang = 1;
            }

            ViewBag.CurrentSupportName = currentSupportName;

            var all = db.Supports.OrderBy(p => p.Ord).ToList();

            if (!String.IsNullOrEmpty(currentSupportName))
            {
                all = all.Where(p => p.Name.ToUpper().Contains(currentSupportName.ToUpper())).OrderBy(p => p.Ord).ToList();
            }

            int pageSize = 10;
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

            return View(all.ToPagedList(pageNumber, pageSize));
        }
        #endregion
        #region[SupportCreate]

        #region TypeSupport
        public List<ddl> TypeSupport()
        {
            List<ddl> type = new List<ddl>();
            type.Add(new ddl { value = "0", text = "Yahoo" });
            type.Add(new ddl { value = "1", text = "Skype" });
            type.Add(new ddl { value = "2", text = "Hotline" });
            type.Add(new ddl { value = "3", text = "Tel" });
            type.Add(new ddl { value = "4", text = "Mobile" });
            return type;
        }
        #endregion TypeSupport

        [HttpGet]
        public ActionResult Create()
        {
            //ViewBag.GroupSupport = new SelectList(db.GroupSupports, "Id", "Name");
            //ViewBag.Type = new SelectList(TypeSupport(), "value", "text");
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Rico.Models.Support supports)
        {
            if (Request.Cookies["Username"] != null)
            {
                var Name = collection["Name"];               
                var Tel = collection["Tel"];
                var Skype = collection["Skype"];               
                var Ord = Convert.ToInt32(collection["Ord"]);
                var Active = (collection["Active"] == "false") ? false : true;
                //var Location = collection["Location"];                         
                supports.Name = Name;              
                supports.Tel = Tel;           
                supports.Skype = Skype;
                supports.Ord = Ord;                
                supports.Active = Active;
                db.sp_Support_Insert(supports.Name, supports.Tel, supports.Skype, supports.Active, supports.Ord );
                //db.Supports.Add(supports);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

        #endregion SupportCreate

        #region Edit

        #region[SupportEdit_GET]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var supportEdit = db.Supports.SingleOrDefault(m => m.Id == id);          
            return View(supportEdit);
        }
        #endregion

        #region[SupportEdit_POST]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                var supports = db.Supports.First(model => model.Id == id);
                var Name = collection["Name"];
                var Tel = collection["Tel"];
                var Skype = collection["Skype"];                
                var Ord = Convert.ToInt32(collection["Ord"]);             
                var Active = (collection["Active"] == "false") ? false : true;
                supports.Name = Name;               
                supports.Tel = Tel;
                supports.Skype = Skype;
                supports.Ord = Ord;
                supports.Active = Active;
                //supports.Active = (collection["Active"] == "false") ? false : true;
                db.sp_Support_Update(supports.Id, supports.Name, supports.Tel, supports.Skype, supports.Active, supports.Ord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion
        #endregion Edit

        #region DeleteItem
        public ActionResult Delete(int id, int trang, int kichco)
        {
            if (Request.Cookies["Username"] != null)
            {
                //var del = (from support in db.Supports where support.Id == id select support).Single();
                var del = db.Supports.Where(sp => sp.Id == id).SingleOrDefault();
                db.Supports.Remove(del);
                db.SaveChanges();

                List<Rico.Models.Support> support = db.Supports.ToList();

                if ((support.Count % kichco) == 0)
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
        #endregion DeleteItem

        public ActionResult getNumberRecord(string numberRecord, int? trang)
        {
            int pageNumber = (trang ?? 1);
            int pageSize;

            if (numberRecord != null)
                pageSize = Convert.ToInt32(numberRecord);
            else
                pageSize = 3;


            
            List<SelectListItem> sllNumberSupport = new List<SelectListItem>();
            for (int i = 2; i < 10; i += 2)
            {
                if (i == pageSize)
                    sllNumberSupport.Add(new SelectListItem() { Text = i + " hỗ trợ viên", Value = i + "", Selected = true });
                else
                    sllNumberSupport.Add(new SelectListItem() { Text = i + " hỗ trợ viên", Value = i + "" });
            }

            PagedList<Rico.Models.Support> mSupports = (PagedList<Rico.Models.Support>)db.Supports.OrderByDescending(s => s.Id).ToPagedList(pageNumber, pageSize);
            ViewBag.sllSupport = sllNumberSupport;
            return PartialView("_Data", mSupports);
        }


        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collection)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (collection["btnDeleteAll"] != null)
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
                                var Del = (from emp in db.Supports where emp.Id == id select emp).SingleOrDefault();
                                db.Supports.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }
                else if (collection["btnSearch"] != null)
                {
                    if (collection["SupportName"].Length > 0)
                    {
                        Session["SupportName"] = collection["SupportName"];
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion


        #region[ChangeActive]
        // AJAX: /Picture/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var p = db.Supports.Find(id);
                if (p != null)
                {
                    p.Active = p.Active == true ? false : true;
                }
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

                var results = "Trạng thái đã được thay đổi.";
                return Json(results);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region[Ajax Change Support]

        // AJAX: /Support/ChangeSupport
        [HttpPost]
        public ActionResult UpdateDirect(int id, string ord, string name, string skype, string tel)
        {
            var results = "";
            var vSize = db.Supports.Find(id);
            if (vSize != null)
            {
                if (ord != null)
                {
                    vSize.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                if (name != null)
                {
                    vSize.Name = name;
                    results = "Tên Support đã được thay đổi.";
                }
                if (skype != null)
                {
                    vSize.Skype = skype;
                    results = "Skype Support đã được thay đổi.";
                }
                if (tel != null)
                {
                    vSize.Tel = tel;
                    results = "Điện thoại Support đã được thay đổi.";
                }
            }
            db.Entry(vSize).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }

        #endregion
        public class ddl
        {
            public string value { get; set; }
            public string text { get; set; }
        }

        //#region Backup

        //#region[SupportCreate_back]
        //public ActionResult SupportCreate_back()
        //{
        //    ViewBag.GroupSupport = new SelectList(db.GroupSupports, "Id", "Name");
        //    ViewBag.Type = new SelectList(TypeSupport(), "value", "text");
        //    return View();
        //}
        //#endregion
        //#region[SupportCreate_back]
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult SupportCreate_back(FormCollection collection, Rico.Models.Support supports)
        //{
        //    if (Request.Cookies["Username"] != null)
        //    {
        //        var Name = collection["Name"];
        //        var Tel = collection["Tel"];
        //        var Type = collection["Type"];
        //        var Nick = collection["Nick"];
        //        var Ord = collection["Order"];
        //        var Location = collection["Location"];
        //        var GroupSupport = collection["GroupSupport"];
        //        var Target = collection["Target"];
        //        supports.IdGroupSupport = Convert.ToInt32(GroupSupport);
        //        supports.Name = Name;
        //        supports.Type = int.Parse(Type);
        //        supports.Tel = Tel;
        //        supports.Nick = Nick;
        //        supports.Ord = Convert.ToInt32(Ord);
        //        supports.Target = Target;
        //        supports.Active = (collection["Active"] == "false") ? false : true;
        //        db.Supports.Add(supports);
        //        db.SaveChanges();
        //        return RedirectToAction("SupportIndex");
        //    }
        //    else
        //    {
        //        return Redirect("/Admins/admins");
        //    }
        //}
        //#endregion

        //#region[SupportEdit_back]
        //public ActionResult SupportEdit_back(int id)
        //{
        //    var Edit = db.Supports.First(m => m.Id == id);
        //    ViewBag.GroupSupport = new SelectList(db.GroupSupports, "Id", "Name", Edit.IdGroupSupport);
        //    ViewBag.Type = new SelectList(TypeSupport(), "value", "text", Edit.Type);
        //    return View(Edit);
        //}
        //#endregion
        //#region[SupportEdit_back]
        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult SupportEdit_back(int id, FormCollection collection)
        //{
        //    if (Request.Cookies["Username"] != null)
        //    {
        //        var supports = db.Supports.First(model => model.Id == id);
        //        var Name = collection["Name"];
        //        var Tel = collection["Tel"];
        //        var Type = collection["Type"];
        //        var Nick = collection["Nick"];
        //        var Ord = collection["Ord"];
        //        var Location = collection["Location"];
        //        var GroupSupport = collection["GroupSupport"];
        //        var Target = collection["Target"];
        //        supports.IdGroupSupport = Convert.ToInt32(GroupSupport);
        //        supports.Name = Name;
        //        supports.Type = int.Parse(Type);
        //        supports.Tel = Tel;
        //        supports.Nick = Nick;
        //        supports.Ord = Convert.ToInt32(Ord);
        //        supports.Target = Target;
        //        supports.Active = (collection["Active"] == "false") ? false : true;
        //        db.SaveChanges();
        //        return RedirectToAction("SupportIndex");
        //    }
        //    else
        //    {
        //        return Redirect("/Admins/admins");
        //    }
        //}
        //#endregion

        //#region[MultiDelete_back]
        //[HttpPost]
        //public ActionResult MultiDelete_back()
        //{
        //    if (Request.Cookies["Username"] != null)
        //    {
        //        foreach (string key in Request.Form)
        //        {
        //            var checkbox = "";
        //            if (key.StartsWith("chk"))
        //            {
        //                checkbox = Request.Form["" + key];
        //                if (checkbox != "false")
        //                {
        //                    Int32 id = Convert.ToInt32(key.Remove(0, 3));
        //                    var Del = (from emp in db.Supports where emp.Id == id select emp).SingleOrDefault();
        //                    db.Supports.Remove(Del);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //        return RedirectToAction("SupportIndex");
        //    }
        //    else
        //    {
        //        return Redirect("/Admins/admins");
        //    }
        //}
        //#endregion

        //#region[SupportIndex_back]
        //public ActionResult SupportIndex_back()
        //{
        //    string page = "1";//so phan trang hien tai
        //    var pagesize = 25;//so ban ghi tren 1 trang
        //    var numOfNews = 0;//tong so ban ghi co duoc truoc khi phan trang
        //    int curpage = 0; // trang hien tai dung cho phan trang
        //    if (Request["page"] != null)
        //    {
        //        page = Request["page"];
        //        curpage = Convert.ToInt32(page) - 1;
        //    }
        //    var all = db.Supports.ToList();
        //    var pages = all.Skip(curpage * pagesize).Take(pagesize).ToList();
        //    //var product = db.sp_Support_Phantrang(page, productize, "", "").ToList();
        //    var url = Request.Path;
        //    numOfNews = all.Count;
        //    ViewBag.Pager = Rico.Models.Phantrang.PhanTrang(25, curpage, numOfNews, url);
        //    return View(pages);
        //}


        ///// <summary>
        ///// SupportIndexot
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult SupportIndexot(string sortOrder, string sortName, string sortGroup, string sortDate, string sortSPTon, int? page)
        //{
        //    ViewBag.CurrentSortOrder = sortOrder;
        //    ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

        //    ViewBag.CurrentSortName = sortName;
        //    ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

        //    ViewBag.CurrentSortGroup = sortGroup;
        //    ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";

        //    ViewBag.CurrentSortDate = sortDate;
        //    ViewBag.SortDateParm = sortDate == "date asc" ? "date desc" : "date asc";

        //    ViewBag.CurrentSortSPTon = sortSPTon;
        //    ViewBag.SortSPTonParm = sortSPTon == "spton asc" ? "spton desc" : "spton asc";

        //    var all = db.sp_Support_GetByAll().OrderByDescending(support => support.Id).ToList();

        //    switch (sortOrder)
        //    {
        //        case "ord desc":
        //            all = all.OrderByDescending(support => support.Ord).ToList();
        //            break;
        //        case "ord asc":
        //            all = all.OrderBy(support => support.Ord).ToList();
        //            break;
        //        default:
        //            break;
        //    }

        //    switch (sortName)
        //    {
        //        case "name desc":
        //            all = all.OrderByDescending(support => support.Name).ToList();
        //            break;
        //        case "name asc":
        //            all = all.OrderBy(support => support.Name).ToList();
        //            break;
        //        default:
        //            break;
        //    }

        //    //switch (sortGroup)
        //    //{
        //    //    case "group desc":
        //    //        all = all.OrderByDescending(support => support.Expr1).ToList();
        //    //        break;
        //    //    case "group asc":
        //    //        all = all.OrderBy(support => support.Expr1).ToList();
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}

        //    //switch (sortDate)
        //    //{
        //    //    case "date desc":
        //    //        all = all.OrderByDescending(p => p.Date).ToList();
        //    //        break;
        //    //    case "date asc":
        //    //        all = all.OrderBy(p => p.Date).ToList();
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}

        //    //switch (sortSPTon)
        //    //{
        //    //    case "spton desc":
        //    //        all = all.OrderByDescending(p => p.SpTon).ToList();
        //    //        break;
        //    //    case "spton asc":
        //    //        all = all.OrderBy(p => p.SpTon).ToList();
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}


        //    if (Session["DeletePro"] != null)
        //    {
        //        var a = Session["DeletePro"].ToString();
        //        ViewBag.DelErr = "<p class='require'>" + a + "</p>";
        //        Session["DeletePro"] = null;
        //    }


        //    int pageSize = 2;
        //    int pageNumber = (page ?? 1);

        //    // Thiết lập phân trang
        //    PagedListRenderOptions pro = new PagedListRenderOptions();

        //    pro.DisplayLinkToFirstPage = PagedListDisplayMode.Always;
        //    pro.DisplayLinkToLastPage = PagedListDisplayMode.Always;


        //    pro.DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
        //    pro.DisplayLinkToNextPage = PagedListDisplayMode.Always;
        //    pro.DisplayLinkToIndividualPages = true;
        //    pro.DisplayPageCountAndCurrentLocation = false;
        //    pro.MaximumPageNumbersToDisplay = 5;
        //    pro.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
        //    pro.EllipsesFormat = "&#8230;";
        //    pro.LinkToFirstPageFormat = "Trang đầu";
        //    pro.LinkToPreviousPageFormat = "«";
        //    pro.LinkToIndividualPageFormat = "{0}";
        //    pro.LinkToNextPageFormat = "»";
        //    pro.LinkToLastPageFormat = "Trang cuối";
        //    pro.PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
        //    pro.ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
        //    pro.FunctionToDisplayEachPageNumber = null;
        //    pro.ClassToApplyToFirstListItemInPager = null;
        //    pro.ClassToApplyToLastListItemInPager = null;
        //    pro.ContainerDivClasses = new[] { "pagination-container" };
        //    pro.UlElementClasses = new[] { "pagination" };
        //    pro.LiElementClasses = Enumerable.Empty<string>();

        //    ViewBag.Pro = pro;

        //    return View(all.ToPagedList(pageNumber, pageSize));
        //}



        //#endregion

        //#endregion Backup
        //#endregion

      
        
    }
}
