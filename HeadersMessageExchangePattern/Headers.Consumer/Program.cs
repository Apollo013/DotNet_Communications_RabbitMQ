using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace Headers.Consumer
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
            eventingBasicConsumer.Received += HeaderMessageEventReceiver;

            // Consume message
            channel.BasicConsume("company.queue.headers", false, eventingBasicConsumer);
        }

        private static void HeaderMessageEventReceiver(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
            Console.WriteLine($"Message received from the exchange {e.Exchange}");
            Console.WriteLine($"Content type: {e.BasicProperties.ContentType}");
            Console.WriteLine($"Consumer tag: {e.ConsumerTag}");
            Console.WriteLine($"Delivery tag: {e.DeliveryTag}");

            // Grab Headers
            Console.WriteLine();
            StringBuilder headersBuilder = new StringBuilder();
            headersBuilder.Append("Headers: ").Append(Environment.NewLine);
            foreach (var kvp in e.BasicProperties.Headers)
            {
                headersBuilder.Append(kvp.Key).Append(": ").Append(Encoding.UTF8.GetString(kvp.Value as byte[])).Append(Environment.NewLine); //each header value is transmitted as type object which must be cast to a byte array.
            }
            Console.WriteLine(headersBuilder.ToString());

            string message = Encoding.UTF8.GetString(e.Body);
            Console.WriteLine($"Message received: {message}");

            // Acknowledge
            channel.BasicAck(e.DeliveryTag, false);
        }
    }
}
