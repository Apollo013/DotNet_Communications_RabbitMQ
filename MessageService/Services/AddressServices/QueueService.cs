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
    /// Responsible for managing queues
    /// </summary>
    public sealed class QueueService : ServiceBase, IQueueService
    {
        #region Constructors
        /// <summary>
        /// Creates a new Queue Service instance
        /// </summary>
        /// <param name="channel"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public QueueService(IModel channel) : base(channel)
        { }
        #endregion

        #region 'IQueueService' Implementations
        /// <summary>
        /// Creates a single queue
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="durable"></param>
        /// <param name="exclusive"></param>
        /// <param name="autoDelete"></param>
        /// <param name="args"></param>
        /// <param name="routingKeys"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Declare(string exchange, string queue, bool durable = true, bool exclusive = false, bool autoDelete = false, Dictionary<string, object> args = null, string[] routingKeys = null)
        {
            Declare(new QueueAddressModel()
            {
                ExchangeName = exchange,
                Name = queue,
                Durable = durable,
                Exclusive = exclusive,
                AutoDelete = autoDelete,
                Arguments = args,
                RoutingKeys = routingKeys
            });
        }
        /// <summary>
        /// Creates a single queue
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        /// <param name="queue"></param>
        public void Declare(QueueAddressModel queue)
        {
            try
            {
                Validate(queue, "Queue to declare cannot be null");
                // Declare the queue
                Channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, queue.Arguments);
                // Apply any bindings
                if (!string.IsNullOrWhiteSpace(queue.ExchangeName))
                {
                    // Check if this queue will be bound using multiple routing keya
                    if (queue.HasRoutingKeys())
                    {
                        foreach (var key in queue.RoutingKeys)
                        {
                            Bind(queue.ExchangeName, queue.Name, key);
                        }
                    }
                    else
                    {
                        // Just bind the queue without a routing key
                        Bind(queue.ExchangeName, queue.Name, "");
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Queue Service Exception: please see log for more details.");
            }
        }

        /// <summary>
        /// Creates multiple queues
        /// </summary>
        /// <param name="queues"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>        
        public void DeclareMany(IEnumerable<QueueAddressModel> queues)
        {
            if (queues == null)
            {
                Logger.Error("Invalid collection of queues provided");
                throw new ArgumentNullException("Please supply a valid collection of queues to process");
            }

            foreach (var q in queues)
            {
                Declare(q);
            }
        }

        /// <summary>
        /// Binds a single queue to an exchange
        /// </summary>
        /// <param name="binding"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Bind(QueueBindingModel binding)
        {
            try
            {
                Validate(binding, "Queue to bind cannot be null");
                Channel.QueueBind(binding.Destination, binding.Source, binding.RoutingKey, binding.Arguments);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (OperationInterruptedException ex)
            {
                Logger.Warn(ex.Message);
                throw new ServiceException("Queue Service Exception: Exchange or queue does not exist is the likely cause, please see log for more details.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Queue Service Exception: please see log for more details.");
            }
        }

        /// <summary>
        /// Binds multiple queues to an exchange
        /// </summary>
        /// <param name="bindingAddresses"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception> 
        public void BindMany(IEnumerable<QueueBindingModel> bindings)
        {
            if (bindings == null)
            {
                Logger.Warn("Null bindings collection provided");
                throw new ArgumentNullException("Please supply a valid collection of bindings to process");
            }
            foreach (var binding in bindings)
            {
                Bind(binding);
            }
        }

        /// <summary>
        /// Deletes a single queue
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ifUnused"></param>
        /// <param name="ifEmpty"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Delete(string name, bool ifUnused = true, bool ifEmpty = true)
        {
            Delete(new QueueDeleteModel() { Name = name, IfUnused = ifUnused, IfEmpty = ifEmpty });
        }

        /// <summary>
        /// Deletes a single queue
        /// </summary>
        /// <param name="queue"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Delete(QueueDeleteModel queue)
        {
            try
            {
                Validate(queue, "Queue to delete cannot be null");
                Channel.QueueDelete(queue.Name, queue.IfUnused, queue.IfEmpty);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (OperationInterruptedException ex)
            {
                Logger.Warn(ex.Message);
                throw new ServiceException("Queue Service Exception: Queue still in use or is not empty the likely cause, please see log for more details.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Queue Service Exception: please see log for more details.");
            }
        }

        /// <summary>
        /// Delete multiple queues
        /// </summary>
        /// <param name="queues"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>        
        public void DeleteMany(IEnumerable<QueueDeleteModel> queues)
        {
            if (queues == null)
            {
                Logger.Warn("Null list of queues provided for delete");
                throw new ArgumentNullException("Please supply a valid collection of queues to process");
            }
            foreach (var q in queues)
            {
                Delete(q);
            }
        }

        /// <summary>
        /// Purges any message within a queue
        /// </summary>
        /// <param name="queue"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>      
        public void Purge(string queue)
        {
            if (string.IsNullOrWhiteSpace(queue))
            {
                Logger.Warn("Null queue provided for purge");
                throw new ArgumentNullException("Please supply a valid queue name to purge");
            }

            try
            {
                Channel.QueuePurge(queue);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Queue Service Exception: please see log for more details.");
            }
        }

        /// <summary>
        /// Binds a single queue to an exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Bind(string exchange, string queue, string routingKey = "")
        {
            Bind(new QueueBindingModel()
            {
                Source = exchange,
                Destination = queue,
                RoutingKey = routingKey
            });
        }

        /// <summary>
        /// Unbinds a single queue from it's exchange
        /// </summary>
        /// <param name="binding"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ServiceException"></exception>
        /// <exception cref="ValidationException"></exception>
        public void Unbind(QueueBindingModel binding)
        {
            try
            {
                Validate(binding, "Queue to unbind cannot be null");
                Channel.QueueUnbind(binding.Destination, binding.Source, binding.RoutingKey, binding.Arguments);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ServiceException || ex is ValidationException)
            {
                throw;
            }
            catch (OperationInterruptedException ex)
            {
                Logger.Warn(ex.Message);
                throw new ServiceException("Queue Service Exception: Exchange or queue does not exist is the likely cause, please see log for more details.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new ServiceException("Queue Service Exception: please see log for more details.");
            }
        }
        #endregion
    }
}

