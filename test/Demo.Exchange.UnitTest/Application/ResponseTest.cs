namespace Demo.Exchange.UnitTest.Application
{
    using Demo.Exchange.Application;
    using Demo.Exchange.Application.Models;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using NUnit.Framework;
    using System.Text.Json;

    [TestFixture]
    public class ResponseTest
    {
        [Test]
        public void Should_Return_Response_OK_With_No_Content()
        {
            var response = Response.Ok();
            response.Content.Should().Be(default(ResponseContent));
            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            response.ErrorResponse.Should().Be(default(ErrorResponse));
            response.ErrorResponse.Error.Should().Be(default(Error));
            response.IsSuccess.Should().BeTrue();
            response.IsFailure.Should().BeFalse();
        }

        [Test]
        public void Should_Return_Response_OK_With_Content()
        {
            //arrange
            var taxaResponse = FakesData.FakeData.TaxaResponseValid;
            var taxaResponseAsJson = JsonSerializer.Serialize(taxaResponse);
            var taxaResponseAsByte = JsonSerializer.SerializeToUtf8Bytes(taxaResponse);

            //act
            var response = Response.Ok(ResponseContent.Create(taxaResponse));
            var raw = response.Content.GetRaw(typeof(TaxaResponse));

            //assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Content.Value.Should().BeEquivalentTo(taxaResponseAsByte);
            response.Content.ValueAsJsonString.Should().Be(taxaResponseAsJson);

            response.IsSuccess.Should().BeTrue();
            response.IsFailure.Should().BeFalse();
            response.ErrorResponse.Should().Be(default(ErrorResponse));
            response.ErrorResponse.Error.Should().Be(default(Error));

            ((TaxaResponse)raw).Id.Should().Be(taxaResponse.Id);
        }

        [Test]
        public void Should_Return_Response_Created_With_Content()
        {
            //arrange
            var taxaResponse = FakesData.FakeData.TaxaResponseValid;
            var taxaResponseAsJson = JsonSerializer.Serialize(taxaResponse);
            var taxaResponseAsByte = JsonSerializer.SerializeToUtf8Bytes(taxaResponse);

            //act
            var response = Response.Ok(StatusCodes.Status201Created, ResponseContent.Create<TaxaResponse>(taxaResponse));
            var rawContent = response.Content.GetRaw(typeof(TaxaResponse));

            //assert
            response.StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Content.Value.Should().BeEquivalentTo(taxaResponseAsByte);
            response.Content.ValueAsJsonString.Should().Be(taxaResponseAsJson);

            response.IsSuccess.Should().BeTrue();
            response.IsFailure.Should().BeFalse();
            response.ErrorResponse.Should().Be(default(ErrorResponse));
            response.ErrorResponse.Error.Should().Be(default(Error));

            ((TaxaResponse)rawContent).Id.Should().Be(taxaResponse.Id);
        }

        [Test]
        public void Should_Return_Response_BadRequest_With_ErrorResponse()
        {
            //arrange
            var error = Errors.General.InternalProcessError("InternalProcessError");

            //act
            var response = Response.Fail(StatusCodes.Status400BadRequest, error);

            //assert
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            response.Content.Should().Be(default(ResponseContent));

            response.ErrorResponse.Should().NotBeNull();
            response.IsFailure.Should().BeTrue();
            response.IsSuccess.Should().BeFalse();
            response.ErrorResponse.Error.Should().NotBeNull();
            response.ErrorResponse.Error.Code.Should().Be(error.Code);
        }
    }
}