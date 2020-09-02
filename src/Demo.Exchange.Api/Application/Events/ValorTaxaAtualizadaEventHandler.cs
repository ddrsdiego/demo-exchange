namespace Demo.Exchange.Application.Events
{
    using Demo.Exchange.Application.Commands.RegistrarTaxaInCache;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class ValorTaxaAtualizadaEventHandler : Handler, INotificationHandler<ValorTaxaAtualizadaEvent>
    {
        public ValorTaxaAtualizadaEventHandler(IMediator mediator, ILoggerFactory logger)
            : base(mediator, logger.CreateLogger<ValorTaxaAtualizadaEventHandler>())
        {
        }

        public async Task Handle(ValorTaxaAtualizadaEvent notification, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(notification.ConverteEventoParaCommand());
            if (response.IsFailure)
                Logger.LogWarning("@{ErrorResponse}", response.ErrorResponse);
        }
    }

    internal static class ValorTaxaAtualizadaEventEx
    {
        public static RegistrarTaxaInCacheCommand ConverteEventoParaCommand(this ValorTaxaAtualizadaEvent taxaAtualizadaEvent)
            => new RegistrarTaxaInCacheCommand(taxaAtualizadaEvent.RequestId, taxaAtualizadaEvent.Id);
    }
}