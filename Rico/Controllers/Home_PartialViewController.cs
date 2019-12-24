using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Data.Linq;
using Rico.Models;
using Rico.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
using PagedList;
using PagedList.Mvc;


namespace Rico.Controllers
{
    public class Home_PartialViewController : Controller
    {
        //
        // GET: /Home_PartialView/
        DieutriungthuEntities db = new DieutriungthuEntities();
        #region[Logo]
        public ActionResult _Logo()
        {
            var img = db.Advertises.Where(p => p.Active == true && p.Position == 1).SingleOrDefault();
            string imgLink = img.Link;
            string imgSource = img.Image;
            string imgAlt = img.Name;
            ViewBag.ImgLink = imgLink;
            ViewBag.ImgSource = imgSource;
            ViewBag.ImgAlt = imgAlt;
            return PartialView();
        }
        #endregion

        #region [Main Menu]
        public ActionResult _MainMenu()
        {
            string chuoi = "";
            var cat = db.Menus.Where(c => c.Active == true && c.Level.Length == 5 && c.Position == 1 || c.Position == 4).OrderBy(c => c.Ord).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                List<Menu> menus = db.Menus.ToList();
                List<Menu> catsub = new List<Menu>();
                string levelm = cat[i].Level;
                catsub = db.Menus.Where(m => m.Level.Length == 10 && m.Level.Substring(0, 5) == levelm && m.Active == true).OrderBy(m => m.Level).ToList();

                if (catsub.Count > 0)
                {
                    if ((Request.Url.ToString().IndexOf(cat[i].Link) > 0 && cat[i].Link != "/"))                   
                    {
                        chuoi += "<li><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";
                    }
                    else                
                    {
                        chuoi += "<li><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>";
                    }
                    chuoi += "<ul>";
                    for (int k = 0; k < catsub.Count; k++)
                    {
                        string levelm10 = catsub[k].Level;
                        List<Menu> catsub10 = new List<Menu>();
                        catsub10 = db.Menus.Where(m => m.Level.Length == 15 && m.Level.Substring(0, 10) == levelm10 && m.Active == true).OrderBy(m => m.Level).ToList();
                        if (catsub10.Count == 0)
                        {
                            chuoi += "<li><a href=\"" + catsub[k].Link + "\"><span>" + catsub[k].Name + "</span></a></li>";
                        }
                        else
                        { 
                        chuoi += "<li><a href=\"" + catsub[k].Link + "\"><span>" + catsub[k].Name + "</span></a>";
                            chuoi += "<ul>";
                            for (int n = 0; n < catsub10.Count; n++)
                            {
                                string levelm15 = catsub10[n].Level;
                                List<Menu> catsub15 = new List<Menu>();
                                catsub15 = db.Menus.Where(m => m.Level.Length == 20 && m.Level.Substring(0, 15) == levelm15).OrderBy(m => m.Level).ToList();
                                if (catsub15.Count == 0)
                                {
                                    chuoi += "<li><a href=\"" + catsub10[n].Link + "\">" + catsub10[n].Name + "</a></li>";
                                }
                                else
                                {
                                    chuoi += "<li><a href=\"" + catsub10[n].Link + "\">" + catsub10[n].Name + "</a>";
                                    chuoi += "<ul>";
                                    for (int m = 0; m < catsub15.Count; m++)
                                    {
                                        chuoi += "<li><a href=\"" + catsub15[m].Link + "\">" + catsub15[m].Name + "</a></li>";
                                    }
                                    chuoi += "</ul></li>";
                                }
                            }
                            chuoi += "</ul></li>";
                        }
                    }
                    chuoi += "</ul></li>";
                }
                else
                {
                    if ((Request.Url.ToString().IndexOf(cat[i].Link) > 0 && cat[i].Link != "/"))
                    {
                        if (i == 0)
                        {
                            chuoi += "<li><a class=\"home\" title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                        }
                        else
                        {
                            chuoi += "<li><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                        }
                    }
                    //chuoi += "<li class=\"active\"><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";                    
                    else
                    {
                        if (i == 0)
                        {
                            chuoi += "<li><a class=\"home\" title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                        }
                        else
                        {
                            chuoi += "<li><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a></li>";
                        }
                    }
                }
            }

            ViewBag.MainMenu = chuoi;
            cat.Clear();            
            return PartialView();
        }
        #endregion

        #region [Post Menu]
        public ActionResult _PostMenu()
        {
            string chuoi = "";
            var cat = db.Menus.Where(c => c.Active == true && c.Level.Length == 10 && c.Position == 2).OrderBy(c => c.Ord).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                List<Menu> menus = db.Menus.ToList();
                List<Menu> catsub = new List<Menu>();
                string levelm = cat[i].Level;
                catsub = db.Menus.Where(m => m.Level.Length == 15 && m.Level.Substring(0, 10) == levelm && m.Active == true).OrderBy(m => m.Level).ToList();

                if (catsub.Count > 0)
                {
                    if ((Request.Url.ToString().IndexOf(cat[i].Link) > 0 && cat[i].Link != "/"))
                    {                      
                        chuoi += "<li><div class=\"category-title active\"><h3 title=\"" + cat[i].Title + "\">" + cat[i].Name + "</h3></div>";
                        //chuoi += "<li><div class=\"category-title active\"><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link+"\"><h3>" + cat[i].Name + "</h3></a></div>";
                    }
                    else
                    {                        
                        chuoi += "<li><div class=\"category-title\"><h3 title=\"" + cat[i].Title + "\">" + cat[i].Name + "</h3></div>";
                        //chuoi += "<li><div class=\"category-title\"><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\"><h3>" + cat[i].Name + "</h3></a></div>";

                    }
                    chuoi += "<div class=\"list\"><ul>";
                    for (int k = 0; k < catsub.Count; k++)
                    {
                        if ((Request.Url.ToString().IndexOf(catsub[k].Link) > 0 && catsub[k].Link != "/"))
                        {
                            chuoi += "<li><a class=\"active\" title=\"" + catsub[k].Title + "\" href=\"" + catsub[k].Link + "\"><span>" + catsub[k].Name + "</span></a></li>";
                        }
                        else
                        {
                            chuoi += "<li><a title=\"" + catsub[k].Title + "\" href=\"" + catsub[k].Link + "\"><span>" + catsub[k].Name + "</span></a></li>";
                        }
                    }
                    chuoi += "</ul></div></li>";
                }
                else
                {
                    if ((Request.Url.ToString().IndexOf(cat[i].Link) > 0 && cat[i].Link != "/"))
                    {                       
                        chuoi += "<li><div class=\"category-title active\"><h3 title=\"" + cat[i].Title + "\">" + cat[i].Name + "</h3></div></li>";
                        //chuoi += "<li><div class=\"category-title active\"><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\"><h3>" + cat[i].Name + "</h3></a></div></li>";
                    }
                                       
                    else
                    {                       
                        chuoi += "<li><div class=\"category-title\"><h3 title=\"" + cat[i].Title + "\">" + cat[i].Name + "</h3></div></li>";
                        //chuoi += "<li><div class=\"category-title\"><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\"><h3>" + cat[i].Name + "</h3></a></div></li>";
                    }
                }
            }
            ViewBag.PostMenu = chuoi;
            cat.Clear();           
            return PartialView();
        }
        #endregion

        #region [Nav Menu]
        public ActionResult _NavMenu()
        {
            
            string nav = "";
            var cat = db.Menus.Where(c => c.Active == true && c.Level.Length == 10 && c.Position == 2).OrderBy(c => c.Ord).ToList();
            for (int i = 0; i < cat.Count; i++)
            {                
                        nav += "<li><a title=\"" + cat[i].Title + "\" href=\"" + cat[i].Link + "\"><span>" + cat[i].Name + "</span></a></li>";                       
            }

            ViewBag.NavMenu = nav;
            cat.Clear();
            return PartialView();
        }
        #endregion    

        #region[Carousel Slide]
        public ActionResult _Slide()
        {
            //var sl = db.Pictures.Where(s => s.Active == true && s.Position == 3).SingleOrDefault();
            var sl = db.Advertises.Where(s => s.Active == true && s.Position == 2).OrderBy(s => s.Ord).ToList();
            string str2 = "";
            str2 += "<ol class=\"carousel-indicators\">";
            for (int i = 0; i < sl.Count; i++)
            {
                if (i == 0)
                {
                    str2 += "<li data-target=\"#myCarousel\" data-slide-to=\"" + i + "\" class=\"active\"></li>";
                }
                else
                {
                    str2 += "<li data-target=\"#myCarousel\" data-slide-to=\"" + i + "\"></li>";
                }
            }
            str2 += "</ol>";
            str2 += "<div class=\"carousel-inner\" role=\"listbox\">";
            for (int i = 0; i < sl.Count; i++)
            {
                if (i == 0)
                {
                    str2 += "<div class=\"item active\"><a href=\"" + sl[i].Link + "\"><img src=\"" + sl[i].Image + "\" alt=\"" + sl[i].Name + "\" width=\"460\" height=\"345\"></a></div>";
                }
                else
                {
                    str2 += "<div class=\"item\"><a href=\"" + sl[i].Link + "\"><img src=\"" + sl[i].Image + "\" alt=\"" + sl[i].Name + "\" width=\"460\" height=\"345\"></a></div>";
                }
            }
            str2 += "</div>";
            ViewBag.Slide = str2;
            return PartialView();
        }
        #endregion Slide

        #region[Supersized Slide]
        public ActionResult _FullBackgroundSlide()
        {
            //var sl = db.Pictures.Where(s => s.Active == true && s.Position == 3).SingleOrDefault();
            var sl = db.Advertises.Where(s => s.Active == true && s.Position == 2).OrderBy(s => s.Ord).ToList();
            string str2 = "";

            for (int i = 0; i < sl.Count; i++)
            {
                if (i != sl.Count - 1)
                {
                    str2 += "{image: \'" + sl[i].Image + "\', ";
                    str2 += "title: \'" + sl[i].Name + "\', ";
                    str2 += "thumb: \'" + sl[i].Image + "\', ";
                    str2 += "url: \'" + sl[i].Link + "\'},";

                }
                else
                {
                    str2 += "{image: \'" + sl[i].Image + "\', ";
                    str2 += "title: \'" + sl[i].Name + "\', ";
                    str2 += "thumb: \'" + sl[i].Image + "\', ";
                    str2 += "url: \'" + sl[i].Link + "\'}";
                }
            }

            ViewBag.FullBackGroundSlide = str2;
            return PartialView();
        }

        #endregion

        #region[Vegas]
        public ActionResult _Vegas()
        {
            return PartialView();
        }
        #endregion

        #region[Photo Album]
        public ActionResult _PhotoAlbum()
        {
            var album = db.GroupPictures.Where(al => al.Name == "Album ảnh cưới").Single();
            string str = "";
            string albumname = album.Name;
            ViewBag.AlbumName = albumname;
            string albumlink = "/danh-muc-anh/" + album.Tag;
            ViewBag.AlbumLink = albumlink;
            var gphoto = db.GroupPictures.Where(g => g.ParentId == album.Id && g.Index == true && g.Active == true).OrderByDescending(g => g.Ord).ToList();
            for (int i = 0; i < gphoto.Count; i++)
            {              
                    str += "<li><a href=\"/anh/" + gphoto[i].Tag + "\"><img alt=\"" + gphoto[i].Name + "\" src=\"" + gphoto[i].Image + "\" /></a></li>";            
            }

            ViewBag.PhotoAlbum = str;
            return PartialView();
        }
        #endregion

        #region[Dịch vụ]
        public ActionResult _Services()
        {
            var gp = db.GroupNews.Where(g => g.Active == true && g.Name == "Dịch vụ").Single();
            string gpLevel = gp.Level;
            string str = "";
            var list = db.GroupNews.Where(l => l.Level.Substring(0, 5) == gpLevel && l.Level != gpLevel && l.Active == true && l.Index == true).OrderBy(l => l.Ord).ToList();
            //string str = "";
            //var menu=db.Menus.Where(g => g.Active == true && g.Name == "Dịch vụ").Single();
            //string mLevel = menu.Level;
            //var list = db.Menus.Where(l => l.Level.Substring(0, 5) == mLevel && l.Level!= mLevel && l.Active == true).OrderBy(l => l.Ord).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                str += "<li class=\"home-services-wrapper\"><div class=\"services-img\"><a href=\"/danh-muc-tin/" + list[i].Tag + "\"><img src=\"" + list[i].Image + "\" alt=\"" + list[i].Name + "\" /></a></div>";
                str += "<h1 class=\"service-title\"><a href=\"/danh-muc-tin/" + list[i].Tag + "\">" + list[i].Name + "</a></h1>";
                str += "<div class=\"describe\">" + list[i].Detail + "</div></li>";
            }
            ViewBag.HomeServices = str;
            return PartialView();
        }
        #endregion

