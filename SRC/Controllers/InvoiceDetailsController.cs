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
    public class InvoiceDetailsController : Controller
    {
        private readonly FootballContext _context;

        public InvoiceDetailsController(FootballContext context)
        {
            _context = context;
        }

        // GET: InvoiceDetails
        public async Task<IActionResult> Index()
        {
            ViewBag.CurrentUser = ConstVar.User;
            var footballContext = _context.InvoiceDetail.Include(i => i.Invoice).Include(i => i.YardDetail);
            return View(await footballContext.ToListAsync());
        }

        // GET: InvoiceDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id == null || _context.InvoiceDetail == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetail
                .Include(i => i.Invoice)
                .Include(i => i.YardDetail)
                .FirstOrDefaultAsync(m => m.InvoiceDetailId == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Create
        public IActionResult Create()
        {
            ViewBag.CurrentUser = ConstVar.User;
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId");
            ViewData["YardDetailId"] = new SelectList(_context.YardDetail, "YardDetailId", "YardDetailId");
            return View();
        }

        // POST: InvoiceDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvoiceDetailId,InvoiceId,YardDetailId,DateBook,Price")] InvoiceDetail invoiceDetail)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (ModelState.IsValid)
            {
                _context.Add(invoiceDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["YardDetailId"] = new SelectList(_context.YardDetail, "YardDetailId", "YardDetailId", invoiceDetail.YardDetailId);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id == null || _context.InvoiceDetail == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetail.FindAsync(id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["YardDetailId"] = new SelectList(_context.YardDetail, "YardDetailId", "YardDetailId", invoiceDetail.YardDetailId);
            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvoiceDetailId,InvoiceId,YardDetailId,DateBook,Price")] InvoiceDetail invoiceDetail)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id != invoiceDetail.InvoiceDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceDetailExists(invoiceDetail.InvoiceDetailId))
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
            ViewData["InvoiceId"] = new SelectList(_context.Invoice, "InvoiceId", "InvoiceId", invoiceDetail.InvoiceId);
            ViewData["YardDetailId"] = new SelectList(_context.YardDetail, "YardDetailId", "YardDetailId", invoiceDetail.YardDetailId);
            return View(invoiceDetail);
        }

        // GET: InvoiceDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (id == null || _context.InvoiceDetail == null)
            {
                return NotFound();
            }

            var invoiceDetail = await _context.InvoiceDetail
                .Include(i => i.Invoice)
                .Include(i => i.YardDetail)
                .FirstOrDefaultAsync(m => m.InvoiceDetailId == id);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return View(invoiceDetail);
        }

        // POST: InvoiceDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.CurrentUser = ConstVar.User;
            if (_context.InvoiceDetail == null)
            {
                return Problem("Entity set 'FootballContext.InvoiceDetail'  is null.");
            }
            var invoiceDetail = await _context.InvoiceDetail.FindAsync(id);
            if (invoiceDetail != null)
            {
                _context.InvoiceDetail.Remove(invoiceDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceDetailExists(int id)
        {
          return (_context.InvoiceDetail?.Any(e => e.InvoiceDetailId == id)).GetValueOrDefault();
        }
    }
}
