using RabbitMQ.Client;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace RoutingKeys.Publisher
{
    class Program
    {
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();

        static void Main(string[] args)
        {
            // Exchange / Queue Declaration & Binding
            channel.ExchangeDeclare("company.exchange.routing", ExchangeType.Direct, true, false, null);
            channel.QueueDeclare("company.exchange.queue", true, false, false, null);

            // A queue can be dedicated to one or more routing keys.
            channel.QueueBind("company.exchange.queue", "company.exchange.routing", "asia");
            channel.QueueBind("company.exchange.queue", "company.exchange.routing", "americas");
            channel.QueueBind("company.exchange.queue", "company.exchange.routing", "europe");

            // Basic message properties
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Keep even after server restart
            properties.ContentType = "text/plain";

            // Publish a message 'Asia'
            PublicationAddress address = new PublicationAddress(ExchangeType.Direct, "company.exchange.routing", "asia");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from Asia!"));
            Console.WriteLine("Message sent to: Asia");

            // Publish a message 'America'
            address = new PublicationAddress(ExchangeType.Direct, "company.exchange.routing", "americas");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from America!"));
            Console.WriteLine("Message sent to: America");

            // Publish a message 'Europe'
            address = new PublicationAddress(ExchangeType.Direct, "company.exchange.routing", "europe");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from Europe!"));
            Console.WriteLine("Message sent to: Europe");

            // Publish a message 'Africa' ( THIS WILL BE DISCARDED AS THERE IS NO ROUTING KEY FOR 'AFRICA')
            address = new PublicationAddress(ExchangeType.Direct, "company.exchange.routing", "africa");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The latest news from Africa!"));
            Console.WriteLine("Message sent to: Africa");


            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }
    }
}
