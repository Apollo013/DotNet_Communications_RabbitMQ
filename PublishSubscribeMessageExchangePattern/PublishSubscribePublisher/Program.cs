using RabbitMQ.Client;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace PublishSubscribePublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Connection and Channel for transporting message
            IConnection connection = ConnectionService.CreateConnection();
            IModel channel = connection.CreateModel();
            Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));

            // Declare Exchange & Queues
            channel.ExchangeDeclare("mycompany.fanout.exchange", ExchangeType.Fanout, true, false, null);
            channel.QueueDeclare("mycompany.queues.accounting", true, false, false, null);
            channel.QueueDeclare("mycompany.queues.management", true, false, false, null);

            // Bind Queues to Exchange
            channel.QueueBind("mycompany.queues.accounting", "mycompany.fanout.exchange", "");
            channel.QueueBind("mycompany.queues.management", "mycompany.fanout.exchange", "");

            // Basic message properties
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Keep even after server restart
            properties.ContentType = "text/plain";

            // Publish a message
            PublicationAddress address = new PublicationAddress(ExchangeType.Fanout, "mycompany.fanout.exchange", "");
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
