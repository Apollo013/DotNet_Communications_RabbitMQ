using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;
using System.Threading;

namespace ScatterGather.ConsumerB
{
    class Program
    {
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();
        private static string consumerId = "B";

        static void Main(string[] args)
        {
            // Configure how we receive messages
            channel.BasicQos(0, 1, false); // Process only one message at a time

            // Configure handler for message
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += TwoWayMessageEventReceiver;

            Console.WriteLine($"Consumer {consumerId}, up and running, waiting for the publisher to start the bidding process.");

            channel.BasicConsume("mycompany.queues.scattergather.b", false, eventingBasicConsumer);

            // Give it time to process
            Thread.Sleep(60000);

            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }

        private static void TwoWayMessageEventReceiver(object sender, BasicDeliverEventArgs e)
        {
            // Decode Message
            string message = Encoding.UTF8.GetString(e.Body);
            channel.BasicAck(e.DeliveryTag, false);
            Console.WriteLine($"Message: {message}, Enter your response: ");

            // Prompt for reply
            Console.WriteLine("Enter your response: ");
            string response = $"Consumer ID: {consumerId}, bid:{Console.ReadLine()}";

            // Configure reply
            IBasicProperties replyBasicProperties = channel.CreateBasicProperties();
            replyBasicProperties.CorrelationId = e.BasicProperties.CorrelationId;
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            // Send reply
            channel.BasicPublish("", e.BasicProperties.ReplyTo, replyBasicProperties, responseBytes);
        }
    }
}
