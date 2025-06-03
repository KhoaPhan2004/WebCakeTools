using Microsoft.AspNetCore.Mvc;
using WebCakeTools.Models;

namespace WebCakeTools.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly CaketoolsContext _caketoolsContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LoginController(ILogger<LoginController> logger, CaketoolsContext caketoolsContext, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _caketoolsContext = caketoolsContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            var user = _caketoolsContext.Users
                        .FirstOrDefault(u => u.UserName == username && u.UserPassword == password);

            if (user != null)
            {
                // Đăng nhập thành công
                HttpContext.Session.SetString("UserName", user.UserName);
                return RedirectToAction("Index", "_2004WCTAdmin"); // hoặc tên controller bạn muốn
            }
            else
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }
    }
}
