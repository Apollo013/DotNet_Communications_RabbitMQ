using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;

namespace MessageService.Services.TransportServices.Consumers.Base
{
    public interface IEnumerableConsumerService : IEnumerable, IEnumerator, IDisposable
    {
        IModel Channel { get; }
        EventingBasicConsumer MessageConsumer { get; }
        BasicDeliverEventArgs LatestEvent { get; }
        string QueueName { get; }
        string ConsumerTag { get; }
        bool NoAck { get; }
        bool IsRunning { get; }
        void Read(string queueName, bool noAck, string consumerTag);
        bool Ack();
        bool Ack(BasicDeliverEventArgs args);
        bool Ack(ulong deliveryTag);
        void Nack(bool requeue);
        void Nack(bool multiple, bool requeue);
        void Nack(BasicDeliverEventArgs args, bool multiple, bool requeue);
        BasicDeliverEventArgs Next();
        bool Next(int millisecondsTimeout, out BasicDeliverEventArgs result);
        void Close();
    }
}
