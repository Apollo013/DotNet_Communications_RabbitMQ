using Models.ServiceModels.MessageModels;
using RabbitMQ.Client.Events;

namespace MessageService.Services.TransportServices.Publishers.Base
{
    public interface IMessagePublisher : IPublisherService<MessageModel>
    {
        void BasicReturn(object sender, BasicReturnEventArgs e);
    }
}
