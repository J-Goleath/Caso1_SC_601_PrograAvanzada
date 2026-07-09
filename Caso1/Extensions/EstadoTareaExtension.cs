using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caso1.Common;

namespace Caso1.Extensions
{
    public static class EstadoTareaExtension
    {
        public static string ToText(this EstadoTarea estado)
        {
            return estado.ToString();
        }

        public static EstadoTarea ToEstado(this string estado)
        {
            estado = estado.Trim().ToLower();

            switch (estado)
            {
                case "pendiente":
                    return EstadoTarea.Pendiente;

                case "enproceso":
                    return EstadoTarea.EnProceso;

                case "completada":
                    return EstadoTarea.Completada;

                case "cancelada":
                    return EstadoTarea.Cancelada;

                default:
                    throw new ArgumentException($"Estado de tarea inválido: {estado}");
            }
        }
    }
}