        #region[Ao cuoi hien dai]
        public ActionResult _AoCuoiHienDai()
        {
            var album = db.GroupPictures.Where(al => al.Name == "Áo cưới hiện đại").Single();
            string str = "";   
            var photo = db.GroupPictures.Where(g => g.ParentId == album.Id && g.Index == true && g.Active == true).OrderByDescending(g => g.Ord).ToList();
            for (int i = 0; i < photo.Count; i++)
            {
                str += "<li><a href=\"/anh/" + photo[i].Tag + "\"><img alt=\"" + photo[i].Name + "\" src=\"" + photo[i].Image + "\" /></a></li>";
            }

            ViewBag.AoCuoiHienDai = str;
            return PartialView();
        }
        #endregion

        #region[Ao cuoi TruyenThong]
        public ActionResult _AoCuoiTruyenThong()
        {
          
            var album = db.GroupPictures.Where(b => b.Name == "Áo dài truyền thống").Single();
            string str = "";
            var photo = db.GroupPictures.Where(g => g.ParentId == album.Id && g.Index == true && g.Active == true).OrderByDescending(g => g.Ord).ToList();
            for (int i = 0; i < photo.Count; i++)
            {
                str += "<li><a href=\"/anh/" + photo[i].Tag + "\"><img alt=\"" + photo[i].Name + "\" src=\"" + photo[i].Image + "\" /></a></li>";
            }

            ViewBag.AoCuoiTruyenThong = str;
            return PartialView();          
        }
        #endregion

