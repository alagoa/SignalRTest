using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace SignalRTest
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        private readonly LogLevel minimumLogLevel;

        public ConsoleLoggerProvider(LogLevel minimumLogLevel)
        {
            this.minimumLogLevel = minimumLogLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(categoryName, minimumLogLevel);
        }

        public void Dispose()
        {
            Console.Out.Flush();
            Console.ResetColor();
        }
    }

    public class ConsoleLogger : ILogger
    {
        private readonly string category;
        private readonly LogLevel minimumLogLevel;

        public ConsoleLogger(string category, LogLevel minimumLogLevel)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException($"Invalid '{nameof(category)}' argument. Must not be empty nor contain only whitespaces.");

            this.category = category;
            this.minimumLogLevel = minimumLogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (int)logLevel >= (int)minimumLogLevel;
        }

        private readonly object syncRoot = new object();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel) == false)
                return;

            string level;
            ConsoleColor fore = Console.ForegroundColor;
            ConsoleColor back = Console.BackgroundColor;

            switch (logLevel)
            {
                case LogLevel.Trace:
                    level = "TRCE";
                    fore = ConsoleColor.Gray;
                    break;
                case LogLevel.Debug:
                    level = "DBUG";
                    break;
                case LogLevel.Information:
                    level = "INFO";
                    fore = ConsoleColor.Green;
                    break;
                case LogLevel.Warning:
                    level = "WARN";
                    fore = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    level = "FAIL";
                    fore = ConsoleColor.Red;
                    break;
                case LogLevel.Critical:
                    level = "CRIT";
                    fore = ConsoleColor.Red;
                    back = ConsoleColor.White;
                    break;
                default:
                    level = "----";
                    break;
            }

            string[] parts = formatter(state, exception).Split('\n');

            lock (syncRoot)
            {
                Console.Write($"[{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}] [");
                Console.ForegroundColor = fore;
                Console.BackgroundColor = back;
                Console.Write(level);
                Console.ResetColor();
                if (parts.Length == 1)
                    Console.WriteLine($"] [{category}] {parts[0].TrimEnd('\r')}");
                else
                {
                    Console.WriteLine($"] [{category}]");
                    foreach (string str in parts.Select(x => x.TrimEnd('\r')))
                        Console.WriteLine($"    {str}");
                    Console.WriteLine();
                }
            }
        }
    }
}
