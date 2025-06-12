using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DearlerPlatform.Api.Filters;

public class CustomerAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
        foreach (var claim in claimsIdentity.Claims)
        {
            if (claim.Type == "CustomerNo")
            {
                context.HttpContext.Items.Add("CustomerNo", claim.Value);
            }
        }
    }

}
