using Models.Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Models.ServiceModels.MessageModels.Base
{
    public abstract class PublishMessageBase : IValidationModel
    {
        #region Constants
        public const string CONTENTTYPE_PLAINTEXT = "plain/text";
        public const string CONTENTTYPE_JSON = "application/json";
        #endregion

        #region Properties
        private IBasicProperties _basicProperties;
        public IBasicProperties BasicProperties
        {
            get
            {
                if (_basicProperties == null)
                {
                    _basicProperties = new BasicProperties()
                    {
                        Persistent = this.Persistent,
                        ContentType = this.ContentType,
                        Priority = this.Priority
                    };
                }
                return _basicProperties;
            }
            set { _basicProperties = value; }
        }

        private string _exchangeName;
        public string ExchangeName
        {
            get { return _exchangeName ?? ""; }
            set { _exchangeName = value; }
        }

        private string _routingKey;
        public string RoutingKey
        {
            get { return _routingKey ?? ""; }
            set { _routingKey = value; }
        }

        public string Body { get; set; }
        public byte Priority { get; set; } = 0;
        public bool Persistent { get; set; } = true;
        public string ContentType { get; set; } = CONTENTTYPE_PLAINTEXT;
        public string SendType { get; set; } = RabbitMQ.Client.ExchangeType.Direct;
        #endregion

        #region Abstract
        public abstract PublicationAddress GetPublicationAddress();
        public abstract byte[] GetBytes();

        public abstract bool TryValidate(out string error);
        #endregion
    }
}
