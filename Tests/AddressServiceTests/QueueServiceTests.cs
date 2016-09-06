using MessageService.Exceptions;
using MessageService.Services.AddressServices;
using Models.ServiceModels.AddressModels;
using Models.ServiceModels.BindingModels;
using Models.ServiceModels.DeleteModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Tests.Base;

namespace Tests.AddressServiceTests
{
    [TestFixture]
    class QueueServiceTests : TestBase
    {
        [Test]
        public void TestQueueService_CTOR_ChannelNull()
        {
            Assert.That(() =>
               { var qs = new QueueService(null); },
               Throws.TypeOf<ArgumentNullException>()
            );
        }

        [Test]
        public void TestQueueService_CTOR_ChannelNotNull()
        {
            Assert.DoesNotThrow(() => { var qs = new QueueService(Channel); });
        }

        [Test]
        public void TestQueueService_DeclareQueue_InvalidExchange()
        {
            Assert.That(() =>
            {
                BaseQueueService.Declare("non.existing.exchange", "company.queues.finance2");
            }, Throws.TypeOf<ServiceException>());
        }

        [Test]
        public void TestQueueService_DeclareQueue_ValidExchange()
        {
            Assert.DoesNotThrow(() => { BaseQueueService.Declare("amq.direct", "company.queues.finance2"); });
        }

        [Test]
        public void TestQueueService_DeclareQueue_ServiceChannelClosed()
        {
            Assert.That(() =>
            {
                var exchange = new QueueService(ClosedChannel);
                var addr = new QueueAddressModel()
                {
                    Name = "Test.Exchange",
                    ExchangeName = "TEST"
                };
                exchange.Declare(addr);
            }, Throws.TypeOf<ServiceException>());
        }

        [Test]
        public void TestQueueService_BindQueue()
        {
            // Create exchanges
            TestQueueService_DeclareQueue_ValidExchange();

            // Data
            QueueBindingModel qu = new QueueBindingModel()
            {
                Source = "amq.direct",
                Destination = "company.queues.finance2"
            };

            // Assert
            Assert.DoesNotThrow(() => { BaseQueueService.Bind(qu); });
        }

        [Test]
        public void TestQueueService_UnbindQueue()
        {
            // Create exchanges
            TestQueueService_DeclareQueue_ValidExchange();

            // Data
            QueueBindingModel qu = new QueueBindingModel()
            {
                Source = "amq.direct",
                Destination = "company.queues.finance2"
            };

            // Assert
            Assert.DoesNotThrow(() => { BaseQueueService.Unbind(qu); });
        }

        [Test]
        public void TestQueueService_DeleteObject()
        {
            var q = new QueueDeleteModel() { Name = "Test.ExchangeWithQueues.Queue1" };

            // Assert
            Assert.DoesNotThrow(() => { BaseQueueService.Delete(q); });
        }

        [Test]
        public void TestQueueService_DeleteString()
        {
            // Assert
            Assert.DoesNotThrow(() => { BaseQueueService.Delete("Test.ExchangeWithQueues.Queue2"); });
        }

        [Test]
        public void TestQueueService_DeleteMany()
        {
            var qa = new QueueAddressModel() { Name = "company.queues.finance6", ExchangeName = "Test.Exchange" };
            var qa1 = new QueueAddressModel() { Name = "company.queues.finance7", ExchangeName = "Test.Exchange" };

            var q = new QueueDeleteModel() { Name = "company.queues.finance6" };
            var q1 = new QueueDeleteModel() { Name = "company.queues.finance7" };

            List<QueueDeleteModel> qs = new List<QueueDeleteModel>() { q, q1 };
            List<QueueAddressModel> qas = new List<QueueAddressModel>() { qa, qa1 };

            BaseQueueService.DeclareMany(qas);

            // Assert
            Assert.DoesNotThrow(() => { BaseQueueService.DeleteMany(qs); });
        }
    }
}
