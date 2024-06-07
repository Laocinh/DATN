using Microsoft.AspNetCore.Mvc;
using DATN.Models;
using DATN.ViewModels;
using System.Text;
using NuGet.Protocol;
using System.Security.Cryptography;

namespace DATN.Controllers
{
   
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
            var login = HttpContext.Session.GetString("StaffLogin");
            if(string.IsNullOrEmpty(login)){
                return View();
            }
            return RedirectToAction("Index", "Home");
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
            var dataLogin = _context.UserStaffs.Where(x => x.Username.Equals(model.Email) && x.Password.Equals(pass)).FirstOrDefault();
            var data = dataLogin.ToJson();
            if (dataLogin != null)
            {
                //lưu session khi đăng nhập thành công
                HttpContext.Session.SetString("StaffLogin", data);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StaffLogin");
            return RedirectToAction("Index");
        }


    }
}
