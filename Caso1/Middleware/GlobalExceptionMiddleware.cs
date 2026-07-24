using Microsoft.Owin;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Caso1.Middleware
{
    public class GlobalExceptionMiddleware : OwinMiddleware
    {
        public GlobalExceptionMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                GuardarError(context, ex);

                if (context.Request.Path.Value != null &&
                    context.Request.Path.Value.StartsWith("/Error"))
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("Ocurrió un error inesperado.");
                    return;
                }

                context.Response.StatusCode = 500;
                context.Response.Redirect("/Error/ServerError");
            }
        }

        private void GuardarError(IOwinContext context, Exception ex)
        {
            try
            {
                string ruta = System.Web.Hosting.HostingEnvironment.MapPath("~/Logging/errores.txt");

                if (string.IsNullOrEmpty(ruta))
                {
                    return;
                }

                string carpeta = Path.GetDirectoryName(ruta);

                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                string mensaje =
                    "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + Environment.NewLine +
                    "Ruta: " + context.Request.Path + Environment.NewLine +
                    "Mensaje: " + ex.Message + Environment.NewLine +
                    "Detalle: " + ex.StackTrace + Environment.NewLine +
                    "----------------------------------------" + Environment.NewLine;

                File.AppendAllText(ruta, mensaje);
            }
            catch
            {
                // No se debe romper el sistema si falla el guardado del log.
            }
        }
    }
}