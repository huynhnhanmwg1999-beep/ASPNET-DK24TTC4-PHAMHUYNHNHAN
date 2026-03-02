using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuanLySanBong.Classes;
using QuanLySanBong.Data;
using QuanLySanBong.Models;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using X.PagedList;

namespace QuanLySanBong.Controllers
{
    public class HomeController : Controller
    {
        private FootballContext db;
        public HomeController(FootballContext context)
        {
            db = context;
        }
        public IActionResult Index(int? page, string? searchInput)
        {
            ViewBag.Title = "Danh sách sân bóng";
            ViewBag.active = 1;
            int pageSize = 6;
            int pageNum = page == null || page < 0 ? 1 : page.Value;
            if (searchInput == null) searchInput = "";
            if (Classes.ConstVar.User.UserId != null)
            {
                var user = db.User.FirstOrDefault(u => u.UserId == Classes.ConstVar.User.UserId);
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
                var playgrounds = db.PlayGround.AsNoTracking()
                    .Where(p => p.PlayGroundName.Contains(searchInput) || p.Address.Contains(searchInput))
                    .OrderBy(p => p.PlayGroundName);
                PagedList<PlayGround> lst = new PagedList<PlayGround>(playgrounds, pageNum, pageSize);
                var MyModelView = new Tuple<IPagedList<PlayGround>, User>(lst, user);
                ViewBag.CurrentUser = user;
                return View(MyModelView);
            }
            return View();
        }

        [HttpGet]
        public IActionResult ListSanBong(int? page, string? searchInput)
        {
            ViewBag.Title = "Danh sách sân bóng";
            ViewBag.active = 1;
            int pageSize = 6;
            int pageNum = page == null || page < 0 ? 1 : page.Value;
            if (searchInput == null) searchInput = "";
            if (Classes.ConstVar.User.UserId != null)
            {
                var user = db.User.FirstOrDefault(u => u.UserId == Classes.ConstVar.User.UserId);
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
                var playgrounds = db.PlayGround.AsNoTracking()
                    .Where(p => p.PlayGroundName.Contains(searchInput) || p.Address.Contains(searchInput))
                    .OrderBy(p => p.PlayGroundName);
                PagedList<PlayGround> lst = new PagedList<PlayGround>(playgrounds, pageNum, pageSize);
                var MyModelView = new Tuple<IPagedList<PlayGround>, User>(lst, user);
                ViewBag.CurrentUser = user;
                return View(MyModelView);
            }
            return View();
        }

        public IActionResult SanDaDat()
        {
            ViewBag.Title = "Sân bóng đã đặt";
            ViewBag.active = 2;
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
            else
            {
                return RedirectToAction("Login", "Access");
            }
            var Invoices = db.Invoice.Where(u => u.UserId == ConstVar.User.UserId).OrderByDescending(u => u.InvoiceId);
            //var Invoices = db.Invoice.FirstOrDefault(u => u.UserId == mid);
            if (Invoices != null)
            {
                var InvoiceDetails = (from Invoice in Invoices
                                      join InvoiceDetail in db.InvoiceDetail on Invoice.InvoiceId equals InvoiceDetail.InvoiceId
                                      select InvoiceDetail).Distinct();

                var YardDetails = (from InvoiceDetail in InvoiceDetails
                                   join YardDetail in db.YardDetail on InvoiceDetail.YardDetailId equals YardDetail.YardDetailId
                                   select new YardDetail
                                   {
                                       YardDetailId = YardDetail.YardDetailId,
                                       PlayGround = YardDetail.PlayGround,
                                       SubYard = YardDetail.SubYard,
                                       TimeSlot = YardDetail.TimeSlot
                                   }).ToList();

                var MyModelView = new Tuple<List<YardDetail>, List<InvoiceDetail>, List<Invoice>, User>(
                      YardDetails,
                      InvoiceDetails.ToList(),
                      Invoices.ToList(),
                      user
                   );
                return View(MyModelView);
            }

            return View();
        }