        #region[Make Up]
        public ActionResult _MakeUp()
        {
            var grp = db.GroupPictures.Where(al => al.Name == "Trang điểm").Single();
            string str = "";
            string muname = grp.Name;
            ViewBag.MUName = muname;
            string grplink = "/danh-muc-anh/" + grp.Tag;
            ViewBag.MULink = grplink;
            var ph = db.Pictures.Where(g => g.IdCategory == grp.Id && g.Index == true && g.Active == true).OrderByDescending(g => g.Ord).ToList();
            //var gphoto = db.GroupPictures.Where(g => g.ParentId == grp.Id && g.Index == true && g.Active == true).OrderByDescending(g => g.Ord).ToList();
            if (ph != null) {
                for (int i = 0; i < ph.Count; i++)
                {
                    str += "<li><img alt=\"" + muname + "\" src=\"" + ph[i].Image + "\" /></li>";
                }
            }
            ViewBag.Str = str;

            return PartialView();
        }
        #endregion

        #region[News]
        public ActionResult _News()
        {
            var gn = db.GroupNews.Where(g => g.Name == "Cẩm nang cưới").Single();
            string gLevel = gn.Level;
            string str = "";
            List<News> sum = new List<News>();
            var cate = db.GroupNews.Where(l => l.Level.Substring(0, 5) == gLevel && l.Active==true).OrderBy(l => l.Ord).ToList();
            for (int i = 0; i < cate.Count; i++)
            {
                int cId = cate[i].Id;              
                var list = db.News.Where(n => n.Active == true && n.IdGroup == cId).OrderByDescending(n => n.SDate).Take(4).ToList();                                         
                sum.AddRange(list);    
            }
            sum = db.News.OrderByDescending(ss => ss.SDate).Take(6).ToList();
            for (int j = 0; j < sum.Count; j++)
            {
                str += "<div class=\"col-xs-12 col-sm-4 home-post\">";
                str += "<article><h3><a href=\"/tin/" + sum[j].Tag + "\">" + sum[j].Name + "</a></h3>";
                str += "<figure><img class=\"img-responsive\" src=\"" + sum[j].Image + "\" alt=\"" + sum[j].Name + "\" /></figure>";
                str += "<p>" + sum[j].Content + "</p></article></div>";
            }

            ViewBag.News = str;
            return PartialView();
        }
        #endregion

