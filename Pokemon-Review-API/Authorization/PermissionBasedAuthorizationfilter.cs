using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pokemon_Review_API.Data;
using ProjectCRUD.Data;
using System.Security.Claims;

namespace Pokemon_Review_API.Authorization
{
    public class PermissionBasedAuthorizationfilter(ApplictionDBCotext dBCotext ) : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var attripute=(CheckPermissionAttribute)context.ActionDescriptor.
                EndpointMetadata.FirstOrDefault(x=>x is CheckPermissionAttribute);
            if (attripute != null)
            {
                var claimIdintity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimIdintity == null || !claimIdintity.IsAuthenticated) {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(claimIdintity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var hasPermission = dBCotext.Set<UserPermission>().Any(X => X.UserId == userId 
                    && X.PermissionId == attripute.premission);
                    if (!hasPermission)
                    {
                        context.Result = new ForbidResult();
                    }

                }

            }
        }

    }
}
