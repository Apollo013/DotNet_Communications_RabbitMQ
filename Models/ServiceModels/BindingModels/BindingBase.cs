using Models.Common.Interfaces;
using System.Collections.Generic;

namespace Models.ServiceModels.BindingModels
{
    public abstract class BindingBase : IValidationModel
    {
        #region Properties
        /// <summary>
        /// The address in which the source will be bound to
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// The child address that needs to be bound
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// gets or sets the routing key
        /// </summary>
        private string _routingKey;
        public string RoutingKey
        {
            get { return _routingKey ?? ""; }
            set { _routingKey = value; }
        }

        public Dictionary<string, object> Arguments { get; set; }
        #endregion

        #region 'IValidationModel' Implementation
        public abstract bool TryValidate(out string error);
        #endregion
    }
}
