using RabbitMQ.Client;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace OneWayConsumer
{
    class Program
    {
        static void Main(string[] args)
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
    }

    /// <summary>
    /// Message Handler
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
