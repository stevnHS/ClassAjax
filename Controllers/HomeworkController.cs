using Microsoft.AspNetCore.Mvc;

namespace ClassAjax.Controllers
{
    public class HomeworkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
