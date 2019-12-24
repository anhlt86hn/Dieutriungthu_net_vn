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
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        DieutriungthuEntities db = new DieutriungthuEntities();

        #region Index
        public ActionResult Index(string sortDate, string sortName, int? trang, string dateSearch, string contactname, string sortDatePagelist, string currentContactAmount, string currentContactNameFilter)
        {
            if ((Request.Cookies["Username"] != null))
            {
                if (Request.HttpMethod == "GET")
                {
                   contactname = currentContactNameFilter;
                    if (Session["ContactAmount"] != null)
                    {
                        currentContactAmount = Session["ContactAmount"].ToString();
                        Session["ContactAmount"] = null;
                    }
                }
                else
                {
                    trang = 1;
                }
                ViewBag.CurrentContactNameFilter = contactname;
                ViewBag.CurrentContactAmount = currentContactAmount;
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
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

                //var all = db.GroupNews.OrderByDescending(g => g.Ord).ToList();
                var all = db.Contacts.OrderByDescending(g => g.SDate).ToList();
                // Tìm theo tên sản phẩm
                if (!String.IsNullOrEmpty(contactname))
                {
                    if (contactname != "Tên phản hồi")
                    {
                        all = all.Where(p => p.Name.ToUpper().Contains(contactname.ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                }

                #region[Sắp xếp]                
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
                #endregion

                int pageSize = 10;
                if (currentContactAmount != null && currentContactAmount != "")
                {
                    pageSize = Convert.ToInt32(currentContactAmount);
                }
                int pageNumber = (trang ?? 1);

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
                
                #region OrderDate
                
                ViewBag.CurrentSortDate = sortDate;
                ViewBag.SortDateParm = sortDate == "dateAsc" ? "dateDesc" : "dateAsc";

                switch (sortDate)
                {
                    case "dateDesc":
                        all = db.Contacts.OrderByDescending(a => a.SDate).ToList();
                        break;
                    case "dateAsc":
                        all = db.Contacts.OrderBy(a => a.SDate).ToList();
                        break;
                    default:
                        break;
                }


                #endregion Order
                #region[Số sản phẩm hiển thị trên trang]
                // Số sản phẩm hiển thị trên trang
                List<SelectListItem> items = new List<SelectListItem>();
                for (int i = 10; i <= 100; i += 10)
                {
                    if (i == pageSize)
                    {
                        items.Add(new SelectListItem { Text = i + " Phản hồi / trang", Value = i + "", Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = i + " Phản hồi / trang", Value = i + "" });
                    }
                }
                ViewBag.ddlContactAmount = items;
                #endregion

                #region PagedList
                PagedList<Contact> ct = (PagedList<Contact>)all.ToPagedList(pageNumber, pageSize);
                /// Kiem tra co tim kiem theo vi tri va phan trang theo vi tri khong
                
                    ct = (PagedList<Rico.Models.Contact>)all.ToPagedList(pageNumber, pageSize);
                    #endregion

                    if (Request.IsAjaxRequest())
                    return PartialView("_Data", ct);                

                return View(ct);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion


        [HttpGet]
        public ActionResult Create()
        {        
                return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, Contact contact)
        {
            try
            {
                var menu = db.Menus.Where(m => m.Active == true && m.Name == "Liên hệ").SingleOrDefault();
                string menuName = menu.Name; ViewBag.MenuName = menuName;
                string menuLink = menu.Link; ViewBag.MenuLink = menuLink;

                

                contact.Name = collection["Name"];
                contact.Address = collection["Address"];
                contact.Email = collection["Email"];
                contact.Title = collection["Title"];
                contact.Detail = collection["content"];
                contact.Company = collection["company"];
                contact.Phone = collection["Phone"];
                contact.SDate = System.DateTime.Now;
                contact.Status = false;
                db.sp_Contact_Insert(contact.Name, contact.Title, contact.Address, contact.Email, contact.Phone, contact.Company, contact.Detail,  contact.SDate, contact.Status);
                db.SaveChanges();
                #region[Title, Keyword, Deskription]
                if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
                if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
                if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
                #endregion

               
                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }

          
        }


       


        public ActionResult ContactCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactCreate(FormCollection collection, Contact contact)
        {
            try
            {
                var menu = db.Menus.Where(m => m.Active == true && m.Name == "Liên hệ").SingleOrDefault();
                string menuName = menu.Name; ViewBag.MenuName = menuName;
                string menuLink = menu.Link; ViewBag.MenuLink = menuLink;


                     #region[Title, Keyword, Deskription]
                if (menu.Title.Length > 0) { ViewBag.tit = menu.Title; } else { ViewBag.tit = menu.Name; }
                if (menu.Description.Length > 0) { ViewBag.des = menu.Description; } else { ViewBag.des = menu.Name; }
                if (menu.Keyword.Length > 0) { ViewBag.key = menu.Keyword; } else { ViewBag.key = menu.Name; }
                #endregion

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ContactEdit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactEdit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("ContactIndex");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ContactDelete(int id)
        {
            return View();
        }

        #region MultiCommand
        public ActionResult MultiCommand(FormCollection collect)
        {
            int trang = int.Parse(collect["mPage"]);
            int kichco = int.Parse(collect["PageSize"]);

            List<Contact> ct = db.Contacts.ToList();
            int lastpage = ct.Count / kichco;
            if (ct.Count % kichco > 0)
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
                            if (key.Contains("chkIndex") || key.Contains("chkStatus"))
                            {

                            }
                            else
                            {
                                checkbox = Request.Form["" + key];
                                if (checkbox != "false")
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 3));
                                    var Del = (from del in db.Contacts where del.Id == id select del).SingleOrDefault();
                                    db.Contacts.Remove(Del);
                                    db.SaveChanges();
                                }
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
                else if (collect["ddlContactAmount"] != null)
                {
                    if (collect["ddlContactAmount"].Length > 0)
                    {
                        Session["ContactAmount"] = collect["ddlContactAmount"];
                    }
                    return RedirectToAction("Index");
                }
                else if (collect["btnSearch"] != null)
                {
                    //string Model = collect["txtModel"];
                    string Namesearch = collect["ContactName"];
                    //string cat = collect["Cat"];
                    //if (Model.Length > 0)
                    //{
                    //    Session["Model"] = Model;
                    //}
                    if (Namesearch.Length > 0)
                    {
                        Session["ContactName"] = Namesearch;
                    }
                    //if (cat.Length > 0)
                    //{
                    //    Session["Cat"] = cat;
                    //}
                    return RedirectToAction("Index", new { trang = trang });
                }
            }
            return RedirectToAction("Default", "Admin");
        }
        #endregion

        #region ChangeStatus
        [HttpPost]
        public ActionResult ChangeStatus(int id)
        {
            var m = db.Contacts.Find(id);
            if (m != null)
            {
                m.Status = m.Status == true ? false : true;
            }
            db.Entry(m).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Trạng thái đã được thay đổi.";
            return Json(results);
        }
        #endregion

        #region[Ajax LoadPictureAmount]
        // AJAX: /Product/LoadProductAmount
        [HttpPost]
        public ActionResult LoadContactAmount(string contactAmount, string sortName)
        {

            if (contactAmount != null)
            {
                Session["ContactAmount"] = contactAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("Index");
        }


        #endregion


        #region Delete
        [HttpGet]
        public ActionResult Delete(int id, int trang, int kichco)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {
                var mDelete = db.Contacts.Where(m => m.Id == id).SingleOrDefault();

                db.Contacts.Remove(mDelete);
                db.SaveChanges();

                List<Rico.Models.Contact> adv = db.Contacts.ToList();

                if ((adv.Count % kichco) == 0)
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


        public ActionResult View(int id)
        {
            var c = db.Contacts.Find(id);
            ViewBag.ContactId = c.Id;
            
            return View(c);
        }

        public string Capcha(string captcha)
        {
            string chuoi = "";
            if (captcha != "")
            {
                CaptchaProvider captchaPro = new CaptchaProvider();
                if (!captchaPro.IsValidCode(captcha))
                {
                    chuoi = "Mã àn toàn không chính xác!";
                }
            }
            return chuoi;
        }
    }
}