        #region[Contact]
        public ActionResult _Contact()
        {
            var config = db.Configs.Where(i => i.Id == 1).Single();

            string w = config.Website;
            string e = config.Email;
            string t = config.Title;
            string d = config.Description;
            string k = config.Keyword;
            string f = config.Facebook;
            string g = config.GoogleId;
            string c = config.Copyright;
            string p = config.Phone;
            ViewBag.Website = w;
            ViewBag.Email = e;
            ViewBag.Title = t;
            ViewBag.Description = d;
            ViewBag.Keyword = k;
            ViewBag.Facebook = f;
            ViewBag.GoogleId = g;
            ViewBag.Copyright = c;
            ViewBag.Phone = p;
            return PartialView();
        }
        #endregion

        #region[Footer]
        public ActionResult _Footer()
        {
            var config = db.Configs.Where(i => i.Id == 1).Single();

            //string w = config.Website;
            string e = config.Email;
            string t = config.Title;
            string d = config.Description;
            string k = config.Keyword;
            string f = config.Facebook;
            string g = config.GoogleId;
            string c = config.Copyright;

            //ViewBag.Website = w;
            ViewBag.Email = e;
            ViewBag.Title = t;
            ViewBag.Description = d;
            ViewBag.Keyword = k;
            ViewBag.Facebook = f;
            ViewBag.GoogleId = g;
            ViewBag.Copyright = c;
            return PartialView();
        }
        #endregion

        #region[Support]
        public ActionResult _Support()
        {
            var sp = db.Supports.Where(s => s.Active == true).OrderBy(s => s.Ord).ToList();
            
            string str = "";
            for(int i = 0; i<sp.Count;i++)           
            {
            str+="<p><a class=\"hotline\" href=\"tel:"+sp[i].Tel+"\">"+sp[i].Tel+"</a>";
            str+="<a href=\"Skype:"+sp[i].Skype+"\"><img class=\"img_sk\" src=\"/Content/Images/skype-mini.png\"></a></p>";
            }
            ViewBag.Support = str;
            return PartialView();
        }
        #endregion

        public ActionResult Statistic()
        {
            ViewBag.PageView = HttpContext.Application["DaTruyCap"].ToString();
            ViewBag.Online = HttpContext.Application["DangTruyCap"].ToString();
            return PartialView();
        }

         
        public ActionResult _Facebook()
        {
            var fb = db.Configs.Where(f => f.Id == 1).Single();
            string fbID = fb.FaceAppID;
            string fbLink = fb.Facebook;
            ViewBag.FbID = fbID;
            ViewBag.FbLink = fbLink;
            return PartialView();
        }
     

       // #region[Slide]
       // public ActionResult _MainSlide()
       // {
       //     var sl = db.Pictures.Where(s => s.Active == true && s.Position == 3).OrderBy(s => s.Ord).ToList();
       //     string[] str = { 
       //                        //"cubeRandom", 
       // //"block", "cubeStop", "cubeStopRandom", "cubeHide", "cubeSize", "fadeFour", "paralell", "blind", "blindHeight", "blindWidth", "directionTop", "directionBottom", "directionRight", "directionLeft", "cubeSpread", "glassCube", "glassBlock", "circlesRotate", "cubeShow", "upBars", "downBars", "hideBars", "swapBars", "swapBarsBack", "swapBlocks", "cut" };
       //"block", "cubeStop", "cubeStopRandom", "cubeHide", "cubeSize", "fadeFour", "paralell", "blind", "blindHeight", "blindWidth", "directionTop", "directionBottom", "directionRight", "directionLeft", "cubeSpread", "glassCube", "glassBlock", "circlesRotate", "cubeShow", "upBars", "downBars", "hideBars", "swapBars", "swapBarsBack", "swapBlocks", "cut" };
       //     string str2 = "";
       //     for (int i = 0; i < sl.Count; i++)
       //     {
       //         str2 += "<li><a target=\'_blank\' href=\'" + sl[i].Link + "\'><img class=\"" + str[i].ToString() + "\" src=\"" + sl[i].Image + "\" title=\'\'></a><div class=\'label_text\'><div class=\'lablecontent\'><a href=\'\' class=\'name\'></a><span class=\'clear\'></span><p class=\'des\'></p><div class=\'clear\'></div></div></div></li>";
       //     }
       //     ViewBag.MainSlide = str2;
       //     return PartialView();
       // }
       // #endregion Slide
        //#region[Latest News]
        //public ActionResult _LatestNews()
        //{
        //    string ln = "";
        //    string topNewsName = "";
        //    string topNewsLink = "";
        //    string topNewsDesc = "";
        //    string topNewsImg = "";
        //    var latest = db.News.Where(n => n.Active == true && n.Index == true).OrderByDescending(n => n.MDate).Take(10).ToList();

