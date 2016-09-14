using RabbitMQ.Client;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;

namespace Topics.Publisher
{
    class Program
    {
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();

        static void Main(string[] args)
        {
            // Exchange / Queue Declaration & Binding
            channel.ExchangeDeclare("company.exchange.topic", ExchangeType.Topic, true, false, null);
            channel.QueueDeclare("company.queue.topic", true, false, false, null);

            // A queue can be dedicated to one or more routing keys.
            channel.QueueBind("company.queue.topic", "company.exchange.topic", "*.world");
            channel.QueueBind("company.queue.topic", "company.exchange.topic", "world.#");

            // Basic message properties
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Keep even after server restart
            properties.ContentType = "text/plain";

            // Publish messages (THESE MATCH THE ABOVE PATTERNS)            
            PublicationAddress address = new PublicationAddress(ExchangeType.Topic, "company.exchange.topic", "beautiful.world");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("The world is beautiful"));

            address = new PublicationAddress(ExchangeType.Topic, "company.exchange.topic", "world.news.and.more");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("It's Friday night"));

            // Publish messages (THESE DO NOT MATCH THE ABOVE PATTERNS !!!)
            address = new PublicationAddress(ExchangeType.Topic, "company.exchange.topic", "news of the world");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("This is some random news from the world"));

            address = new PublicationAddress(ExchangeType.Topic, "company.exchange.topic", "news.of.the.world");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("trololo"));

            address = new PublicationAddress(ExchangeType.Topic, "company.exchange.topic", "the world is crumbling");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("whatever"));

            address = new PublicationAddress(ExchangeType.Topic, "company.exchange.topic", "the.world.is.crumbling");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello"));


            address = new PublicationAddress(ExchangeType.Topic, "company.exchange.topic", "world news and more");
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("No more tears"));


            Console.WriteLine("Messages sent");

            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }
    }
}
