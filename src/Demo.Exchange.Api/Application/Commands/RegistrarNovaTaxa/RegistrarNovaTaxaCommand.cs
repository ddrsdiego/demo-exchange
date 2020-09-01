namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using MediatR;
    using System;

    public readonly struct RegistrarNovaTaxaCommand : IRequest<Response>
    {
        public RegistrarNovaTaxaCommand(string segmento, decimal valorTaxa)
            : this(Guid.NewGuid().ToString(), segmento, valorTaxa)
        {
            Segmento = segmento;
            ValorTaxa = valorTaxa;
        }

        public RegistrarNovaTaxaCommand(string requestId, string segmento, decimal valorTaxa) =>
            (RequestId, Segmento, ValorTaxa) = (requestId, segmento, valorTaxa);

        public string Segmento { get; }
        public decimal ValorTaxa { get; }
        public string RequestId { get; }
    }

    public struct RegistrarNovaTaxaRequest
    {
        public RegistrarNovaTaxaRequest(string segmento, decimal valorTaxa) => (Segmento, ValorTaxa) = (segmento, valorTaxa);

        public string Segmento { get; set; }
        public decimal ValorTaxa { get; set; }
    }
}