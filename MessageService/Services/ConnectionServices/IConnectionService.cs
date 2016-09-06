using RabbitMQ.Client;
using System;

namespace MessageService.Services.ConnectionServices
{
    /// <summary>
    /// Interface for managing RabbitMQ connections
    /// </summary>
    public interface IConnectionService : IDisposable
    {
        IConnection Connection { get; }
        IModel Channel { get; }
    }
}
