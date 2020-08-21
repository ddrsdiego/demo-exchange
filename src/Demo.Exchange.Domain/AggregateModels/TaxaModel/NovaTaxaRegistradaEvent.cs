namespace Demo.Exchange.Domain.AggregateModel.TaxaModel
{
    using MediatR;

    public class NovaTaxaRegistradaEvent : INotification
    {
        public NovaTaxaRegistradaEvent(TaxaCobranca taxaCobranca)
        {
            TaxaCobranca = taxaCobranca;
        }

        public TaxaCobranca TaxaCobranca { get; }
    }
}