        [HttpGet]
        public IActionResult HoaDon(int? invoideId)
        {        
            if (invoideId == null)
            {
               return RedirectToAction("SanDaDat");                    
            }          
            ViewBag.Title = "Hóa đơn đặt sân";
            ViewBag.active = 3;
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
            var Invoices = db.Invoice.FirstOrDefault(u => u.UserId == ConstVar.User.UserId && u.InvoiceId == invoideId);
            if (Invoices != null)
            {
                int InvoicesId = Invoices.InvoiceId;
                var InvoiceDetails = db.InvoiceDetail.Where(u => u.InvoiceId == InvoicesId);
                var YardDetails = (from InvoiceDetail in InvoiceDetails
                                   join YardDetail in db.YardDetail on InvoiceDetail.YardDetailId equals YardDetail.YardDetailId
                                   select new YardDetail
                                   {
                                       YardDetailId = YardDetail.YardDetailId,
                                       PlayGround = YardDetail.PlayGround,
                                       SubYard = YardDetail.SubYard,
                                       TimeSlot = YardDetail.TimeSlot
                                   }).ToList();

                var MyModelView = new Tuple<List<YardDetail>, List<InvoiceDetail>, Invoice, User>(
                      YardDetails,
                      InvoiceDetails.ToList(),
                      Invoices,
                      user
                   );
                return View(MyModelView);
            }
            return View();
        }

        public IActionResult HoaDonTrong()
        {
            ViewBag.Title = "Hóa đơn đặt sân";
            ViewBag.active = 3;
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
            return View();
        }        
        public IActionResult DatSan(int mid, string? dateBook)
        {
            ViewBag.Title = "Đặt sân bóng";
            ViewBag.active = 1;
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
            
            var MyModelView = SelectList(dateBook, mid);
            ViewBag.PlayGroundId = mid;
            return View(MyModelView);
        }
        public IActionResult CheckSan(string subYardId, string timeSlotId, string playGroundId, string dateBook)
        {
            var cart = db.Cart.FirstOrDefault(p => p.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = ConstVar.User;
            if (ConstVar.User?.UserId == null || cart == null)
            {
                //TempData["Message"] = "Vui lòng đăng nhập trước khi đặt sân!";
                return RedirectToAction("Login", "Access");
            }

            var timeslot = db.TimeSlot.FirstOrDefault(p => p.TimeSlotId == int.Parse(timeSlotId));
            var playGroundid = int.Parse(playGroundId);

            var yardDetail = db.YardDetail
                    .FirstOrDefault(p => p.PlayGroundId == int.Parse(playGroundId)
                                                     && p.SubYardId == int.Parse(subYardId)
                                                     && p.TimeSlotId == int.Parse(timeSlotId));

            if (dateBook == DateTime.Now.ToString("yyyy-MM-dd") && timeslot.TimeEnd < DateTime.Now.TimeOfDay)
            {
                TempData["Message"] = "Đã quá giờ nên không thể đặt sân!";
                return RedirectToAction("DatSan", "Home", new { mid = playGroundId, dateBook });
            }
            var cartDetail = new CartDetail
            {
                Cart = cart,
                YardDetail = yardDetail,
                DateBook = DateTime.Parse(dateBook),
                Price = yardDetail.Price,
            };
            db.CartDetail.Add(cartDetail);
            db.SaveChanges();
            return RedirectToAction("Cart", "Cart");
        }

        public IActionResult DateBookChange(string datebook, int mid)
        {
            var MyModelView = SelectList(datebook, mid);
            ViewBag.PlayGroundId = mid;
            ViewBag.CurrentUser = ConstVar.User;
            return PartialView("BangLichSan", MyModelView);
        }
        public Tuple<List<TimeSlot>, List<SubYard>, List<YardDetail>, List<YardDetail>> SelectList(string datebook, int mid)
        {
            DateTime currentDate = DateTime.Now.Date;
            if (datebook == null) datebook = currentDate.ToString("yyyy-MM-dd");
            DateTime selectedDate = DateTime.ParseExact(datebook, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var YardDetails = from YardDetail in db.YardDetail
                              where YardDetail.PlayGroundId == mid
                              select YardDetail;

            var YardDetailsBooked = from InvoiceDetail in db.InvoiceDetail
                                    join YardDetail in YardDetails on InvoiceDetail.YardDetailId equals YardDetail.YardDetailId
                                    where InvoiceDetail.DateBook.Date == selectedDate
                                    select YardDetail;
            var TimeSlots = (from YardDetail in YardDetails
                             join TimeSlot in db.TimeSlot on YardDetail.TimeSlotId equals TimeSlot.TimeSlotId
                             select TimeSlot).Distinct();
            var SubYards = (from YardDetail in YardDetails
                            join SubYard in db.SubYard on YardDetail.SubYardId equals SubYard.SubYardId
                            select SubYard).Distinct();
            ViewBag.CurrentUser = ConstVar.User;
            return new Tuple<List<TimeSlot>, List<SubYard>, List<YardDetail>, List<YardDetail>>(
                    TimeSlots.ToList(),
                    SubYards.ToList(),
                    YardDetails.ToList(),
                    YardDetailsBooked.ToList()
                );
        }
    }
}