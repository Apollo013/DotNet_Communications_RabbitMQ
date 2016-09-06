using MessageService.Services.TransportServices.Consumers.Base;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace MessageService.Services.TransportServices.Consumers
{
    /// <summary>
    /// Enumerable class responsible for consuming messages
    /// </summary>
    public class EnumerableConsumerService : IEnumerableConsumerService
    {
        protected readonly object messageEventLock = new object();
        private CancellationTokenSource messageQueueCts = new CancellationTokenSource();
        protected volatile EventingBasicConsumer messageConsumer;
        private BlockingCollection<BasicDeliverEventArgs> messageQueue = new BlockingCollection<BasicDeliverEventArgs>(new ConcurrentQueue<BasicDeliverEventArgs>());

        #region PROPERTIES
        public IModel Channel { get; protected set; }
        public IBasicConsumer Consumer { get { return messageConsumer; } }
        public BasicDeliverEventArgs LatestDelivery { get; protected set; }
        public string QueueName { get; protected set; }
        public string ConsumerTag { get; protected set; }
        public bool NoAck { get; protected set; }
        #endregion

        #region CONSTRUCTORS
        public EnumerableConsumerService(IModel channel) : this(channel, "", true)
        { }

        public EnumerableConsumerService(IModel channel, string queueName) : this(channel, queueName, false)
        { }

        public EnumerableConsumerService(IModel channel, string queueName, bool noAck, string consumerTag = null)
        {
            Channel = channel;
            QueueName = queueName;
            NoAck = noAck;
            Read(queueName, noAck, consumerTag);
        }
        #endregion

        #region READ
        public void Read(string queueName, bool noAck = false, string consumerTag = null)
        {
            if (String.IsNullOrWhiteSpace(queueName))
            {
                return;
            }

            QueueName = queueName;
            NoAck = noAck;
            messageConsumer = new EventingBasicConsumer(Channel);
            messageConsumer.Received += (sender, args) =>
            {
                Console.ForegroundColor = ConsoleColor.White;
                messageQueue.Add(args);
                Console.WriteLine($"Message Delivered: {messageQueue.Count} - {Encoding.UTF8.GetString(args.Body)}");
            };
            messageConsumer.ConsumerCancelled += ConsumerCancelledEventHandler;
            // messageConsumer.
            ConsumerTag = (consumerTag == null) ? Channel.BasicConsume(QueueName, NoAck, messageConsumer) : Channel.BasicConsume(QueueName, NoAck, consumerTag, messageConsumer);
            LatestDelivery = null;
            //return this;
        }

        public bool IsRunning()
        {
            return messageConsumer.IsRunning;
        }
        #endregion

        #region ACKNOWLEDGEMENTS - 'Ack'
        /// <summary>
        /// Acknowledges the latest message delivered <see cref="LatestDelivery"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If we are not in "noAck" mode (<see cref="NoAck"/> == false), calls IModel.BasicAck with the delivery-tag from <see cref="LatestDelivery"/>;
        /// Otherwise we send nothing to the server.
        /// </para>
        /// <para>
        /// Passing an event that did not originate with this Subscription's channel, will lead to unpredictable behaviour
        /// </para>
        /// </remarks>
        /// <returns></returns>
        public bool Ack()
        {
            return Ack(LatestDelivery);
        }

        /// <summary>
        /// Acknowledges a delivered message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If we are not in "noAck" mode (<see cref="NoAck"/> == false), calls IModel.BasicAck with the delivery-tag from <paramref name="args"/>;
        /// Otherwise we send nothing to the server.
        /// </para>
        /// <para>
        /// Passing an event that did not originate with this Subscription's channel, will lead to unpredictable behaviour
        /// </para>
        /// </remarks>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Ack(BasicDeliverEventArgs args)
        {
            if (args == null) { return false; }

            if (args == LatestDelivery)
            {
                UpdateLatestDelivery(null);
            }

            return Ack(args.DeliveryTag);
        }

        /// <summary>
        /// Acknowledges a delivered message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If we are not in "noAck" mode (<see cref="NoAck"/> == false), calls IModel.BasicAck with the delivery-tag from <paramref name="args"/>;
        /// Otherwise we send nothing to the server.
        /// </para>
        /// <para>
        /// Passing an event that did not originate with this Subscription's channel, will lead to unpredictable behaviour
        /// </para>
        /// </remarks>
        /// <param name="deliveryTag"></param>
        /// <returns></returns>
        public bool Ack(ulong deliveryTag)
        {
            if (!NoAck && Channel.IsOpen)
            {
                Channel.BasicAck(deliveryTag, false); // Acknowledge this message only
            }

            return true;
        }
        #endregion

        #region REJECTIONS - 'Negative Ack'
        /// <summary>
        /// Rejects the latest message delivery <see cref="LatestDelivery"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// If we are not in "noAck" mode (<see cref="NoAck"/> == false), calls IModel.BasicAck with the delivery-tag from <see cref="LatestDelivery"/>;
        /// Otherwise we send nothing to the server.
        /// </para>
        /// <para>
        /// Passing an event that did not originate with this Subscription's channel, will lead to unpredictable behaviour
        /// </para>
        /// </remarks>
        /// <param name="requeue">If True, instructs server to requeue the message</param>
        public void Nack(bool requeue)
        {
            Nack(LatestDelivery, false, requeue);
        }

        /// <summary>
        /// Rejects the latest message delivery <see cref="LatestDelivery"/>
        /// </summary>
        /// <remarks>
        /// <para>
        /// If we are not in "noAck" mode (<see cref="NoAck"/> == false), calls IModel.BasicAck with the delivery-tag from <see cref="LatestDelivery"/>;
        /// Otherwise we send nothing to the server.
        /// </para>
        /// <para>
        /// Passing an event that did not originate with this Subscription's channel, will lead to unpredictable behaviour
        /// </para>
        /// </remarks>
        /// <param name="multiple">If True, instructs the server to reject all prior messages</param>
        /// <param name="requeue">If True, instructs server to requeue the message</param>
        public void Nack(bool multiple, bool requeue)
        {
            Nack(LatestDelivery, multiple, requeue);
        }

        /// <summary>
        /// Rejects a delivered message
        /// </summary>
        /// <remarks>
        /// <para>
        /// If we are not in "noAck" mode (<see cref="NoAck"/> == false), calls IModel.BasicAck with the delivery-tag from <paramref name="args"/>;
        /// Otherwise we send nothing to the server.
        /// </para>
        /// <para>
        /// Passing an event that did not originate with this Subscription's channel, will lead to unpredictable behaviour
        /// </para>
        /// </remarks>
        /// <param name="args">The message to reject</param>
        /// <param name="multiple">If True, instructs the server to reject all prior messages</param>
        /// <param name="requeue">If True, instructs server to requeue the message</param>
        public void Nack(BasicDeliverEventArgs args, bool multiple, bool requeue)
        {
            if (args == null) { return; }

            if (!NoAck && Channel.IsOpen)
            {
                Channel.BasicNack(args.DeliveryTag, multiple, requeue);
            }

            if (args == LatestDelivery)
            {
                UpdateLatestDelivery(null);
            }
        }
        #endregion

        #region NEXT
        /// <summary>
        /// Retrieves the next incoming delivery in our subscription queue
        /// </summary>
        /// <remarks>
        /// <para>
        /// Returns null when the end of the stream is reached and on every subsequent call. End-of-stream can arise through the
        /// action of the Subscription.Close() method, or through the closure of the IModel or its underlying IConnection.
        /// </para>
        /// <para>
        /// Updates LatestEvent to the value returned.
        /// </para>
        /// <para>
        /// Does not acknowledge any deliveries at all (but in "noAck" mode, the server will have auto-acknowledged each event
        /// before it is even sent across the wire to us).
        /// </para>
        /// </remarks>
        /// <returns></returns>
        public BasicDeliverEventArgs Next()
        {
            BasicDeliverEventArgs args = null;
            EventingBasicConsumer consumer = messageConsumer;
            try
            {
                if (consumer == null || Channel.IsClosed)
                {
                    UpdateLatestDelivery(null);
                }
                else
                {
                    args = messageQueue.Take(messageQueueCts.Token);
                    UpdateLatestDelivery(args);
                }
            }
            catch (EndOfStreamException)
            {
                UpdateLatestDelivery(null);
            }
            return LatestDelivery;
        }

        /// <summary>
        /// Retrieves the next incoming delivery in our subscription queue, or times out after a specified number of milliseconds.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Returns false only if the timeout expires before either a delivery appears or the end-of-stream is reached. If false
        /// is returned, the out parameter "result" is set to null, but LatestEvent is not updated.
        /// </para>
        /// <para>
        /// Returns true to indicate a delivery or the end-of-stream.
        /// </para>
        /// <para>
        /// If a delivery is already waiting in the queue, or one arrives before the timeout expires, it is removed from the
        /// queue and placed in the "result" out parameter. If the end-of-stream is detected before the timeout expires,
        /// "result" is set to null.
        /// </para>
        /// <para>
        /// Whenever this method returns true, it updates LatestEvent to the value placed in "result" before returning.
        /// </para>
        /// <para>
        /// End-of-stream can arise through the action of the Subscription.Close() method, or through the closure of the
        /// IModel or its underlying IConnection.
        ///</para>
        ///<para>
        /// This method does not acknowledge any deliveries at all (but in "noAck" mode, the server will have
        /// auto-acknowledged each event before it is even sent across the wire to us).
        /// </para>
        /// <para>
        /// A timeout of -1 (i.e. System.Threading.Timeout.Infinite) will be interpreted as a command to wait for an
        /// indefinitely long period of time for an item or the end of the stream to become available. 
        /// Usage of such a timeout is equivalent to calling Next() with no arguments (modulo predictable method signature differences).
        /// </para>
        /// </remarks>
        /// <param name="millisecondsTimeout">Time-out value</param>
        /// <param name="result">The next delivered message</param>
        /// <returns></returns>
        public bool Next(int millisecondsTimeout, out BasicDeliverEventArgs result)
        {
            try
            {
                var consumer = messageConsumer;
                if (consumer == null || Channel.IsClosed)
                {
                    UpdateLatestDelivery(null);
                    result = null;
                    return false;
                }
                else
                {
                    BasicDeliverEventArgs qValue;
                    if (!messageQueue.TryTake(out qValue, millisecondsTimeout))
                    {
                        result = null;
                        return false;
                    }
                    UpdateLatestDelivery(qValue);
                }
            }
            catch (EndOfStreamException)
            {
                UpdateLatestDelivery(null);
            }
            result = LatestDelivery;
            return true;
        }
        #endregion

        #region MISC METHODS
        private void ConsumerCancelledEventHandler(object sender, ConsumerEventArgs e)
        {
            lock (messageEventLock)
            {
                messageConsumer = null;
                UpdateLatestDelivery(null);
            }
        }

        protected void UpdateLatestDelivery(BasicDeliverEventArgs args)
        {
            lock (messageEventLock)
            {
                LatestDelivery = args;
            }
        }
        #endregion

        #region IDISPOSABLE IMPLEMENTATION
        /// <summary>
        /// Disposes of managed objects. Allows subscriber to be used with 'using' keyword or just simply called as is.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Disposes of any managed objects
        /// </summary>
        public void Close()
        {
            try
            {
                var cancelConsumer = false;

                if (messageConsumer != null)
                {
                    cancelConsumer = messageConsumer.IsRunning;
                    messageConsumer = null;
                }

                if (cancelConsumer)
                {
                    if (Channel.IsOpen)
                    {
                        Channel.BasicCancel(ConsumerTag);
                    }

                    ConsumerTag = null;
                }

                messageQueueCts.Cancel(true);

                if (messageQueue != null)
                {
                    messageQueue.Dispose();
                    messageQueue = null;
                }
            }
            catch (OperationInterruptedException) { }
        }
        #endregion

        #region IENUMERATOR IMPLEMENTATIONS
        /// <summary>
        /// Fetches the latest message
        /// </summary>
        /// <remarks>
        /// <para>
        /// As per the IEnumerator interface definition, throws InvalidOperationException if LatestEvent is null.
        /// </para>
        /// <para>
        /// Does not acknowledge any deliveries at all. Ack() must be called explicitly on received deliveries.
        /// </para>
        /// </remarks>
        public object Current
        {
            get
            {
                if (LatestDelivery == null)
                {
                    throw new InvalidOperationException();
                }
                return LatestDelivery;
            }
        }

        public BasicDeliverEventArgs LatestEvent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            return Next() != null;
        }

        public void Reset()
        {
            // It really doesn't make sense to try to reset a subscription.
            throw new InvalidOperationException("Subscription.Reset() does not make sense");
        }
        #endregion
    }
}
