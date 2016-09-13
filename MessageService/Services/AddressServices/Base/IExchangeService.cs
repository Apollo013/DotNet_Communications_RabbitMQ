using Models.ServiceModels.AddressModels;
using Models.ServiceModels.BindingModels;
using Models.ServiceModels.DeleteModels;
using System.Collections.Generic;

namespace MessageService.Services.AddressServices.Base
{
    public interface IExchangeService : IAddressService<ExchangeAddressModel, ExchangeBindingModel, ExchangeDeleteModel>
    {
        IQueueService QueueService { get; }
        void Declare(string name, string type, bool durable = true, bool autoDelete = false, IDictionary<string, object> args = null, List<QueueAddressModel> queues = null);
        void Declare(string name, string queue, string type, bool durable = true, bool autoDelete = false, IDictionary<string, object> args = null);
        void Delete(string name, bool ifUnused = true);
    }
}
