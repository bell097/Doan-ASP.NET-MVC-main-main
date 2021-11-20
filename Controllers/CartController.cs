using Doan_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan_ASP.NET_MVC.Controllers
{
    public class CartController : Controller
    {
        private ShopModelContext db = new ShopModelContext();
    
        // GET: Cart
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
        }

        public ActionResult ThemVaoGio(int SanPhamID)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<CartItem>();  
            }

            List<CartItem> giohang = Session["giohang"] as List<CartItem>;  

           

            if (giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID) == null) 
            {
                Product p = db.Products.Find(SanPhamID);  
                if (p.productsale == 0)
                {
                    CartItem newItem = new CartItem()
                    {
                        SanPhamID = p.product_id,
                        TenSanPham = p.product_name,
                        SoLuong = 1,
                        Hinh = p.product_image,
                        DonGia = (int)p.product_price

                    }; 
                    giohang.Add(newItem);
                }
                else {
                    CartItem newItem = new CartItem()
                    {
                        SanPhamID = p.product_id,
                        TenSanPham = p.product_name,
                        SoLuong = 1,
                        Hinh = p.product_image,
                        DonGia = (int)p.productsale

                    };  
                    giohang.Add(newItem);
                }
                 
            }
            else
            {
               
                CartItem cardItem = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
                cardItem.SoLuong++;
            }

            
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }
        public ActionResult XoaKhoiGio(int SanPhamID)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem itemXoa = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SuaSoLuong(int SanPhamID, int soluongmoi)
        {
            
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem itemSua = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
            if (soluongmoi != 0)
            {
                if (itemSua != null)
                {
                    itemSua.SoLuong = soluongmoi;
                }
                return RedirectToAction("Index");
            }
            else {
                CartItem itemXoa = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
                if (itemXoa != null)
                {
                    giohang.Remove(itemXoa);
                }
                return RedirectToAction("Index");
            }
               
        }

        public ActionResult Cartmini()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return PartialView("Cartmini",giohang);
        }

        public ActionResult XoaKhoiGiomini(int SanPhamID)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem itemXoa = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

        public ActionResult ThemVaoGio_Detail(int SanPhamID, int soluong)
        {
            if (Session["giohang"] == null) 
            {
                Session["giohang"] = new List<CartItem>();  
            }

            List<CartItem> giohang = Session["giohang"] as List<CartItem>;  


            if (giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID) == null) 
            {
                Product p = db.Products.Find(SanPhamID);  
                if (p.productsale == 0)
                {
                    CartItem newItem = new CartItem()
                    {
                        SanPhamID = p.product_id,
                        TenSanPham = p.product_name,
                        SoLuong = soluong,
                        Hinh = p.product_image,
                        DonGia = (int)p.product_price

                    };  
                    giohang.Add(newItem);
                }
                else
                {
                    CartItem newItem = new CartItem()
                    {
                        SanPhamID = p.product_id,
                        TenSanPham = p.product_name,
                        SoLuong = soluong,
                        Hinh = p.product_image,
                        DonGia = (int)p.productsale

                    };  
                    giohang.Add(newItem);
                }
                
            }
            else
            {
                
                CartItem cardItem = giohang.FirstOrDefault(m => m.SanPhamID == SanPhamID);
                cardItem.SoLuong+= soluong;
            }

            
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }

    }
}