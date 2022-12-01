using System.Security.Claims;
using CM.Model.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CM.Core.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly PermissionActions _action;
   
    public AuthorizeAttribute(PermissionActions action)
    {
        _action = action;
    }
    
    public AuthorizeAttribute()
    {
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var user = context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }

    private bool CheckValidatePermission(string controller, string action, IEnumerable<Claim> claimList)
    {
        var filterClaim = claimList
            .FirstOrDefault(x => string.Equals(x.Value.Split('|')[0], controller, StringComparison.CurrentCultureIgnoreCase));
        var claimFlag = filterClaim!.Value.Split('|');
        Enum.TryParse(action, out PermissionActions value);

        switch (value)
        {
            case PermissionActions.Read:
                if (claimFlag[1] == "0")
                {
                    return false;
                }
                break;
            case PermissionActions.Update:
                if (claimFlag[2] == "0")
                {
                    return false;
                }
                break;
            case PermissionActions.Create:
                if (claimFlag[3] == "0")
                {
                    return false;
                }
                break;
            case PermissionActions.Delete:
                if (claimFlag[4] == "0")
                {
                    return false;
                }
                break;
            default:
                return false;
        }
        return true;
    }
}