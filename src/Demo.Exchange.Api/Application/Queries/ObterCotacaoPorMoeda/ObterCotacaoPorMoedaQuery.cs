namespace Demo.Exchange.Application.Queries.ObterCotacaoPorMoeda
{
    using MediatR;
    using System;

    public readonly struct ObterCotacaoPorMoedaQuery : IRequest<Response>
    {
        public ObterCotacaoPorMoedaQuery(string segmento, string moeda, decimal quantidade)
            : this(Guid.NewGuid().ToString(), segmento, moeda, quantidade)
        {
        }

        public ObterCotacaoPorMoedaQuery(string requestId, string segmento, string moeda, decimal quantidade) =>
            (RequestId, Segmento, Moeda, Quantidade) = (requestId, segmento, moeda, quantidade);

        public string RequestId { get; }
        public string Moeda { get; }
        public string Segmento { get; }
        public decimal Quantidade { get; }
    }
}