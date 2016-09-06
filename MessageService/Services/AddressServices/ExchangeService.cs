using MessageService.Exceptions;
using MessageService.Services.AddressServices.Base;
using MessageService.Services.Common;
using Models.ServiceModels.AddressModels;
using Models.ServiceModels.BindingModels;
using Models.ServiceModels.DeleteModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;

namespace MessageService.Services.AddressServices
{
    /// <summary>
    /// Responsible for managing exchanges
    /// </summary>
    public class ExchangeService : ServiceBase, IExchangeService
    {
        #region Properties
        private IQueueService _queueService;
        public IQueueService QueueService
        {
            get
            {
                if (_queueService == null)
                {
                    return new QueueService(Channel);
                }
                return _queueService;
            }

            private set { _queueService = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new Exchange Service instance
        /// </summary>
        /// <param name="channel"></param>        
        /// <exception cref="ArgumentNullException"></exception>
        public ExchangeService(IModel channel) : base(channel)
        { }

        /// <summary>
        /// Creates a new Exchange Service instance that will also manage queues
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="queueService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExchangeService(IModel channel, IQueueService queueService) : base(channel)
        {
            if (queueService == null)
            {
                Logger.Warn("Null Queue Service provided to exchange constructor");
                throw new ArgumentNullException("Please provide a valid queue service");
            }
            QueueService = queueService;
        }
        #endregion

        #region 'IExchangeService' Implementations
        /// <summary>
        /// Creates a single exchange
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="durable"></param>
        /// <param name="autoDelete"></param>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Declare(string name, string type = RabbitMQ.Client.ExchangeType.Direct, bool durable = true, bool autoDelete = false, IDictionary<string, object> args = null, IList<QueueAddressModel> queues = null)
        {
            Declare(new ExchangeAddressModel()
            {
                Name = name,
                ExchangeType = type,
                Durable = durable,
                AutoDelete = autoDelete,
                Arguments = args,
                Queues = queues
            });
        }


        public void Declare(string name, string queue, string type = "direct", bool durable = true, bool autoDelete = false, IDictionary<string, object> args = null)
        {
            Declare(new ExchangeAddressModel()
            {
                Name = name,
                ExchangeType = type,
                Durable = durable,
                AutoDelete = autoDelete,
                Arguments = args,
                Queues = new List<QueueAddressModel>() { new QueueAddressModel() { Name = queue } }
            });
        }

        /// <summary>
        /// Creates a single exchange
        /// </summary>
        /// <param name="address"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <returns></returns>
        public void Declare(ExchangeAddressModel exchange)
        {
            try
            {
                Validate(exchange, "Exchange to declare cannot be null");
                Channel.ExchangeDeclare(
                        exchange.Name,
                        exchange.ExchangeType,
                        exchange.Durable,
                        exchange.AutoDelete,
                        exchange.Arguments
                    );

                if (QueueService != null && exchange.HasQueues())
                {
                    QueueService.DeclareMany(exchange.Queues);
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Exchange Service Exception: please see log for more details.");
            }
        }

        /// <summary>
        /// Creates multiple exchanges
        /// </summary>
        /// <param name="exchanges"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void DeclareMany(IEnumerable<ExchangeAddressModel> exchanges)
        {
            if (exchanges == null)
            {
                Logger.Warn("Null collection of exchanges provided");
                throw new ArgumentNullException("Please supply a valid collection of exchanges to process");
            }

            foreach (var exchange in exchanges)
            {
                Declare(exchange);
            }
        }

        /// <summary>
        /// Binds 2 exchanges together
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <param name="binding"></param>
        public void Bind(ExchangeBindingModel binding)
        {
            try
            {
                Validate(binding, "Exchange to Bind cannot be null");
                Channel.ExchangeBind(binding.Destination, binding.Source, binding.RoutingKey, binding.Arguments);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (OperationInterruptedException ex)
            {
                Logger.Warn(ex.Message);
                throw new ServiceException("Exchange Service Exception: One or more exchanges do not exist is the likely cause, please see log for more details.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Exchange Service Exception: please see log for more details.");
            }
        }

        /// <summary>
        /// Binds multiple exchanges together
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <param name="bindings"></param>
        public void BindMany(IEnumerable<ExchangeBindingModel> bindings)
        {
            if (bindings == null)
            {
                Logger.Warn("Null collection of bindings provided");
                throw new ArgumentNullException("Please supply a valid collection of bindings to process");
            }
            foreach (var binding in bindings)
            {
                Bind(binding);
            }
        }

        public void Delete(string name, bool ifUnused = true)
        {
            Delete(new ExchangeDeleteModel() { Name = name, IfUnused = ifUnused });
        }

        /// <summary>
        /// Deletes a single exchange
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <param name="exchange"></param>
        public void Delete(ExchangeDeleteModel exchange)
        {
            try
            {
                Validate(exchange, "Exchange to delete cannot be null");
                Channel.ExchangeDelete(exchange.Name, exchange.IfUnused);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (OperationInterruptedException ex)
            {
                Logger.Warn(ex.Message);
                throw new ServiceException("Exchange Service Exception: Exchange still in use is the likely cause, please see log for more details.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Exchange Service Exception: please see log for more details.");
            }
        }

        /// <summary>
        /// Delete multiple exchanges
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <param name="exchanges"></param>
        public void DeleteMany(IEnumerable<ExchangeDeleteModel> exchanges)
        {
            if (exchanges == null)
            {
                Logger.Warn("Null collection of exchanges provided");
                throw new ArgumentNullException("Please supply a valid collection of exchanges to process");
            }
            foreach (var exchange in exchanges)
            {
                Delete(exchange);
            }
        }

        /// <summary>
        /// Unbinds a single exchange from another exchange
        /// </summary>
        /// <param name="binding"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Unbind(ExchangeBindingModel binding)
        {
            try
            {
                Validate(binding, "Exchange to unbind cannot be null");
                Channel.ExchangeUnbind(binding.Destination, binding.Source, binding.RoutingKey, binding.Arguments);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (OperationInterruptedException ex)
            {
                Logger.Warn(ex.Message);
                throw new ServiceException("Exchange Service Exception: One or more exchanges do not exist is the likely cause, please see log for more details.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Exchange Service Exception: please see log for more details.");
            }
        }

        #endregion
    }
}
