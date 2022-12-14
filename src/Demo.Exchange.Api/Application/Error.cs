namespace Demo.Exchange.Application
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;

    public readonly struct Error
    {
        public Error(string code, string message)
            : this(code, message, StatusCodes.Status400BadRequest)
        {
        }

        public Error(string code, string message, int statusCode)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            StatusCode = statusCode;
            Details = new List<Error>();
        }

        public string Code { get; }
        public string Message { get; }
        [JsonIgnore] public int StatusCode { get; }

        public Error AddErroDetail(Error error)
        {
            if (error.Code.Equals(Code, StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentNullException(nameof(error.Code));

            if (Details.Any(x => x.Code.Equals(error.Code, StringComparison.InvariantCultureIgnoreCase)))
                return error;

            Details.Add(error);

            return this;
        }

        public List<Error> Details { get; }
    }
}