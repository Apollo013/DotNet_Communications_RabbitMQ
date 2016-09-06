namespace LoggingService.Base
{
    /// <summary>
    /// Base class for all logging services
    /// </summary>
    public abstract class BaseLogger
    {
        public string ClassName { get; set; }
        public BaseLogger(string className)
        {
            ClassName = className;
            Configure();
        }
        protected abstract void Configure();
        public abstract void Trace(string message);
        public abstract void Debug(string message);
        public abstract void Info(string message);
        public abstract void Warn(string message);
        public abstract void Error(string message);
        public abstract void Fatal(string message);
    }
}
