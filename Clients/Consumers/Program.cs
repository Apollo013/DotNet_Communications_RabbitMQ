using MessageService.Managers;
using Models.ServiceModels.MessageModels;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumers
{
    /* Related publishers for these messages can be found in the 'Publishers' console project
     * Note: Because we have not explicitly set the virtual host in the config file, '/' will be used.
     *       Also, 'guest' will be the default user.
     */

    class Program
    {
        static void Main(string[] args)
        {
            //DirectConsumer();
            //DirectConsumer_CustomEventHandler();
            DirectConsumer_Worker(); // WORKER QUEUE - RUN MULTIPLE INSTANCES OF THIS PROGRAM TO CHECK
        }

        #region ONE WAY MESSAGE PATTERN EXAMPLES
        private static void DirectConsumer()
        {
            /* Pattern: One Way Message Pattern
             * Related Publisher(s): SendDirect
             * What this will do ...
             * (A) Read all messages from a specified queue
             * (B) Process at most 10 of them.
             * (C) Acknowledge half of them
             * (D) Reject the other half (These will be requeued.
             */

            using (var srvcmngr = new ServiceManager())
            {
                var srvc = srvcmngr.ConsumerService;

                var count = 0;
                srvc.Read("a.new.queue");

                // Here we loop through the consumer service and acknowledge 50% of them (reject 50% and requeue them)
                while (srvc.MoveNext())
                {
                    // Grab the message
                    BasicDeliverEventArgs message = (BasicDeliverEventArgs)srvc.Current;
                    // Acknowledge evens
                    if (count % 2 == 0)
                    {
                        Console.WriteLine($"Accept:\t{Encoding.UTF8.GetString(message.Body)}");
                        srvc.Ack(message);
                    }
                    // Reject Odds
                    else
                    {
                        Console.WriteLine($"Reject:\t{Encoding.UTF8.GetString(message.Body)}");
                        srvc.Nack(message, false, true);
                    }
                    count++;

                    // Only take 10 for this exercise
                    if (count == 10) { break; }
                }
                Console.WriteLine(srvc.IsRunning);
            }
            Console.ReadLine();
        }

        private static void DirectConsumer_CustomEventHandler()
        {
            /* Pattern: One Way Message Pattern
             * Related Publisher(s): SendDirect
             * What this will do ...
             * (A) Read all messages from a specified queue
             * (B) Send them to a custom handler 'CustomReceiveHandler'
             */

            var srvcmngr = new ServiceManager();
            var srvc = srvcmngr.ConsumerService;
            srvc.MessageConsumer.Received += CustomReceiveHandler;
            srvc.Read("a.new.queue");
            Console.ReadLine();

            // Connection will be disposed off when console window closed
        }

        #endregion

        #region WORKER QUEUE MESSAGE PATTERN EXAMPLES
        private static void DirectConsumer_Worker()
        {
            /* Pattern: Worker Queue Message Pattern
             * Related Publisher(s): SendDirect
             * What this will do ...
             * (A) Read a single message from a specified queue
             * (B) Send it to a custom handler 'CustomReceiveHandler'
             */

            var srvcmngr = new ServiceManager();
            var srvc = srvcmngr.ConsumerService;
            srvc.MessageConsumer.Received += CustomReceiveHandler;
            srvc.Read(new ConsumerRequest()
            {
                QueueName = "a.new.queue",
                QualityOfService = new QualityOfService() { PrefetchCount = 1 }
            });
            Console.ReadLine();

            // Connection will be disposed off when console window closed
        }

        #endregion

        /// <summary>
        /// Custom 'Receive' handler, see <see cref="DirectConsumer_CustomEventHandler"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void CustomReceiveHandler(object sender, BasicDeliverEventArgs args)
        {
            Console.WriteLine($"Custom Handler:\t\t{Encoding.UTF8.GetString(args.Body)}");
        }
    }
}
