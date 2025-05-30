using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LaLiga.Filters
{
    public class RequireRoleAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public RequireRoleAttribute(string role)
        {
            _requiredRole = role;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("rola");

            if (string.IsNullOrEmpty(role) || role != _requiredRole)
            {
                // Ustaw tymczasową wiadomość (TempData przetrwa przekierowanie)
                context.HttpContext.Session.SetString("AccessDeniedMessage", "Brak dostępu do tej strony.");

                // Pobierz poprzedni adres URL
                var referer = context.HttpContext.Request.Headers["Referer"].ToString();
                if (string.IsNullOrEmpty(referer))
                {
                    referer = "/"; // fallback
                }

                context.Result = new RedirectResult(referer);
            }

            base.OnActionExecuting(context);
        }
    }
}