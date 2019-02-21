using Microsoft.Extensions.Logging;

namespace LinkitAir.PerformanceLogger
{
    public class LogOptions : ILogOptions, IOptions
    {
        public LogLevel LogLevel { get; set; }

        public string Format { get; set; }

        public LogOptions()
        {
            Default();
        }

        public void Default()
        {
            LogLevel = LogLevel.Information;
            Format = "request to {Operation} took {Duration}ms";
        }

        public IOptions WithFormat(string format)
        {
            this.Format = format;
            return this;
        }

        public IOptions Configure()
        {
            return this;
        }

        public IOptions WithLogLevel(LogLevel logLevel)
        {
            this.LogLevel = logLevel;
            return this;
        }

    }
}