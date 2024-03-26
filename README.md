<h1>
LogTemplate
</h1>

[![Build Status](https://img-static-file.s3.amazonaws.com/LogTemplate/log_template_ci.svg)](https://github.com/CarlosSoaresDev/log-template/actions/runs/8399493686) [![NuGet](https://img-static-file.s3.amazonaws.com/LogTemplate/log_template_version.svg)](https://www.nuget.org/packages/LogTemplate) [![Nuget](https://img-static-file.s3.amazonaws.com/LogTemplate/log_template_downloads.svg)](https://www.nuget.org/packages/LogTemplate)


A library commonly used for log standardization, aimed at enhancing flow tracking, it's is a ILogger extension.

---

### Get Started

LogTemplate can be installed using the Nuget package manager or the `dotnet` CLI.

```
dotnet add package LogTemplate --version 6.0.1
```

---

### Example program file
```csharp
var builder = WebApplication.CreateBuilder(args); 
// Inject context
builder.Services.AddLogTemplate();

var app = builder.Build();

app.MapControllers();
// Set parameters
app.UseLogTemplate("your-application-name", "x-your-correlation-identifier");

```

### Example case
```csharp

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                _logger.StartLogInformation(new Template
                {
                    Title = "Started Process",
                    Description = "No details"
                });

                var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();

                _logger.StartLogInformation(new Template
                {
                    Title = "Finished Process",
                    Description = "No details"
                });

                return weatherForecasts;
            }
            catch (Exception ex)
            {
                _logger.EndLogError(new TemplateError
                {
                    Title = "Processamento finalizado",
                    Description = "Sem detalhe",
                    MessageErro = ex.Message,
                    StackTrace = ex.StackTrace
                });

                throw;
            }
        }

```

### Output log without error
``` json
info: LogTemplateApplication.Controllers.WeatherForecastController[0]
{
  "Title": "Started Process",
  "Description": "No details",
  "Status": "Started",
  "Application": "LogTemplate",
  "CorrelationId": "7d38107e-48e6-4f17-b653-3e71654a0c45",
  "LogLevel": "Information"
}
info: LogTemplateApplication.Controllers.WeatherForecastController[0]
{
  "Title": "Finished Process",
  "Description": "No details",
  "Status": "Started",
  "Application": "LogTemplate",
  "CorrelationId": "7d38107e-48e6-4f17-b653-3e71654a0c45",
  "LogLevel": "Information"
}
```

### Output log with error
``` json
LogTemplateApplication.Controllers.WeatherForecastController[0]
{
  "Title": "Started Process",
  "Description": "No details",
  "Status": "Started",
  "Application": "LogTemplate",
  "CorrelationId": "7d38107e-48e6-4f17-b653-3e71654a0c45",
  "LogLevel": "Information"
}
fail: LogTemplateApplication.Controllers.WeatherForecastController[0]
{
  "MessageErro": "Intentional error",
  "StackTrace": "at LogTemplateApplication.Controllers.WeatherForecastController.GetWithError() in LogTemplateApplication\\Controllers\\WeatherForecastController.cs:line 73",
  "Title": "Ended Process",
  "Description": "No details",
  "Status": "Finished",
  "Application": "LogTemplate",
  "CorrelationId": "7d38107e-48e6-4f17-b653-3e71654a0c45",
  "LogLevel": "Error"
}
```

### Mantainer
[Carlos Alves](https://www.linkedin.com/in/carlos-alves-soares-b707a4152/) Linkedin profile