using LoggingService.Base;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace LoggingService.Loggers
{
    public class NLogger : BaseLogger
    {
        private Logger _logger;
        private Logger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(_className);
                }
                return _logger;
            }
        }
        private string _className = "";
        public NLogger(string className) : base(className)
        { _className = ClassName; }

        protected override void Configure()
        {
            // Create configuration object 
            var config = new LoggingConfiguration();

            // Create targets and add them to the configuration 
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Set target properties 
            fileTarget.FileName = "${basedir}/logs.txt";
            fileTarget.Layout = @"${date} ${logger} - ${level} - ${message}";

            // Define rules
            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Activate the configuration
            LogManager.Configuration = config;
        }

        public override void Debug(string message)
        {
            Logger.Debug(message);
        }

        public override void Error(string message)
        {
            Logger.Error(message);
        }

        public override void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public override void Info(string message)
        {
            Logger.Info(message);
        }

        public override void Trace(string message)
        {
            Logger.Trace(message);
        }

        public override void Warn(string message)
        {
            Logger.Warn(message);
        }
    }
}
