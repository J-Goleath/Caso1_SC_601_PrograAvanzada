using System.Web.Mvc;
using Caso1.Filters;

namespace Caso1.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuditFilter());

            filters.Add(new GlobalExceptionFilter());
        }
    }
}
