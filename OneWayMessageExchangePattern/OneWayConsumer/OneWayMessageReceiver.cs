using RabbitMQ.Client;
using System;
using System.Text;

namespace OneWayConsumer
{
    public class OneWayMessageReceiver : DefaultBasicConsumer
    {
        private IModel _channel { get; set; }

        public OneWayMessageReceiver(IModel channel)
        {
            _channel = channel;
        }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            Console.WriteLine("Message received by the consumer. Check the debug window for details.");
            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Console.WriteLine(string.Concat("Content type: ", properties.ContentType));
            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));
            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body)));
            _channel.BasicAck(deliveryTag, false);
        }
    }
}
