using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Rico.Models;
using PagedList;
using PagedList.Mvc;
using System.Data;
using System.Data.Entity;

namespace Rico.Controllers.Admin.Member
{
    public class MemberController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region[(Index)]
        public ActionResult Index(int? trang, string currentMemberName, string currentMemberAmount, string sortName, string sortPermiss, string Membername, string currentMemberNameFilter)
        {
            if (Request.HttpMethod == "GET")
            {
                Membername = currentMemberNameFilter;
                if (Session["MemberName"] != null)
                {
                    currentMemberName = Session["MemberName"].ToString();
                    Session["MemberName"] = null;
                }
                if (Session["MemberAmount"] != null)
                {
                    currentMemberAmount = Session["MemberAmount"].ToString();
                    Session["MemberAmount"] = null;
                }
            }
            else
            {
                trang = 1;
            }
            ViewBag.CurrentMemberNameFilter = Membername;
            ViewBag.CurrentMemberName = currentMemberName;
            ViewBag.CurrentMemberAmount = currentMemberAmount;

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
            if (sortPermiss != "")
            {
                ViewBag.CurrentSortPermiss = sortPermiss;
                ViewBag.SortPermissParm = sortPermiss == "permiss asc" ? "permiss desc" : "permiss asc";
            }
            if (Session["sortPermiss"] != null)
            {
                sortPermiss = Session["sortPermiss"].ToString();
                ViewBag.CurrentSortPermiss = Session["sortPermiss"].ToString();
                Session["sortPermiss"] = null;
            }
            int pageSize = 10;
            if (currentMemberAmount != null && currentMemberAmount != "")
            {
                pageSize = Convert.ToInt32(currentMemberAmount);
            }

            int pageNumber = (trang ?? 1);
            ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";
            ViewBag.SortPermissParm = sortPermiss == "permiss asc" ? "permiss desc" : "permiss asc";

            #region[Số sản phẩm hiển thị trên trang]
            // Số sản phẩm hiển thị trên trang
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 10; i <= 100; i += 10)
            {
                if (i == pageSize)
                {
                    items.Add(new SelectListItem { Text = i + " Thành viên / trang", Value = i + "", Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = i + " Thành viên / trang", Value = i + "" });
                }
            }
            ViewBag.ddlMemberAmount = items;
            #endregion

            var all = db.Members.OrderByDescending(m => m.Id).ToList();

            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3"))
            {
               
                all = all.Where(p => p.Permiss == 0 || p.Permiss == 1||p.Permiss==2).OrderBy(p => p.Username).ToList();
                if (!String.IsNullOrEmpty(currentMemberName))
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(currentMemberName.ToUpper())).OrderByDescending(p => p.Id).ToList();
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

