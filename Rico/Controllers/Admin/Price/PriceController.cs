using System;
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

namespace Rico.Controllers.Admin.Price
{
    public class PriceController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region Index
        public ActionResult Index(string sortOrder, string sortName, string sortSDate, int? trang, string currentPriceAmount)
        {
            if ((Request.Cookies["Username"] != null))
            {
                if (Request.HttpMethod == "GET")
            {               
                if (Session["PriceAmount"] != null)
                {
                    currentPriceAmount = Session["PriceAmount"].ToString();
                    Session["PriceAmount"] = null;
                }
            }
            else
            {
                trang = 1;
            }

            //ViewBag.CurrentPriceNameFilter = Pricename;
            //ViewBag.currentGroupPriceFilter = GroupPrice;
            ViewBag.CurrentPriceAmount = currentPriceAmount;
            //ViewBag.CurrentSortOrder = sortOrder;
            //ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";

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

            if (sortSDate != "")
            {
                ViewBag.CurrentSortSDate = sortSDate;
                ViewBag.SortSDateParm = sortSDate == "sdate asc" ? "sdate desc" : "sdate asc";
            }
            // Sort Name (Truyền sortName khi sắp xếp)
            if (Session["sortSDate"] != null)
            {
                sortSDate = Session["sortSDate"].ToString();
                ViewBag.CurrentSortSDate = Session["sortSDate"].ToString();
                Session["sortSDate"] = null;
            }
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            ViewBag.SortOrderParm = sortOrder == "ord asc" ? "ord desc" : "ord asc";
            ViewBag.SortSDateParm = sortSDate == "sdate asc" ? "sdate desc" : "sdate asc";
            //ViewBag.CurrentSortGroup = sortGroup;
            //ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";

            var all = db.Prices.OrderByDescending(p => p.Id).ToList();
            //var all = db.sp_Price_GetByAll().OrderByDescending(n => n.Ord).ToList();
         
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

            

            int pageSize = 10;
            if (currentPriceAmount != null && currentPriceAmount != "")
            {
                pageSize = Convert.ToInt32(currentPriceAmount);
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

           
            #region[Số Tin hiển thị trên trang]
            // Số Tin hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 10; i <= 100; i += 10)
            {
                if (i == pageSize)
                {
                    items.Add(new SelectListItem { Text = i + " Báo giá / trang", Value = i + "", Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = i + " Báo giá / trang", Value = i + "" });
                }
            }
            ViewBag.ddlPriceAmount = items;
            #endregion

            ViewBag.TotalPrice = db.Prices.Count();

            // Phân trang
            PagedList<Rico.Models.Price> nn = (PagedList<Rico.Models.Price>)all.ToPagedList(pageNumber, pageSize);

            if (Request.IsAjaxRequest())
                return PartialView("_Data", nn);

            return View(nn);
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
                return View();
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Rico.Models.Price price)
        {
            int idmem = 0;
            if (Session["uId"] != null) { idmem = int.Parse(Session["uId"].ToString()); }
            if (Request.Cookies["Username"] != null)
            {
                price.Name = collection["Name"];
                price.Tag = StringClass.NameToTag(collection["Name"]);
                price.Quote = collection["Quote"];
                price.Promotion1 = collection["Promotion1"];
                price.Promotion2 = collection["Promotion2"];
                price.Album = collection["Album"];
                price.BigPhoto = collection["BigPhoto"];
                price.Dress = collection["Dress"];            
                price.Active = (collection["Active"] == "false") ? false : true;
                price.Detail = collection["Detail"];                  
                price.Ord = Convert.ToInt32(collection["Ord"]);               
                //price.View = 0;//so luot xem
                price.SDate = System.DateTime.Now;
                //price.MDate = System.DateTime.Now;
                db.sp_Price_Insert(price.Name, price.Tag, price.Quote, price.Promotion1, price.Promotion2, price.Album, price.BigPhoto, price.Dress, price.Active,  price.SDate,  price.Detail, price.Ord);
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
                //string chuoi = "";
                var Edit = db.Prices.First(m => m.Id == id);
                return View(Edit);
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
            if (Request.Cookies["Username"] != null)
            {
                var nn = db.Prices.Find(id);
                string Name = collection["Name"];
                string Tag = StringClass.NameToTag(collection["Name"]);
                string Quote = collection["Quote"];
                string Promotion1 = collection["Promotion1"];
                string Promotion2 = collection["Promotion2"];
                string Album = collection["Album"];
                string BigPhoto = collection["BigPhoto"];
                string Dress = collection["Dress"];
                bool Active = (collection["Active"] == "false") ? false : true;
                string Detail = collection["Detail"];             
                int Ord = Convert.ToInt32(collection["Ord"]);                
                DateTime SDate = nn.SDate;
                //DateTime SDate = DateTime.Now;
                db.sp_Price_Update(id, Name, Tag, Quote, Promotion1, Promotion2, Album, BigPhoto, Dress, Active, SDate, Detail, Ord);
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
                var del = db.Prices.First(p => p.Id == id);
                db.Prices.Remove(del);
                db.SaveChanges();
                List<Rico.Models.Price> pp = db.Prices.ToList();
                if ((pp.Count % kichco) == 0)
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
        public ActionResult MultiCommand(FormCollection collect)
        {
            int m = int.Parse(collect["mPage"]);
            int kichco = int.Parse(collect["PageSize"]);

            List<Rico.Models.Price> pr = db.Prices.ToList();
            int lastpage = pr.Count / kichco;
            if (pr.Count % kichco > 0)
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
                                    var Del = (from del in db.Prices where del.Id == id select del).SingleOrDefault();
                                    db.Prices.Remove(Del);
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
                else if (collect["ddlPriceAmount"] != null)
                {
                    if (collect["ddlPriceAmount"].Length > 0)
                    {
                        Session["ddlPriceAmount"] = collect["ddlPriceAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else
                {                   
                    return RedirectToAction("Index", new { trang = m });
                }
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region[Ajax Autocomplete for search]
        // Autocomplete for search productcode 
        [HttpGet]
        public ActionResult PriceAutocomplete(string term)
        {
            var newsCodes = from p in db.Prices
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
            var pr = db.Prices.Find(id);
            if (pr != null)
            {
                pr.Active = pr.Active == true ? false : true;
            }
            db.Entry(pr).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(results);
        }
        #endregion

     
        #region UpdateDirect
        [HttpPost]
        public ActionResult UpdateDirect(int id, string name, string active, string ord, string tag, string quote)
        {
            var results = "";
            var pr = db.Prices.Find(id);
            if (pr != null)
            {
                if (name != null)
                {
                    pr.Name = name;
                    results = "Tên đã được thay đổi.";
                }
                if (ord != null)
                {
                    pr.Ord = Int32.Parse(ord);
                    results = "Thứ tự đã được thay đổi.";
                }
                if (quote != null)
                {
                    pr.Quote = quote;
                    results = "Báo giá đã được thay đổi.";
                }
                if (tag != null)
                {
                    pr.Tag = tag;
                    results = "Tag đã được thay đổi.";
                }
                //else
                //{
                //    news.Active = news.Active == true ? false : true;
                //    results = "Trạng thái tin đã được thay đổi.";
                //}
            }
            db.Entry(pr).State = EntityState.Modified;
            db.SaveChanges();

            return Json(results);
        }
        #endregion

        #region LoadPriceAmount

        [HttpPost]
        public ActionResult LoadPriceAmount(string priceAmount, string sortName)
        {

            if (priceAmount != null)
            {
                Session["PriceAmount"] = priceAmount;
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
        public ActionResult SortByName(string sortName, string currentPriceAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentPriceAmount != null)
                {
                    Session["PriceAmount"] = currentPriceAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Ord
        [HttpPost]
        public ActionResult SortByOrd(string sortOrder, string currentPriceAmount)
        {

            if (sortOrder != null)
            {
                Session["sortOrder"] = sortOrder;
                if (currentPriceAmount != null)
                {
                    Session["PriceAmount"] = currentPriceAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Ord
        [HttpPost]
        public ActionResult SortBySDate(string sortSDate, string currentPriceAmount)
        {

            if (sortSDate != null)
            {
                Session["sortSDate"] = sortSDate;
                if (currentPriceAmount != null)
                {
                    Session["PriceAmount"] = currentPriceAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
