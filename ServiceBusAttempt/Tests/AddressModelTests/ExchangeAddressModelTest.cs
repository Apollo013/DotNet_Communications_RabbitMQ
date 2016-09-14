using Models.ServiceModels.AddressModels;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests.AddressModelTests
{
    [TestFixture]
    class ExchangeAddressModelTest
    {
        [Test]
        public void TestExchangeAddress_MissingExchangeName()
        {
            ExchangeAddressModel ex = new ExchangeAddressModel()
            {
                Name = "",
                ExchangeType = "direct"
            };

            var errorMessage = "";
            if (ex.TryValidate(out errorMessage))
            {
                Assert.Fail("Should have failed");
            }
            Assert.Pass(errorMessage);
        }

        [Test]
        public void TestExchangeAddress_MissingExchangeType()
        {
            ExchangeAddressModel ex = new ExchangeAddressModel()
            {
                Name = "sdfsf",
                ExchangeType = ""
            };

            var errorMessage = "";
            if (ex.TryValidate(out errorMessage))
            {
                Assert.Fail("Should have failed");
            }
            Assert.Pass(errorMessage);
        }

        [Test]
        public void TestExchangeAddress_InvalidExchangeType()
        {
            ExchangeAddressModel ex = new ExchangeAddressModel()
            {
                Name = "sdfsf",
                ExchangeType = "incorrect.type"
            };

            var errorMessage = "";
            if (ex.TryValidate(out errorMessage))
            {
                Assert.Fail("Should have failed");
            }
            Assert.Pass(errorMessage);
        }

        [Test]
        public void TestExchangeAddress_ValidAddreses()
        {
            ExchangeAddressModel ex = new ExchangeAddressModel()
            {
                Name = "sdfsf",
                ExchangeType = "direct"
            };

            var errorMessage = "";
            if (ex.TryValidate(out errorMessage))
            {
                Assert.Pass(errorMessage);
            }
            Assert.Fail("Should have failed");
        }

        [Test]
        public void TestExchangeAddress_ArgsDictionary()
        {
            ExchangeAddressModel ex = new ExchangeAddressModel()
            {
                Name = "sdfsf",
                ExchangeType = "direct",
                Arguments = new Dictionary<string, object>() { { "test", 32 }, { "test2", new string('o', 30) } }
            };
            Assert.Pass();
        }
    }
}
