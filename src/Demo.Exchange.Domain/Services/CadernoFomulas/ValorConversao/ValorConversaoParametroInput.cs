namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    public class ValorConversaoParametroInput : ParametroInput
    {
        public ValorConversaoParametroInput(decimal quantidadeDesejada, decimal taxaConversao)
        {
            TaxaConversao = taxaConversao;
            QuantidadeDesejada = quantidadeDesejada;
        }

        public decimal TaxaConversao { get; }
        public decimal QuantidadeDesejada { get; }
    }
}