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
            if (name.IsNullOrEmpty() || _context.Members.Any(x => x.Name == name))
            {
                return Content("帳號已存在", "text/plain", System.Text.Encoding.UTF8);
            }
            return Content("帳號可以使用", "text/plain", System.Text.Encoding.UTF8);
        }

        //public IActionResult Register(string userName, string email, int age = 20)
        [HttpPost]
        public IActionResult Register(Member member, IFormFile avatar)
        {
            // 照片存起來
            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", avatar.FileName);
            string info = uploadPath;
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                avatar.CopyTo(fileStream);
            }
            //檔案上傳轉成二進位
            byte[] imgByte = null;
            using (var memoryStream = new MemoryStream())
            {
                avatar.CopyTo(memoryStream);
                imgByte = memoryStream.ToArray();
            }

            //寫進資料庫
            member.FileName = avatar.FileName;
            member.FileData = imgByte;
            _context.Members.Add(member);
            _context.SaveChanges();

            return Content(info, "text/plain", System.Text.Encoding.UTF8);
        }

        [HttpPost]
        public IActionResult Spots([FromBody] SearchDTO searchDTO)
        {
            //根據分類編號搜尋景點資料
            var spots = searchDTO.categoryId == 0 ? _context.SpotImagesSpots : _context.SpotImagesSpots.Where(s => s.CategoryId == searchDTO.categoryId);

            //根據關鍵字搜尋景點資料(title、desc)
            if (!string.IsNullOrEmpty(searchDTO.keyword))
            {
                spots = spots.Where(s => s.SpotTitle.Contains(searchDTO.keyword) || s.SpotDescription.Contains(searchDTO.keyword));
            }




            //總共有多少筆資料
            int totalCount = spots.Count();
            //每頁要顯示幾筆資料
            int pageSize = searchDTO.pageSize;   //searchDTO.pageSize ?? 9;
            //目前第幾頁
            int page = searchDTO.page;

            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            //分頁
            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);


            //包裝要傳給client端的資料
            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();
            spotsPaging.TotalCount = totalCount;
            spotsPaging.TotalPages = totalPages;
            spotsPaging.SpotsResult = spots.ToList();


            return Json(spotsPaging);
        }
    }
}
