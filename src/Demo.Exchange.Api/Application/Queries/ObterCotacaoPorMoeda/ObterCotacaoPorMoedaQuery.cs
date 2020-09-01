namespace Demo.Exchange.Application.Queries.ObterCotacaoPorMoeda
{
    using MediatR;

    public readonly struct ObterCotacaoPorMoedaQuery : IRequest<Response>
    {
        public ObterCotacaoPorMoedaQuery(string segmento, string moeda, decimal quantidade)
        {
            Segmento = segmento;
            Quantidade = quantidade;
            Moeda = moeda.Trim().ToUpperInvariant();
        }

        public string Segmento { get; }
        public string Moeda { get; }
        public decimal Quantidade { get; }
    }
}