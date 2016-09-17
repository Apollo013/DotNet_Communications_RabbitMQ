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
|Message Exchange Patterns| (see below)|
|Exchanges|Declaring & Binding |
|Queues|Declaring & Binding |
|Exchange Types| Direct, Topic, Header & Fanout types|
|Persistence|Exchange, Queue & Message |
|Message Handler|Event based consuming and creation of a receiver class that derives from 'DefaultBasicConsumer'|
|Acknowledgements| Ack & Nack|
|Connection Configuration|RabbitMQ server configuration through appSettings - see ['ConnectionProperties' class](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/Common/RabbitMQCommon/ConnectionServices), which automatically reads from app.config file or applies defaults using **Reflection** & a custom **Attribute** |

---

###Message Exchange Patterns
|Pattern|Location|
|-------|--------|
|One Way | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/OneWayMessageExchangePattern) |
|Worker Queues | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/OneWayMessageExchangePattern) |
|Publish / Subscribe | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/PublishSubscribeMessageExchangePattern) |
|Two Way with RPC | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/TwoWayMessageExchangePattern) |
|Routing Keys | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/RoutingKeys) |
|Topics | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/TopicsMessageExchangePattern) |
|Headers | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/HeadersMessageExchangePattern) |
|Scatter / Gather | [Here](https://github.com/Apollo013/RabbitMQ_CSharp/tree/master/ScatterGatherMessageExchangePattern) |

---

###Resources
|Title|Author|Publisher/Website|
|-----|------|-----------------|
|[Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html)||RabbitMQ|
|[RabbitMQ Introduction](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html)| |RabbitMQ|
|[Reliability Guide](https://www.rabbitmq.com/reliability.html)||RabbitMQ|
|[Messaging with RabbitMQ and .NET review part 1: foundations and terminology](https://dotnetcodr.com/2016/08/02/messaging-with-rabbitmq-and-net-review-part-1-foundations-and-terminology/)|Andras Nemes| dotnetcodr.com|

