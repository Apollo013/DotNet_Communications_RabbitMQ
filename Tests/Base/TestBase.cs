using MessageService.Services.AddressServices;
using MessageService.Services.AddressServices.Base;
using NUnit.Framework;
using RabbitMQ.Client;

namespace Tests.Base
{
    [SetUpFixture]
    public class TestBase
    {
        protected IConnection Connection { get; private set; }
        protected IModel Channel { get; private set; }
        public IQueueService BaseQueueService { get; private set; }
        public IExchangeService BaseExchangeService { get; protected set; }


        // We'll use the following when we want to test connection or channel failures
        public IModel ClosedChannel { get; private set; }


        [SetUp]
        public virtual void Init()
        {
            var connFactory = new ConnectionFactory();
            Connection = connFactory.CreateConnection();
            Channel = Connection.CreateModel();
            BaseQueueService = new QueueService(Channel);
            BaseExchangeService = new ExchangeService(Channel);

            // Closed Channel
            ClosedChannel = Connection.CreateModel();
            ClosedChannel.Close();
        }

        [TearDown]
        public virtual void Close()
        {
            if (Channel.IsOpen)
            {
                Channel.Close();
            }
            if (Connection.IsOpen)
            {
                Connection.Close();
            }
            BaseExchangeService = null;
            BaseQueueService = null;
        }
    }
}
