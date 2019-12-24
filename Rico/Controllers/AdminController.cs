using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;
using Rico.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.Globalization;
using System.IO;

namespace Rico.Controllers
{
    public class AdminController : Controller
    {
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region[Mặc định (Default)]
        public ActionResult Default()
        {
            if ((Request.Cookies["Username"] != null) && (Session["Member"] != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        #endregion   
     
        #region[Cấu hình (ConfigEditor)]
        public ActionResult Config()
        {
            var config = db.Configs.FirstOrDefault();
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {
                if (config != null) { return View(config); }         
            }
            else { return RedirectToAction("Default", "Admin"); }
            return View();
        }
        #endregion

        #region[Cấu hình (ConfigEditor)]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Config(FormCollection collection)
        {
            if ((Request.Cookies["Username"] != null) && (Request.Cookies["Username"].Values["Permiss"].ToString() == "3") || (Request.Cookies["Username"].Values["Permiss"].ToString() == "2"))
            {
                var config = db.Configs.FirstOrDefault();
                //var name = collection["Name"];
                //var address = collection["Address"];
                var phone = collection["Phone"];
                //var hotline = collection["Hotline"];
                var website = collection["Website"];
                var email = collection["Email"];
                var facebook = collection["Facebook"];
                //var Mail_Port = collection["Mail_Port"];
                //var Mail_Info = collection["Mail_Info"];
                //var Mail_Noreply = collection["Mail_Noreply"];
                //var Mail_Password = collection["Mail_Password"];
                var faceappid = collection["FaceAppID"];
                var googleId = collection["GoogleId"];
                var copyright = collection["Copyright"];
                //var Payment = collection["Payment"];
                //var Warning = collection["Warning"];
                //var GoogleMap = collection["GoogleMap"];
                //var Video = collection["Video"];

                var title = collection["Title"];
                var description = collection["Description"];
                var keyword = collection["Keyword"];

                if (config != null)
                {
                    config.Phone = phone;    
                    config.Website = website;
                    config.Email = email;
                    config.Facebook = facebook;
                    config.FaceAppID = faceappid;
                    config.GoogleId = googleId;
                    //config.Mail_Port = short.Parse(Mail_Port);
                    //config.Mail_Info = Mail_Info;
                    //config.Mail_Noreply = Mail_Noreply;
                    //config.Mail_Password = Mail_Password;
                    config.Copyright = copyright;
                    //config.Payment = Payment;
                    //config.Warning = Warning;
                    //config.GoogleMap = GoogleMap;
                    //config.Video = Video;

                    config.Title = title;
                    config.Description = description;
                    config.Keyword = keyword;
                    //db.sp_Config_Update(config.Id, config.Title, config.Description, config.Keyword, config.Phone, config.Email, config.Website,
                    //    config.Facebook, config.FaceAppID, config.GoogleId, config.Copyright);
                    UpdateModel(config);
                    db.SaveChanges();
                    return RedirectToAction("Config", "Admin");
                }
                else
                {
                    config = new Config();
                    config.Phone = phone;
                    config.Website = website;
                    config.Email = email;
                    config.Facebook = facebook;
                    config.FaceAppID = faceappid;
                    //config.Contact = Contact;
                    //config.Mail_Port = short.Parse(Mail_Port);
                    //config.Mail_Info = Mail_Info;
                    //config.Mail_Noreply = Mail_Noreply;
                    //config.Mail_Password = Mail_Password;
                    config.GoogleId = googleId;
                    config.Copyright = copyright;
                    //config.Payment = Payment;
                    //config.Warning = Warning;
                    config.Title = title;
                    config.Description = description;
                    config.Keyword = keyword;
                    db.sp_Config_Insert(config.Title, config.Description, config.Keyword, config.Phone, config.Email, config.Website,
                       config.Facebook, config.FaceAppID, config.GoogleId, config.Copyright);
                    //db.Entry(config).State = EntityState.Added;
                    db.SaveChanges();
                    return RedirectToAction("Config", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Default", "Admin");
            }  
        }
        #endregion

        #region[Đăng nhập (Login)]
        public ActionResult Index()
        {
            if ((Request.Cookies["Username"] != null) && (Session["Member"] != null))
            {
                return RedirectToAction("Default");     
            }
            else
            {
                HttpCookie UserCookie = new HttpCookie("Username", "");
                UserCookie.Expires = DateTime.Now;
                Response.Cookies.Add(UserCookie);
                return View();
            }  
        }
        #endregion

        #region[Đăng nhập (Login)]
        [HttpPost]
        public ActionResult Index(FormCollection collect)
        {
            var user = collect["Username"];
            var pass = collect["Pass"];
            var list = db.Members.Where(n => n.Username == user && n.Password == pass).ToList();
            if (list.Count > 0)
            {
                HttpCookie UserCookie = new HttpCookie("Username");
                UserCookie.Values["UserNameText"] = user.ToString();
                UserCookie.Values["Permiss"] = list[0].Permiss.ToString();
                UserCookie.Values["PasswordText"] = pass.ToString();
                UserCookie.Values["FullName"] = Server.UrlEncode(list[0].Name.Trim());
                UserCookie.Expires = DateTime.Now.AddHours(2);
                Response.Cookies.Add(UserCookie);
                Session["Member"] = list;
                //Session["Member"] = UserCookie;           
                return RedirectToAction("Default", "Admin");
                //return RedirectToAction("Default", new { cookie = UserCookie });
            }
            else
            {
                ViewBag.Err = "Đăng nhập không thành công!";
                return View();
            }
        }
        #endregion

        #region[Đăng xuất (Logout)]
        public ActionResult Logout()
        {
            var cookie = new HttpCookie("Username");
            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);
            Session["Member"] = null;
            return RedirectToAction("Index");
        }
        #endregion

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
