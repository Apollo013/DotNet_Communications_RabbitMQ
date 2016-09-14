using RabbitMQ.Client;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Headers.Publisher
{
    class Program
    {
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();

        static void Main(string[] args)
        {
            // Exchange / Queue Declaration & Binding
            channel.ExchangeDeclare("company.exchange.headers", ExchangeType.Headers, true, false, null);
            channel.QueueDeclare("company.queue.headers", true, false, false, null);

            // Define Headers For Queue
            Dictionary<string, object> headerOptionsWithAll = new Dictionary<string, object>();
            headerOptionsWithAll.Add("x-match", "all");
            headerOptionsWithAll.Add("category", "animal");
            headerOptionsWithAll.Add("type", "mammal");

            // Bind
            channel.QueueBind("company.queue.headers", "company.exchange.headers", "", headerOptionsWithAll);

            // Define another set of Headers For Queue
            Dictionary<string, object> headerOptionsWithAny = new Dictionary<string, object>();
            headerOptionsWithAny.Add("x-match", "any");
            headerOptionsWithAny.Add("category", "plant");
            headerOptionsWithAny.Add("type", "tree");

            // Bind
            channel.QueueBind("company.queue.headers", "company.exchange.headers", "", headerOptionsWithAny);


            // Create address to deliver message to
            IBasicProperties properties = channel.CreateBasicProperties();
            PublicationAddress address = new PublicationAddress(ExchangeType.Headers, "company.exchange.headers", "");


            // These will NOT get delivered
            Dictionary<string, object> messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            messageHeaders.Add("type", "insect");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of insects YOLO !!!"));


            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of animals YOLO !!!"));


            // These will get delivered
            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            messageHeaders.Add("type", "mammal");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of mammals"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "animal");
            messageHeaders.Add("type", "mammal");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of mammals Again !!!"));

            properties = channel.CreateBasicProperties();
            messageHeaders = new Dictionary<string, object>();
            messageHeaders.Add("category", "plant");
            messageHeaders.Add("type", "tree");
            properties.Headers = messageHeaders;
            channel.BasicPublish(address, properties, Encoding.UTF8.GetBytes("Hello from the world of trees YOLO !!!"));


            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine("Messages sent");
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();

        }
    }
}
