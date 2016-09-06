using Models.ServiceModels.AddressModels;
using Models.ServiceModels.BindingModels;
using Models.ServiceModels.DeleteModels;
using System.Collections.Generic;

namespace MessageService.Services.AddressServices.Base
{
    public interface IAddressService<TAddress, TBindingAddress, TDeleteAddress>
        where TAddress : AddressBase
        where TBindingAddress : BindingBase
        where TDeleteAddress : DeleteBase
    {
        void Declare(TAddress address);
        void DeclareMany(IEnumerable<TAddress> addresses);
        void Bind(TBindingAddress binding);
        void BindMany(IEnumerable<TBindingAddress> bindings);
        void Delete(TDeleteAddress address);
        void DeleteMany(IEnumerable<TDeleteAddress> addresses);
        void Unbind(TBindingAddress binding);
    }
}
