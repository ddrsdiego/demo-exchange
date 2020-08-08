namespace Demo.Exchange.Application.Models
{
    public readonly struct CotacaoPorMoedaResponse
    {
        public CotacaoPorMoedaResponse(MoedaResponse moedaDe, MoedaResponse moedaPara, decimal taxaConversao, decimal quantidadeDesejada, decimal valorCotacao) =>
            (MoedaDe, MoedaPara, TaxaConversao, QuantidadeDesejada, ValorCotacao) = (moedaDe, moedaPara, taxaConversao, quantidadeDesejada, valorCotacao);

        public MoedaResponse MoedaDe { get; }
        public MoedaResponse MoedaPara { get; }
        public decimal TaxaConversao { get; }
        public decimal QuantidadeDesejada { get; }
        public decimal ValorCotacao { get; }
    }

    public readonly struct MoedaResponse
    {
        public MoedaResponse(string moeda, decimal valorUnitario) => (Moeda, ValorUnitario) = (moeda, valorUnitario);

        public string Moeda { get; }
        public decimal ValorUnitario { get; }
    }
}