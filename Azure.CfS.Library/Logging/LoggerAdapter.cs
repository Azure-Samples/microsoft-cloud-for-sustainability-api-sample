using Azure.CfS.Library.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace Azure.CfS.Library.Logging
{
    internal class LoggerAdapter<TType> : ILoggerAdapter<TType>
    {
        private readonly ILogger<TType> _logger;

        public LoggerAdapter(ILogger<TType> logger)
        {
            _logger = logger;
        }

        public void LogError(Exception exception, string message)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(exception, message);
            }
        }

        public void LogError(string message)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(message);
            }
        }
    }
}
