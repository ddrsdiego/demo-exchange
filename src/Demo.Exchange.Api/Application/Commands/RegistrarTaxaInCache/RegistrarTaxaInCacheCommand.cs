namespace Demo.Exchange.Application.Commands.RegistrarTaxaInCache
{
    using MediatR;
    using System;

    public readonly struct RegistrarTaxaInCacheCommand : IRequest<Response>
    {
        public RegistrarTaxaInCacheCommand(string id)
            : this(Guid.NewGuid().ToString(), id) => Id = id;

        public RegistrarTaxaInCacheCommand(string requestId, string id) => (RequestId, Id) = (requestId, id);

        public string Id { get; }
        public string RequestId { get; }
    }
}