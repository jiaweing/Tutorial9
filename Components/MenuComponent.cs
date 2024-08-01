using Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Components
{
    [Authorize]
    [ViewComponent(Name = "Menu")]
    public class MenuComponent : ViewComponent
    {
        private readonly UserClaims _claims;

        public MenuComponent(IHttpContextAccessor contentAccessor)
        {
            if (contentAccessor.HttpContext.User.Identity.IsAuthenticated)
                _claims = new UserClaims(contentAccessor.HttpContext.User);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_claims);
        }
    }
}
