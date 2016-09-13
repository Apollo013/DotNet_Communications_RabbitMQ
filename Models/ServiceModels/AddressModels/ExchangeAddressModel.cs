using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.ServiceModels.AddressModels
{
    /// <summary>
    /// Contains the properties needed to declare an exchange container
    /// </summary>
    public class ExchangeAddressModel : AddressBase
    {
        #region Properties
        private string _name = "";
        /// <summary>
        /// Gets or sets the name for the exchange
        /// </summary>
        public override string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                CheckQueueIntegrity();
            }
        }

        /// <summary>
        /// Gets or sets the type of exchange to create
        /// </summary>
        public string ExchangeType { get; set; } = RabbitMQ.Client.ExchangeType.Direct;

        private List<QueueAddressModel> _queues;
        /// <summary>
        /// Gets or sets a list of related queues that are bound to the exchange
        /// </summary>
        public List<QueueAddressModel> Queues
        {
            get
            {
                if (_queues == null)
                {
                    _queues = new List<QueueAddressModel>();
                }
                return _queues;
            }
            set
            {
                _queues = value;
                CheckQueueIntegrity();
            }
        }
        #endregion

        #region 'IValidationObject' Implementation
        public override bool TryValidate(out string error)
        {
            error = "";

            if (string.IsNullOrWhiteSpace(Name))
            {
                error = "Please supply a name for the exchange";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ExchangeType))
            {
                error = "Please supply the type of exchange to create";
                return false;
            }

            // Check that this is a valid exchange type
            if (!RabbitMQ.Client.ExchangeType.All().Contains(ExchangeType.ToLower()))
            {
                error = "Please supply a valid type of exchange to create";
                return false;
            }

            return true;
        }
        #endregion

        #region Queues
        /// <summary>
        /// Determines if there are any associated queues with this exchange
        /// </summary>
        /// <returns></returns>
        public bool HasQueues()
        {
            CheckQueueIntegrity();
            return Queues.Any();
        }

        /// <summary>
        /// Make sure that queues have the same exchange name as above
        /// </summary>
        private void CheckQueueIntegrity()
        {
            if (_queues == null) { return; }

            _queues.RemoveAll(x => String.IsNullOrWhiteSpace(x.Name));

            foreach (var q in _queues)
            {
                q.ExchangeName = Name;
            }
        }
        #endregion
    }
}
