using Models.ServiceModels.MessageModels.Base;

namespace MessageService.Services.TransportServices.Publishers.Base
{
    public interface IPublisherService<TMessage> where TMessage : class
    {
        bool Publish(TMessage message);
        bool Publish(string body, string exchangeName, string routingKey = "", byte priority = 0, bool persistent = true, string contentType = PublishMessageBase.CONTENTTYPE_PLAINTEXT);
    }
}
