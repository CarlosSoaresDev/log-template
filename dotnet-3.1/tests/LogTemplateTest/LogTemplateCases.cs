using FluentAssertions;
using LogTemplateApplication.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace LogTemplateTest
{
    public class LogTemplateCases
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly ApplicationBuilder _application;
        private readonly ILogger _logger;
        public LogTemplateCases()
        {
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            context.Request.Headers["x-CorrelationId"] = Guid.NewGuid().ToString();
            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(_httpContextAccessor.Object);
            serviceCollection.AddSingleton<ILoggerFactory>(new LoggerFactory());

            _application = new ApplicationBuilder(serviceCollection.BuildServiceProvider());

            var factory = _application.ApplicationServices.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<WeatherForecastController>();

            _application.UseLogTemplate("Application-One", "x-CorrelationId");

            _application.Build();
        }

        [Fact(DisplayName = "When setup use log template, cause an error when application name parameters is null or empty")]
        public void SetupUseLogTemplateNullApplicationParameterValue()
        {
            //Arrange 
            var messageError = "Value cannot be null. (Parameter 'applicationName')";

            //Act
            var nullresult = Assert.Throws<ArgumentNullException>(() => _application.UseLogTemplate(null, "X-correlationId"));
            var emptyResult = Assert.Throws<ArgumentNullException>(() => _application.UseLogTemplate("", "X-correlationId"));

            //Assert
            Assert.Equal(messageError, nullresult.Message);
            Assert.Equal(messageError, emptyResult.Message);

        }

        [Fact(DisplayName = "When setup use log template, cause an error when correlation id parameters is null or empty")]
        public void SetupUseLogTemplateNullCorrelationParameterValue()
        {
            //Arrange 
            var messageError = "Value cannot be null. (Parameter 'correlationIdHeaderName')";

            //Act
            var nullresult = Assert.Throws<ArgumentNullException>(() => _application.UseLogTemplate("Application", null));
            var emptyResult = Assert.Throws<ArgumentNullException>(() => _application.UseLogTemplate("Application", ""));

            //Assert
            Assert.Equal(messageError, nullresult.Message);
            Assert.Equal(messageError, emptyResult.Message);

        }

        [Fact(DisplayName = "When call start log information, finalize with success")]
        public void StartLogInformationSuccess()
        {
            //Arrange
            var template = new Template
            {
                Title = "Test",
                Description = "Description test",
            };

            //Act
            _logger.StartLogInformation(template);

            //Assert
            _logger.Should().NotBeNull();
        }

        [Fact(DisplayName = "When call end log information, finalize with success")]
        public void EndLogInformationSuccess()
        {
            //Arrange
            var template = new Template
            {
                Title = "Test",
                Description = "Description test",
            };

            //Act
            _logger.EndLogInformation(template);

            //Assert
            _logger.Should().NotBeNull();
        }

        [Fact(DisplayName = "When call start log error, finalize with success")]
        public void StartLogErrorSuccess()
        {
            try
            {
                throw new ArgumentException("Force Error");
            }
            catch (Exception ex)
            {
                //Arrange
                var template = new TemplateError
                {
                    Title = "Test",
                    Description = "Description test",
                    MessageErro = ex.Message,
                    StackTrace = ex.StackTrace
                };

                //Act
                _logger.EndLogError(template);

                //Assert
                _logger.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "When call end log error, finalize with success")]
        public void EndLogInErrorSuccess()
        {
            try
            {
                throw new ArgumentException("Force Error");
            }
            catch (Exception ex)
            {
                //Arrange
                var template = new TemplateError
                {
                    Title = "Test",
                    Description = "Description test",
                    MessageErro = ex.Message,
                    StackTrace = ex.StackTrace
                };

                //Act
                _logger.EndLogError(template);

                //Assert
                _logger.Should().NotBeNull();
            }
        }
    }
}