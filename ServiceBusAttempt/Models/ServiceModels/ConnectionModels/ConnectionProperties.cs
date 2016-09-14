using Models.Common.Attributes;
using Models.ServiceModels.Base;
using RabbitMQ.Client;
using static Models.Common.Constants.ConnectionConstants;

namespace Models.ServiceModels.ConnectionModels
{
    /// <summary>
    /// Default properties used to create a 'ConnectionFactory' object.
    /// </summary>
    public class ConnectionProperties : PropertyBase
    {
        /// <summary>
        /// Gets or sets the Uri for the broker that we will connect to.
        /// </summary>
        [PropertySetting(Name = CONNECTION_URI)]
        public string Uri
        {
            get { return ((string)base[CONNECTION_URI]); }
            set { base[CONNECTION_URI] = value; }
        }

        /// <summary>
        /// Gets or sets the hostname of the broker that we will connect to.
        /// </summary>
        [PropertySetting(Name = CONNECTION_HOST, DefaultValue = "localhost", IsRequired = true)]
        public string HostName
        {
            get { return ((string)base[CONNECTION_HOST]); }
            set { base[CONNECTION_HOST] = value; }
        }

        /// <summary>
        /// the virtual host on the broker to access
        /// </summary>
        [PropertySetting(Name = CONNECTION_VHOST, DefaultValue = ConnectionFactory.DefaultVHost, IsRequired = true)]
        public string VirtualHost
        {
            get { return ((string)base[CONNECTION_VHOST]); }
            set { base[CONNECTION_VHOST] = value; }
        }

        /// <summary>
        /// Gets or sets the port number of the broker that we will connect to.
        /// </summary>
        [PropertySetting(Name = CONNECTION_PORT, DefaultValue = AmqpTcpEndpoint.UseDefaultPort, IsRequired = true)]
        public int Port
        {
            get { return ((int)base[CONNECTION_PORT]); }
            set { base[CONNECTION_PORT] = value; }
        }

        /// <summary>
        /// Gets or sets the username to use when authenticating with the broker
        /// </summary>
        [PropertySetting(Name = CONNECTION_USERNAME, DefaultValue = ConnectionFactory.DefaultUser, IsRequired = true)]
        public string UserName
        {
            get { return ((string)base[CONNECTION_USERNAME]); }
            set { base[CONNECTION_USERNAME] = value; }
        }

        /// <summary>
        /// Gets or sets the password to use when authenticating with the broker
        /// </summary>
        [PropertySetting(Name = CONNECTION_PASSWORD, DefaultValue = ConnectionFactory.DefaultPass, IsRequired = true)]
        public string Password
        {
            get { return ((string)base[CONNECTION_PASSWORD]); }
            set { base[CONNECTION_PASSWORD] = value; }
        }

        /// <summary>
        /// Gets the protocol used for connecting with the broker
        /// </summary>
        public IProtocol Protocol { get; } = Protocols.DefaultProtocol;
    }
}
