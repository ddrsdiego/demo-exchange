namespace Demo.Exchange.Application.Events
{
    using Demo.Exchange.Application.Commands.RegistrarTaxaInCache;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class NovaTaxaRegistradaEventHandler : Handler, INotificationHandler<NovaTaxaRegistradaEvent>
    {
        public NovaTaxaRegistradaEventHandler(IMediator mediator, ILoggerFactory logger)
            : base(mediator, logger.CreateLogger<NovaTaxaRegistradaEventHandler>())
        {
        }

        public async Task Handle(NovaTaxaRegistradaEvent notification, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(notification.ConverteEventoParaCommand());
            if (response.IsFailure)
                Logger.LogWarning("@{ErrorResponse}", response.ErrorResponse);
        }
    }

    internal static class NovaTaxaRegistradaEventEx
    {
        public static RegistrarTaxaInCacheCommand ConverteEventoParaCommand(this NovaTaxaRegistradaEvent notification)
            => new RegistrarTaxaInCacheCommand(notification.RequestId, notification.TaxaCobranca.TaxaCobrancaId);
    }
}