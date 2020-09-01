namespace Demo.Exchange.Application
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public readonly struct Response
    {
        private Response(int statusCode, ResponseContent content, ErrorResponse errorResponse)
        {
            Content = content;
            ErrorResponse = errorResponse;
            StatusCode = statusCode;
        }

        public int StatusCode { get; }
        public ResponseContent Content { get; }
        public ErrorResponse ErrorResponse { get; }

        public static Response Ok() => Ok(StatusCodes.Status200OK, new ResponseContent());

        public static Response Ok(ResponseContent content)
        {
            if (content.Equals(default))
                throw new ArgumentNullException(nameof(content));

            return new Response(StatusCodes.Status200OK, content, new ErrorResponse());
        }

        public static Response Ok(int statusCode, ResponseContent content)
        {
            if (statusCode < 200 || statusCode > 299)
                throw new ArgumentException(nameof(statusCode));

            if (content.Equals(default))
                throw new ArgumentNullException(nameof(content));

            return new Response(statusCode, content, new ErrorResponse());
        }

        public static Response Fail(Error error)
        {
            if (error.Equals(default))
                throw new ArgumentNullException(nameof(error));

            return new Response(StatusCodes.Status400BadRequest, new ResponseContent(), new ErrorResponse(error));
        }

        public static Response Fail(int statusCode, Error error)
        {
            if (statusCode >= 200 && statusCode <= 299)
                throw new ArgumentException(nameof(statusCode));

            if (error.Equals(default))
                throw new ArgumentNullException(nameof(error));

            return new Response(statusCode, new ResponseContent(), new ErrorResponse(error));
        }

        [JsonIgnore] public bool IsFailure => !IsSuccess;

        [JsonIgnore] public bool IsSuccess => VerifyResponseIsSuccess();

        private bool VerifyResponseIsSuccess()
            => EqualityComparer<ErrorResponse>.Default.Equals(ErrorResponse, default) || EqualityComparer<Error>.Default.Equals(ErrorResponse.Error, default);
    }
}