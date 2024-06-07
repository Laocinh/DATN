using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DATN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BaseController : Controller
    {
        //Lưu thông tin đăng nhập vào 1 session, nếu session đó đang tồn tại mới cho đăng nhập vào trang admin
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Session.GetString("AdminLogin") == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Admin" }));
            }

            base.OnActionExecuted(context);
        }
    }
}
