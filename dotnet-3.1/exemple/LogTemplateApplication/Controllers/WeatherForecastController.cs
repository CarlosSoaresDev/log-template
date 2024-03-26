using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogTemplateApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.StartLogInformation(new Template
            {
                Title = "Started Process",
                Description = "No details"
            });

            var rng = new Random();
            var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            _logger.StartLogInformation(new Template
            {
                Title = "Finished Process",
                Description = "No details"
            });

            return weatherForecasts;
        }

        [HttpGet("Error")]
        public IEnumerable<WeatherForecast> GetWithError()
        {
            try
            {
                _logger.StartLogInformation(new Template
                {
                    Title = "Started Process",
                    Description = "No details"
                });

                throw new ArgumentException("Intentional error");
            }
            catch (Exception ex)
            {
                _logger.EndLogError(new TemplateError
                {
                    Title = "Ended Process",
                    Description = "No details",
                    MessageErro = ex.Message,
                    StackTrace = ex.StackTrace
                });

                throw;
            }
        }
    }
}