        //    for (int i = 0; i < latest.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            topNewsName = latest[i].Name;
        //            topNewsLink = latest[i].Tag;
        //            topNewsDesc = latest[i].Content;
        //            topNewsImg = latest[i].Image;
        //        }
        //        else
        //        {
        //            ln += "<li><a href=\"/" + latest[i].Tag + "\" title=\"" + latest[i].Title + "\">" + latest[i].Name + "</a><span>  (" + latest[i].MDate.ToShortDateString() + ")</span></li>";
        //        }
        //    }
        //    ViewBag.LatestNews = ln;
        //    ViewBag.TopNewsName = topNewsName;
        //    ViewBag.TopNewsLink = topNewsLink;
        //    ViewBag.TopNewsDesc = topNewsDesc;
        //    ViewBag.TopNewsImg = topNewsImg;
        //    latest.Clear();
        //    return PartialView();
        //}
        //#endregion

        //#region[Training]
        //public ActionResult _Training()
        //{
        //    var rootGroupNews = db.GroupNews.Where(r => r.Active == true && r.Index == true && r.Name == "Đào tạo").SingleOrDefault();
        //    var menu = db.Menus.Where(m => m.Active == true && m.Tag == "dao-tao").SingleOrDefault();
        //    string menuName = menu.Name;
        //    string menuLink = menu.Link;
        //    string rootLevel = rootGroupNews.Level;
        //    string newsList = "";
        //    string groupNewsList = "";
        //    string topNewsLink = "";
        //    string topNewsName = "";
        //    string topNewsDesc = "";
        //    string topNewsImg = "";
        //    string groupNewsIDs = "";
        //    string strNews = "";

        //    List<News> list = new List<News>();

        //    var unsortedNews = db.News.Where(n => n.Active == true && n.Index == true).OrderByDescending(n => n.SDate).ToList();
        //    var group = db.GroupNews.Where(g => g.Active == true & g.Index == true && g.Level.Length == 10 && g.Level.Substring(0, 5) == rootLevel).OrderBy(g => g.Ord).ToList();
        //    for (int i = 0; i < group.Count; i++)
        //    {
        //        groupNewsIDs += group[i].Id.ToString();
        //        groupNewsList += "<li><a href=\"/menu-con/" + group[i].Tag + "\" title=\"" + group[i].Title + "\">" + group[i].Name + "</a></li>";

        //    }
        //    for (int j = 0; j < unsortedNews.Count; j++)
        //    {
        //        strNews = unsortedNews[j].IdGroup.ToString();
        //        if (groupNewsIDs.Contains(strNews))
        //        {
        //            list.Add(unsortedNews[j]);
        //        }
        //    }
        //    list=list.OrderByDescending(l => l.MDate).Take(4).ToList();
        //    for (int k = 0; k < list.Count; k++)
        //    {
        //        if (k == 0)
        //        {
        //            topNewsName = list[k].Name;
        //            topNewsLink = list[k].Tag;
        //            topNewsDesc = list[k].Content;
        //            topNewsImg = list[k].Image;
        //        }
        //        else
        //        {
        //            newsList += "<li><a href=\"/" + list[k].Tag + "\" title=\"" + list[k].Title + "\">" + list[k].Name + "</a><span>  (" + list[k].SDate.ToShortDateString() + ")</span></li>";
        //        }
        //    }
        //    ViewBag.TopNewsName = topNewsName;
        //    ViewBag.TopNewsLink = topNewsLink;
        //    ViewBag.TopNewsDesc = topNewsDesc;
        //    ViewBag.TopNewsImg = topNewsImg;
        //    ViewBag.NewsList1 = newsList;
        //    ViewBag.GroupNewsList = groupNewsList;
        //    ViewBag.MenuName = menuName;
        //    ViewBag.MenuLink = menuLink;
        //    unsortedNews.Clear();
        //    unsortedNews = null;
        //    list.Clear();
        //    list = null;
        //    newsList = null;
        //    return PartialView();
        //}
        //#endregion


        //#region[Admissions]
        //public ActionResult _Admissions()
        //{
        //    var rootGroupNews = db.GroupNews.Where(r => r.Active == true && r.Index == true && r.Name == "Tuyển sinh").SingleOrDefault();
        //    var menu = db.Menus.Where(m => m.Active == true && m.Tag == "tuyen-sinh").SingleOrDefault();
        //    string menuName = menu.Name;
        //    string menuLink = menu.Link;
        //    string rootLevel = rootGroupNews.Level;
        //    string newsList = "";
        //    string groupNewsList = "";
        //    string topNewsLink = "";
        //    string topNewsName = "";
        //    string topNewsDesc = "";
        //    string topNewsImg = "";
        //    string groupNewsIDs = "";
        //    string strNews = "";

        //    List<News> list = new List<News>();

        //    var unsortedNews = db.News.Where(n => n.Active == true && n.Index == true).OrderByDescending(n => n.SDate).ToList();
        //    var group = db.GroupNews.Where(g => g.Active == true & g.Index == true && g.Level.Length == 10 && g.Level.Substring(0, 5) == rootLevel).OrderBy(g => g.Ord).ToList();
        //    for (int i = 0; i < group.Count; i++)
        //    {
        //        groupNewsIDs += group[i].Id.ToString();
        //        groupNewsList += "<li><a href=\"/menu-con/" + group[i].Tag + "\" title=\"" + group[i].Title + "\">" + group[i].Name + "</a></li>";

