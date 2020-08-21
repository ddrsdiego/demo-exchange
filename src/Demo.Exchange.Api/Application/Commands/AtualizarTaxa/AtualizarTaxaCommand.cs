namespace Demo.Exchange.Application.Commands.AtualizarTaxa
{
    using MediatR;

    public class AtualizarTaxaCommand : Request, IRequest<AtualizarTaxaResponse>
    {
        public AtualizarTaxaCommand(string id, decimal novaTaxa) => (Id, NovaTaxa) = (id, novaTaxa);

        public string Id { get; }
        public decimal NovaTaxa { get; }

        public override Response Response => new AtualizarTaxaResponse(RequestId);
    }

    public struct AtualizarTaxaRequest
    {
        public decimal NovaTaxa { get; set; }
    }
}