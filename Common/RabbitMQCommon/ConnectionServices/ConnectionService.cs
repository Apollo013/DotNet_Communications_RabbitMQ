using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace RabbitMQCommon.ConnectionServices
{
    /// <summary>
    /// Service for managing RabbitMQ connections
    /// </summary>
    public sealed class ConnectionService
    {
        /// <summary>
        /// Creates a new IConnection object
        /// </summary>
        /// <returns></returns>
        public static IConnection CreateConnection(ConnectionProperties properties = null)
        {
            if (properties == null)
            {
                properties = new ConnectionProperties();
            }

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
    }
}
