using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;
using System.Threading;

namespace RPC.Consumer
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
            eventingBasicConsumer.Received += TwoWayMessageEventReceiver;

            // Consume message from queue
            channel.BasicConsume("mycompany.queues.rpc", false, eventingBasicConsumer);

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
            channel.BasicAck(e.DeliveryTag, false); // acknowledge the message

            // Output message
            Console.WriteLine($"Message: {message}");

            // Prompt for reply
            Console.WriteLine("Enter your response: ");
            string response = Console.ReadLine();

            // Configure reply
            IBasicProperties replyBasicProperties = channel.CreateBasicProperties();
            replyBasicProperties.CorrelationId = e.BasicProperties.CorrelationId; // same correlation ID to its message as the one received in the initial message from the publisher
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            // Send reply
            channel.BasicPublish("", e.BasicProperties.ReplyTo, replyBasicProperties, responseBytes); // ReplyTo property of the delivery arguments to publish a response to original publisher
        }
    }
}
