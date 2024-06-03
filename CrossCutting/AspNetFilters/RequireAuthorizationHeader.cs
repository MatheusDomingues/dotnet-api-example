using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using api.CrossCutting.HttpContext;
using System.Net;

namespace api.CrossCutting.AspNetFilters;

public class RequireAuthorizationHeader : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var _user = context.HttpContext.RequestServices.GetService<IUserContext>();

        if (string.IsNullOrEmpty(_user?.GetToken()))
        {
            context.Result = new ContentResult()
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Content = "Token de autorização é necessário"
            };
        }
    }
}