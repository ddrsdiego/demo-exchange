namespace Demo.Exchange.Domain.AggregateModel.TaxaModel
{
    using MediatR;
    using System;

    public readonly struct ValorTaxaAtualizadaEvent : INotification
    {
        public ValorTaxaAtualizadaEvent(string id)
            : this(Guid.NewGuid().ToString(), id, DateTime.Now)
        {
        }

        public ValorTaxaAtualizadaEvent(string id, DateTime atualizadoEm)
            : this(Guid.NewGuid().ToString(), id, atualizadoEm)
        {
        }

        public ValorTaxaAtualizadaEvent(string requestId, string id, DateTime atualizadoEm) => (RequestId, Id, AtualizadoEm) = (requestId, id, atualizadoEm);

        public string Id { get; }
        public string RequestId { get; }
        public DateTime AtualizadoEm { get; }
    }
}