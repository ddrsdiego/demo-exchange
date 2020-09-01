namespace Demo.Exchange.Application.Commands.AtualizarTaxa
{
    using MediatR;
    using System;

    public readonly struct AtualizarTaxaCommand : IRequest<Response>
    {
        public AtualizarTaxaCommand(string id, decimal novaTaxa)
            : this(Guid.NewGuid().ToString(), id, novaTaxa)
        {
        }

        public AtualizarTaxaCommand(string requestId, string id, decimal novaTaxa)
            => (RequestId, Id, NovaTaxa) = (requestId, id, novaTaxa);

        public string Id { get; }
        public decimal NovaTaxa { get; }
        public string RequestId { get; }
    }

    public struct AtualizarTaxaRequest
    {
        public decimal NovaTaxa { get; set; }
    }
}