        //    }
        //    for (int j = 0; j < unsortedNews.Count; j++)
        //    {
        //        strNews = unsortedNews[j].IdGroup.ToString();
        //        if (groupNewsIDs.Contains(strNews))
        //        {
        //            list.Add(unsortedNews[j]);
        //        }
        //    }
        //    list=list.OrderByDescending(l => l.MDate).Take(4).ToList();
        //    for (int k = 0; k < list.Count; k++)
        //    {
        //        if (k == 0)
        //        {
        //            topNewsName = list[k].Name;
        //            topNewsLink = list[k].Tag;
        //            topNewsDesc = list[k].Content;
        //            topNewsImg = list[k].Image;
        //        }
        //        else
        //        {
        //            newsList += "<li><a href=\"/" + list[k].Tag + "\" title=\"" + list[k].Title + "\">" + list[k].Name + "</a><span>  (" + list[k].MDate.ToShortDateString() + ")</span></li>";
        //        }
        //    }
        //    ViewBag.TopNewsName = topNewsName;
        //    ViewBag.TopNewsLink = topNewsLink;
        //    ViewBag.TopNewsDesc = topNewsDesc;
        //    ViewBag.TopNewsImg = topNewsImg;
        //    ViewBag.NewsList2 = newsList;
        //    ViewBag.GroupNewsList = groupNewsList;
        //    ViewBag.MenuName = menuName;
        //    ViewBag.MenuLink = menuLink;
        //    unsortedNews.Clear();
        //    unsortedNews = null;
        //    list.Clear();
        //    list = null;
        //    newsList = null;
        //    return PartialView();
        //}
        //#endregion

       

        #region[Announcement]
        public ActionResult _Announcement()
        {
            string l = "";
            var group = db.GroupPictures.Where(g => g.Name == ("Sức khỏe học đường")).SingleOrDefault();
            string gTag=group.Tag;
            var menu = db.Menus.Where(m => m.Tag == gTag).SingleOrDefault();
            string menuName = menu.Name;
            string menuLink = menu.Link;
            int id = group.Id;
            var news = db.News.Where(n => n.Active == true && n.Index == true && n.IdGroup ==id).OrderByDescending(n => n.SDate).Take(10).ToList();

            for (int i = 0; i < news.Count; i++)
            {
                l += "<li><a href=\"/" + news[i].Tag + "\" title=\"" + news[i].Title + "\">" + news[i].Name + "</a></li>";
            }
            ViewBag.Announcement = l;
            ViewBag.MenuName = menuName;
            ViewBag.MenuLink = menuLink;
            return PartialView();
        }
        #endregion

        #region[Announcement]
        public ActionResult _Announcement2()
        {
            string l = "";
            var group = db.GroupPictures.Where(g => g.Name == ("Tin giáo dục trong nước")).SingleOrDefault();
            string gTag = group.Tag;
            var menu = db.Menus.Where(m => m.Tag == gTag).SingleOrDefault();
            string menuName = menu.Name;
            string menuLink = menu.Link;
            int id = group.Id;
            var news = db.News.Where(n => n.Active == true && n.Index == true && n.IdGroup == id).OrderByDescending(n => n.SDate).Take(10).ToList();

            for (int i = 0; i < news.Count; i++)
            {
                l += "<li><a href=\"/" + news[i].Tag + "\" title=\"" + news[i].Title + "\">" + news[i].Name + "</a></li>";
            }
            ViewBag.Announcement = l;
            ViewBag.MenuName = menuName;
            ViewBag.MenuLink = menuLink;
            return PartialView();
        }
        #endregion

        #region[Announcement]
        public ActionResult _Announcement3()
        {
            string l = "";
            var group = db.GroupPictures.Where(g => g.Name == ("Tin tức xã hội")).SingleOrDefault();
            string gTag = group.Tag;
            var menu = db.Menus.Where(m => m.Tag == gTag).SingleOrDefault();
            string menuName = menu.Name;
            string menuLink = menu.Link;
            int id = group.Id;
            var news = db.News.Where(n => n.Active == true && n.Index == true && n.IdGroup == id).OrderByDescending(n => n.SDate).Take(10).ToList();

            for (int i = 0; i < news.Count; i++)
            {
                l += "<li><a href=\"/" + news[i].Tag + "\" title=\"" + news[i].Title + "\">" + news[i].Name + "</a></li>";
            }
            ViewBag.Announcement = l;
            ViewBag.MenuName = menuName;
            ViewBag.MenuLink = menuLink;
            return PartialView();
        }
        #endregion

        #region[Announcement]
        public ActionResult _Announcement4()
        {
            string l = "";
            var group = db.GroupPictures.Where(g => g.Name == ("Hướng nghiệp")).SingleOrDefault();
            string gTag = group.Tag;
            var menu = db.Menus.Where(m => m.Tag == gTag).SingleOrDefault();
            string menuName = menu.Name;
            string menuLink = menu.Link;
            int id = group.Id;
            var news = db.News.Where(n => n.Active == true && n.Index == true && n.IdGroup == id).OrderByDescending(n => n.SDate).Take(10).ToList();

            for (int i = 0; i < news.Count; i++)
            {
                l += "<li><a href=\"/" + news[i].Tag + "\" title=\"" + news[i].Title + "\">" + news[i].Name + "</a></li>";
            }
            ViewBag.Announcement = l;
            ViewBag.MenuName = menuName;
            ViewBag.MenuLink = menuLink;
            return PartialView();
        }
        #endregion
         
     

