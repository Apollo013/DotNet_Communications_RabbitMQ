using Models.Common.Interfaces;
using System.Collections.Generic;

namespace Models.ServiceModels.AddressModels
{
    public abstract class AddressBase : IValidationModel
    {
        #region Properties
        public virtual string Name { get; set; }

        /// <summary>
        /// Determines whether this address is permanent or temporary
        /// </summary>
        public bool Durable { get; set; } = true;

        /// <summary>
        /// Gets or sets the flag to indicate whether or not to delete the address after the last consumer has disconnected from it
        /// </summary>
        public bool AutoDelete { get; set; } = false;

        /// <summary>
        ///  Gets or sets address arguments
        /// </summary>
        public IDictionary<string, object> Arguments { get; set; }
        #endregion

        #region 'IValidationModel' Implementation
        public abstract bool TryValidate(out string error);
        #endregion
    }
}
