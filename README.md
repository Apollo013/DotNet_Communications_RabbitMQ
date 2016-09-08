# RabbitMQ_CSharp
A multi assembly solution demonstrating various message patterns for publishing and consuming messages with RabbitMQ. (still under development)


---

Developed with Visual Studion 2015 Community

---

###Techs & Languages
|Tech|
|----|
|C#|
|RabbitMQ|
|NUnit|
|NLog|
---

###Message Patterns
|Pattern|Publisher Code|ConsumerCode|
|-------|--------------|------------|
|One Way Message Pattern|[Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Publishers/Program.cs)|[Here](https://github.com/Apollo013/RabbitMQ_CSharp/blob/master/Clients/Consumers/Program.cs)|

---

###Solution Layout
|Assembly|Description|
|--------|-----------|
|Message Service|Contains all the services that interact with the RabbitMQ server|
|Logging Service| Abstract logging service|
|Models| Contains any entity models used to pass information|
|Tests|Unit tests using Nunit|
|Client/Publishers|Client used for pushing messages to the server|
|Client/Consumers|Client used to read messages from the server|


---

###Resources
|Title|Author|Publisher/Website|
|-----|------|-----------------|
|[Downloading and Installing RabbitMQ](https://www.rabbitmq.com/download.html)||RabbitMQ|
|[RabbitMQ Introduction](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html)| |RabbitMQ|
|[Reliability Guide](https://www.rabbitmq.com/reliability.html)||RabbitMQ|
|[Messaging with RabbitMQ and .NET review part 1: foundations and terminology](https://dotnetcodr.com/2016/08/02/messaging-with-rabbitmq-and-net-review-part-1-foundations-and-terminology/)|Andras Nemes| dotnetcodr.com|

