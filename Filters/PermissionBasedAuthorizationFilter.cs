using System.Security.Claims;
using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class PermissionBasedAuthorizationFilter(ApplicationDbContext dbcontext) : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {       //in first or default , it will get result or null, because we are in refrense types
            var attribute = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(chkA => chkA is CheckPermissionAttribute);
            if(attribute != null)
            {
                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if(claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var hasPermissions = dbcontext.userPermissions.Any(userp => userp.UserId == userId && userp.permision == attribute._permision);
                    if(!hasPermissions)
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }


        }
    }
}
