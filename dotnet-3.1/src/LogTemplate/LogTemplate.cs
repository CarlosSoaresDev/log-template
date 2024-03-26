using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace Microsoft.Extensions.Logging
{
    public static class LogTemplate
    {
        private static string ApplicationName { get; set; }
        private static string CorrelationIdHeaderName { get; set; }
        private static IHttpContextAccessor _context { get; set; }

        public static void AddLogTemplate(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        public static void UseLogTemplate(this IApplicationBuilder applicationBuilder, string applicationName, string correlationIdHeaderName)
        {
            ApplicationName = !string.IsNullOrEmpty(applicationName) ? applicationName : throw new ArgumentNullException(nameof(applicationName));
            CorrelationIdHeaderName = !string.IsNullOrEmpty(correlationIdHeaderName) ? correlationIdHeaderName : throw new ArgumentNullException(nameof(correlationIdHeaderName));

            _context = applicationBuilder.ApplicationServices.GetService<IHttpContextAccessor>();
        }

        public static void StartLogInformation(this ILogger logger, Template template)
        {
            SetFields(template, LogLevel.Information, LogType.Started);

            logger.LogInformation(JsonSerializer.Serialize(template));
        }

        public static void EndLogInformation(this ILogger logger, Template template)
        {
            SetFields(template, LogLevel.Information, LogType.Finished);

            logger.LogInformation(JsonSerializer.Serialize(template));
        }

        public static void StartLogError(this ILogger logger, Template template)
        {
            SetFields(template, LogLevel.Error, LogType.Started);

            logger.LogError(JsonSerializer.Serialize(template));
        }

        public static void EndLogError(this ILogger logger, TemplateError template)
        {
            SetFields(template, LogLevel.Error, LogType.Finished);

            logger.LogError(JsonSerializer.Serialize(template));
        }

        private static void SetFields(TemplateBase template, LogLevel logLevel, LogType logType)
        {
            template.Status = logType.ToString();
            template.LogLevel = logLevel.ToString();
            template.CorrelationId = GetCorrelationIdByHeader();
            template.Application = ApplicationName;
        }

        private static string? GetCorrelationIdByHeader()
            => _context.HttpContext.Request?.Headers?[CorrelationIdHeaderName];

    }
}