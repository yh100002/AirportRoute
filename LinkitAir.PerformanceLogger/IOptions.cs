using System;
using Microsoft.Extensions.Logging;

namespace LinkitAir.PerformanceLogger
{
    public interface IOptions
    {
        IOptions WithLogLevel(LogLevel logLevel);
        IOptions WithFormat(string format);
    }
}
