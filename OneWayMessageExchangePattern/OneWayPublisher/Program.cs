using RabbitMQ.Client;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace OneWayPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Connection and Channel for transporting message
            IConnection connection = ConnectionService.CreateConnection();
            IModel channel = connection.CreateModel();
            Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));

            // Declare an Exchange and Queue and bind them
            channel.ExchangeDeclare("my.first.exchange", ExchangeType.Direct, true, false, null);
            channel.QueueDeclare("my.first.queue", true, false, false, null);
            channel.QueueBind("my.first.queue", "my.first.exchange", "");

            // Basic message properties
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Keep even after server restart
            properties.ContentType = "text/plain";

            // Publish a message
            PublicationAddress address = new PublicationAddress(ExchangeType.Direct, "my.first.exchange", "");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("This is a message from the RabbitMq .NET driver"));
            Console.WriteLine("Message sent");

            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }
    }
}
