namespace Microsoft.Extensions.Logging
{
    public class TemplateError: TemplateBase
    {
        public string MessageErro { get; set; }
        public string? StackTrace { get; set; }
    }
}
