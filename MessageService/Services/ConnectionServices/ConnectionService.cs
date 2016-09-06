using Models.ServiceModels.ConnectionModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;

namespace MessageService.Services.ConnectionServices
{
    /// <summary>
    /// Service for managing RabbitMQ connections
    /// </summary>
    public sealed class ConnectionService : IConnectionService
    {
        #region Properties
        private IModel _channel;
        /// <summary>
        /// Gets a RabbitMQ IModel object
        /// </summary>
        public IModel Channel
        {
            get
            {
                if (_channel == null || _channel.IsClosed)
                {
                    _channel = Connection.CreateModel();
                }
                return _channel;
            }
            private set
            {
                _channel = value;
            }
        }

        /// <summary>
        /// Gets a RabbitMQ connection object
        /// </summary>
        public IConnection Connection { get; private set; }
        #endregion

        #region Constructors
        public ConnectionService() : this(new ConnectionProperties())
        { }
        public ConnectionService(ConnectionProperties properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException("No connection properties specified");
            }

            Connection = CreateConnection(properties);
            Channel = Connection.CreateModel();
        }
        #endregion

        #region Connection
        /// <summary>
        /// Creates a new IConnection object
        /// </summary>
        /// <returns></returns>
        private IConnection CreateConnection(ConnectionProperties properties)
        {
            try
            {
                var connFactory = new ConnectionFactory()
                {
                    HostName = properties.HostName,
                    Port = properties.Port,
                    Protocol = properties.Protocol,
                    VirtualHost = properties.VirtualHost,
                    Password = properties.Password,
                    UserName = properties.UserName
                };
                return connFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                throw;
            }
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Disposes managed objects
        /// </summary>
        public void Dispose()
        {
            if (Channel != null)
            {
                if (Channel.IsOpen)
                {
                    Channel.Close();
                }
                Channel.Dispose();
                Channel = null;
            }

            if (Connection != null)
            {
                if (Connection.IsOpen)
                {
                    Connection.Close();
                }
                Connection.Dispose();
                Connection = null;
            }
        }
        #endregion
    }
}
