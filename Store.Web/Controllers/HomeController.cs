using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Store.Web.Helpers;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //IsAuthenticated
        //[Authorize(Roles = "Customer, Admin")]

        //[Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult Example()
        //{
        //    if (ConfigSideBar.ShowSideBar)
        //        ConfigSideBar.ShowSideBar = false;
        //    else
        //        ConfigSideBar.ShowSideBar = true;
        //    return RedirectToAction("Index");
        //}

        public IActionResult Dashboard()
        {
            ViewData["urlimg"] = CodeQrHerper.CreateQrCode("Warlin123456");
            return View();
        }

        public IActionResult General()
        {
            return View();
        }

        public IActionResult Icons()
        {
            return View();
        }

        public IActionResult Buttons()
        {
            return View();
        }

        public IActionResult widgets()
        {
            return View();
        }

        public IActionResult sliders()
        {
            return View();
        }

        public IActionResult timeline()
        {
            return View();
        }

        public IActionResult modals()
        {
            return View();
        }
        public IActionResult generalForm()
        {
            return View();
        }
        public IActionResult editors()
        {
            return View();
        }
        public IActionResult advanced()
        {
            return View();
        }
        public IActionResult simple()
        {
            return View();
        }
        public IActionResult data()
        {
            return View();
        }
        public IActionResult Calendar()
        {
            return View();
        }
        public IActionResult mailbox()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
