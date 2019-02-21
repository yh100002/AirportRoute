using System;
using Microsoft.AspNetCore.Builder;

namespace LinkitAir.PerformanceLogger
{
    public static class LogMiddlewareExtension
    {
        public static IApplicationBuilder UsePerformanceLog(this IApplicationBuilder app, LogOptions options)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return app.UseMiddleware<LogMiddleware>(options);
        }

        public static IApplicationBuilder UsePerformanceLog(this IApplicationBuilder app, Action<ILogOptions> action)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var options = new LogOptions();

            action.Invoke(options);

            return app.UseMiddleware<LogMiddleware>(options);
        }
    }
}