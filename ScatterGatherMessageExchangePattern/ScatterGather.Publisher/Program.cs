using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ScatterGather.Publisher
{
    class Program
    {
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();
        private static List<string> responses = new List<string>();
        private static string rpcResponseQueue = channel.QueueDeclare().QueueName;
        private static string correlationId = Guid.NewGuid().ToString();
        private static int minResponses = 2;

        static void Main(string[] args)
        {
            // Declare Exchange & Queues
            channel.ExchangeDeclare("mycompany.exchanges.scattergather", ExchangeType.Fanout, true, false, null);
            channel.QueueDeclare("mycompany.queues.scattergather.a", true, false, false, null);
            channel.QueueDeclare("mycompany.queues.scattergather.b", true, false, false, null);

            // Bind queues to exchange
            channel.QueueBind("mycompany.queues.scattergather.a", "mycompany.exchanges.scattergather", "");
            channel.QueueBind("mycompany.queues.scattergather.b", "mycompany.exchanges.scattergather", "");

            // Publish & Consume messages
            Send();

            // Give it time to process
            Thread.Sleep(60000);

            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine("Messages sent");
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }

        private static void Send()
        {
            IBasicProperties basicProperties = channel.CreateBasicProperties();
            basicProperties.ReplyTo = rpcResponseQueue;
            basicProperties.CorrelationId = correlationId;
            Console.WriteLine("Enter your message and press Enter.");

            string message = Console.ReadLine();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("mycompany.exchanges.scattergather", "", basicProperties, messageBytes);

            // Setup receive handler
            EventingBasicConsumer scatterGatherEventingBasicConsumer = new EventingBasicConsumer(channel);
            scatterGatherEventingBasicConsumer.Received += TwoWayMessageEventReceiver;

            // Consume messages
            channel.BasicConsume(rpcResponseQueue, false, scatterGatherEventingBasicConsumer);
        }

        private static void TwoWayMessageEventReceiver(object sender, BasicDeliverEventArgs e)
        {
            // Set up the IBasicProperties object and specify the temporary queue name to reply to and the correlation ID
            IBasicProperties props = e.BasicProperties;

            // Acknowledge message
            channel.BasicAck(e.DeliveryTag, false);

            // Check message id
            if (props != null && props.CorrelationId == correlationId)
            {
                string response = Encoding.UTF8.GetString(e.Body);
                Console.WriteLine("Response: {0}", response);

                responses.Add(response);

                if (responses.Count >= minResponses)
                {
                    Console.WriteLine($"Responses received from consumers: \n{responses}");
                    channel.Close();
                    connection.Close();
                }
            }
        }
    }
}
