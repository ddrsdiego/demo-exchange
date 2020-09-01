namespace Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento
{
    using MediatR;

    public readonly struct ObterTaxaCobrancaPorSegmentoQuery : IRequest<Response>
    {
        public ObterTaxaCobrancaPorSegmentoQuery(string tipoSegmento) => TipoSegmento = tipoSegmento;

        public string TipoSegmento { get; }
    }
}