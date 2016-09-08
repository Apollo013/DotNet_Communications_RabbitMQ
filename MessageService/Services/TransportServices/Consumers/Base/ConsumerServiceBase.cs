using Models.ServiceModels.MessageModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections;

namespace MessageService.Services.TransportServices.Consumers.Base
{
    public abstract class EnumerableConsumerServiceBase : IEnumerable, IEnumerator
    {
        #region PROPERTIES
        protected IModel Channel { get; set; }
        protected BasicDeliverEventArgs LatestDelivery { get; set; }
        private volatile EventingBasicConsumer _messageConsumer;
        public EventingBasicConsumer MessageConsumer
        {
            get
            {
                if (_messageConsumer == null)
                {
                    _messageConsumer = new EventingBasicConsumer(Channel);
                }
                return _messageConsumer;
            }
            set { _messageConsumer = value; }
        }
        protected ConsumerRequest Request { get; set; }
        protected QualityOfService ServiceQuality
        {
            get { return Request.QualityOfService; }
        }

        protected string QueueName { get { return Request.QueueName; } }
        protected string ConsumerTag
        {
            get { return Request.ConsumerTag; }
            set { Request.ConsumerTag = value; }
        }
        protected bool NoAck { get { return Request.NoAck; } }
        public bool IsRunning { get { return MessageConsumer.IsRunning; } }
        public abstract object Current { get; }
        #endregion

        #region METHODS
        public abstract void Read(string queueName, bool noAck = false, string consumerTag = null, QualityOfService quality = null);
        public abstract void Read(ConsumerRequest request);
        public abstract bool Ack();
        public abstract bool Ack(BasicDeliverEventArgs args);
        public abstract bool Ack(ulong deliveryTag);
        public abstract void Nack(bool requeue);
        public abstract void Nack(bool requeue, bool multiple);
        public abstract void Nack(BasicDeliverEventArgs args, bool requeue, bool multiple);
        public abstract BasicDeliverEventArgs Next();
        public abstract bool Next(int millisecondsTimeout, out BasicDeliverEventArgs result);
        public abstract void Close();

        public abstract bool MoveNext();

        public abstract void Reset();

        public abstract IEnumerator GetEnumerator();
        #endregion
    }
}
