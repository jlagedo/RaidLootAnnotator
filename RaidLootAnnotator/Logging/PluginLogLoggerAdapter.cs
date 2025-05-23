using System;
using Dalamud.Plugin.Services;
using Microsoft.Extensions.Logging;

namespace RaidLootAnnotator.Logging
{
    public class PluginLogLoggerAdapter<T> : ILogger<T>
    {
        private readonly IPluginLog _pluginLog;

        public PluginLogLoggerAdapter(IPluginLog pluginLog)
        {
            _pluginLog = pluginLog;
        }

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _pluginLog.Debug(message);
                    break;
                case LogLevel.Information:
                    _pluginLog.Information(message);
                    break;
                case LogLevel.Warning:
                    _pluginLog.Warning(exception, message);
                    break;
                case LogLevel.Error:
                    _pluginLog.Error(exception, message);
                    break;
                case LogLevel.Critical:
                    _pluginLog.Error(exception, message);
                    break;
                default:
                    _pluginLog.Information(message);
                    break;
            }
        }

        private class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new NullScope();
            public void Dispose() { }
        }
    }
}
