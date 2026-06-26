using System.Web.Mvc;
using Caso1.Logging;

namespace Caso1.Filters
{
    public class GlobalExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            AppLogger.Error(
                filterContext.Exception,
                "[ERROR NO CONTROLADO] Controlador: {Controlador} | Acción: {Accion} | Mensaje: {Mensaje}",
                filterContext.RouteData.Values["controller"],
                filterContext.RouteData.Values["action"],
                filterContext.Exception.Message);

            filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(
                    new HandleErrorInfo(
                        filterContext.Exception,
                        filterContext.RouteData.Values["controller"]?.ToString(),
                        filterContext.RouteData.Values["action"]?.ToString()))
            };

            filterContext.HttpContext.Response.StatusCode = 500;
        }
    }
}