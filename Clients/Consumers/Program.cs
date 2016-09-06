using MessageService.Managers;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumers
{
    /* Related publishers for these messages can be found in the 'Publishers' console project
     */

    class Program
    {
        static void Main(string[] args)
        {
            DirectConsumer();
        }

        public static void DirectConsumer()
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
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Accept: {Encoding.UTF8.GetString(message.Body)} - {message.ConsumerTag}");
                        Console.ForegroundColor = ConsoleColor.White;
                        srvc.Ack(message);
                    }
                    // Reject Odds
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Reject: {Encoding.UTF8.GetString(message.Body)} - {message.ConsumerTag}");
                        Console.ForegroundColor = ConsoleColor.White;
                        srvc.Nack(message, false, true);
                    }
                    count++;

                    // Only take 10 for this exercise
                    if (count == 10) { break; }
                }
                Console.WriteLine(srvc.IsRunning());
            }
            Console.ReadLine();
        }
    }
}
