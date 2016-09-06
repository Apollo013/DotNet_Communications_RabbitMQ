using Models.ServiceModels.AddressModels;
using NUnit.Framework;

namespace Tests.AddressModelTests
{
    [TestFixture]
    class QueueAddressModelTests
    {

        [Test]
        public void TestQueueAddress_MissingQueueName()
        {
            QueueAddressModel ex = new QueueAddressModel()
            {
                ExchangeName = "anexchange",
                Name = ""
            };

            var errorMessage = "";
            if (ex.TryValidate(out errorMessage))
            {
                Assert.Fail("Should have failed");
            }
            Assert.Pass(errorMessage);
        }

        [Test]
        public void TestQueueAddress_MissingExchangeName()
        {
            QueueAddressModel ex = new QueueAddressModel()
            {
                ExchangeName = "",
                Name = "aqueue"
            };

            var errorMessage = "";
            if (ex.TryValidate(out errorMessage))
            {
                Assert.Pass(errorMessage);
            }
            Assert.Fail("Should have failed");
        }
    }
}
