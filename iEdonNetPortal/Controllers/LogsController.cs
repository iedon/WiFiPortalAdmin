using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using iEdonNetPortal.Models;

namespace iEdonNetPortal.Controllers
{
    public class LogsController : Controller
    {
        private readonly PortalContext _context;

        public LogsController(PortalContext context)
        {
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            var portalContext = from l in _context.Logs
                                select l;
            portalContext = portalContext.Include(l => l.U);
            switch (sortOrder)
            {
                case "date":
                default:
                    portalContext = portalContext.OrderByDescending(l => l.Date);
                    break;
                case "action":
                    portalContext = portalContext.OrderBy(l => l.Action);
                    break;
                case "user":
                    portalContext = portalContext.OrderBy(l => l.U.User);
                    break;
            }
            ViewData["SortOrder"] = sortOrder;
            return View(await PaginatedList<Logs>.CreateAsync(portalContext.AsNoTracking(), page ?? 1, PaginatedList<Logs>.PageSize));
        }

        // GET: Logs/Details/5
        public async Task<IActionResult> Details(ulong? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);

            if (id == null)
            {
                return NotFound();
            }

            var logs = await _context.Logs
                .Include(l => l.U)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logs == null)
            {
                return NotFound();
            }

            return View(logs);
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            ViewData["Uid"] = new SelectList(_context.Accounts, "Uid", "User");
            return View();
        }

        // POST: Logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uid,Id,Action,Log,Date")] Logs logs)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            _context.Add(logs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Logs/Edit/5
        public async Task<IActionResult> Edit(ulong? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var logs = await _context.Logs.FindAsync(id);
            if (logs == null)
            {
                return NotFound();
            }
            ViewData["Uid"] = new SelectList(_context.Accounts, "Uid", "User", logs.Uid);
            return View(logs);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ulong id, [Bind("Uid,Id,Action,Log,Date")] Logs logs)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id != logs.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(logs);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogsExists(logs.Id))
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

        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(ulong? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var logs = await _context.Logs
                .Include(l => l.U)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logs == null)
            {
                return NotFound();
            }

            return View(logs);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ulong id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            var logs = await _context.Logs.FindAsync(id);
            _context.Logs.Remove(logs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogsExists(ulong id)
        {
            return _context.Logs.Any(e => e.Id == id);
        }
    }
}
