namespace Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento
{
    using MediatR;
    using System;

    public readonly struct ObterTaxaCobrancaPorSegmentoQuery : IRequest<Response>
    {
        public ObterTaxaCobrancaPorSegmentoQuery(string tipoSegmento)
            : this(Guid.NewGuid().ToString(), tipoSegmento)
        {
        }

        public ObterTaxaCobrancaPorSegmentoQuery(string requestId, string tipoSegmento)
            => (RequestId, TipoSegmento) = (requestId, tipoSegmento);

        public string RequestId { get; }
        public string TipoSegmento { get; }
    }
}