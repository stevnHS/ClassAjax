using ClassAjax.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ClassAjax.Controllers
{
    public class ClassApiController : Controller
    {
        private readonly MyDB2Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ClassApiController(MyDB2Context context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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

        

        public IActionResult checkAccount(string name)
        {
            if(name.IsNullOrEmpty() || _context.Members.Any(x=>x.Name == name))
            {
                return Content("帳號已存在", "text/plain", System.Text.Encoding.UTF8);
            }
            return Content("帳號可以使用", "text/plain", System.Text.Encoding.UTF8);
        }

        //public IActionResult Register(string userName, string email, int age = 20)
        [HttpPost]
        public IActionResult Register(Member member, IFormFile FileName)
        {
            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", FileName.FileName);
            string info = uploadPath;
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                FileName.CopyTo(fileStream);
            }

            return Content(info, "text/plain", System.Text.Encoding.UTF8);
        }
    }
}
