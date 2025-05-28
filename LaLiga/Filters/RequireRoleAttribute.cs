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
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }

            base.OnActionExecuting(context);
        }
    }
}