using RabbitMQ.Client;
using RabbitMQCommon.Attributes;

namespace RabbitMQCommon.ConnectionServices
{
    /// <summary>
    /// Default properties used to create a 'ConnectionFactory' object.
    /// </summary>
    public class ConnectionProperties : PropertyBase
    {
        /// <summary>
        /// Gets or sets the Uri for the broker that we will connect to.
        /// </summary>
        [PropertySetting(Name = "conn")]
        public string Uri
        {
            get { return ((string)base["conn"]); }
            set { base["conn"] = value; }
        }

        /// <summary>
        /// Gets or sets the hostname of the broker that we will connect to.
        /// </summary>
        [PropertySetting(Name = "hostname", DefaultValue = "localhost", IsRequired = true)]
        public string HostName
        {
            get { return ((string)base["hostname"]); }
            set { base["hostname"] = value; }
        }

        /// <summary>
        /// the virtual host on the broker to access
        /// </summary>
        [PropertySetting(Name = "vhost", DefaultValue = ConnectionFactory.DefaultVHost, IsRequired = true)]
        public string VirtualHost
        {
            get { return ((string)base["vhost"]); }
            set { base["vhost"] = value; }
        }

        /// <summary>
        /// Gets or sets the port number of the broker that we will connect to.
        /// </summary>
        [PropertySetting(Name = "port", DefaultValue = AmqpTcpEndpoint.UseDefaultPort, IsRequired = true)]
        public int Port
        {
            get { return ((int)base["port"]); }
            set { base["port"] = value; }
        }

        /// <summary>
        /// Gets or sets the username to use when authenticating with the broker
        /// </summary>
        [PropertySetting(Name = "username", DefaultValue = ConnectionFactory.DefaultUser, IsRequired = true)]
        public string UserName
        {
            get { return ((string)base["username"]); }
            set { base["username"] = value; }
        }

        /// <summary>
        /// Gets or sets the password to use when authenticating with the broker
        /// </summary>
        [PropertySetting(Name = "password", DefaultValue = ConnectionFactory.DefaultPass, IsRequired = true)]
        public string Password
        {
            get { return ((string)base["password"]); }
            set { base["password"] = value; }
        }

        /// <summary>
        /// Gets the protocol used for connecting with the broker
        /// </summary>
        public IProtocol Protocol { get; } = Protocols.DefaultProtocol;
    }
}
