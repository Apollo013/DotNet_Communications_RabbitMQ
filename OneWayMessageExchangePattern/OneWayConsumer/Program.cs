using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace OneWayConsumer
{
    class Program
    {
        private static IModel channelForEventing;

        static void Main(string[] args)
        {
            // ReceiveMessagesWithDerivedClass ();
            ReceiveMessagesWithEvents();
        }

        private static void ReceiveMessagesWithDerivedClass()
        {
            // Create a Connection and Channel for receiving message
            IConnection connection = ConnectionService.CreateConnection();
            IModel channel = connection.CreateModel();

            // Configure how we receive messages
            channel.BasicQos(0, 1, false); // Receive only one message at a time

            // Configure handler for message
            DefaultBasicConsumer basicConsumer = new OneWayMessageReceiver(channel);

            // Consume message from queue
            channel.BasicConsume("my.first.queue", false, basicConsumer);

            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }

        private static void ReceiveMessagesWithEvents()
        {
            // Create a Connection and Channel for receiving message
            IConnection connection = ConnectionService.CreateConnection();
            channelForEventing = connection.CreateModel();

            // Configure how we receive messages
            channelForEventing.BasicQos(0, 1, false); // Receive only one message at a time

            // Configure handler for message
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channelForEventing);
            eventingBasicConsumer.Received += EventingBasicConsumer_Received;

            // Consume message from queue
            channelForEventing.BasicConsume("my.first.queue", false, eventingBasicConsumer);

            // Close Connection & Channel
            channelForEventing.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channelForEventing.IsClosed));
            Console.ReadKey();
        }

        /// <summary>
        /// Event Based Message Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            IBasicProperties basicProperties = e.BasicProperties;
            Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
            Console.WriteLine(string.Concat("Message received from the exchange ", e.Exchange));
            Console.WriteLine(string.Concat("Content type: ", basicProperties.ContentType));
            Console.WriteLine(string.Concat("Consumer tag: ", e.ConsumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", e.DeliveryTag));
            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(e.Body)));
            channelForEventing.BasicAck(e.DeliveryTag, false);
        }
    }

    /// <summary>
    /// Derived Class Message Handler
    /// </summary>
    class OneWayMessageReceiver : DefaultBasicConsumer
    {
        private IModel _channel { get; set; }

        public OneWayMessageReceiver(IModel channel)
        {
            _channel = channel;
        }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            Console.WriteLine("Message received by the consumer. Check the debug window for details.");
            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Console.WriteLine(string.Concat("Content type: ", properties.ContentType));
            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));
            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body)));
            _channel.BasicAck(deliveryTag, false);
        }
    }
}
