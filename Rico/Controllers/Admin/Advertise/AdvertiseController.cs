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


namespace Rico.Controllers.Admin.Advertise
{
    public class AdvertiseController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();

        #region[Index]
        public ActionResult Index(string thutu, string sortName, int? trang, string positionSearch, string sortPos, string currentAdvertiseName, string currentAdvertiseAmount)
        {
            if ((Request.Cookies["Username"] != null))
            {
                if (Request.HttpMethod == "GET")
                {
                  if (Session["AdvertiseName"] != null)
                    {
                        currentAdvertiseName = Session["AdvertiseName"].ToString();
                        Session["AdvertiseName"] = null;
                    }
                    if (Session["AdvertiseAmount"] != null)
                    {
                        currentAdvertiseAmount = Session["AdvertiseAmount"].ToString();
                        Session["AdvertiseAmount"] = null;
                    }
                }
                else
                {
                    trang = 1;
                }

                ViewBag.CurrentAdvertiseName = currentAdvertiseName;
                ViewBag.CurrentAdvertiseAmount = currentAdvertiseAmount;

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


                if (thutu != "")
                {
                    ViewBag.CurrentSortOrd = thutu;
                    ViewBag.SortOrdParm = thutu == "ord asc" ? "ord desc" : "ord asc";
                }
                if (Session["sortOrder"] != null)
                {
                    thutu = Session["sortOrder"].ToString();
                    ViewBag.CurrentSortOrd = Session["sortOrder"].ToString();
                    Session["sortOrder"] = null;
                }
                
                ViewBag.SortOrdParm = thutu == "thu-tu-tang-dan" ? "thu-tu-giam-dan" : "thu-tu-tang-dan";             
                ViewBag.SortNameParm = sortName == "name asc" ? "name desc" : "name asc";

                var all = db.sp_Advertise_GetByAll().OrderBy(a => a.Position).ToList();
                #region Controls SelectListItem
                List<SelectListItem> sllTar = new List<SelectListItem>();
                List<SelectListItem> sllPos = new List<SelectListItem>();
                sllTar = CreateTarget("_blank", "_self", "_parent", "_top");
                sllPos = CreateSSL("Chưa đặt vị trí", "Logo", "Slide", "Icon Dịch vụ");
                ViewBag.sllTarget = sllTar;
                ViewBag.sllPosition = sllPos;
                //ViewBag.sllPosition = new SelectList(Position(), "value", "text");
                //ViewBag.sllTarget = new SelectList(Target(), "value", "text");
                //List<SelectList> sllPos = new List<SelectList>();
                //sllPos = ViewBag.sllPosition;
                #endregion SelectListItem

                if (!String.IsNullOrEmpty(currentAdvertiseName))
                {
                    all = all.Where(p => p.Name.ToUpper().Contains(currentAdvertiseName.ToUpper())).OrderByDescending(p => p.Id).ToList();
                }
                int pageSize = 10;

                if (currentAdvertiseAmount != null && currentAdvertiseAmount != "")
                {
                    pageSize = Convert.ToInt32(currentAdvertiseAmount);
                }
                int pageNumber = (trang ?? 1);

            #region GetLastPage
            // begin [get last page]
            if (trang != null)
                ViewBag.mPage = (int)trang;
            else
                ViewBag.mPage = 1;

            int lastPage = all.Count / pageSize;
            if (all.Count % pageSize > 0)
            {
                lastPage++;
            }
            ViewBag.LastPage = lastPage;

            ViewBag.PageSize = pageSize;
            //end [get last page]
            #endregion GetLastPage

            #region Order
           
            
            switch (thutu)
            {
                case "thu-tu-giam-dan":
                    all = all.OrderByDescending(a => a.Ord).ToList();
                    break;
                case "thu-tu-tang-dan":
                    all = all.OrderBy(a => a.Ord).ToList();
                    break;
                default:
                    break;
            }

            switch (sortName)
            {
                case "name desc":
                    all = all.OrderByDescending(a => a.Name).ToList();
                    break;
                case "name asc":
                    all = all.OrderBy(a => a.Name).ToList();
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
                        items.Add(new SelectListItem { Text = i + " Link ảnh / trang", Value = i + "", Selected = true });
                    }
                    else
                    {
                        items.Add(new SelectListItem { Text = i + " Link ảnh / trang", Value = i + "" });
                    }
                }
                ViewBag.ddlAdvertiseAmount = items;
                #endregion

                PagedList<sp_Advertise_GetByAll_Result> adv = (PagedList<sp_Advertise_GetByAll_Result>)all.ToPagedList(pageNumber, pageSize);

            /// Kiem tra co tim kiem theo vi tri va phan trang theo vi tri khong
            if ((String.IsNullOrEmpty(positionSearch) || positionSearch == "") && (sortPos == null))
            {
                if (Request.IsAjaxRequest())
                    return PartialView("_Data", adv);
                else
                    return View(adv);
            }
            else /// Truong hop co tim kiem theo vi tri
            {
                int pos;
                /// Kiem tra xem co phan trang khong
                if (sortPos != null) /// Phan trang
                {
                    ViewBag.CurrentPos = sortPos;                    
                    pos = Convert.ToInt32(sortPos);
                    if (sortName == "name asc")
                        all = all.Where(a => a.Position == pos).OrderBy(a => a.Name).ToList();
                    else if (sortName == "name desc")
                        all = all.Where(a => a.Position == pos).OrderByDescending(a => a.Name).ToList();
                    else if (thutu == "thu-tu-giam-dan")
                        all = all.Where(a => a.Position == pos).OrderByDescending(a => a.Ord).ToList();
                    else if (thutu == "thu-tu-tang-dan")
                        all = all.Where(a => a.Position == pos).OrderBy(a => a.Ord).ToList();
                    else
                        all = all.Where(a => a.Position == pos).OrderByDescending(a => a.Id).ToList();
                }
                else /// ko phan trang
                {
                    ViewBag.CurrentPos = positionSearch;
                    pos = Convert.ToInt32(positionSearch);
                    if (sortName == "name asc")
                        all = all.Where(a => a.Position == pos).OrderBy(a => a.Name).ToList();
                    else if (sortName == "name desc")
                        all = all.Where(a => a.Position == pos).OrderByDescending(a => a.Name).ToList();
                    else if (thutu == "thu-tu-giam-dan")
                        all = all.Where(a => a.Position == pos).OrderByDescending(a => a.Ord).ToList();
                    else if (thutu == "thu-tu-tang-dan")
                        all = all.Where(a => a.Position == pos).OrderBy(a => a.Ord).ToList();
                    else
                        all = all.Where(a => a.Position == pos).OrderByDescending(a => a.Id).ToList();
                }                    
                    sllPos[pos].Selected = true;

                    adv = (PagedList<sp_Advertise_GetByAll_Result>)all.ToPagedList(pageNumber, pageSize);

                if (Request.IsAjaxRequest())
                    return PartialView("_Data", adv);
                else
                    return View(adv);
            }

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
            if ((Request.Cookies["Username"] != null))
            {
                //ViewBag.sllPosition = new SelectList(Position(), "value", "text");
                //ViewBag.sllTarget = new SelectList(Target(), "value", "text");
                ViewBag.sllPosition = CreateSSL("Chưa đặt vị trí", "Logo", "Slide","Icon Dịch vụ");
                ViewBag.sllTarget = CreateTarget("_blank", "_self", "_parent", "_top");
                return View();
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Rico.Models.Advertise advertise)
        {
            if (Request.Cookies["Username"] != null)
            {
                advertise.Name = collection["Name"];
                advertise.Detail = collection["Detail"];
                advertise.Link = collection["Link"];
                advertise.Image = collection["Image"];
                advertise.Active = (collection["Active"] == "false") ? false : true;
                if (collection["Width"] != "")
                {
                    advertise.Width = Int32.Parse(collection["Width"]);
                }
                else
                {
                    advertise.Width = 0;
                }

                if (collection["Height"] != "")
                {
                    advertise.Height = Int32.Parse(collection["Height"]);
                }
                else
                {
                    advertise.Height = 0;
                }
                advertise.Position = Convert.ToInt32(collection["sllPosition"]);
                advertise.Ord = Convert.ToInt32(collection["Ord"]);
              
                if (Convert.ToInt32(collection["sllTarget"]) == 0)
                {
                    advertise.Target = "_blank";
                }
                else if (Convert.ToInt32(collection["sllTarget"]) == 1)
                {
                    advertise.Target = "_self";
                }
                else if (Convert.ToInt32(collection["sllTarget"]) == 2)
                {
                    advertise.Target = "_parent";
                }
                else if (Convert.ToInt32(collection["sllTarget"]) == 3)
                {
                    advertise.Target = "_top";
                }
                db.sp_Advertise_Insert(advertise.Name, advertise.Detail, advertise.Target, advertise.Link, advertise.Image, 
                    advertise.Active, advertise.Width, advertise.Height, advertise.Position, advertise.Ord);                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion Create

        #region [Sửa (Edit)]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Request.Cookies["Username"] != null)
            {
                var advertiseEdit = db.Advertises.SingleOrDefault(m => m.Id == id);
               
                //ViewBag.sllTarget = new SelectList(Target(), "value", "text");
                //ViewBag.sllPosition = CreateSSL("Chưa đặt vị trí", "Logo", "Slide", "Icon Dịch vụ");
                ViewBag.sllTarget = CreateTarget("_blank", "_self", "_parent", "_top");
                var pos = advertiseEdit.Position;
                ViewBag.sllPosition = new SelectList(Position(), "value", "text", pos);
                return View(advertiseEdit);
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }

        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection collection, Rico.Models.Advertise advertise)
        {
            if (Request.Cookies["Username"] != null)
            {
                advertise.Name = collection["Name"];
                advertise.Image = collection["Image"];
                advertise.Height = Convert.ToInt32(collection["Height"]);
                advertise.Width = Convert.ToInt32(collection["Width"]);
                advertise.Position = Convert.ToInt32(collection["sllPosition"]);
                advertise.Link = collection["Link"];
                advertise.Target = collection["sllTarget"];
                advertise.Ord = Convert.ToInt32(collection["Ord"]);
                advertise.Active = (collection["Active"] == "false") ? false : true;
                advertise.Detail = collection["Detail"];
                db.sp_Advertise_Update(advertise.Id, advertise.Name, advertise.Detail, advertise.Target, advertise.Link, advertise.Image, advertise.Active, advertise.Width, advertise.Height, advertise.Position, advertise.Ord);              
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }

        #endregion Edit

        #region DeleteItem
        public ActionResult Delete(int id, int trang, int kichco)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "1"))
            {

                var aDelete = db.Advertises.Where(m => m.Id == id).SingleOrDefault();

                db.sp_Advertise_Delete(aDelete.Id);
                //db.Advertises.Remove(aDelete);
                db.SaveChanges();
                List<sp_Advertise_GetByAll_Result> adv = db.sp_Advertise_GetByAll().ToList();
                //List<Rico.Models.Advertise> adv = db.Advertises.ToList();

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


        #region[MultiCommand]
        [HttpPost]
        public ActionResult MultiCommand(FormCollection collect)
        {
            if (Request.Cookies["Username"] != null)
            {
                if (collect["btnDeleteAll"] != null)
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
                                var Del = db.sp_Advertise_GetById(id).SingleOrDefault();
                                //var Del = db.Advertises.Where(sp => sp.Id == id).SingleOrDefault();
                                db.sp_Advertise_Delete(Del.Id);
                                //db.Advertises.Remove(Del);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else if (collect["ddlAdvertiseAmount"] != null)
                {
                    if (collect["ddlAdvertiseAmount"].Length > 0)
                    {
                        Session["AdvertiseAmount"] = collect["ddlAdvertiseAmount"];
                    }
                    return RedirectToAction("Index");
                }

                else if (collect["btnSearch"] != null)
                {
                    if (collect["AdvertiseName"].Length > 0)
                    {
                        Session["AdvertiseName"] = collect["AdvertiseName"];
                    }
                    return RedirectToAction("Index");
                }
                else if (collect["sllPosition"] != null)
                {
                    //string i = collect["sllPossition"];
                    Session["sllPosition"] = collect["sllPosition"];
                    return RedirectToAction("Index", "Advertise");
                }
                else
                {
                    foreach (string key in Request.Form)
                    {
                        if (key.StartsWith("Ord"))
                        {
                            Int32 id = Convert.ToInt32(key.Remove(0, 3));

                            var Up = db.Advertises.Where(sp => sp.Id == id).FirstOrDefault();

                            if (Up != null)
                            {
                                if (!collect["Ord" + id].Equals(""))
                                {
                                    Up.Ord = int.Parse(collect["Ord" + id]);
                                }
                                //db.sp_Advertise_Update(Up.Id, Up.Name, Up.Detail, Up.Target, Up.Link, Up.Image, Up.Active, Up.Width, Up.Height, Up.Position, Up.Ord);
                                db.Entry(Up).State = System.Data.EntityState.Modified;
                                db.SaveChanges();
                            }

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

        #region[ChangeActive]
        public ActionResult ChangeActive(int id)
        {
            var advertise = db.Advertises.Find(id);
            if (advertise != null)
                advertise.Active = advertise.Active == true ? false : true;
            //db.sp_Advertise_Update(advertise.Id, advertise.Name, advertise.Detail, advertise.Target, advertise.Link, advertise.Image, advertise.Active, advertise.Width, advertise.Height, advertise.Position, advertise.Ord);
            db.Entry(advertise).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            var result = "Trạng thái kích hoạt đã được thay đổi.";
            return Json(result);
        }
        #endregion

        #region[UpdateDirect]
        public ActionResult UpdateDirect(int id, string ord, string width, string height, string name, string image, string link)
        {
            var advertise = db.Advertises.Find(id);
            var result = string.Empty;
            if (advertise != null)
            {
                if (ord != null)
                {
                    advertise.Ord = Convert.ToInt32(ord);
                    result = "Thứ tự Link ảnh đã được thay đổi.";
                }
                else if (width != null)
                {
                    advertise.Width = Convert.ToInt32(width);
                    result = "Độ rộng Link ảnh đã được thay đổi.";
                }
                else if (height != null)
                {
                    advertise.Height = Convert.ToInt32(height);
                    result = "Chiều cao Link ảnh đã được thay đổi.";
                }
                else if (name != null)
                {
                    advertise.Name = name;
                    result = "Tên Link ảnh đã được thay đổi.";
                }
                else if (link != null)
                {
                    advertise.Link = link;
                    result = "Link Link ảnh đã được thay đổi.";
                }
                else if (image != null)
                {
                    advertise.Image = image;
                    result = "Ảnh Link ảnh đã được thay đổi.";
                }
                else
                {
                    advertise.Active = advertise.Active == true ? false : true;
                    result = "Trạng thái Link ảnh đã được thay đổi.";
                }
            }
            //db.sp_Advertise_Update(advertise.Id, advertise.Name, advertise.Detail, advertise.Target, advertise.Link, advertise.Image, advertise.Active, advertise.Width, advertise.Height, advertise.Position, advertise.Ord);
            db.Entry(advertise).State = System.Data.EntityState.Modified;
            db.SaveChanges();
            return Json(result);
        }
        #endregion

        #region OtherMethods

        public List<SelectListItem> CreateSSL(string v1, string v2, string v3,string v4)       
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v1});
            sll.Add(new SelectListItem { Value = "1", Text = v2 });
            sll.Add(new SelectListItem { Value = "2", Text = v3 });
            sll.Add(new SelectListItem { Value = "3", Text = v4 });
            return sll;
        }


        public List<SelectListItem> CreateTarget(string v1, string v2, string v3, string v4)
        {
            List<SelectListItem> sll = new List<SelectListItem>();

            sll.Add(new SelectListItem { Value = "0", Text = v1 });
            sll.Add(new SelectListItem { Value = "1", Text = v2 });
            sll.Add(new SelectListItem { Value = "2", Text = v3 });
            sll.Add(new SelectListItem { Value = "3", Text = v4 });
            return sll;
        }

        

        #region Target
        public List<SelectListItem> Target()
        {
            List<SelectListItem> Target = new List<SelectListItem>();
            Target.Add(new SelectListItem { Value = "0", Text = "_blank" });
            Target.Add(new SelectListItem { Value = "1", Text = "_self" });
            Target.Add(new SelectListItem { Value = "2", Text = "_parent" });
            Target.Add(new SelectListItem { Value = "3", Text = "_top" });
            return Target;
        }
        #endregion Target


        #region Position
        public List<SelectListItem> Position()
        {
            List<SelectListItem> Position = new List<SelectListItem>();
            Position.Add(new SelectListItem { Value = "0", Text = "Chưa đặt vị trí" });
            Position.Add(new SelectListItem { Value = "1", Text = "Logo" });
            Position.Add(new SelectListItem { Value = "2", Text = "Slide" });
            Position.Add(new SelectListItem { Value = "3", Text = "Icon Dịch vụ" });

            return Position;
        }
        #endregion Possition
        #endregion

        #region AddMultileAdvertise

        #region[AddMutilImage]
        public ActionResult AddMulti()
        {
            if (Request.Cookies["Username"] != null)
            {
                ViewBag.sllPosition = CreateSSL("Chưa đặt vị trí", "Logo", "Slide","Icon Dịch vụ");
                //ViewBag.sllPosition = new SelectList(Position(), "value", "text");
                ViewBag.sllTarget = new SelectList(Target(), "value", "text");
                return View();
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion


        #region[AddMultilImage]
        [HttpPost]
        public ActionResult AddMulti(IEnumerable<HttpPostedFileBase> fileImg, FormCollection aForm)
        {
            if (Request.Cookies["Username"] != null)
            {
                int ID;
                var tmp = db.Advertises.Select(a => (int?)a.Id).Max();
                //if (tmp == null)
                //    tmp = 1;
                foreach (var file in fileImg)
                {
                    if (tmp == null)
                    { ID = 1; tmp = 1; }
                    else
                        ID = db.Advertises.Select(a => a.Id).Max();

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
                            Rico.Models.Advertise aImage = new Rico.Models.Advertise();
                            var Filename = Path.GetFileName(file.FileName);
                            //List<string> sizeImg = new List<string>();
                            //sizeImg.Add("_huge");
                            //sizeImg.Add("_big");
                            //sizeImg.Add("_noz");
                            //sizeImg.Add("_small");
                            //string co = "";
                            //string kco = "";
                            //for (int i = 0; i < sizeImg.Count; i++)
                            //{
                            //    var a = Filename.LastIndexOf(sizeImg[i]);
                            //    if (a > 0)
                            //    {
                            //        co = Filename.Substring(0, a);
                            //        kco = sizeImg[i];
                            //        break;
                            //    }
                            //}
                            //var fileName = String.Format("{0}" + kco + ".jpg", Guid.NewGuid().ToString());
                            //String imgPath = String.Format("Uploads/{0}{1}", file.FileName, FileExtn);
                            //file.Save(String.Format("{0}{1}", Server.MapPath("~"), imgPath), Img.RawFormat);
                            var path = Path.Combine(Server.MapPath(Url.Content("/Uploads/link")), Filename);
                            file.SaveAs(path);

                            if (Convert.ToInt32(aForm["sllPosition"]) == 0)
                                aImage.Position = 0;
                            else if (Convert.ToInt32(aForm["sllPosition"]) == 1)
                                aImage.Position = 1;
                            else if (Convert.ToInt32(aForm["sllPosition"]) == 2)
                                aImage.Position = 2;
                            else if (Convert.ToInt32(aForm["sllPosition"]) == 3)
                                aImage.Position = 3;

                            aImage.Id = ID + 1;
                            aImage.Name = "Link ảnh số " + ID;
                            aImage.Ord = 0;
                            //aImage.Click = 0;
                            aImage.Target = "_top";
                            aImage.Width = 0;
                            aImage.Height = 0;
                            aImage.Active = true;
                            //aImage.Position = 0;
                            aImage.Image = "/Uploads/link/" + Filename;
                            //img.Date = DateTime.Now;

                            //        db.sp_Advertise_Insert(aImage.Name, aImage.Detail, aImage.Target, aImage.Link, aImage.Image,
                            //aImage.Active, aImage.Width, aImage.Height, aImage.Position, aImage.Ord);
                            db.Advertises.Add(aImage);
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
        #endregion

        #endregion AddMultileAdvertise

        #region[Ajax LoadAdvertiseAmount]
        // AJAX: LoadAdvertiseAmount
        [HttpPost]
        public ActionResult LoadAdvertiseAmount(string advertiseAmount, string sortName)
        {

            if (advertiseAmount != null)
            {
                Session["AdvertiseAmount"] = advertiseAmount;
                if (sortName != null)
                {
                    Session["sortName"] = sortName;
                }
            }
            return RedirectToAction("Index");
        }


        #endregion

        #region Search
        [HttpPost]
        public ActionResult Search(string searchString, int? trang)
        {
            if (Request.Cookies["Username"] != null)
            {
                int pageSize = 4;
                int pageNumber = (trang ?? 1);

                PagedList<Rico.Models.Advertise> adv1 = (PagedList<Rico.Models.Advertise>)db.Advertises.Where(a => a.Name.ToUpper().Contains(searchString.ToUpper())).OrderByDescending(a => a.Name).ToPagedList(pageNumber, pageSize);
                //PagedList<sp_Advertise_GetByAll_Result> adv1 = (PagedList<sp_Advertise_GetByAll_Result>)db.sp_Advertise_GetByAll().Where(a => a.Name.ToUpper().Contains(searchString.ToUpper())).OrderByDescending(a => a.Name).ToPagedList(pageNumber, pageSize);

                if (adv1.Count > 0)
                    return PartialView("_Data", adv1);
                else
                    return PartialView("ErrorSearch");
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }
        }
        #endregion
    }
}
