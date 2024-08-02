using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Database;
using Web.Models;
using Image = Web.Models.Image;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _db;
        private readonly UserClaims _claims;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            DatabaseContext db,
            IHttpContextAccessor contentAccessor,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            ILogger<HomeController> logger
        )
        {
            _db = db;
            _configuration = configuration;
            _environment = environment;

            if (contentAccessor.HttpContext.User.Identity.IsAuthenticated)
                _claims = new UserClaims(contentAccessor.HttpContext.User);

            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            return View("Dashboard", _claims);
        }

        [Authorize(CustomRoles.Admin)]
        public async Task<IActionResult> Admin()
        {
            return View("Admin", _claims);
        }

        [Authorize]
        public IActionResult Canvas()
        {
            return View();
        }

        [Authorize]
        public IActionResult Camera()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Photo upload failed");
                return View();
            }

            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new Image
            {
                FileName = fileName,
                FilePath = filePath,
                UploadDate = DateTime.UtcNow
            };

            _db.Images.Add(image);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Image uploaded successfully", image });
        }

        [Authorize]
        public IActionResult Video()
        {
            return View();
        }

        [Authorize]
        public IActionResult Geolocation()
        {
            var model = new GeolocationModel()
            {
                APIKey = _configuration["Web:GoogleMaps:APIKey"]
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("Denied")]
        public IActionResult Denied()
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
