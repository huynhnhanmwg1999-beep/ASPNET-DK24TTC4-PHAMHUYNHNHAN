using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySanBong.Classes;
using QuanLySanBong.Data;
using QuanLySanBong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLySanBong.Controllers
{
    public class YardDetailsController : Controller
    {
        private readonly FootballContext _context;

        public YardDetailsController(FootballContext context)
        {
            _context = context;
        }

        // GET: YardDetails
        public async Task<IActionResult> Index()
        {
            ViewBag.CurrentUser = ConstVar.User;
            var footballContext = _context.YardDetail.Include(y => y.PlayGround).Include(y => y.SubYard).Include(y => y.TimeSlot);
            return View(await footballContext.ToListAsync());
        }

        // GET: YardDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id == null || _context.YardDetail == null)
            {
                return NotFound();
            }

            var yardDetail = await _context.YardDetail
                .Include(y => y.PlayGround)
                .Include(y => y.SubYard)
                .Include(y => y.TimeSlot)
                .FirstOrDefaultAsync(m => m.YardDetailId == id);
            if (yardDetail == null)
            {
                return NotFound();
            }

            return View(yardDetail);
        }

        // GET: YardDetails/Create
        public IActionResult Create()
        {
            ViewBag.CurrentUser = ConstVar.User;
            ViewData["PlayGroundId"] = new SelectList(_context.PlayGround, "PlayGroundId", "PlayGroundId");
            ViewData["SubYardId"] = new SelectList(_context.SubYard, "SubYardId", "SubYardId");
            ViewData["TimeSlotId"] = new SelectList(_context.TimeSlot, "TimeSlotId", "TimeSlotId");
            return View();
        }

        // POST: YardDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("YardDetailId,PlayGroundId,SubYardId,TimeSlotId,Price")] YardDetail yardDetail)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (ModelState.IsValid)
            {
                _context.Add(yardDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayGroundId"] = new SelectList(_context.PlayGround, "PlayGroundId", "PlayGroundId", yardDetail.PlayGroundId);
            ViewData["SubYardId"] = new SelectList(_context.SubYard, "SubYardId", "SubYardId", yardDetail.SubYardId);
            ViewData["TimeSlotId"] = new SelectList(_context.TimeSlot, "TimeSlotId", "TimeSlotId", yardDetail.TimeSlotId);
            return View(yardDetail);
        }

        // GET: YardDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id == null || _context.YardDetail == null)
            {
                return NotFound();
            }

            var yardDetail = await _context.YardDetail.FindAsync(id);
            if (yardDetail == null)
            {
                return NotFound();
            }
            ViewData["PlayGroundId"] = new SelectList(_context.PlayGround, "PlayGroundId", "PlayGroundId", yardDetail.PlayGroundId);
            ViewData["SubYardId"] = new SelectList(_context.SubYard, "SubYardId", "SubYardId", yardDetail.SubYardId);
            ViewData["TimeSlotId"] = new SelectList(_context.TimeSlot, "TimeSlotId", "TimeSlotId", yardDetail.TimeSlotId);
            return View(yardDetail);
        }

        // POST: YardDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("YardDetailId,PlayGroundId,SubYardId,TimeSlotId,Price")] YardDetail yardDetail)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id != yardDetail.YardDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yardDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YardDetailExists(yardDetail.YardDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayGroundId"] = new SelectList(_context.PlayGround, "PlayGroundId", "PlayGroundId", yardDetail.PlayGroundId);
            ViewData["SubYardId"] = new SelectList(_context.SubYard, "SubYardId", "SubYardId", yardDetail.SubYardId);
            ViewData["TimeSlotId"] = new SelectList(_context.TimeSlot, "TimeSlotId", "TimeSlotId", yardDetail.TimeSlotId);
            return View(yardDetail);
        }

        // GET: YardDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id == null || _context.YardDetail == null)
            {
                return NotFound();
            }

            var yardDetail = await _context.YardDetail
                .Include(y => y.PlayGround)
                .Include(y => y.SubYard)
                .Include(y => y.TimeSlot)
                .FirstOrDefaultAsync(m => m.YardDetailId == id);
            if (yardDetail == null)
            {
                return NotFound();
            }

            return View(yardDetail);
        }

        // POST: YardDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (_context.YardDetail == null)
            {
                return Problem("Entity set 'FootballContext.YardDetail'  is null.");
            }
            var yardDetail = await _context.YardDetail.FindAsync(id);
            if (yardDetail != null)
            {
                _context.YardDetail.Remove(yardDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YardDetailExists(int id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            return (_context.YardDetail?.Any(e => e.YardDetailId == id)).GetValueOrDefault();
        }
    }
}
