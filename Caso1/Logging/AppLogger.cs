using System;
using System.IO;
using Serilog;

namespace Caso1.Logging
{
    public static class AppLogger
    {
        private static ILogger _logger;

        public static void Configurar()
        {
            string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logging");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string ruta = Path.Combine(carpeta, $"app-{timestamp}.log");

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    path: ruta,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Logger = _logger;

            _logger.Information("Aplicación iniciada correctamente.");
        }

        public static void Info(string messageTemplate, params object[] values)
            => _logger?.Information(messageTemplate, values);

        public static void Warning(string messageTemplate, params object[] values)
            => _logger?.Warning(messageTemplate, values);

        public static void Error(Exception ex, string messageTemplate, params object[] values)
            => _logger?.Error(ex, messageTemplate, values);

        public static void Debug(string messageTemplate, params object[] values)
            => _logger?.Debug(messageTemplate, values);
    }
}