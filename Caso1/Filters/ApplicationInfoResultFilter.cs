using System;
using System.Configuration;
using System.Web.Mvc;

namespace Caso1.Filters
{
    public class ApplicationInfoResultFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (!(filterContext.Result is ViewResultBase))
            {
                base.OnResultExecuting(filterContext);
                return;
            }

            string currentUser = filterContext.HttpContext
                .User
                .Identity
                .IsAuthenticated
                ? filterContext.HttpContext
                    .User
                    .Identity
                    .Name
                : "Anonimo";

            filterContext.Controller
                .ViewBag
                .CurrentUser = currentUser;

            filterContext.Controller
                .ViewBag
                .CurrentDate = DateTime.Now
                .ToString("dd/MM/yyyy HH:mm:ss");

            string environmentName = ConfigurationManager
                .AppSettings["EnvironmentName"] ?? "No definido";

            filterContext.Controller
                .ViewBag
                .EnvironmentName = environmentName;

            filterContext.Controller
                .ViewBag
                .ServerName = Environment.MachineName;

            string version = System.Reflection
                .Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version?
                .ToString() ?? "1.0.0.0";

            filterContext.Controller
                .ViewBag
                .ApplicationVersion = version;

            base.OnResultExecuting(filterContext);
        }
    }
}