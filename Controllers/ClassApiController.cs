using ClassAjax.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClassAjax.Controllers
{
    public class ClassApiController : Controller
    {
        private readonly MyDB2Context _context;
        public ClassApiController(MyDB2Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cities = _context.Addresses.Select(a => a.City).Distinct();
            return Json(cities);
        }
        public IActionResult CitiesByDistrict(string city)
        {
            return Json(_context.Addresses.Where(x => x.City == city)
                .Select(c => c.SiteId).Distinct());
        }
        public IActionResult RoadsByDistrict(string district)
        {
            return Json(_context.Addresses.Where(x => x.SiteId == district)
                .Select(d => d.Road).Distinct());
        }

        public IActionResult Avatar(int id = 1)
        {

            Member? member = _context.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                return File(img, "image/jpeg");
            }

            return NotFound();
        }
        public IActionResult First()
        {
            return Content("看好了~世界!!");
        }
        public IActionResult Person(int id, string name, int age = 20)
        {
            if (string.IsNullOrEmpty(name)) name = "guest";

            return Content($"id:{id},你好{name},今年{age}歲");
        }

        
    }
}
