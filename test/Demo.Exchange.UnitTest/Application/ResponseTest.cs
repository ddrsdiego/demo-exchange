namespace Demo.Exchange.UnitTest.Application
{
    using Demo.Exchange.Application;
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
            var customerResponse = new CustomerResponseStruct { CustomerId = "32927484880", Name = "Diego Dias Ribeiro da Silva", Age = 37 };
            var customerAsJson = JsonSerializer.Serialize(customerResponse);
            var customerAsByte = JsonSerializer.SerializeToUtf8Bytes(customerResponse);

            //act
            var response = Response.Ok(ResponseContent.Create<CustomerResponseStruct>(customerResponse));
            var raw = response.Content.GetRaw(typeof(CustomerResponseStruct));

            //assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Content.Value.Should().BeEquivalentTo(customerAsByte);
            response.Content.ValueAsJsonString.Should().Be(customerAsJson);

            response.IsSuccess.Should().BeTrue();
            response.IsFailure.Should().BeFalse();
            response.ErrorResponse.Should().Be(default(ErrorResponse));
            response.ErrorResponse.Error.Should().Be(default(Error));

            ((CustomerResponseStruct)raw).CustomerId.Should().Be(customerResponse.CustomerId);
        }

        [Test]
        public void Should_Return_Response_Created_With_Content()
        {
            //arrange
            var customerResponse = new CustomerResponseStruct { CustomerId = "32927484880", Name = "Diego Dias Ribeiro da Silva", Age = 37 };
            var customerAsJson = JsonSerializer.Serialize(customerResponse);
            var customerAsByte = JsonSerializer.SerializeToUtf8Bytes(customerResponse);

            //act
            var response = Response.Ok(StatusCodes.Status201Created, ResponseContent.Create<CustomerResponseStruct>(customerResponse));
            var rawContent = response.Content.GetRaw(typeof(CustomerResponseStruct));

            //assert
            response.StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Content.Value.Should().BeEquivalentTo(customerAsByte);
            response.Content.ValueAsJsonString.Should().Be(customerAsJson);

            response.IsSuccess.Should().BeTrue();
            response.IsFailure.Should().BeFalse();
            response.ErrorResponse.Should().Be(default(ErrorResponse));
            response.ErrorResponse.Error.Should().Be(default(Error));

            ((CustomerResponseStruct)rawContent).CustomerId.Should().Be(customerResponse.CustomerId);
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