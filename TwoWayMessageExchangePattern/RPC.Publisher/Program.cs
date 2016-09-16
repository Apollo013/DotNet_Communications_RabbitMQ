using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;
using System.Threading;

namespace RPC.Publisher
{
    class Program
    {
        // Create a Connection and Channel for transporting message
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();
        private static string rpcResponseQueue = channel.QueueDeclare().QueueName;
        private static string correlationId = Guid.NewGuid().ToString();
        private static string responseFromConsumer = null;
        private static IBasicProperties basicProperties = channel.CreateBasicProperties();
        private static string message;
        private static byte[] messageBytes;

        static void Main(string[] args)
        {

            Console.WriteLine(string.Concat("Connection open: ", connection.IsOpen));

            // Declare a Queue (response queue will be dynamically set up !)
            channel.QueueDeclare("mycompany.queues.rpc", true, false, false, null);

            // Publisg & Consume messages
            SendRpcMessagesBackAndForth(channel);

            // Give it time to process
            Thread.Sleep(60000);

            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }

        private static void SendRpcMessagesBackAndForth(IModel channel)
        {
            // Configure message
            basicProperties.ReplyTo = rpcResponseQueue;
            basicProperties.CorrelationId = correlationId; // message correlation ID so that we can match the sender’s message to the response from the receiver.

            // Prompt for message to publish
            Console.WriteLine("Enter your message and press Enter.");
            message = Console.ReadLine();
            messageBytes = Encoding.UTF8.GetBytes(message);

            // Send It
            channel.BasicPublish("", "mycompany.queues.rpc", basicProperties, messageBytes);

            // Configure return handler
            EventingBasicConsumer rpcEventingBasicConsumer = new EventingBasicConsumer(channel);
            rpcEventingBasicConsumer.Received += TwoWayMessageEventReceiver;

            // Listen for reply
            channel.BasicConsume(rpcResponseQueue, false, rpcEventingBasicConsumer);
        }

        private static void TwoWayMessageEventReceiver(object sender, BasicDeliverEventArgs e)
        {
            // Set up the IBasicProperties object and specify the temporary queue name to reply to and the correlation ID
            IBasicProperties props = e.BasicProperties;
            if (props != null && props.CorrelationId == correlationId)
            {
                string response = Encoding.UTF8.GetString(e.Body);
                responseFromConsumer = response;
            }

            // Acknowledge message
            channel.BasicAck(e.DeliveryTag, false);
            Console.WriteLine($"Response: {responseFromConsumer}");

            // Prompt for reply
            Console.WriteLine("Enter your message and press Enter.");
            message = Console.ReadLine();
            messageBytes = Encoding.UTF8.GetBytes(message);

            // Send Reply            
            channel.BasicPublish("", "mycompany.queues.rpc", props, messageBytes); //empty string parameter denotes the nameless default AMQP exchange
        }
    }
}
