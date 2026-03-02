using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLySanBong.Data;
using QuanLySanBong.Models;

namespace QuanLySanBong.Controllers
{
    // ViewModel nhỏ cho Index
    public class PlayGroungListItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
    public class DanhMucController : Controller
    {
        private readonly FootballContext _context;

        public DanhMucController(FootballContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            if (Classes.ConstVar.User == null || Classes.ConstVar.User.UserId == 0)
            {
                return RedirectToAction("Login", "Access");
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = await _context.Cart.Include(p => p.CartDetails)
                        .Where(p => p.UserId == Classes.ConstVar.User.UserId)
                        .Select(p => p.CartDetails.Count())
                        .FirstOrDefaultAsync();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }

            var cats = await _context.PlayGround
                                     .OrderBy(c => c.PlayGroundId)
                                     .Select(c => new
                                     {
                                         c.PlayGroundId,
                                         c.PlayGroundName,
                                         c.PhoneNumber,
                                         c.Address,
                                         c.Description,
                                         c.Image
                                     })
                                     .ToListAsync();

            // map sang viewmodel đơn giản
            var vm = cats.Select(c => new PlayGroungListItemVM
            {
                Id = c.PlayGroundId,
                Name = c.PlayGroundName,
                Phone = c.PhoneNumber, 
                Address = c.Address,
                Description = c.Description + "",       
                Image = c.Image
            });

            ViewBag.active = 5;
            return View(vm);
        }

        // GET: /Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = await _context.Cart.Include(p => p.CartDetails)
                        .Where(p => p.UserId == Classes.ConstVar.User.UserId)
                        .Select(p => p.CartDetails.Count())
                        .FirstOrDefaultAsync();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }
            ViewBag.active = 5;
            var cat = await _context.PlayGround.FindAsync(id);
            if (cat == null) return NotFound();

            return View(cat);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, bool reassign = false)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = await _context.Cart.Include(p => p.CartDetails)
                        .Where(p => p.UserId == Classes.ConstVar.User.UserId)
                        .Select(p => p.CartDetails.Count())
                        .FirstOrDefaultAsync();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }
            ViewBag.active = 5;
            var cat = await _context.PlayGround.FindAsync(id);
            if (cat == null) return NotFound();

            _context.PlayGround.Remove(cat);
            await _context.SaveChangesAsync();
            TempData["Ok"] = "Đã xoá sân bóng.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Create
        public async Task<IActionResult> Create()
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = await _context.Cart.Include(p => p.CartDetails)
                        .Where(p => p.UserId == Classes.ConstVar.User.UserId)
                        .Select(p => p.CartDetails.Count())
                        .FirstOrDefaultAsync();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }
            ViewBag.active = 5;
            return View(new PlayGround());
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayGround model)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = await _context.Cart.Include(p => p.CartDetails)
                        .Where(p => p.UserId == Classes.ConstVar.User.UserId)
                        .Select(p => p.CartDetails.Count())
                        .FirstOrDefaultAsync();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }
            ViewBag.active = 5;
            if (!ModelState.IsValid) return View(model);
            _context.PlayGround.Add(model);
            await _context.SaveChangesAsync();
            TempData["Ok"] = "Đã tạo sân bóng mới.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = await _context.Cart.Include(p => p.CartDetails)
                        .Where(p => p.UserId == Classes.ConstVar.User.UserId)
                        .Select(p => p.CartDetails.Count())
                        .FirstOrDefaultAsync();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }
            ViewBag.active = 5;
            var cat = await _context.PlayGround.FindAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlayGround model)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == Classes.ConstVar.User.UserId);
            ViewBag.CurrentUser = user;
            if (user != null)
            {
                ViewBag.Name = user.DisplayName;
                ViewBag.userId = user.UserId;
                try
                {
                    ViewBag.Quantity = await _context.Cart.Include(p => p.CartDetails)
                        .Where(p => p.UserId == Classes.ConstVar.User.UserId)
                        .Select(p => p.CartDetails.Count())
                        .FirstOrDefaultAsync();
                }
                catch (System.Exception ex)
                {
                    ViewBag.Quantity = 0;
                }
            }
            ViewBag.active = 5;
            if (id != model.PlayGroundId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _context.Update(model);
            await _context.SaveChangesAsync();
            TempData["Ok"] = "Đã cập nhật sân nóng.";
            return RedirectToAction(nameof(Index));
        }
    }
}
