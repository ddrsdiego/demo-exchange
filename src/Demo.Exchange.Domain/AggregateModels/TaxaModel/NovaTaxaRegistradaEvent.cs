namespace Demo.Exchange.Domain.AggregateModel.TaxaModel
{
    using MediatR;
    using System;

    public readonly struct NovaTaxaRegistradaEvent : INotification
    {
        public NovaTaxaRegistradaEvent(TaxaCobranca taxaCobranca)
            : this(Guid.NewGuid().ToString(), taxaCobranca)
        {
            TaxaCobranca = taxaCobranca;
        }

        public NovaTaxaRegistradaEvent(string requestId, TaxaCobranca taxaCobranca)
        {
            RequestId = requestId;
            TaxaCobranca = taxaCobranca;
        }

        public string RequestId { get; }
        public TaxaCobranca TaxaCobranca { get; }
    }
}