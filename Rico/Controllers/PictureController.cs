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


namespace Rico.Controllers
{
    public class PictureController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();

        #region[Danh sách (Index)]
        public ActionResult Index(string sortOrder, string sortName, int? trang, string sortGroup, string sortPlace, string sortSDate, string currentPictureNameFilter, string picturename, string currentCatPictureFilter, string catpicture, string currentPictureAmount)
        {
            if ((Request.Cookies["Username"] != null))
            {
                if (Request.HttpMethod == "GET")
                {                  
                    picturename = currentPictureNameFilter;
                    catpicture = currentCatPictureFilter;
                    if (Session["PictureAmount"] != null)
                    {
                        currentPictureAmount = Session["PictureAmount"].ToString();
                        Session["PictureAmount"] = null;
                    }
                }
                else
                {
                    trang = 1;
                }

                ViewBag.CurrentPictureNameFilter = picturename;
                ViewBag.CurrentCatPictureFilter = catpicture;
                ViewBag.CurrentPictureAmount = currentPictureAmount;

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

                if (sortPlace != "")
                {
                    ViewBag.CurrentSortPlace = sortPlace;
                    ViewBag.SortPlaceParm = sortPlace == "place asc" ? "place desc" : "place asc";
                }
                if (Session["sortPlace"] != null)
                {
                    sortPlace = Session["sortPlace"].ToString();
                    ViewBag.CurrentSortPlace = Session["sortPlace"].ToString();
                    Session["sortPlace"] = null;
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
              
                if (sortGroup != "")
                {
                    ViewBag.CurrentSortGroup = sortGroup;
                    ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";
                }               
                if (Session["sortGroup"] != null)
                {
                    sortGroup = Session["sortGroup"].ToString();
                    ViewBag.CurrentSortGroup = Session["sortGroup"].ToString();
                    Session["sortGroup"] = null;
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
                ViewBag.SortGroupParm = sortGroup == "group asc" ? "group desc" : "group asc";
                ViewBag.SortPlaceParm = sortPlace == "place asc" ? "place desc" : "place asc";
                ViewBag.SortSDateParm = sortSDate == "sdate asc" ? "sdate desc" : "sdate asc";


                    var all = db.sp_Picture_GetByAll_NameGroup().OrderByDescending(p => p.Id).ToList();

                #region[Tìm kiếm]
            
                // Tìm theo tên sản phẩm
                if (!String.IsNullOrEmpty(picturename))
                {
                    if (picturename != "Tên ảnh")
                    {
                        all = all.Where(p => p.Name.ToUpper().Contains(picturename.ToUpper())).OrderByDescending(p => p.Id).ToList();
                    }
                }

                //Tìm theo Danh Mục sản phẩm
                if (!String.IsNullOrEmpty(catpicture))
                {
                    int catid = Int32.Parse(catpicture);
                    all = all.Where(p => p.IdCategory == catid).OrderByDescending(p => p.Id).ToList();
                }
                
                if (Session["PictureName"] != null)
                {
                        all = all.Where(p => p.Name.ToUpper().Contains(Session["PictureName"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();
                    //all = all.Where(p => p.Code.ToUpper().Contains(Session["Model"].ToString().ToUpper()) && p.Name.ToUpper().Contains(Session["PictureName"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();                 
                }
                if (Session["Cat"] != null)
                {
                    if (Session["PictureName"] != null)
                    {                      
                            all = all.Where(p => p.IdCategory == int.Parse(Session["Cat"].ToString()) && p.Name.ToUpper().Contains(Session["PictureName"].ToString().ToUpper()) ).OrderByDescending(p => p.Id).ToList();
                        //all = all.Where(p => p.IdCategory == int.Parse(Session["Cat"].ToString()) && p.Name.ToUpper().Contains(Session["PictureName"].ToString().ToUpper()) && p.Code.ToUpper().Contains(Session["Model"].ToString().ToUpper())).OrderByDescending(p => p.Id).ToList();                        
                    }
                    Session["Cat"] = null;
                }              
                if (Session["PictureName"] != null) { Session["PictureName"] = null; }
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

                switch (sortGroup)
                {
                    case "group desc":
                        all = all.OrderByDescending(p => p.Expr1).ToList();
                        break;
                    case "group asc":
                        all = all.OrderBy(p => p.Expr1).ToList();
                        break;
                    default:
                        break;
                }

                switch (sortPlace)
                {
                    case "place desc":
                        all = all.OrderByDescending(p => p.Place).ToList();
                        break;
                    case "place asc":
                        all = all.OrderBy(p => p.Place).ToList();
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

                if (currentPictureAmount != null && currentPictureAmount != "")
                {
                    pageSize = Convert.ToInt32(currentPictureAmount);
                }

                int pageNumber = (trang ?? 1);

                #region Lấy trang cuối (GetLastPage)
                // begin [get last page]
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
                #endregion GetLastPage
                #region[view drop search]
                var cat = db.GroupPictures.Where(n => n.Active == true).ToList();
                for (int i = 0; i < cat.Count; i++)
                {
                    ViewBag.Cat = new SelectList(cat, "Id", "Name");
                }
                #endregion
                #region[Số sản phẩm hiển thị trên trang]
                // Số sản phẩm hiển thị trên trang
                List<SelectListItem> items = new List<SelectListItem>();
                for (int i = 10; i <= 100; i += 10)
                {
                    if (i == pageSize)
                    {
                        items.Add(new SelectListItem { Text = i + " Ảnh / trang", Value = i + "", Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = i + " Ảnh / trang", Value = i + "" });
                    }
                }
                ViewBag.ddlPictureAmount = items;
                #endregion
                ViewBag.TotalPicture = db.Pictures.Count();
                #region PagedList
                PagedList<sp_Picture_GetByAll_NameGroup_Result> img = (PagedList<sp_Picture_GetByAll_NameGroup_Result>)all.ToPagedList(pageNumber, pageSize);
                if (Request.IsAjaxRequest())
                    return PartialView("_Data", img);
                #endregion
                return View(img);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        //#region[Search]
        //[ChildActionOnly]
        //public ActionResult Search()
        //{
        //    var cat = db.Categories.Where(n => n.Active == true && n.Level.Length == 5).ToList();
        //    for (int i = 0; i < cat.Count; i++)
        //    {
        //        ViewBag.Cat = new SelectList(cat, "Id", "Name");
        //    }
        //    return PartialView();
        //}
        //#endregion

        #region[Search]
        [HttpPost]
        public ActionResult Search(FormCollection collect)
        {
            string url = "";
            var Cat = collect["Cat"];
            var Name = collect["Name"];
            if (Cat != "" && Name == "")
            {
                url = "/admin/anh?Cat=" + Cat + "";
                //url = "/Product/ProductIndex?Cat=" + Cat + "";
            }
            else if (Cat == "" && Name != "")
            {
                url = "/admin/anh?Name=" + Name + "";
                //url = "/Product/ProductIndex?Name=" + Name + "";
            }
            else if (Cat != "" && Name != "")
            {
                url = "/admin/anh?Cat=" + Cat + "&Name=" + Name + "";
                //url = "/Product/ProductIndex?Cat=" + Cat + "&Name=" + Name + "";
            }
            else
            {
                url = "/admin/anh";
                //url = "/Product/ProductIndex";
            }
            return Redirect(url);
        }
        #endregion



        #region[Thêm (Create)]

        [HttpGet]
        public ActionResult Create()
        {
            if ((Request.Cookies["Username"] != null))
            {
                //string chuoi = "";
                var cat = db.GroupPictures.Where(n => n.Active == true).ToList();
                for (int i = 0; i < cat.Count; i++)
                {
                    ViewBag.Cat = new SelectList(cat, "Id", "Name");
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
        public ActionResult Create(FormCollection collection, Rico.Models.Picture img, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                img.Name = collection["Name"];
                img.Image = collection["Image"];
                if (collection["Width"] != "")
                {
                    img.Width = Int32.Parse(collection["Width"]);
                }
                else
                {
                    img.Width = 0;
                }

                if (collection["Height"] != "")
                {
                    img.Height = Int32.Parse(collection["Height"]);
                }
                else
                {
                    img.Height = 0;
                }
                img.Tag = StringClass.NameToTag(collection["Name"]);
                img.Ord = Convert.ToInt32(collection["Ord"]);               
                img.Place = collection["Place"];
                img.Active = (collection["Active"] == "false") ? false : true;
                img.Index = (collection["Index"] == "false") ? false : true;               
                img.SDate = Convert.ToDateTime(DateTime.Now.ToString());
                int cat = Convert.ToInt32(collection["Cat"]);
                //var grp = db.GroupPictures.Where(m => m.Id == cat).FirstOrDefault();
                //if (grp.Level.Length > 5)
                //{
                //    var grp2 = db.GroupProducts.Where(m => m.Level == grp.Level.Substring(0, 5)).FirstOrDefault();
                //    pro.IdCategory = Convert.ToInt32(grp2.Id);
                //}
                //else
                //{
                //    pro.IdCategory = Convert.ToInt32(grp.Id);
                //}
                //if (collection["CatL2"] != null && collection["CatL2"] != "")
                //{
                //    pro.IdCategory2 = int.Parse(collection["CatL2"]);
                //}
                //else { pro.IdCategory2 = null; }
                //img.IdCategory = Convert.ToInt32(grp.Id);
                img.IdCategory = cat;
                db.sp_Picture_Insert(img.Name, img.Place, img.Tag, img.Image, img.SDate, img.Index, img.Active, img.Width, img.Height, img.Ord,
                    img.IdCategory2, img.IdCategory);
                //db.Pictures.Add(img);
                //var im = db.Pictures.OrderByDescending(m => m.Id).FirstOrDefault();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion ImageCreate

        #region [Sửa (Edit)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                //var edit = db.Pictures.SingleOrDefault(m => m.Id == id);
                var Edit = db.Pictures.First(m => m.Id == id);
                var cat = db.GroupPictures.Where(n => n.Active == true).ToList();
                ViewBag.Cat = new SelectList(cat, "Id", "Name", Edit.IdCategory);

                //var catL1 = db.Categories.Where(m => m.Id == Edit.IdCategory).ToList();

                return View(Edit);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }

        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase fileImg)
        {
            if (Request.Cookies["Username"] != null)
            {
                var pp = db.Pictures.Find(id);
                string Name = collection["Name"];
                string Image = collection["Image"];
                int Height = Convert.ToInt32(collection["Height"]);
                int Width = Convert.ToInt32(collection["Width"]);
                string Tag = StringClass.NameToTag(collection["Name"]);
                int Ord = Convert.ToInt32(collection["Ord"]);
                string Place = collection["Place"];
                DateTime SDate = pp.SDate;
                bool Active = (collection["Active"] == "false") ? false : true;
                bool Index = (collection["Index"] == "false") ? false : true;
                int cId = Convert.ToInt32(collection["Cat"]);
                var grp = db.GroupPictures.Where(m => m.Id == cId).FirstOrDefault();
                int IdCategory = 0;
                int IdCategory2 = 0;
                if (grp.Level.Length > 5)
                {
                    var grp2 = db.GroupPictures.Where(m => m.Level == grp.Level.Substring(0, 5)).FirstOrDefault();
                    IdCategory = Convert.ToInt32(grp.Id);
                    //IdCategory = Convert.ToInt32(grp2.Id);
                }
                else
                {
                    IdCategory = Convert.ToInt32(grp.Id);
                }
                if (collection["CatL2"] != null && collection["CatL2"] != "")
                {
                    IdCategory2 = int.Parse(collection["CatL2"]);
                }

                db.sp_Picture_Update(id, Name, Place, Tag, Image, SDate, Index, Active, Width, Height,Ord , IdCategory2,  IdCategory  );
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion Edit

        #region[ChangeActive]
        // AJAX: /Picture/ChangeActive
        [HttpPost]
        public ActionResult ChangeActive(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var p = db.Pictures.Find(id);
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

        #region[ChangeIndex]
        // AJAX: /Picture/ChangeIndex
        [HttpPost]
        public ActionResult ChangeIndex(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var p = db.Pictures.Find(id);
            if (p != null)
            {
                p.Index = p.Index == true ? false : true;
            }
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();

            var results = "Hiển thị ảnh ở trang chủ đã được thay đổi.";
            return Json(results);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion

        #region [Xóa (Delete)]
        public ActionResult Delete(int id, int trang, int kichco)
        {
            if ((Request.Cookies["Username"] != null) )
            {
                var del = db.Pictures.Where(m => m.Id == id).SingleOrDefault();

                db.Pictures.Remove(del);
                db.SaveChanges();

                List<Rico.Models.Picture> img = db.Pictures.ToList();

                if ((img.Count % kichco) == 0)
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



        #region[Nhiều lệnh (MultiCommand)]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect)
        {
            int trang = int.Parse(collect["mPage"]);
            int kichco = int.Parse(collect["PageSize"]);

            List<Rico.Models.Picture> pictures = db.Pictures.ToList();
            int lastpage = pictures.Count / kichco;
            if (pictures.Count % kichco > 0)
            {
                lastpage++;
            }
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
                            if (key.Contains("chkStatus") || key.Contains("chkIndex") || key.Contains("chkActive"))
                            {

                            }
                            else
                            {
                                checkbox = Request.Form["" + key];
                                if (checkbox != "false")
                                {
                                    int id = Convert.ToInt32(key.Remove(0, 3));
                                    var Del = (from del in db.Pictures where del.Id == id select del).SingleOrDefault();
                                    //var Delorder = (from deloder in db.OrderDetails where deloder.IdPro == id select deloder).ToList();
                                    //for (int l = 0; l < Delorder.Count; l++)
                                    //{
                                    //    int idp = Delorder[l].Id;
                                    //    var orldel = (from orldels in db.OrderDetails where orldels.Id == idp select orldels).SingleOrDefault();
                                    //    db.OrderDetails.Remove(orldel);
                                        //db.SaveChanges();
                                    //}
                                    //if (Del.SpTon == 0)
                                    //{
                                    db.Pictures.Remove(Del);
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
                else if (collect["ddlPictureAmount"] != null)
                {
                    if (collect["ddlPictureAmount"].Length > 0)
                    {
                        Session["PictureAmount"] = collect["ddlPictureAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else if (collect["btnSearch"] != null)
                {
                    //string Model = collect["txtModel"];
                    string Namesearch = collect["PictureName"];
                    string cat = collect["Cat"];
                    //if (Model.Length > 0)
                    //{
                    //    Session["Model"] = Model;
                    //}
                    if (Namesearch.Length > 0)
                    {
                        Session["PictureName"] = Namesearch;
                    }
                    if (cat.Length > 0)
                    {
                        Session["Cat"] = cat;
                    }
                    return RedirectToAction("Index", new { trang = trang });
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));
                            var Up = db.Pictures.Where(e => e.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Ord = int.Parse(collect["Ord" + id]);
                                }
                                
                                if (!collect["chkIndex" + id].Equals(""))
                                {
                                    bool Index = (collect["chkIndex" + id] == "false") ? false : true;
                                    Up.Index = Index;
                                }
                                if (!collect["chkActive" + id].Equals(""))
                                {
                                    bool Active = (collect["chkActive" + id] == "false") ? false : true;
                                    Up.Active = Active;
                                }
                                
                                db.Entry(Up).State = System.Data.EntityState.Modified;
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

    
         
        #region [Thêm nhiều (AddMulti)]
        public ActionResult AddMulti()
        {
            if (Request.Cookies["Username"] != null)
            {
                var cat = db.GroupPictures.Where(n => n.Active == true).ToList();
                for (int i = 0; i < cat.Count; i++)
                {
                    ViewBag.Cat = new SelectList(cat, "Id", "Name");
                }
                return View();
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AddMulti(IEnumerable<HttpPostedFileBase> fileImg, FormCollection aForm, string cat)
        {
            if (Request.Cookies["Username"] != null)
            {
                int ID;
                var tmp = db.Pictures.Select(a => (int?)a.Id).Max();
                //if (tmp == null)
                //    tmp = 1;
                foreach (var file in fileImg)
                {
                    if (tmp == null)
                    { ID = 1; tmp = 1; }
                    else
                        ID = db.Pictures.Select(a => a.Id).Max();

                    if (file.ContentLength > 0)
                    {
                        //var b = (from k in db.ProImages select k.Id).Max();
                        var ab = Request.Files["fileImg"];
                        String FileExtn = System.IO.Path.GetExtension(file.FileName).ToLower();
                        if (!(FileExtn == ".jpg" || FileExtn == ".png" || FileExtn == ".gif"))
                        {
                            ViewBag.error = "Only jpg, gif and png files are allowed!";
                        }
                        else
                        {
                            Rico.Models.Picture img = new Rico.Models.Picture();
                            var Filename = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath(Url.Content("/Uploads/anh")), Filename);
                            file.SaveAs(path);
                           
                            img.Id = ID + 1;
                            img.Name = "Ảnh thứ " + ID;
                            img.Ord = 0;
                            img.Place = null;
                            img.IdCategory2 = 0;            
                            img.Width = 0;
                            img.Height = 0;
                            img.Active = true;
                            img.Index = true;
                            //aImage.Position = 0;
                            img.Image = "/Uploads/anh/" + Filename;
                            img.SDate = DateTime.Now;
                            img.IdCategory = Convert.ToInt32(cat);
                            db.Pictures.Add(img);
                            db.SaveChanges();
                        }
                    }
                    var fd = file;
                }         
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }    
        #endregion AddMulti

        #region[Sửa trực tiếp (UpdateDirect)]
        public ActionResult UpdateDirect(int id, string ord, string width, string height, string name, string image, string place, string tag)
        {
            var img = db.Pictures.Find(id);
            var result = string.Empty;
            if (img != null)
            {
                if (ord != null)
                {
                    img.Ord = Convert.ToInt32(ord);
                    result = "Thứ tự đã được thay đổi.";
                }
                else if (width != null)
                {
                    img.Width = Convert.ToInt32(width);
                    result = "Độ rộng ảnh đã được thay đổi.";
                }
                else if (height != null)
                {
                    img.Height = Convert.ToInt32(height);
                    result = "Chiều cao ảnh đã được thay đổi.";
                }
                else if (name != null)
                {
                    img.Name = name;
                    result = "Tên ảnh đã được thay đổi.";
                }
                else if (image != null)
                {
                    img.Image = image;
                    result = "ảnh đã được thay đổi.";
                }
                else if (place != null)
                {
                    img.Place = place;
                    result = "Địa điểm đã được thay đổi.";
                }
                else if (tag != null)
                {
                    img.Tag = tag;
                    result = "Tag đã được thay đổi.";
                }
                else
                {
                    //img.Index = img.Index == true ? false : true;
                    //result = "Hiển thị ảnh ở trang chủ đã được thay đổi.";
                    img.Active = img.Active == true ? false : true;
                    result = "Trạng thái ảnh đã được thay đổi.";
                }
            }

            db.Entry(img).State = EntityState.Modified;
            db.SaveChanges();
            //var result = "Dữ liệu đã được thay đổi.";// userModule.Active;
            return Json(result);
        }
        #endregion

        #region[Ajax LoadPictureAmount]
        // AJAX: /Product/LoadProductAmount
        [HttpPost]
        public ActionResult LoadPictureAmount(string pictureAmount, string sortName)
        {

            if (pictureAmount != null)
            {
                Session["PictureAmount"] = pictureAmount;
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
        public ActionResult SortByName(string sortName, string currentPictureAmount)
        {

            if (sortName != null)
            {
                Session["sortName"] = sortName;
                if (currentPictureAmount != null)
                {
                    Session["PictureAmount"] = currentPictureAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Group
        [HttpPost]
        public ActionResult SortByGroup(string sortGroup, string currentPictureAmount)
        {

            if (sortGroup != null)
            {
                Session["sortGroup"] = sortGroup;
                if (currentPictureAmount != null)
                {
                    Session["PictureAmount"] = currentPictureAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Ord
        [HttpPost]
        public ActionResult SortByOrd(string sortOrder, string currentPictureAmount)
        {

            if (sortOrder != null)
            {
                Session["sortOrder"] = sortOrder;
                if (currentPictureAmount != null)
                {
                    Session["PictureAmount"] = currentPictureAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by Place
        [HttpPost]
        public ActionResult SortByPlace(string sortPlace, string currentPictureAmount)
        {

            if (sortPlace != null)
            {
                Session["sortPlace"] = sortPlace;
                if (currentPictureAmount != null)
                {
                    Session["PictureAmount"] = currentPictureAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Sort by SDate
        [HttpPost]
        public ActionResult SortBySDate(string sortSDate, string currentPictureAmount)
        {

            if (sortSDate != null)
            {
                Session["sortSDate"] = sortSDate;
                if (currentPictureAmount != null)
                {
                    Session["PictureAmount"] = currentPictureAmount;
                }
            }
            return RedirectToAction("Index");
        }

        #endregion


    }
}
