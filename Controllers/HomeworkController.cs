using Microsoft.AspNetCore.Mvc;

namespace ClassAjax.Controllers
{
    public class HomeworkController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HW2()
        {
            return View();
        }
        public IActionResult HW3()
        {
            return View();
        }
    }
}
