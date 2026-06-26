using System;
using System.Web.Mvc;
using Caso1.Logging;

namespace Caso1.Filters
{
    public class AuditFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controlador = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var accion      = filterContext.ActionDescriptor.ActionName;
            var metodo      = filterContext.HttpContext.Request.HttpMethod;
            var url         = filterContext.HttpContext.Request.RawUrl;

            AppLogger.Info(
                "[AUDITORÍA] {Fecha} {Hora} | {Metodo} {Url} | Controlador: {Controlador} | Acción: {Accion}",
                DateTime.Now.ToString("yyyy-MM-dd"),
                DateTime.Now.ToString("HH:mm:ss"),
                metodo,
                url,
                controlador,
                accion);

            base.OnActionExecuting(filterContext);
        }
    }
}
