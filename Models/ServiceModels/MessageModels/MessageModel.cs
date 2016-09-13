using Models.ServiceModels.MessageModels.Base;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Models.ServiceModels.MessageModels
{
    public class MessageModel : PublishMessageBase
    {
        public override byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(Body);
        }

        public override PublicationAddress GetPublicationAddress()
        {
            switch (SendType)
            {
                case ExchangeType.Fanout:
                    return new PublicationAddress(SendType, ExchangeName, "");

                default: // Direct
                    return String.IsNullOrWhiteSpace(RoutingKey) ?
                        new PublicationAddress(SendType, ExchangeName, "") :
                        new PublicationAddress(SendType, "", RoutingKey);
            }
        }

        public override bool TryValidate(out string error)
        {
            error = "";
            if (String.IsNullOrEmpty(ExchangeName) && String.IsNullOrEmpty(RoutingKey))
            {
                error = "Please supply either an exchange name or routing key to send the message to";
                return false;
            }


            if (SendType.Equals(ExchangeType.Direct) && String.IsNullOrEmpty(RoutingKey))
            {
                error = "Please supply a queue name to send the message directly to";
                return false;
            }

            if (SendType.Equals(ExchangeType.Fanout) && String.IsNullOrEmpty(ExchangeName))
            {
                error = "Please supply an exchange name to send the message to";
                return false;
            }

            return true;
        }
    }
}
