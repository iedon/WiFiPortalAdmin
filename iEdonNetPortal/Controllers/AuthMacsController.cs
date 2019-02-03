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
    public class AuthMacsController : Controller
    {
        private readonly PortalContext _context;

        public AuthMacsController(PortalContext context)
        {
            _context = context;
        }

        // GET: AuthMacs
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            var portalContext = from a in _context.AuthMacs
                                select a;
            portalContext = portalContext.Include(a => a.U);
            switch (sortOrder)
            {
                case "date":
                default:
                    portalContext = portalContext.OrderByDescending(a => a.Date);
                    break;
                case "traffic":
                    portalContext = portalContext.OrderByDescending(a => a.Traffic);
                    break;
                case "user":
                    portalContext = portalContext.OrderBy(a => a.U.User);
                    break;
            }
            ViewData["SortOrder"] = sortOrder;
            return View(await PaginatedList<AuthMacs>.CreateAsync(portalContext.AsNoTracking(), page ?? 1, PaginatedList<AuthMacs>.PageSize));
        }

        // GET: AuthMacs/Details/5
        public async Task<IActionResult> Details(ulong? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var authMacs = await _context.AuthMacs
                .Include(a => a.U)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authMacs == null)
            {
                return NotFound();
            }

            return View(authMacs);
        }

        // GET: AuthMacs/Create
        public IActionResult Create()
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            ViewData["Uid"] = new SelectList(_context.Accounts, "Uid", "User");
            return View();
        }

        // POST: AuthMacs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uid,Id,Mac,Traffic,Date")] AuthMacs authMacs)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            _context.Add(authMacs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AuthMacs/Edit/5
        public async Task<IActionResult> Edit(ulong? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var authMacs = await _context.AuthMacs.FindAsync(id);
            if (authMacs == null)
            {
                return NotFound();
            }
            ViewData["Uid"] = new SelectList(_context.Accounts, "Uid", "User", authMacs.Uid);
            return View(authMacs);
        }

        // POST: AuthMacs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ulong id, [Bind("Uid,Id,Mac,Traffic,Date")] AuthMacs authMacs)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id != authMacs.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(authMacs);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthMacsExists(authMacs.Id))
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

        // GET: AuthMacs/Delete/5
        public async Task<IActionResult> Delete(ulong? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var authMacs = await _context.AuthMacs
                .Include(a => a.U)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authMacs == null)
            {
                return NotFound();
            }

            return View(authMacs);
        }

        // POST: AuthMacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ulong id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            var authMacs = await _context.AuthMacs.FindAsync(id);
            _context.AuthMacs.Remove(authMacs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthMacsExists(ulong id)
        {
            return _context.AuthMacs.Any(e => e.Id == id);
        }
    }
}
