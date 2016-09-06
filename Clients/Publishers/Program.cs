using MessageService.Managers;
using Models.ServiceModels.MessageModels;

namespace Publishers
{
    /* Related consumers for these messages can be found in the 'Consumers' console project
     * Note: Because we have not explicitly set the virtual host in the config file, '/' will be used.
     *       Also, 'guest' will be the default user.
     */

    class Program
    {
        static void Main(string[] args)
        {
            SendDirect();
        }

        private static void SendDirect()
        {
            /* Pattern: One Way Message Pattern
             * Related Consumer(s): DirectConsumer
             * What this will do ...
             * (A) Create an Exchange called "a.new.exchange". The default type for this is 'direct'
             * (B) Create a Queue called "a.new.queue".
             * (C) Bind the queue to the exchange
             * (D) Because the default for the message 'SendType' is 'direct' and a 'RoutingKey' is
             *     provided, the message will be sent directly to "a.new.queue".        
             */
            using (var mngr = new ServiceManager())
            {
                var str = "Just testing";
                var message = new MessageModel()
                {
                    Body = str,
                    ExchangeName = "a.new.exchange",
                    RoutingKey = "a.new.queue"
                };

                // Send the message 50 times;
                for (int i = 0; i < 50; i++)
                {
                    message.Body = $"{i} - {str}";
                    mngr.PublisherService.Publish(message);
                }
            }
        }
    }
}
