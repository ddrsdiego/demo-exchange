namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    public class ValorSegmentoParametroInput : ParametroInput
    {
        public ValorSegmentoParametroInput(decimal taxaSegmento)
        {
            TaxaSegmento = taxaSegmento;
        }

        public decimal TaxaSegmento { get; }
    }
}