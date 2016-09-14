using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace Topics.Consumer
{
    class Program
    {
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();

        static void Main(string[] args)
        {
            // Configure how we receive messages
            channel.BasicQos(0, 1, false); // Process only one message at a time

            // Configure handler for message
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += OneWayMessageEventReceiver;

            // Consume message
            channel.BasicConsume("company.queue.topic", false, eventingBasicConsumer);
        }

        private static void OneWayMessageEventReceiver(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
            Console.WriteLine($"Message received from the exchange {e.Exchange}");
            Console.WriteLine($"Content type: {e.BasicProperties.ContentType}");
            Console.WriteLine($"Consumer tag: {e.ConsumerTag}");
            Console.WriteLine($"Delivery tag: {e.DeliveryTag}");

            string message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine($"Message received: {message}");

            // Acknowledge
            channel.BasicAck(e.DeliveryTag, false);
        }
    }
}
