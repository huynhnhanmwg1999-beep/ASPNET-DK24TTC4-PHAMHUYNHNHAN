using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySanBong.Data;
using QuanLySanBong.Models;
using System.Linq;

namespace QuanLySanBong.Controllers
{
    public class BaoCaoController : Controller
    {
        private readonly FootballContext _context;

        public BaoCaoController(FootballContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (Classes.ConstVar.User == null || Classes.ConstVar.User.UserId == 0)
            {
                return RedirectToAction("Login", "Access");
            }

            var user = _context.User.FirstOrDefault(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = _context.Cart.Include(p => p.CartDetails).
                   FirstOrDefault(p => p.UserId == Classes.ConstVar.User.UserId).CartDetails.Count();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }

            var totalRevenue = _context.Invoice
                .Where(i => i.Total != null)
                .Sum(i => (decimal)i.Total);

            var totalOrders = _context.Invoice.Count();

            var totalCustomers = _context.User
                .Where(u => u.LoaiUser == false) 
                .Count();

            var currentYear = DateTime.Now.Year;
            var revenueByMonthData = _context.Invoice
                .Where(i => i.DateCreate.Year == currentYear && i.Total != null)
                .GroupBy(i => i.DateCreate.Month)
                .Select(g => new 
                { 
                    Month = g.Key, 
                    Revenue = g.Sum(x => (decimal)x.Total) 
                })
                .ToList();

            var revenueByMonths = new List<RevenueByMonth>();
            for (int i = 1; i <= 12; i++)
            {
                var monthData = revenueByMonthData.FirstOrDefault(m => m.Month == i);
                revenueByMonths.Add(new RevenueByMonth
                {
                    Month = "T" + i,
                    Revenue = monthData?.Revenue ?? 0
                });
            }

            var recentInvoices = _context.Invoice
                .Include(i => i.User)
                .OrderByDescending(i => i.DateCreate)
                .Take(5)
                .ToList();
            var revenueByPlaygroundData = _context.InvoiceDetail
                .Include(id => id.YardDetail)
                .ThenInclude(yd => yd.PlayGround)
                .GroupBy(id => id.YardDetail.PlayGround.PlayGroundName)
                .Select(g => new
                {
                    PlaygroundName = g.Key,
                    Revenue = g.Sum(x => (decimal?)x.Price) ?? 0 
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToList();

            var revenueByPlaygrounds = revenueByPlaygroundData.Select(x => new RevenueByPlayground
            {
                PlaygroundName = x.PlaygroundName,
                Revenue = x.Revenue
            }).ToList();

            var topPlaygroundName = revenueByPlaygrounds.FirstOrDefault()?.PlaygroundName ?? "Chưa có dữ liệu";

            var viewModel = new ReportViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalCustomers = totalCustomers,
                RevenueByMonths = revenueByMonths,
                RecentInvoices = recentInvoices,
                RevenueByPlaygrounds = revenueByPlaygrounds,
                TopPlaygroundName = topPlaygroundName
            };

            ViewBag.active = 4;
            return View(viewModel);
        }
    }
}
