using System;
using Serilog;
using Common;

namespace Infrastructure.Logging
{

    public class SerilogAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger _logger = Log.ForContext<T>();

        public void LogInformation(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.Warning(message, args);
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.Error(ex, message, args);
        }
    }
}