        #region[Left Menu]
        public ActionResult _MenuFooter()
        {
            string f = "";
            var cat = db.Menus.Where(c => c.Active == true && c.Level.Length == 5 && c.Position == 1 || c.Position == 4).OrderBy(c => c.Ord).ToList();
            for (int i = 0; i < cat.Count; i++)
            {
                f += "<td><a href=\"" + cat[i].Link + "\">" + cat[i].Name + "</a>&nbsp; &nbsp; &nbsp;| &nbsp; &nbsp; &nbsp;</td>";                            
            }
            ViewBag.MenuFooter = f;
            return PartialView();
        }
        #endregion

         #region[Linked]
         public ActionResult _Linked()
         {
             return PartialView();
         }
         #endregion

         //#region[Album]
         //public ActionResult _Album()
         //{
         //    string album = "";
         //    var p = db.Pictures.Where(n => n.Position == 3 && n.Active == true).OrderBy(n=>n.Ord).ToList();
         //    for(int i=0;i<p.Count;i++)
         //    {
         //        album+="<div align=\"center\"><span style=\"color:#164c92\">";
         //        album+="<strong>"+p[i].Name+"</strong></span></div>";
         //       //album += "<div align=\"center\"><img src=\"" + p[i].Image + "\" title=\"" + p[i].Name + "\" border=\"0\" width=\"182px\"></div>";
         //       album += "<div align=\"center\"><a href=\"" + p[i].Image + "\" title=\"" + p[i].Name + "\">";
         //       album += "<img src=\"" + p[i].Image + "\" title=\"" + p[i].Name + "\" border=\"0\" width=\"182px\"></a></div>";
         //       album +="<div class=\"frameline\" style=\"padding-bottom:7px;padding-top:5px;\"><div class=\"vienxanh\"></div></div>";
         //    }
         //    ViewBag.Album = album;
            
         //    return PartialView();
         //}
         //#endregion

         #region[Statistic]
         public ActionResult _Statistic()
         {
             return PartialView();
         }
         #endregion


        // #region[NhaTaiTro]
        // public ActionResult _NhaTaiTro()
        // {
        //     string chuoi="";
        //     var ntt = db.NhaTaiTroes.Where(nt => nt.Active == true).OrderBy(nt => nt.SDate).ToList();
        //     for(int i=0;i<ntt.Count;i++)
        //     {
        //         chuoi+="<div class=\"titleTLV\" align=\"center\">"+ntt[i].Name+"</div>";
        //         chuoi+="<div align=\"center\"><p>"+ntt[i].Detail+"</p>";
        //         chuoi+="<p>Địa chỉ:"+ntt[i].Address+"</p></div><div class=\"frameline\"><div class=\"vienxanh\"></div></div>";
        //     }
        //     ViewBag.ShowNhaTaiTro = chuoi;
        //     return PartialView();
        // }
        //#endregion

        //#region[Video]
        //public ActionResult _Video()
        //{
        //    var videoss = db.Videos.Where(m => m.Active==true).OrderByDescending(m => m.Ord).Take(2).ToList();
        //    string vo = "";
        //    for(int i=0;i<videoss.Count;i++)
        //    {
        //        vo+="<object width=\"210\" height=\"220\" type=\"application/x-shockwave-flash\" data=\""+videoss[i].Link+"\" title=\"Adobe Flash Player\">";
        //        vo += "<param name=\"movie\" value=\"" + videoss[i].Link + "\"><param name=\"wmode\" value=\"transparent\"></object>";                
        //    }
        //    ViewBag.VideoTaiLieu = vo;
        //    return PartialView();
        //}
        //#endregion

        #region[DoiTac]
        public ActionResult _DoiTac()
         {

             return PartialView();
         }
        #endregion

        public ActionResult _TinNoiBat()
        {
            var news = db.News.Where(n => n.Active == true && n.Index == true).OrderBy(n => n.SDate).ToList();
            string tt = "";
            for(int i=0;i<news.Count;i++)
            {
                tt += "<div class=\"viencenter\"><div class=\"frameanh2\"><div><a href=\"/" + news[i].Tag + "\">";
                tt += "<img src=\"" + news[i].Image + "\" width=\"132\" height=\"104\" border=\"0\"></a></div></div>";
                tt += "<div class=\"frametexttv\"><div class=\"frametexttvcon1\"><a href=\"/" + news[i].Tag + "\">" + news[i].Name + "</a></div>";
                tt += "<div class=\"frametexttvcon2\"><p>" + news[i].Content + "</p></div></div></div>";
            }
            ViewBag.TinNoiBat = tt;
            return PartialView();
        }


        #region[GT]
        public ActionResult _GioiThieu()
         {
             var mgt = db.Menus.Where(m => m.Active == true && m.Name=="Giới thiệu" &&m.Tag=="gioi-thieu").Single();
             string content = mgt.Content;
             string img = mgt.Image;
             string name = mgt.Name;
             string link = mgt.Link;
             ViewBag.GTContent = content;
             ViewBag.GTImg = img;
             ViewBag.GTName = name;
             ViewBag.GTLink = link;

             return PartialView();
         }
         #endregion


