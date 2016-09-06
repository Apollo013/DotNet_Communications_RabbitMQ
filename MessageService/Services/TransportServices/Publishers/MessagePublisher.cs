using MessageService.Exceptions;
using MessageService.Services.AddressServices.Base;
using MessageService.Services.Common;
using MessageService.Services.TransportServices.Publishers.Base;
using Models.ServiceModels.MessageModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;

namespace MessageService.Services.TransportServices.Publishers
{
    public class MessagePublisher : ServiceBase, IMessagePublisher
    {
        #region Properties
        private IExchangeService _exchangeService;

        public IExchangeService ExchangeService
        {
            get { return _exchangeService; }
            set { _exchangeService = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Publisher constructor
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="exchangeService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MessagePublisher(IModel channel, IExchangeService exchangeService) : base(channel)
        {
            if (exchangeService == null)
            {
                Logger.Warn("Null Exchange Service provided to message constructor");
                throw new ArgumentNullException("Please provide a valid exchange service");
            }
            ExchangeService = exchangeService;
        }
        #endregion

        #region 'IMessagePublisher' Implementations
        /// <summary>
        /// Publishes a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Publish(MessageModel message)
        {
            try
            {
                Validate(message, "Message cannot be null");
                Channel.BasicReturn += BasicReturn;
                ExchangeService.Declare(name: message.ExchangeName, queue: message.RoutingKey);
                Channel.BasicPublish(message.GetPublicationAddress(), message.BasicProperties, message.GetBytes());
                return true;
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

        public bool Publish(string body, string exchangeName, string routingKey = "", byte priority = 0, bool persistent = true, string contentType = "plain/text")
        {
            try
            {
                var msg = new MessageModel()
                {
                    Body = body,
                    ExchangeName = exchangeName,
                    RoutingKey = routingKey,
                    Priority = priority,
                    Persistent = persistent,
                    ContentType = contentType
                };
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BasicReturn(object sender, BasicReturnEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Code: {e.ReplyCode} - {e.ReplyText}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
    }
}
