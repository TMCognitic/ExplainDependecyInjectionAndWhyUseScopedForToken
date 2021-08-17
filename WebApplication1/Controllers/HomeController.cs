using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Singleton _singleton;
        private readonly Scoped _scoped;
        private readonly Transient _transient;

        public HomeController(ILogger<HomeController> logger, Singleton singleton, Scoped scoped, Transient transient)
        {
            _logger = logger;
            _singleton = singleton;
            _scoped = scoped;
            _transient = transient;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("Uid"))
                HttpContext.Session.SetString("Uid", Guid.NewGuid().ToString());

            return View();
        }

        public IActionResult ShowUID()
        {
            ViewBag.SingletonByInjection = _singleton.GetHashCode();
            ViewBag.Singleton = HttpContext.RequestServices.GetService(typeof(Singleton)).GetHashCode();

            ViewBag.ScopedByInjection = _scoped.GetHashCode();
            ViewBag.Scoped = HttpContext.RequestServices.GetService(typeof(Scoped)).GetHashCode();

            ViewBag.TransientByInjection = _transient.GetHashCode();
            ViewBag.Transient = HttpContext.RequestServices.GetService(typeof(Transient)).GetHashCode();

            return View();
        }

        public IActionResult Privacy()
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
