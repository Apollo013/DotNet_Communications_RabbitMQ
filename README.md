# RabbitMQ_CSharp
A dotnet solution demonstrating various message patterns for publishing and consuming messages with RabbitMQ. (still under development)


---

Developed with Visual Studio 2015 Community

---


###Techs & Languages
|Tech|
|----|
|C#|
|RabbitMQ|
|NUnit|
|NLog|

---

###Features being demonstrated
|Feature|Description
|-------|----------|
|Message Exchange Patterns|Various message exchange patterns using RabbitMQ (see below)|
|Exchanges|Declaring & Binding |
|Queues|Declaring & Binding |
|Exchange Types| Direct & Fanout types|
|Persistence|Exchange, Queue & Message |
|Message Handler|Event based consuming and creation of a receiver class that derives from 'DefaultBasicConsumer'|
|Acknowledgements| Ack & Nack|
|Connection Configuration|RabbitMQ server configuration through appSettings - see ['ConnectionProperties' class](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/Common/RabbitMQCommon/ConnectionServices), which automatically reads appsettings from the app.config file|

---

###Message Exchange Patterns
|Pattern|Location|
|-------|--------|
|One Way Message Exchange Pattern | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/OneWayMessageExchangePattern) |
|Worker Queues Message Exchange Pattern | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/OneWayMessageExchangePattern) |
|Publish / Subscribe Message Exchange Pattern | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/PublishSubscribeMessageExchangePattern) |

---

###Resources
|Title|Author|Publisher/Website|
|-----|------|-----------------|
|[Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html)||RabbitMQ|
|[RabbitMQ Introduction](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html)| |RabbitMQ|
|[Reliability Guide](https://www.rabbitmq.com/reliability.html)||RabbitMQ|
|[Messaging with RabbitMQ and .NET review part 1: foundations and terminology](https://dotnetcodr.com/2016/08/02/messaging-with-rabbitmq-and-net-review-part-1-foundations-and-terminology/)|Andras Nemes| dotnetcodr.com|