                switch (sortPermiss)
                {
                    case "permiss desc":
                        all = all.OrderByDescending(p => p.Permiss).ToList();
                        break;
                    case "permiss asc":
                        all = all.OrderBy(p => p.Permiss).ToList();
                        break;
                    default:
                        break;
                }
                #endregion
                //
                #region PagedList
                PagedList<Rico.Models.Member> gp = (PagedList<Rico.Models.Member>)all.ToPagedList(pageNumber, pageSize);
                if (Request.IsAjaxRequest())
                    return PartialView("_Data", gp);
                #endregion
                return View(gp);
            }
            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {

                all = all.Where(p=>p.Permiss==0||p.Permiss==1).OrderBy(p => p.Username).ToList();
            if (!String.IsNullOrEmpty(currentMemberName))
            {
                all = all.Where(p => p.Name.ToUpper().Contains(currentMemberName.ToUpper())).OrderByDescending(p => p.Id).ToList();
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

                switch (sortPermiss)
                {
                    case "permiss desc":
                        all = all.OrderByDescending(p => p.Permiss).ToList();
                        break;
                    case "permiss asc":
                        all = all.OrderBy(p => p.Permiss).ToList();
                        break;
                    default:
                        break;
                }
                #endregion
                #region PagedList
                PagedList<Rico.Models.Member> gp = (PagedList<Rico.Models.Member>)all.ToPagedList(pageNumber, pageSize);
                if (Request.IsAjaxRequest())
                    return PartialView("_Data", gp);
                #endregion
                return View(gp);
            }
            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {

                all = all.Where(p => p.Permiss == 0).OrderBy(p => p.Username).ToList();
                if (!String.IsNullOrEmpty(currentMemberName))
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(currentMemberName.ToUpper())).OrderByDescending(p => p.Id).ToList();
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

                switch (sortPermiss)
                {
                    case "permiss desc":
                        all = all.OrderByDescending(p => p.Permiss).ToList();
                        break;
                    case "permiss asc":
                        all = all.OrderBy(p => p.Permiss).ToList();
                        break;
                    default:
                        break;
                }
                #endregion

                #region PagedList
                PagedList<Rico.Models.Member> gp = (PagedList<Rico.Models.Member>)all.ToPagedList(pageNumber, pageSize);
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
        #region[Thêm (Create)]       
        [HttpGet]
        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3"))
            {
                ViewBag.Permiss = GodPermission("Nhân viên", "Manager", "Administrator");
               
                return View();
            }
            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {
                ViewBag.Permiss = AdminPermission("Nhân viên", "Manager");
                return View();
            }

            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                ViewBag.Permiss = ManagerPermission("Nhân viên");
                return View();
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

     

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Rico.Models.Member pc)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                pc.Name = collection["Name"];
                pc.Username = collection["Username"];
                string pass = collection["Password"].ToString();
                pc.Password = pass;
                pc.Email = collection["Email"];
                pc.Address = collection["Address"];
                pc.Phone = collection["Phone"];
                string per = "0";
                per = collection["Permiss"];
                pc.Permiss = int.Parse(per);
                pc.Active = (collection["Active"] == "false") ? false : true;
                db.sp_Member_Insert(pc.Name, pc.Username, pc.Password, pc.Permiss, pc.Active);
                //db.Members.Add(pc);
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
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var memberEdit = db.Members.SingleOrDefault(m => m.Id == id);
            var p = memberEdit.Permiss;

            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3"))
            {
                if (p == 3)
                {
                    ViewBag.Permiss = Permission("Nhân viên", "Manager", "Administrator","Chúa trời");
                    return View(memberEdit);
                }
                else
                {
                    ViewBag.Permiss = GodPermission("Nhân viên", "Manager", "Administrator");
                    return View(memberEdit);
                }
            }

            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {        
                if (p == 3 || p==2)
                {
                    return RedirectToAction("Default", "Admin");
                }
                else
                {
                    ViewBag.Permiss = AdminPermission("Nhân viên", "Manager");
                    return View(memberEdit);
                }
            }

            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                if (p == 3 || p == 2 || p==1)
                {
                    return RedirectToAction("Default", "Admin");
                }
                else
                {
                ViewBag.Permiss = ManagerPermission("Nhân viên");
                return View(memberEdit);
                }
            }

            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection collection, Rico.Models.Member pc)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                pc.Name = collection["Name"];
                pc.Username = collection["Username"];
                string pass = collection["Password"].ToString();
                pc.Password = pass;
                pc.Email = collection["Email"];
                pc.Address = collection["Address"];
                pc.Phone = collection["Phone"];
                pc.Permiss = Convert.ToInt32(collection["Permiss"]);
                pc.Active = (collection["Active"] == "false") ? false : true;
                db.sp_Member_Update(pc.Id, pc.Name, pc.Username, pc.Password, pc.Permiss, pc.Active);
               //db.sp_Member_Update();

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
        public ActionResult Delete(int id)
        {
            var del = db.Members.First(p => p.Id == id);
            var d = del.Permiss;

            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3"))
            {
                //db.Members.Remove(del);
                //db.SaveChanges();
            }

            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {
                if (d == 3 || d == 2)
                {
                    return RedirectToAction("Default", "Admin");
                }
                //else
                //{
                //    db.Members.Remove(del);
                //    db.SaveChanges();
                //}
            }

            else if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {
                if (d == 3 || d == 2 || d == 1)
                {
                    return RedirectToAction("Default", "Admin");
                }
                //else
                //{
                //    db.Members.Remove(del);
                //    db.SaveChanges();
                //}
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
            db.Members.Remove(del);
            db.SaveChanges();
            return RedirectToAction("Index", "Member");
        }
        #endregion
        #region[Nhiều Lệnh (MultiCommand)]
        public ActionResult MultiCommand(FormCollection collection)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
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
                                var Del = (from emp in db.Members where emp.Id == id select emp).SingleOrDefault();
                                db.Members.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                    return RedirectToAction("Index");
                }

                else if (collection["ddlMemberAmount"] != null)
                {
                    if (collection["ddlMemberAmount"].Length > 0)
                    {
                        Session["MemberAmount"] = collection["ddlMemberAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else if (collection["btnSearch"] != null)
                {
                    if (collection["MemberName"].Length > 0)
                    {
                        Session["MemberName"] = collection["MemberName"];
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
        #region[Tìm kiếm (Search)]
        // Autocomplete for textbox search 
        [HttpGet]
        public ActionResult Search(string term)
        {
            var MemberName = from p in db.Members
                             select p.Name;
            string[] items = MemberName.ToArray();

            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region[Sửa trực tiếp (UpdateDirect)]
        [HttpPost]
        public ActionResult UpdateDirect(int id, string Username, string Password, string Phone, string Email, string Name)
        {
            var results = "";
            var vMember = db.Members.Find(id);
            if (vMember != null)
            {
                if (Username != null)
                {
                    vMember.Username = Username;
                    results = "Tài khoản đã được thay đổi.";
                }
                else if (Password != null)
                {
                    vMember.Password = Password;
                    results = "Mật khẩu đã được thay đổi.";
                }
                else if (Phone != null)
                {
                    vMember.Phone = Phone;
                    results = "Số điện thoại đã được thay đổi.";
                }
                else if (Email != null)
                {
                    vMember.Email = Email;
                    results = "Địa chỉ Email đã được thay đổi.";
                }
                else if (Name != null)
                {
                    vMember.Name = Name;
                    results = "Tên thành viên đã được thay đổi.";
                }
            }
            db.Entry(vMember).State = EntityState.Modified;
            db.SaveChanges();
            return Json(results);
        }
        #endregion
        #region[ChangeActive]
        // AJAX: /Picture/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var p = db.Members.Find(id);
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
        #region Sort by Name
        [HttpPost]
        public ActionResult SortByName(string sortName, string currentMemberAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentMemberAmount != null)
                {
                    Session["MemberAmount"] = currentMemberAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region Sort by Permiss
        [HttpPost]
        public ActionResult SortByPermiss(string sortPermiss, string currentMemberAmount)
        {

            if (sortPermiss != null)
            {
                Session["sortPermiss"] = sortPermiss;
                if (currentMemberAmount != null)
                {
                    Session["MemberAmount"] = currentMemberAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region[Phân quyền (Permission)]
        public List<SelectListItem> Permission(string v1, string v2, string v3, string v4)
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v1 });
            sll.Add(new SelectListItem { Value = "1", Text = v2 });
            sll.Add(new SelectListItem { Value = "2", Text = v3 });
            sll.Add(new SelectListItem { Value = "3", Text = v4 });
            return sll;
        }

        public List<SelectListItem> GodPermission(string v1, string v2, string v3)
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v1 });
            sll.Add(new SelectListItem { Value = "1", Text = v2 });
            sll.Add(new SelectListItem { Value = "2", Text = v3 });
            return sll;
        }

        public List<SelectListItem> AdminPermission(string v1, string v2)
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v1 });
            sll.Add(new SelectListItem { Value = "1", Text = v2 });
            return sll;
        }

        public List<SelectListItem> ManagerPermission(string v1)
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v1 });
            return sll;
        }

        #endregion
        #region[Ajax LoadPictureAmount]
        // AJAX: /Product/LoadProductAmount
        [HttpPost]
        public ActionResult LoadMemberAmount(string memberAmount, string sortName)
        {

            if (memberAmount != null)
            {
                Session["MemberAmount"] = memberAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("Index");
        }


        #endregion
        //#region[Autocomplete Member Name]
        //// Autocomplete for textbox search 
        //[HttpGet]
        //public ActionResult Autocomplete(string term)
        //{
        //    var MemberName = from p in db.Members
        //                     select p.Name;
        //    string[] items = MemberName.ToArray();

        //    var filteredItems = items.Where(
        //        item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
        //        );
        //    return Json(filteredItems, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

    }
}