        // #region[Thực đơn]
        // public ActionResult _ThucDon()
        //{
        //    var pro = db.Products.Where(p => p.Active == true && p.Index == true).OrderByDescending(p => p.Ord).ToList();
        //    string str = "";
        //    for (int i = 0; i < pro.Count; i++)
        //    {
        //        str += "<li class=\'item\'>";                
        //        str += "<section class=\'imgframe\'><a class=\'image\' href=\'\' title='A'><img alt=\"" + pro[i].Name + "\" class=\"\" src=\"" + pro[i].Image + "\" /></a></section>";
        //        str += "<section id=\"LineBottomProductHome\"></section>";
        //        str += "<a class=\'name\' href=\"/" + pro[i].Tag + "\" title=\"" + pro[i].Name + "\"><p id=\"itemname\">" + pro[i].Name + "</p></a>";
        //        //str += "<section class=\"itemdes\"><a class=\'name\' href=\"/" + pro[i].Tag + "\" title=\"" + pro[i].Name + "\"><p id=\"itemname\">" + pro[i].Name + "</p></a>";
        //        double checkPrice=0;
        //        if (double.TryParse(pro[i].PricePromotion, out checkPrice))
        //        {
        //            str += "<p id=\"itemprice\">Giá: " + pro[i].PricePromotion + " VNĐ</p>";
        //            //str += "<p id=\"itemprice\">Giá: " + pro[i].PricePromotion + " VNĐ</p></section>";
        //        }
        //        else
        //        {
        //            str += "<p id=\"itemprice\">Giá: " + pro[i].PricePromotion + "</p>";
        //            //str += "<p id=\"itemprice\">Giá: " + pro[i].PricePromotion + " VNĐ</p></section>";
        //        }
                
        //        str += "</li>";
        //    }
        //    ViewBag.ThucDon = str;
        //    return PartialView();
        //}
        //#endregion

        #region quang cao
        public ActionResult _QuangCao()
        {
            return PartialView();
        }
        #endregion

        //#region gioi thieu
        //public ActionResult _GioiThieu()
        //{
        //    var group=db.GroupNews.Where(g=>g.Active==true&&g.Name=="Giới thiệu").Single();
        //    int gId=group.Id;
        //    var article = db.News.Where(n => n.Active == true && n.Index == true && n.IdGroup == gId).FirstOrDefault();
        //    string name = article.Name;
        //    string content = article.Content;
        //    string image = article.Image;
        //    string detail = article.Detail;
        //    ViewBag.Name = name;
        //    ViewBag.Content = content;
        //    ViewBag.Image = image;
        //    ViewBag.Detail = detail;
        //    return PartialView();
        //}
        //#endregion

        #region bo suu tap
        public ActionResult _BoSuuTap()
        {
            var group = db.GroupPictures.Where(g => g.Active == true && g.Name == "Giới thiệu").Single();
            return PartialView();
        }
        #endregion

     

        //#region[ListNewsHome]
        //public ActionResult _ListNewsHome(string tag)
        //{
        //    var rootGroupNews = db.GroupNews.Where(r => r.Active == true && r.Index == true && r.Tag == tag).SingleOrDefault();
        //    var menu = db.Menus.Where(m => m.Active == true && m.Tag == tag).SingleOrDefault();
        //    string menuName = menu.Name;
        //    string menuLink = menu.Link;
        //    string rootLevel = rootGroupNews.Level;
        //    string newsList = "";
        //    string groupNewsList = "";
        //    string topNewsLink = "";
        //    string topNewsName = "";
        //    string topNewsDesc = "";
        //    string groupNewsIDs = "";
        //    List<News> list = new List<News>();
        //    var group = db.GroupNews.Where(g => g.Active == true & g.Index == true && g.Level.Length == 10 && g.Level.Substring(0, 5) == rootLevel).OrderBy(g => g.Ord).ToList();
        //    for (int i = 0; i < group.Count; i++)
        //    {
        //        groupNewsIDs += group[i].Id.ToString();
        //        groupNewsList += "<li><a href=\"/danh-muc-tin/" + group[i].Tag + "\" title=\"" + group[i].Title + "\">" + group[i].Name + "</a></li>";
        //        var unsortedNews = db.News.Where(n => n.Active == true && n.Index == true).ToList();
        //        for (int j = 0; j < unsortedNews.Count; j++)
        //        {
        //            if (unsortedNews[j].IdGroup.ToString().Contains(groupNewsIDs))
        //            {
        //                list.Add(unsortedNews[j]);
        //            }
        //        }
        //    }
        //    list = (from n in db.News where n.Active == true && n.Index == true orderby n.MDate descending select n).ToList();
        //    for (int k = 0; k < list.Count; k++)
        //    {
        //        if (k == 0)
        //        {
        //            topNewsName = list[k].Name;
        //            topNewsLink = list[k].Tag;
        //            topNewsDesc = list[k].Content;
        //        }
        //        else
        //        {
        //            newsList += "<li><a href=\"/" + list[k].Tag + "\" title=\"" + list[k].Title + "\">" + list[k].Name + "</a></li>";
        //        }
        //    }
        //    ViewBag.TopNewsName = topNewsName;
        //    ViewBag.TopNewsLink = topNewsLink;
        //    ViewBag.TopNewsDesc = topNewsDesc;
        //    ViewBag.NewsList = newsList;
        //    ViewBag.GroupNewsList = groupNewsList;
        //    ViewBag.MenuName = menuName;
        //    ViewBag.MenuLink = menuLink;
        //    return PartialView();
        //}
        //#endregion

    }
}
