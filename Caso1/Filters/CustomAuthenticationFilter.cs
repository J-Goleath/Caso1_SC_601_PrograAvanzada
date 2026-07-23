using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Caso1.Filters
{
    public class CustomAuthenticationFilter : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            bool isAuthenticated = filterContext.HttpContext
                .User
                .Identity
                .IsAuthenticated;

            if (!isAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}