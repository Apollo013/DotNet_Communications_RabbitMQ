using Models.ServiceModels.AddressModels;
using Models.ServiceModels.BindingModels;
using Models.ServiceModels.DeleteModels;
using System.Collections.Generic;

namespace MessageService.Services.AddressServices.Base
{
    public interface IQueueService : IAddressService<QueueAddressModel, QueueBindingModel, QueueDeleteModel>
    {
        void Declare(string exchange, string queue, bool durable = true, bool exclusive = false, bool autoDelete = false, Dictionary<string, object> args = null, string[] routingKeys = null);
        void Bind(string exchange, string queue, string routingKey = "");
        void Purge(string queue);
        void Delete(string name, bool ifUnused = true, bool ifEmpty = true);
    }
}
