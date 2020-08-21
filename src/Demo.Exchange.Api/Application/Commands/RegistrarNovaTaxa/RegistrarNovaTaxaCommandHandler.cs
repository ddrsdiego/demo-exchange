namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Extensions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RegistrarNovaTaxaCommandHandler : Handler, IRequestHandler<RegistrarNovaTaxaCommand, RegistrarNovaTaxaResponse>
    {
        private TaxaCobranca TaxaCobranca;
        private TipoSegmento TipoSegmento;

        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public RegistrarNovaTaxaCommandHandler(IMediator mediator, ILoggerFactory logger, ITaxaCobrancaRepository taxaCobrancaRepository)
            : base(mediator, logger.CreateLogger<RegistrarNovaTaxaCommandHandler>())
        {
            _taxaCobrancaRepository = taxaCobrancaRepository;
        }

        public async Task<RegistrarNovaTaxaResponse> Handle(RegistrarNovaTaxaCommand request, CancellationToken cancellationToken)
        {
            var response = (RegistrarNovaTaxaResponse)request.Response;

            RegistrarNovaTaxaCommandValidator.ValidarCommand(request, response);
            if (response.IsFailure)
                return response;

            var valorTaxaCobranca = ValorTaxaCobranca.Create(request.ValorTaxa);
            if (valorTaxaCobranca.IsFailure)
            {
                response.AddError(Errors.General.InvalidCommandArguments()
                                                .AddErroDetail(Errors.General.InvalidArgument("ValorTaxaInvalido", string.Join("|", valorTaxaCobranca.Messages))));
                return response;
            }

            ObterTipoSegmentoPorId(request, response);
            if (response.IsFailure)
                return response;

            await VerficarSegmentoJaRegistrado(request, response);
            if (response.IsFailure)
                return response;

            TaxaCobranca = new TaxaCobranca(Guid.NewGuid().ToString(), valorTaxaCobranca.Value, TipoSegmento);

            await Registrar(request, response);
            if (response.IsFailure)
                return response;

            await Mediator.DispatchDomainEvents(TaxaCobranca);

            response.SetPayLoad(TaxaCobranca.ConverterEntidadeParaResponse());

            return response;
        }

        private void ObterTipoSegmentoPorId(RegistrarNovaTaxaCommand request, RegistrarNovaTaxaResponse response)
        {
            TipoSegmento = TipoSegmento.ObterPorIdFor(request.Segmento);
            if (TipoSegmento is null)
            {
                response.AddError(Errors.General.NotFound(nameof(TipoSegmento), request.Segmento));
                Logger.LogWarning($"{response.ErrorResponse}");
            }
        }

        private async Task VerficarSegmentoJaRegistrado(RegistrarNovaTaxaCommand request, RegistrarNovaTaxaResponse response)
        {
            try
            {
                TaxaCobranca = await _taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento(TipoSegmento.Id);
                if (!string.IsNullOrEmpty(TaxaCobranca.TaxaCobrancaId))
                {
                    response.AddError(Errors.RegistrarNovaTaxaErros.TaxaParaSegmentoJaRegistrada(TipoSegmento.Id));
                    return;
                }
            }
            catch (Exception ex)
            {
                response.AddError(Errors.General.InternalProcessError("VerficarSegmentoJaRegistrado", ex.Message));
                return;
            }
        }

        private async Task Registrar(RegistrarNovaTaxaCommand request, RegistrarNovaTaxaResponse response)
        {
            try
            {
                await _taxaCobrancaRepository.Registrar(TaxaCobranca);
            }
            catch (Exception ex)
            {
                response.AddError(Errors.General.InternalProcessError("VerficarSegmentoJaRegistrado", ex.Message));
                return;
            }
        }
    }
}