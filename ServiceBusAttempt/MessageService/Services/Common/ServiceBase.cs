using LoggingService.Base;
using MessageService.Exceptions;
using Models.Common.Interfaces;
using RabbitMQ.Client;
using System;

namespace MessageService.Services.Common
{
    public abstract class ServiceBase
    {
        #region Properties
        private BaseLogger _logger;
        protected BaseLogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LoggerFactory.Create(this.GetType().Name);
                }
                return _logger;
            }
        }

        public IModel Channel { get; protected set; }
        #endregion

        #region Constructors
        public ServiceBase(IModel channel)
        {
            if (channel == null)
            {
                Logger.Warn("Null Channel provided to constructor");
                throw new ArgumentNullException("Please provide a valid communications channel");
            }
            Channel = channel;
        }
        #endregion

        #region Validation
        /// <summary>
        /// Validates the current object
        /// </summary>
        /// <param name="objToValidate"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected bool Validate(IValidationModel objToValidate, string nullArgMessage)
        {
            if (Channel.IsClosed)
            {
                Logger.Error("Exchange Channel is closed");
                throw new ServiceException("Channel is closed");
            }

            string errormsg = "";

            if (objToValidate == null)
            {
                errormsg = $"Please supply a valid {nullArgMessage}";
                Logger.Warn(errormsg);
                throw new ArgumentNullException(errormsg);
            }

            if (!objToValidate.TryValidate(out errormsg))
            {
                Logger.Warn(errormsg);
                throw new ValidationException(errormsg);
            }

            return true;
        }
        #endregion

    }
}
