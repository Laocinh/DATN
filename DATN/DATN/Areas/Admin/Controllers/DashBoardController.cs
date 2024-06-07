using Microsoft.AspNetCore.Mvc;

namespace DATN.Areas.Admin.Controllers

{
   //Cho controller nào kế thừa basecontroller thì bắt buộc phải đăng nhập mới truy cập đc
    public class DashBoardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
