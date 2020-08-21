namespace Demo.Exchange.Domain.AggregateModel.TaxaModel
{
    using MediatR;
    using System;

    public class ValorTaxaAtualizadaEvent : INotification
    {
        public ValorTaxaAtualizadaEvent(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public DateTime AtualizadoEm { get; } = DateTime.Now;
    }
}