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
    class ExchangeServiceTests : TestBase
    {

        [Test]
        public void TestExchangeService_CTOR_NullArguments_Channel()
        {
            Assert.That(() =>
            { var qs = new ExchangeService(null); },
               Throws.TypeOf<ArgumentNullException>()
            );
        }

        [Test]
        public void TestExchangeService_CTOR_ValidArgument()
        {
            Assert.DoesNotThrow(() => { var qs = new ExchangeService(Channel); });
        }

        [Test]
        public void TestExchangeService_CTOR_NullArguments_Queue()
        {
            Assert.That(() =>
            { var qs = new ExchangeService(Channel, null); },
               Throws.TypeOf<ArgumentNullException>()
            );
        }

        [Test]
        public void TestExchangeService_CTOR_ValidArguments()
        {
            Assert.DoesNotThrow(() => { var qs = new ExchangeService(Channel, BaseQueueService); });
        }

        [Test]
        public void TestExchangeService_DeclareExchange_ServiceChannelClosed()
        {
            Assert.That(() =>
            {
                var exchange = new ExchangeService(ClosedChannel);
                var addr = new ExchangeAddressModel()
                {
                    Name = "Test.Exchange"
                };
                exchange.Declare(addr);
            }, Throws.TypeOf<ServiceException>());
        }

        [Test]
        public void TestExchangeService_DeclareExchange_NullArguments()
        {
            Assert.That(() =>
            {
                BaseExchangeService.Declare(null);
            }, Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TestExchangeService_DeclareExchange_AddressObject()
        {
            var addr = new ExchangeAddressModel()
            {
                Name = "Test.Exchange"
            };

            Assert.DoesNotThrow(() => { BaseExchangeService.Declare(addr); });
        }

        [Test]
        public void TestExchangeService_DeclareExchange_StringParams()
        {
            Assert.DoesNotThrow(() => { BaseExchangeService.Declare(name: "Test.Exchange2", type: "direct"); });
        }

        [Test]
        public void TestExchangeService_DeclareExchange_InvalidExchangeName()
        {
            Assert.That(() =>
            {
                BaseExchangeService.Declare(name: "", type: "direct");
            }, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void TestExchangeService_DeclareExchange_InvalidExchangeType()
        {
            Assert.That(() =>
            {
                BaseExchangeService.Declare(name: "test.Example.3", type: "BLAHBLAH");
            }, Throws.TypeOf<ValidationException>());
        }

        [Test]
        public void TestExchangeService_DeleteExchange_ServiceChannelClosed()
        {
            Assert.That(() =>
            {
                var exchange = new ExchangeService(ClosedChannel);
                exchange.Delete("Test.Exchange");
            }, Throws.TypeOf<ServiceException>());
        }

        [Test]
        public void TestExchangeService_DeclareMany()
        {
            var addr = new ExchangeAddressModel() { Name = "Test.Exchange.ToDelete" };
            var addr2 = new ExchangeAddressModel() { Name = "Test.Exchange.ToDelete2" };
            List<ExchangeAddressModel> exchanges = new List<ExchangeAddressModel>() { addr, addr2 };
            // Assert
            Assert.DoesNotThrow(() => { BaseExchangeService.DeclareMany(exchanges); });
        }

        [Test]
        public void TestExchangeService_DeleteMany()
        {
            var ex = new ExchangeDeleteModel() { Name = "Test.Exchange.ToDelete" };
            var ex2 = new ExchangeDeleteModel() { Name = "Test.Exchange.ToDelete2" };
            List<ExchangeDeleteModel> exchanges = new List<ExchangeDeleteModel>() { ex, ex2 };

            // Assert
            Assert.DoesNotThrow(() => { BaseExchangeService.DeleteMany(exchanges); });
        }

        [Test]
        public void TestExchangeService_DeleteExchange_NonExistingExchange()
        {
            Assert.DoesNotThrow(() => { BaseExchangeService.Delete("Test.Exchange6"); });
        }

        [Test]
        public void TestExchangeService_DeleteExchange_DeleteObject()
        {
            // Create data
            ExchangeDeleteModel exd = new ExchangeDeleteModel()
            {
                Name = "mycompany.fanout.exchange",
                IfUnused = false // override, remove the exchange regardless if it is in use or not
            };

            // Assert
            Assert.DoesNotThrow(() => { BaseExchangeService.Delete(exd); });

            // Remove a second one
            exd.Name = "company.direct.exchange";
            Assert.DoesNotThrow(() => { BaseExchangeService.Delete(exd); });
        }

        [Test]
        public void TestExchangeService_BindExchanges()
        {
            // Create exchanges
            TestExchangeService_DeclareExchange_AddressObject();
            TestExchangeService_DeclareExchange_StringParams();

            // At this stage, 2 exchanges have been created - 'Test.Exchange' & 'Test.Exchange2'

            ExchangeBindingModel ex = new ExchangeBindingModel()
            {
                Source = "Test.Exchange2",
                Destination = "Test.Exchange"
            };

            Assert.DoesNotThrow(() => { BaseExchangeService.Bind(ex); });
        }

        [Test]
        public void TestExchangeService_BindExchanges_NonExistingExchanges()
        {
            // These exchanges do not exist
            ExchangeBindingModel ex = new ExchangeBindingModel()
            {
                Source = "Test.Exchange3",
                Destination = "Test.Exchange4"
            };

            Assert.That(() =>
            {
                BaseExchangeService.Bind(ex);
            }, Throws.TypeOf<ServiceException>());

        }

        [Test]
        public void TestExchangeService_UnbindExchanges()
        {
            // Create exchanges
            TestExchangeService_DeclareExchange_AddressObject();
            TestExchangeService_DeclareExchange_StringParams();

            // At this stage, 2 exchanges have been created - 'Test.Exchange' & 'Test.Exchange2'

            ExchangeBindingModel ex = new ExchangeBindingModel()
            {
                Source = "Test.Exchange2",
                Destination = "Test.Exchange"
            };

            Assert.DoesNotThrow(() => { BaseExchangeService.Unbind(ex); });
        }

        [Test]
        public void TestExchangeService_DeclareExchange_WithQueues()
        {
            var exchange = new ExchangeAddressModel()
            {
                Name = "Test.Exchange.WithQueues",
                Queues = new List<QueueAddressModel>()
                {
                    new QueueAddressModel() {Name = "Test.ExchangeWithQueues.Queue1" },
                    new QueueAddressModel() {Name = "Test.ExchangeWithQueues.Queue2" }
                }
            };

            var exc = new ExchangeService(Channel, new QueueService(Channel));
            Assert.DoesNotThrow(() => { exc.Declare(exchange); });
            Assert.DoesNotThrow(() => { exc.Delete("Test.Exchange.WithQueues", false); });
        }
    }
}
