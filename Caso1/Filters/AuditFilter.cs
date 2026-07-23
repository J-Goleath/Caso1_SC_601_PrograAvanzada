using System;
using System.Diagnostics;
using System.Web.Mvc;
using Caso1.Logging;

namespace Caso1.Filters
{
    public class AuditFilter : ActionFilterAttribute
    {
        private const string StartTimeKey = "AuditFilter.StartTime";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Items[StartTimeKey] = DateTime.Now;

            string userName = filterContext.HttpContext
                .User
                .Identity
                .IsAuthenticated
                ? filterContext.HttpContext.User.Identity.Name
                : "Anonimo";

            string controller = filterContext.ActionDescriptor
                .ControllerDescriptor
                .ControllerName;

            string action = filterContext.ActionDescriptor
                .ActionName;

            string ipAddress = filterContext.HttpContext
                .Request
                .UserHostAddress;

            AppLogger.Info(
                "[INICIO] Usuario: {Usuario} | IP: {IP} | Controlador: {Controlador} | Acción: {Accion} | Hora: {Hora}",
                userName,
                ipAddress ?? "IP no disponible",
                controller,
                action,
                DateTime.Now.ToString("HH:mm:ss"));

            Debug.WriteLine("=============================");
            Debug.WriteLine($"INICIO: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            Debug.WriteLine($"Usuario: {userName}");
            Debug.WriteLine($"IP: {ipAddress ?? "IP no disponible"}");
            Debug.WriteLine($"Controller: {controller}");
            Debug.WriteLine($"Action: {action}");

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            DateTime? startTime = filterContext.HttpContext.Items[StartTimeKey] as DateTime?;
            TimeSpan? elapsed = startTime.HasValue
                ? DateTime.Now - startTime.Value
                : (TimeSpan?)null;

            string resultado = filterContext.Exception == null
                ? "Éxito"
                : $"Error: {filterContext.Exception.Message}";

            string userName = filterContext.HttpContext
                .User
                .Identity
                .IsAuthenticated
                ? filterContext.HttpContext.User.Identity.Name
                : "Anonimo";

            string controller = filterContext.ActionDescriptor
                .ControllerDescriptor
                .ControllerName;

            string action = filterContext.ActionDescriptor
                .ActionName;

            AppLogger.Info(
                "[FIN] Usuario: {Usuario} | Controlador: {Controlador} | Acción: {Accion} | Resultado: {Resultado} | Duración: {Duracion}ms | Hora: {Hora}",
                userName,
                controller,
                action,
                resultado,
                elapsed?.TotalMilliseconds.ToString("F2") ?? "N/A",
                DateTime.Now.ToString("HH:mm:ss"));

            if (elapsed.HasValue)
            {
                Debug.WriteLine($"Duración: {elapsed.Value.TotalMilliseconds} ms");
            }
            Debug.WriteLine($"Resultado: {resultado}");
            Debug.WriteLine($"FIN: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            Debug.WriteLine("=============================");

            base.OnActionExecuted(filterContext);
        }
    }
}
