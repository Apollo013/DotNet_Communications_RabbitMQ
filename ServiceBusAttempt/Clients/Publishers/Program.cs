using MessageService.Managers;
using Models.ServiceModels.AddressModels;
using Models.ServiceModels.MessageModels;
using System.Collections.Generic;

namespace Publishers
{
    /* Related consumers for these messages can be found in the 'Consumers' console project
     * Note: Because we have not explicitly set the virtual host in the config file, '/' will be used.
     *       Also, 'guest' will be the default user.
     */

    class Program
    {
        private static ServiceManager srvcmngr = new ServiceManager();

        static void Main(string[] args)
        {
            // DirectSetup();
            FanoutSetup_Example1();
            FanoutSetup_Example2();
        }

        #region WORKER QUEUE & ONE WAY MESSAGE PATTERNS - DIRECT EXCHANGE TYPE
        /// <summary>
        /// Creates a new direct exchange with a queue and publishes message direct to the queue
        /// </summary>
        private static void DirectSetup()
        {
            /* Pattern: One Way Message Pattern
             * Related Consumer(s): DirectConsumer 
             *                      DirectConsumer_CustomEventHandler
             * What this will do ...
             * (A) Create an Exchange called "a.new.exchange". The default type for this is 'direct'
             * (B) Create a Queue called "a.new.queue".
             * (C) Bind the queue to the exchange
             * (D) Because the default for the message 'SendType' is 'direct' and a 'RoutingKey' is
             *     provided, the message will be sent directly to "a.new.queue".        
             */

            var str = "Just testing";
            var message = new MessageModel()
            {
                Body = str,
                ExchangeName = "a.new.exchange",
                RoutingKey = "a.new.queue"
            };

            // Send 50 message;
            for (int i = 0; i < 50; i++)
            {
                message.Body = $"{i} - {str}";
                srvcmngr.PublisherService.Publish(message);
            }

            System.Console.WriteLine("Messages sent");
        }
        #endregion

        #region PUBLISH / SUBSCRIBE MESSAGE PATTER - 'FANOUT' EXCHANGE TYPE
        /// <summary>
        /// Simply sends 2 messages. During this process, the 'PublishService' component will ensure both that 
        /// the exchange and queues are created and bound (using internal 'ExchangeService' component), prior to sending.
        /// </summary>
        private static void FanoutSetup_Example1()
        {
            /* Pattern: Publish/Subscribe Message Pattern
             * Related Consumer(s): FanOutRecevier
             * What this will do ...
             * (A) Send a message to all queues on the 'a.fanout.exchange', The type for this exchange will be 'fanout'.
             *     (This has not been created yet, but Publish service which uses the exchange service component, will create it, the queue, and bind them.)
             * (B) The result of this is; "a.fanout.queue_1" containing a single message        
             * (C) Process '(A)' is repeated but with a different queue: "a.fanout.queue_2".
             * (D) The result of this is: "a.fanout.queue_1" containing 2 messages and "a.fanout.queue_2" containing a single message.
             */
            var str = "Just testing a fanout exchange";
            var message = new MessageModel()
            {
                Body = str + " a.fanout.queue_1",
                ExchangeName = "a.fanout.exchange",
                RoutingKey = "a.fanout.queue_1",
                SendType = RabbitMQ.Client.ExchangeType.Fanout
            };

            srvcmngr.PublisherService.Publish(message);

            // Send a second message
            message = new MessageModel()
            {
                Body = str + " a.fanout.queue_2",
                ExchangeName = "a.fanout.exchange",
                RoutingKey = "a.fanout.queue_2",
                SendType = RabbitMQ.Client.ExchangeType.Fanout
            };

            srvcmngr.PublisherService.Publish(message);
            System.Console.WriteLine("Messages sent");
        }

        /// <summary>
        /// Creates and binds the exchange and queues using the 'ExchangeService' Component, 
        /// then sends the message using the 'PublishService' Component
        /// </summary>
        private static void FanoutSetup_Example2()
        {
            /* Pattern: Publish/Subscribe Message Pattern
             * Related Consumer(s): FanOutRecevier
             * What this will do ...
             * (A) Create an exchange with 2 queues.
             * (B) The type for this exchange will be 'fanout'.
             * (C) Send a single message.
             * (D) The result of this is: Both queues containing 2 a single message .
             */

            // Create the exchange using the exchange service component
            ExchangeAddressModel exchange = new ExchangeAddressModel()
            {
                Name = "b.fanout.exchange",
                ExchangeType = RabbitMQ.Client.ExchangeType.Fanout,
                Queues = new List<QueueAddressModel>()
                {
                    new QueueAddressModel() { Name = "b.fanout.queue_1" },
                    new QueueAddressModel() { Name = "b.fanout.queue_2" }
                }
            };
            srvcmngr.ExchangeService.Declare(exchange);

            // Sends the message using the publish service component
            var str = "Just testing a fanout exchange";
            var message = new MessageModel()
            {
                Body = str,
                ExchangeName = "b.fanout.exchange",
                SendType = RabbitMQ.Client.ExchangeType.Fanout
            };

            srvcmngr.PublisherService.Publish(message);
            System.Console.WriteLine("Messages sent");
        }
        #endregion

    }
}