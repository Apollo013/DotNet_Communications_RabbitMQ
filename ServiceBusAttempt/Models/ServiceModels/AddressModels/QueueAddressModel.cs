using System.Linq;

namespace Models.ServiceModels.AddressModels
{
    /// <summary>
    /// Contains the properties needed to declare a queue container
    /// </summary>
    public class QueueAddressModel : AddressBase
    {
        #region Properties
        /// <summary>
        /// Gets or sets the flag to indicate whether this queue is exclusive to this connection
        /// </summary>
        public bool Exclusive { get; set; } = false;

        /// <summary>
        /// Gets or sets the routing keys used for binding
        /// </summary>
        public string[] RoutingKeys { get; set; }

        /// <summary>
        /// Gets or sets the name of the exchange that the queue needs to be bound to
        /// </summary>
        public string ExchangeName { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Determines if this queue has any related routing keys
        /// </summary>
        /// <returns></returns>
        public bool HasRoutingKeys()
        {
            if (RoutingKeys == null)
            {
                return false;
            }
            return RoutingKeys.Any();
        }
        #endregion

        #region 'IValidationModel' Implementation
        public override bool TryValidate(out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(Name))
            {
                error = "Please supply the name of the queue to create";
                return false;
            }

            return true;
        }
        #endregion
    }
}
