using System;

namespace Microsoft.Extensions.Logging
{
    public class TemplateBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string TemplateId = Guid.NewGuid().ToString();
        public string Status { get; internal set; }
        public string Application { get; internal set; }
        public string? CorrelationId { get; internal set; }
        public string LogLevel { get; internal set; }
    }
}
