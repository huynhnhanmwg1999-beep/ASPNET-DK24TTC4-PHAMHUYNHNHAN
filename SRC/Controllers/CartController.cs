using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuanLySanBong.Classes;
using QuanLySanBong.Data;
using QuanLySanBong.Models;
using System.Security.Cryptography;
using System.Collections;
using System.Text.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuanLySanBong.Controllers
{
    public class CartController : Controller
    {
        private FootballContext db;
        public CartController(FootballContext context)
        {
            db = context;
        }
        public IActionResult Cart()
        {
            if (ConstVar.User?.UserId == null || ConstVar.User.UserId == 0)
                return RedirectToAction("Login", "Access");

            ViewBag.Title = "Giỏ hàng";            
            var user = db.User.FirstOrDefault(u => u.UserId == ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = db.Cart.Include(p => p.CartDetails).
                   FirstOrDefault(p => p.UserId == Classes.ConstVar.User.UserId).CartDetails.Count();
                }
                catch (Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }
            var cartDetails = db.Cart.Include(p => p.CartDetails)
                         .ThenInclude(cd => cd.YardDetail)
                         .ThenInclude(yd => yd.PlayGround)
                         .Include(p => p.CartDetails)
                         .ThenInclude(cd => cd.YardDetail)
                         .ThenInclude(yd => yd.SubYard)
                         .Include(p => p.CartDetails)
                         .ThenInclude(cd => cd.YardDetail)
                         .ThenInclude(yd => yd.TimeSlot)
                         .FirstOrDefault(p => p.UserId == Classes.ConstVar.User.UserId)?.CartDetails
                         .ToList();
            var MyModelView = new Tuple<List<CartDetail>, User>(
                  cartDetails,
                  user
               );          
            return View(MyModelView);
        }
        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult CartChange(int cartDetailId)
        {
            ViewBag.CurrentUser = ConstVar.User;
            var MyModelView = ListItemAfterDelete(ConstVar.User.UserId, cartDetailId);            
            return PartialView("BangCart", MyModelView);
        }

        public Tuple<List<CartDetail>, User> ListItemAfterDelete(int userId, int cartDetailId)
        {
            var user = db.User.FirstOrDefault(u => u.UserId == userId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
            }

                var cartDetail = db.CartDetail.FirstOrDefault(cd => cd.CartDetailId == cartDetailId);
                if (cartDetail != null)
                {
                    db.CartDetail.Remove(cartDetail);
                    db.SaveChanges();
                }

                var cartDetails = db.Cart.Include(p => p.CartDetails)
                              .ThenInclude(cd => cd.YardDetail)
                              .ThenInclude(yd => yd.PlayGround)
                              .Include(p => p.CartDetails)
                              .ThenInclude(cd => cd.YardDetail)
                              .ThenInclude(yd => yd.SubYard)
                              .Include(p => p.CartDetails)
                              .ThenInclude(cd => cd.YardDetail)
                              .ThenInclude(yd => yd.TimeSlot)
                              .FirstOrDefault(p => p.UserId == user.UserId)?.CartDetails
                              .ToList();

            return new Tuple<List<CartDetail>, User>(
                  cartDetails,
                  user
               );
        }

        public IActionResult AddCartDetail()
        {
            int TongTien = 0;
            var cartDetails = db.Cart.Include(p => p.CartDetails)
                              .ThenInclude(cd => cd.YardDetail)
                              .ThenInclude(yd => yd.PlayGround)
                              .Include(p => p.CartDetails)
                              .ThenInclude(cd => cd.YardDetail)
                              .ThenInclude(yd => yd.SubYard)
                              .Include(p => p.CartDetails)
                              .ThenInclude(cd => cd.YardDetail)
                              .ThenInclude(yd => yd.TimeSlot)
                              .FirstOrDefault(p => p.UserId == ConstVar.User.UserId)?.CartDetails
                              .ToList();
            if (cartDetails.Count() == 0) return RedirectToAction("ListSanBong", "Home");
            Invoice invoice = new Invoice();
            invoice.UserId = ConstVar.User.UserId;
            invoice.DateCreate = DateTime.Now.Date;
            invoice.Total = 0;
            db.Invoice.Add(invoice);
            db.SaveChanges();

            int invoiceId = (int)db.Invoice.OrderByDescending(i => i.DateCreate).FirstOrDefault()?.InvoiceId;
            Invoice invoiceMS = db.Invoice.Find(invoiceId);

            foreach (var item in cartDetails)
            {
                InvoiceDetail invoiceDetail = new InvoiceDetail();
                invoiceDetail.InvoiceId = invoice.InvoiceId;
                invoiceDetail.YardDetailId = item.YardDetailId;
                invoiceDetail.DateBook = item.DateBook;
                invoiceDetail.Price = item.Price;
                db.InvoiceDetail.Add(invoiceDetail);
                db.CartDetail.Remove(item);
                TongTien += item.Price;
            }
            invoice.Total = TongTien;
            db.SaveChanges();
            ViewBag.CurrentUser = ConstVar.User;
            return RedirectToAction("HoaDon", "Home", new { invoideId = invoice.InvoiceId });
        }       
    }
}
