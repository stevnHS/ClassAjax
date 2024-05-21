using ClassAjax.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClassAjax.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDB2Context _context;

        public HomeController(ILogger<HomeController> logger, MyDB2Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Addresses.Where(a => a.Id <= 34215);
            return View(products);
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

        public IActionResult Fetch()
        {
            return View();
        }
    }
}
