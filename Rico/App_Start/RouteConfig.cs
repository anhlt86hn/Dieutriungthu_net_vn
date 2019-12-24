using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rico
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(name: "Home", url: "", defaults: new { controller = "Home", action = "Index" });
            routes.MapRoute("HomePage", "{controller}/{action}/{page}", new { controller = "Home", action = "Index", page = UrlParameter.Optional }, new { controller = "^H.*", action = "^Index$" });
            
            #region[Quản trị]
            routes.MapRoute("Quản trị - đăng nhập", url: "admin", defaults: new { controller = "Admin", action = "Index" });
            routes.MapRoute("Quản trị - cấu hình", url: "admin/cau-hinh", defaults: new { controller = "Admin", action = "Config" });
            routes.MapRoute("Quản trị - đăng xuất", url: "admin/dang-xuat", defaults: new { controller = "Admin", action = "Logout" });
            routes.MapRoute("Quản trị - mặc định", url: "admin/default", defaults: new { controller = "Admin", action = "Default" });
            #endregion
            #region[Client-Side]
            routes.MapRoute("Về chúng tôi", "ve-chung-toi", defaults: new { controller = "About", action = "Index" });
            routes.MapRoute("Liên hệ", "lien-he", defaults: new { controller = "Contact", action = "Create" });
            routes.MapRoute("list báo giá", "bao-gia", defaults: new { controller = "PriceView", action = "List" });
            routes.MapRoute("list báo giá chi tiet", "bao-gia/chi-tiet", defaults: new { controller = "PriceView", action = "Detail" });
            routes.MapRoute("list tin", "danhmuc/{tag}", defaults: new { controller = "NewsView", action = "List" });
            routes.MapRoute("list tin page", "danhmuc/{tag}/{trang}", defaults: new { controller = "NewsView", action = "List" });           
            routes.MapRoute("show danh sach nhom anh", "danh-muc-anh/{tag}", defaults: new { controller = "PictureView", action = "List" });
            routes.MapRoute("danh sach nhom anh page", "danh-muc-anh/{tag}/{trang}", defaults: new { controller = "PictureView", action = "List" });           
            routes.MapRoute("show anh", "anh/{tag}", defaults: new { controller = "PictureView", action = "Detail" });
            routes.MapRoute("show anh chi tiet", "anh/{tag}/{item}", defaults: new { controller = "PictureView", action = "ViewDetail" });
            routes.MapRoute("tin", "baiviet/{tag}", defaults: new { controller = "NewsView", action = "Detail" });
            routes.MapRoute("menu noi dung", "noi-dung/{tag}", defaults: new { controller = "Page", action = "Index" });
            #endregion
            #region[Quản trị Module Danh Mục sản phẩm]
            routes.MapRoute("NSP - index", url: "admin/danh-muc-san-pham", defaults: new { controller = "GroupProduct", action = "Index" });
            routes.MapRoute("NSP - them", url: "admin/danh-muc-san-pham/them", defaults: new { controller = "GroupProduct", action = "Create" });
            routes.MapRoute("NSP - sua", url: "admin/danh-muc-san-pham/sua", defaults: new { controller = "GroupProduct", action = "Edit" });
            routes.MapRoute("NSP - xoa", url: "admin/danh-muc-san-pham/xoa", defaults: new { controller = "GroupProduct", action = "Delete" });
            routes.MapRoute("NSP - them cap con", url: "admin/danh-muc-san-pham/them-cap-con", defaults: new { controller = "GroupProduct", action = "CreateSub" });
            #endregion
            #region[Quản trị Module Loại sản phẩm]
            routes.MapRoute("Loại - index", url: "admin/loai-san-pham", defaults: new { controller = "Category", action = "Index" });
            routes.MapRoute("Loại - them", url: "admin/loai-san-pham/them", defaults: new { controller = "Category", action = "Create" });
            routes.MapRoute("Loại - sua", url: "admin/loai-san-pham/sua", defaults: new { controller = "Category", action = "Edit" });
            routes.MapRoute("Loại - xoa", url: "admin/loai-san-pham/xoa", defaults: new { controller = "Category", action = "Delete" });
            routes.MapRoute("Loại - them cap con", url: "admin/loai-san-pham/them-cap-con", defaults: new { controller = "Category", action = "CreateSub" });
            #endregion
            #region[Quản trị Module Sản phẩm]
            routes.MapRoute("sp - sản phẩm", url: "admin/san-pham", defaults: new { controller = "Product", action = "Index" });
            routes.MapRoute("sp - Thêm sản phẩm", url: "admin/san-pham/them", defaults: new { controller = "Product", action = "Create" });            
            routes.MapRoute("sp - Sửa sản phẩm", url: "admin/san-pham/sua", defaults: new { controller = "Product", action = "Edit" });
            routes.MapRoute("sp - Xóa sản phẩm", url: "admin/san-pham/xoa", defaults: new { controller = "Product", action = "Delete" });
            //routes.MapRoute("sp - Xem ảnh sản phẩm", url: "admin/san-pham/xem-Image", defaults: new { controller = "Product", action = "ProductViewImgot" });
            routes.MapRoute("sp - Xem ảnh sản phẩm 2", url: "admin/san-pham/xem-anh", defaults: new { controller = "Product", action = "ProductViewImg" });
            routes.MapRoute("sp - Sửa ảnh sản phẩm", url: "admin/san-pham/sua-anh", defaults: new { controller = "Product", action = "ProductEditImg" });
            routes.MapRoute("sp - Thêm ảnh sản phẩm", url: "admin/san-pham/them-anh", defaults: new { controller = "Product", action = "ProductAddImg" });
            routes.MapRoute("sp - Xóa ảnh sản phẩm", url: "admin/san-pham/xoa-anh", defaults: new { controller = "Product", action = "ProductDelImg" });
            #endregion
            #region[Quản trị Module Menu]
            routes.MapRoute("Menu - danh sách", url: "admin/menu", defaults: new { controller = "Menu", action = "Index" });
            routes.MapRoute("Menu - thêm", url: "admin/menu/them", defaults: new { controller = "Menu", action = "Create" });
            routes.MapRoute("Menu - sửa", url: "admin/menu/sua", defaults: new { controller = "Menu", action = "Edit" });
            routes.MapRoute("Menu - xóa", url: "admin/menu/xoa", defaults: new { controller = "Menu", action = "Delete" });                                             
            routes.MapRoute("Menu - thêm cấp con", url: "admin/menu/them-cap-con", defaults: new { controller = "Menu", action = "CreateSub" });                                               
            #endregion
            #region[Quản trị Module Link ảnh]
            routes.MapRoute("link-anh - dlink-anh sách", url: "admin/link-anh", defaults: new { controller = "Advertise", action = "Index" });
            routes.MapRoute("link-anh - thêm", url: "admin/link-anh/them", defaults: new { controller = "Advertise", action = "Create" });
            routes.MapRoute("link-anh - sửa", url: "admin/link-anh/sua", defaults: new { controller = "Advertise", action = "Edit" });
            routes.MapRoute("link-anh - xóa", url: "admin/link-anh/xoa", defaults: new { controller = "Advertise", action = "Delete" });            
            routes.MapRoute("link-anh - thêm nhiều", url: "admin/link-anh/them-nhieu", defaults: new { controller = "Advertise", action = "AddMulti" });                        
            #endregion        
            #region [Quản trị Module Danh Mục ảnh]
            routes.MapRoute("Danh Mục ảnh - danh sách", url: "admin/danh-muc-anh", defaults: new { controller = "GroupPicture", action = "Index" });
            routes.MapRoute("Danh Mục ảnh - thêm", url: "admin/danh-muc-anh/them", defaults: new { controller = "GroupPicture", action = "Create" });
            routes.MapRoute("Danh Mục ảnh - sửa", url: "admin/danh-muc-anh/sua", defaults: new { controller = "GroupPicture", action = "Edit" });
            routes.MapRoute("Danh Mục ảnh - xóa", url: "admin/danh-muc-anh/xoa", defaults: new { controller = "GroupPicture", action = "Delete" });            
            routes.MapRoute("Danh Mục ảnh - thêm cấp con", url: "admin/danh-muc-anh/them-cap-con", defaults: new { controller = "GroupPicture", action = "CreateSub" });                                    
            #endregion
            #region[Quản trị Module ảnh]
            routes.MapRoute("Ảnh - danh sách", url: "admin/anh", defaults: new { controller = "Picture", action = "Index" });            
            routes.MapRoute("Ảnh - thêm", url: "admin/anh/them", defaults: new { controller = "Picture", action = "Create" });
            routes.MapRoute("Ảnh - sửa", url: "admin/anh/sua", defaults: new { controller = "Picture", action = "Edit" });
            routes.MapRoute("Ảnh - xóa", url: "admin/anh/xoa", defaults: new { controller = "Picture", action = "Delete" });                       
            routes.MapRoute("Ảnh - thêm nhiều", url: "admin/anh/them-nhieu", defaults: new { controller = "Picture", action = "AddMulti" });            
            #endregion
            #region[Quản trị Module Phản hồi]
            routes.MapRoute("Phản hồi - danh sách", url: "admin/phan-hoi", defaults: new { controller = "Contact", action = "Index" });
            routes.MapRoute("Phản hồi - thêm", url: "admin/phan-hoi/them", defaults: new { controller = "Picture", action = "Create" });
            routes.MapRoute("Phản hồi - sửa", url: "admin/phan-hoi/xem-chi-tiet", defaults: new { controller = "Contact", action = "View" });
            routes.MapRoute("Phản hồi - xóa", url: "admin/phan-hoi/xoa", defaults: new { controller = "Contact", action = "Delete" });                                                            
            #endregion         
            #region [Quản trị Module Danh Mục Tin]
            routes.MapRoute("Danh Mục Tin - danh sách", url: "admin/danh-muc-tin", defaults: new { controller = "GroupNew", action = "Index" });
            routes.MapRoute("Danh Mục Tin - thêm", url: "admin/danh-muc-tin/them", defaults: new { controller = "GroupNew", action = "Create" });
            routes.MapRoute("Danh Mục Tin - sửa", url: "admin/danh-muc-tin/sua", defaults: new { controller = "GroupNew", action = "Edit" });
            routes.MapRoute("Danh Mục Tin - xóa", url: "admin/danh-muc-tin/xoa", defaults: new { controller = "GroupNew", action = "Delete" });            
            routes.MapRoute("Danh Mục Tin - thêm cấp con", url: "admin/danh-muc-tin/them-cap-con", defaults: new { controller = "GroupNew", action = "CreateSub" });            
            #endregion     
            #region[Quản trị Module Tin]
            routes.MapRoute("Tin - danh sách", url: "admin/tin", defaults: new { controller = "News", action = "Index" });
            routes.MapRoute("Tin - thêm", url: "admin/tin/them", defaults: new { controller = "News", action = "Create" });
            routes.MapRoute("Tin - sửa", url: "admin/tin/sua", defaults: new { controller = "News", action = "Edit" });
            routes.MapRoute("Tin - xóa", url: "admin/tin/xoa", defaults: new { controller = "News", action = "Delete" });            
            #endregion
            #region[Quản trị Module Báo giá]
            routes.MapRoute("Báo giá - danh sách", url: "admin/bao-gia", defaults: new { controller = "Price", action = "Index" });
            routes.MapRoute("Báo giá - thêm", url: "admin/bao-gia/them", defaults: new { controller = "Price", action = "Create" });
            routes.MapRoute("Báo giá - sửa", url: "admin/bao-gia/sua", defaults: new { controller = "Price", action = "Edit" });
            routes.MapRoute("Báo giá - xóa", url: "admin/bao-gia/xoa", defaults: new { controller = "Price", action = "Delete" });            
            #endregion
            #region[Quản trị Module thành viên]
            routes.MapRoute("Thành viên - danh sách", url: "admin/thanh-vien", defaults: new { controller = "Member", action = "Index" });
            routes.MapRoute("Thành viên - thêm", url: "admin/thanh-vien/them", defaults: new { controller = "Member", action = "Create" });
            routes.MapRoute("Thành viên - sửa", url: "admin/thanh-vien/sua", defaults: new { controller = "Member", action = "Edit" });
            routes.MapRoute("Thành viên - xóa", url: "admin/thanh-vien/xoa", defaults: new { controller = "Member", action = "Delete" });            
            #endregion
            #region[Quản trị Module Hỗ trợ trực tuyến]
            routes.MapRoute("Hỗ trợ trực tuyến - danh sách", url: "admin/ho-tro", defaults: new { controller = "Support", action = "Index" });
            routes.MapRoute("Hỗ trợ trực tuyến - thêm", url: "admin/ho-tro/them", defaults: new { controller = "Support", action = "Create" });
            routes.MapRoute("Hỗ trợ trực tuyến - sửa", url: "admin/ho-tro/sua", defaults: new { controller = "Support", action = "Edit" });
            routes.MapRoute("Hỗ trợ trực tuyến - xóa", url: "admin/ho-tro/xoa", defaults: new { controller = "Support", action = "Delete" });            
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}