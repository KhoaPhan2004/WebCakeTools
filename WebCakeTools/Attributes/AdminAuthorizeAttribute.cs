using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
namespace WebCakeTools.Attributes
{
    public class AdminAuthorizeAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userName = session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                // Chưa đăng nhập → chuyển hướng về trang login
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
