using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Middlewares
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(bool isAnd = false, params string[] policys) : base(typeof(MultiplePolicysAuthorizeFilter))
        {
            Arguments = new object[] { policys, isAnd };
        }

        public PermissionAttribute(params string[] policys) : base(typeof(MultiplePolicysAuthorizeFilter))
        {
            Arguments = new object[] { policys, false };
        }
    }

    public class MultiplePolicysAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorization;
        public string[] Policys { get; private set; }
        public bool IsAnd { get; private set; }

        public MultiplePolicysAuthorizeFilter(string[] policys, bool isAnd, IAuthorizationService authorization)
        {
            Policys = policys;
            IsAnd = isAnd;
            _authorization = authorization;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (IsAnd)
            {
                foreach (var policy in Policys)
                {
                    var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (!authorized.Succeeded)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }

                }
            }
            else
            {
                foreach (var policy in Policys)
                {
                    var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, policy);
                    if (authorized.Succeeded)
                    {
                        return;
                    }

                }
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
