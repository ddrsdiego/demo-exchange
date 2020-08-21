namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using MediatR;

    public class RegistrarNovaTaxaCommand : Request, IRequest<RegistrarNovaTaxaResponse>
    {
        public RegistrarNovaTaxaCommand(string segmento, decimal valorTaxa)
        {
            Segmento = segmento;
            ValorTaxa = valorTaxa;
        }

        public string Segmento { get; }
        public decimal ValorTaxa { get; }

        public override Response Response => new RegistrarNovaTaxaResponse(RequestId);
    }

    public struct RegistrarNovaTaxaRequest
    {
        public RegistrarNovaTaxaRequest(string segmento, decimal valorTaxa) => (Segmento, ValorTaxa) = (segmento, valorTaxa);

        public string Segmento { get; set; }
        public decimal ValorTaxa { get; set; }
    }
}