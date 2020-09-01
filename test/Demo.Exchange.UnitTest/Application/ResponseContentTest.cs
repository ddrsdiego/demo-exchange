namespace Demo.Exchange.UnitTest.Application
{
    using Demo.Exchange.Application;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Text.Json;

    [TestFixture]
    public class ResponseContentTest
    {
        [Test]
        public void Should_Create_ResponseContent_With_JsonString()
        {
            var customerResponse = new CustomerResponseStruct { CustomerId = "32927484880", Name = "Diego Dias Ribeiro da Silva", Age = 37 };
            var customerAsJson = JsonSerializer.Serialize(customerResponse, typeof(CustomerResponseStruct));

            var responseContent = ResponseContent.Create(customerAsJson, typeof(CustomerResponseStruct));
            var contentRaw = responseContent.GetRaw<CustomerResponseStruct>();
            responseContent.Value.Should().NotBeNull();
            customerResponse.CustomerId.Should().Be(contentRaw.CustomerId);
            responseContent.ValueAsJsonString.Should().Be(customerAsJson);
        }

        [Test]
        public void Should_Create_ResponseContent_With_Generics()
        {
            var customerResponse = new CustomerResponseStruct { CustomerId = "32927484880", Name = "Diego Dias Ribeiro da Silva", Age = 37 };
            var responseContent = ResponseContent.Create<CustomerResponseStruct>(customerResponse);

            var contentRaw = responseContent.GetRaw<CustomerResponseStruct>();

            responseContent.Value.Should().NotBeNull();
            customerResponse.CustomerId.Should().Be(contentRaw.CustomerId);
        }
    }

    internal struct CustomerResponseStruct
    {
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}