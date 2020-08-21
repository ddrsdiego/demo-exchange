namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    public class ConversaoMoedaParametroInput : ParametroInput
    {
        public ConversaoMoedaParametroInput(decimal quantidadeDesejada, decimal taxaConversao, decimal taxaSegmento)
        {
            QuantidadeDesejada = quantidadeDesejada;
            TaxaConversao = taxaConversao;
            TaxaSegmento = taxaSegmento;
        }

        public decimal QuantidadeDesejada { get; }
        public decimal TaxaConversao { get; }
        public decimal TaxaSegmento { get; }
    }
}