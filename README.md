# RabbitMQ_CSharp
A dotnet solution demonstrating various message patterns for publishing and consuming messages with RabbitMQ. (still under development)


---

Developed with Visual Studio 2015 Community

---

###Features being demonstrated
|Feature|
|-------|
|Various message exchange patterns using RabbitMQ (see below)|
|Declaring & binding exchanges & queues|
|Exchange Types: direct & fanout types|
|Exchange, Queue & Message Persistence|
|Event based consuming|
|Creating wrapper components for managing Exchanges, Queues, Message Publisher, & Consumer, so that they can work independently, or be brought together as a single unit under a service manager component [See Here] (https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/MessageService/Services) and [Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/MessageService/Managers/ServiceManager.cs)|
|Acknowledging and rejecting messages (Ack / Nack)|
|RabbitMQ server configuration through appSettings - see ['ConnectionProperties' class](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Models/ServiceModels/ConnectionModels/ConnectionProperties.cs) which extends the ['PropertyBase' class](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Models/ServiceModels/Base/PropertyBase.cs) that automatically reads appsettings in the app.config file|

---

### Components

Different functionality has been divided into different components. For Example, Declaring, Binding and Removing Exchanges can be found in the 'ExchangeService.cs' file. The same applies to Queues, Publishing and Consuming messages whom all have their own components. The effect is that these can all operate independently of each other.

However, The 'PublisherService' contains a reference to the 'ExchangeService', which in turn has a reference to the 'QueueService', which allows us to simnply fire a message at the server, and we can be sure (if no unforeseen exceptions occur) that the exchanges and queues will be properly created and bound.

---

###Techs & Languages
|Tech|
|----|
|C#|
|RabbitMQ|
|NUnit|
|NLog|
---

###Message Exchange Patterns
|Pattern|Publisher Code|ConsumerCode|
|-------|--------------|------------|
|One Way Message Exchange Pattern | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Publishers/Program.cs) | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Consumers/Program.cs) |
|Worker Queues Message Exchange Pattern | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Publishers/Program.cs) | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Consumers/Program.cs) |
|Publish / Subscribe Message Exchange Pattern | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Publishers/Program.cs) | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Consumers/Program.cs)|

---

###Solution Layout
|Assembly|Description|
|--------|-----------|
|Message Service|A wrapper service that contains all the components that interact with the RabbitMQ server|
|Logging Service| Logging service wrapper|
|Models| Contains any entity models used to pass information|
|Tests|Unit tests using Nunit|
|Client/Publishers|Client used for pushing messages to the server|
|Client/Consumers|Client used to consume messages from the server|


---

###Resources
|Title|Author|Publisher/Website|
|-----|------|-----------------|
|[Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html)||RabbitMQ|
|[RabbitMQ Introduction](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html)| |RabbitMQ|
|[Reliability Guide](https://www.rabbitmq.com/reliability.html)||RabbitMQ|
|[Messaging with RabbitMQ and .NET review part 1: foundations and terminology](https://dotnetcodr.com/2016/08/02/messaging-with-rabbitmq-and-net-review-part-1-foundations-and-terminology/)|Andras Nemes| dotnetcodr.com|

