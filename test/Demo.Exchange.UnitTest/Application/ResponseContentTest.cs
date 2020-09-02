namespace Demo.Exchange.UnitTest.Application
{
    using Demo.Exchange.Application;
    using Demo.Exchange.Application.Models;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Text.Json;

    [TestFixture]
    public class ResponseContentTest
    {
        [Test]
        public void Should_Create_ResponseContent_With_JsonString()
        {
            var taxaResponse = FakesData.FakeData.TaxaResponseValid;
            var taxaResponseAsJson = JsonSerializer.Serialize(taxaResponse, typeof(TaxaResponse));

            var responseContent = ResponseContent.Create(taxaResponseAsJson, typeof(TaxaResponse));
            var contentRaw = responseContent.GetRaw<TaxaResponse>();

            responseContent.Value.Should().NotBeNull();
            taxaResponse.Id.Should().Be(contentRaw.Id);
            responseContent.ValueAsJsonString.Should().Be(taxaResponseAsJson);
        }

        [Test]
        public void Should_Create_ResponseContent_With_Generics()
        {
            var taxaResponse = FakesData.FakeData.TaxaResponseValid;
            var taxaResponseAsJson = JsonSerializer.Serialize(taxaResponse, typeof(TaxaResponse));

            var responseContent = ResponseContent.Create<TaxaResponse>(taxaResponse);

            var contentRaw = responseContent.GetRaw<TaxaResponse>();

            taxaResponse.Id.Should().Be(contentRaw.Id);

            responseContent.Value.Should().NotBeNull();
            responseContent.ValueAsJsonString.Should().NotBeEmpty();
            responseContent.ValueAsJsonString.Should().Be(taxaResponseAsJson);
        }

        [Test]
        public void Should_Create_ResponseContent_With_ByteArray()
        {
            var taxaResponse = FakesData.FakeData.TaxaResponseValid;
            var taxaResponseAsJson = JsonSerializer.Serialize(taxaResponse, typeof(TaxaResponse));
            var taxaResponseAsByte = JsonSerializer.SerializeToUtf8Bytes<TaxaResponse>(taxaResponse);

            var responseContent = ResponseContent.Create(taxaResponseAsByte, typeof(TaxaResponse));

            var contentRaw = responseContent.GetRaw<TaxaResponse>();

            taxaResponse.Id.Should().Be(contentRaw.Id);

            responseContent.Value.Should().NotBeNull();
            responseContent.ValueAsJsonString.Should().NotBeEmpty();

            responseContent.Value.Should().BeEquivalentTo(taxaResponseAsByte);
            responseContent.ValueAsJsonString.Should().Be(taxaResponseAsJson);
        }
    }
}