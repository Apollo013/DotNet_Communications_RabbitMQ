using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommon.ConnectionServices;
using System;
using System.Text;
using System.Threading;

namespace PublishSubscribeSubscriber
{
    /// <summary>
    /// Demonstracte Publish Subscribe Message Exchange Pattern with 'Fanout' Type
    /// </summary>
    class Program
    {
        private static IConnection connection = ConnectionService.CreateConnection();
        private static IModel channel = connection.CreateModel();

        static void Main(string[] args)
        {
            ReceiveMessagesWithEvents();

            // Give it time to consume messages
            Thread.Sleep(2000);

            // Close Connection & Channel
            channel.Close();
            connection.Close();

            // Finish
            Console.WriteLine(string.Concat("Channel is closed: ", channel.IsClosed));
            Console.ReadKey();
        }

        private static void ReceiveMessagesWithEvents()
        {
            // Configure how we receive messages
            channel.BasicQos(0, 1, false); // Process only one message at a time

            // Configure handler for message
            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += OneWayMessageEventReceiver;

            // Consume message from queue
            channel.BasicConsume("mycompany.queues.management", false, eventingBasicConsumer);
        }

        /// <summary>
        /// Event Based Message Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OneWayMessageEventReceiver(object sender, BasicDeliverEventArgs e)
        {
            IBasicProperties basicProperties = e.BasicProperties;
            Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
            Console.WriteLine(string.Concat("Message received from the exchange ", e.Exchange));
            Console.WriteLine(string.Concat("Content type: ", basicProperties.ContentType));
            Console.WriteLine(string.Concat("Consumer tag: ", e.ConsumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", e.DeliveryTag));
            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(e.Body)));
            channel.BasicAck(e.DeliveryTag, false);
        }

    }
}
