using MessageService.Services.AddressServices;
using MessageService.Services.AddressServices.Base;
using MessageService.Services.ConnectionServices;
using MessageService.Services.TransportServices.Consumers;
using MessageService.Services.TransportServices.Consumers.Base;
using MessageService.Services.TransportServices.Publishers;
using MessageService.Services.TransportServices.Publishers.Base;
using System;

namespace MessageService.Managers
{
    /// <summary>
    /// A RabbitMQ service manager (Unit of work)
    /// </summary>
    public class ServiceManager : IDisposable
    {
        #region Properties
        private IConnectionService _connection;
        /// <summary>
        /// Service for managing Connections
        /// </summary>
        public IConnectionService Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new ConnectionService();
                }
                return _connection;
            }
            private set { _connection = value; }
        }

        private IQueueService _queueService;
        /// <summary>
        /// Service for managing Queues
        /// </summary>
        public IQueueService QueueService
        {
            get
            {
                if (_queueService == null)
                {
                    _queueService = new QueueService(Connection.Channel);
                }
                return _queueService;
            }
            set { _queueService = value; }
        }

        private IExchangeService _exchangeService;
        /// <summary>
        /// Service for managing Exchanges
        /// </summary>
        public IExchangeService ExchangeService
        {
            get
            {
                if (_exchangeService == null)
                {
                    _exchangeService = new ExchangeService(Connection.Channel, QueueService);
                }
                return _exchangeService;
            }
            set { _exchangeService = value; }
        }

        private IMessagePublisher _publisherService;
        /// <summary>
        /// Service for publishing messages
        /// </summary>
        public IMessagePublisher PublisherService
        {
            get
            {
                if (_publisherService == null)
                {
                    _publisherService = new MessagePublisher(Connection.Channel, ExchangeService);
                }
                return _publisherService;
            }
            set { _publisherService = value; }
        }

        private IEnumerableConsumerService _ConsumerService;

        public IEnumerableConsumerService ConsumerService
        {
            get {
                if(_ConsumerService == null)
                {
                    return new EnumerableConsumerService(Connection.Channel);
                }
                return _ConsumerService;
            }
            set { _ConsumerService = value; }
        }

        #endregion

        #region 'IDisposable' Implementation
        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }

            _queueService = null;
            _exchangeService = null;
        }
        #endregion
    }
}
