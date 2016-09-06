using LoggingService.Loggers;

namespace LoggingService.Base
{
    /// <summary>
    /// Log Factory
    /// </summary>
    public class LoggerFactory
    {
        private LoggerFactory()
        { }

        /// <summary>
        /// Creates a new Log service instance
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static BaseLogger Create(string className)
        {
            return new NLogger(className);
        }
    }
}
