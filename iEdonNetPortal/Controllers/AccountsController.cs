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
    public class AccountsController : Controller
    {
        private readonly PortalContext _context;

        public AccountsController(PortalContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            var portalContext = from a in _context.Accounts
                                select a;
            switch (sortOrder)
            {
                case "lastlogin":
                    portalContext = portalContext.OrderByDescending(a => a.Lastlogin);
                    break;
                case "name":
                    portalContext = portalContext.OrderBy(a => a.Name);
                    break;
                case "credit":
                    portalContext = portalContext.OrderByDescending(a => a.Credit);
                    break;
                case "email":
                    portalContext = portalContext.OrderBy(a => a.Email);
                    break;
                case "online":
                    portalContext = portalContext.OrderByDescending(a => a.Online);
                    break;
                case "user":
                default:
                    portalContext = portalContext.OrderBy(a => a.User);
                    break;
            }
            ViewData["SortOrder"] = sortOrder;
            return View(await PaginatedList<Accounts>.CreateAsync(portalContext.AsNoTracking(), page ?? 1, PaginatedList<Accounts>.PageSize));
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(uint? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Uid,User,Password,Name,Online,Credit,Email,Lastlogin,Lastip")] Accounts accounts)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (ModelState.IsValid)
            {
                _context.Add(accounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accounts);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(uint? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return NotFound();
            }
            return View(accounts);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(uint id, [Bind("Uid,User,Password,Name,Online,Credit,Email,Lastlogin,Lastip")] Accounts accounts)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id != accounts.Uid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountsExists(accounts.Uid))
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
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(uint? id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(uint id)
        {
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            var accounts = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(accounts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountsExists(uint id)
        {
            return _context.Accounts.Any(e => e.Uid == id);
        }
    }
}
