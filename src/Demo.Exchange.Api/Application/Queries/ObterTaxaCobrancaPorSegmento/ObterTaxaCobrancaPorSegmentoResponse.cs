namespace Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ObterTaxaCobrancaPorSegmentoResponse : Response<string>
    {
        public ObterTaxaCobrancaPorSegmentoResponse(string requestId)
            : base(requestId)
        {
        }
    }

    public struct ObterTaxaCobrancaPorSegmentoResponseStruct
    {
        public string PayLoad { get; set; }
        public string RequestId { get; }
        public ErrorResponse ErrorResponse { get; set; }

        public void AddError(Error error)
        {
            if (Equals(error, default(Error)))
                return;

            ErrorResponse = new ErrorResponse(RequestId, error);
        }

        [JsonIgnore] public bool IsFailure => !IsSuccess;

        [JsonIgnore] public bool IsSuccess => VerifyResponseIsSuccess();

        private bool VerifyResponseIsSuccess()
            => EqualityComparer<ErrorResponse>.Default.Equals(ErrorResponse, default) || EqualityComparer<Error>.Default.Equals(ErrorResponse.Error, default);
    }
}