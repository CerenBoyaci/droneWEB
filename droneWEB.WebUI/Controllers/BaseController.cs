using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace droneWEB.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
           
            var kullaniciId = context.HttpContext.Session.GetString("KullaniciId");
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .OfType<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>()
                .Any();

            if (string.IsNullOrEmpty(kullaniciId) && !allowAnonymous)
            {
                context.Result = new RedirectToActionResult("Giris", "Kullanici", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
