using Microsoft.AspNetCore.Mvc;
using DATN.Areas.Admin.Models;
using DATN.Models;
using System.Text;
using NuGet.Protocol;
using System.Security.Cryptography;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
      
        private readonly QldiemSvContext _context;
        public LoginController(QldiemSvContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); //trả về trạng thái lỗi
            }

            //xử lý logic đăng nhập tại đây
            var pass = model.Password;
            var dataLogin = _context.UserAdmins.Where(x => x.Username.Equals(model.Email) && x.Password.Equals(pass)).FirstOrDefault();
            var data = dataLogin.ToJson();
            if (dataLogin != null)
            {
                //lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("AdminLogin", data);

                return RedirectToAction("Index", "DashBoard");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminLogin");
            return RedirectToAction("Index");
        }
            
       
    }
}
