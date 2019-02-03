using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iEdonNetPortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace iEdonNetPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortalContext _context;
        public HomeController(PortalContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string loggedInUser = PublicMethods.GetLoggedInUsername(HttpContext);
            ViewData["LoggedIn"] = loggedInUser;
            if (ViewData["LoggedIn"] == null) {
                ViewData["Message"] = "管理员登录";
                return View();
            }
            return RedirectToAction("Index", "Accounts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "输入不能为空";
                return View();
            }

            var user = await _context.Accounts.FindAsync(0U);
            if (user == null || user.User != loginViewModel.Username || user.Password != loginViewModel.Password)
            {
                ViewData["Message"] = "用户或密码错误";
                return View();
            } else {
                PublicMethods.SetSession(HttpContext, loginViewModel.Username);
            }
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            return RedirectToAction("Index", "Accounts");
        }

        public IActionResult Logout()
        {
            PublicMethods.ClearSession(HttpContext);
            return RedirectToAction("Index", "Home"); ;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "iEdon Net Portal / iEdon 网络认证系统";
            ViewData["LoggedIn"] = PublicMethods.GetLoggedInUsername(HttpContext);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
