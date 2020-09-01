namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Extensions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class RegistrarNovaTaxaCommandHandler : Handler, IRequestHandler<RegistrarNovaTaxaCommand, Response>
    {
        private TaxaCobranca TaxaCobranca;
        private TipoSegmento TipoSegmento;

        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public RegistrarNovaTaxaCommandHandler(IMediator mediator, ILoggerFactory logger, ITaxaCobrancaRepository taxaCobrancaRepository)
            : base(mediator, logger.CreateLogger<RegistrarNovaTaxaCommandHandler>())
        {
            _taxaCobrancaRepository = taxaCobrancaRepository;
        }

        public async Task<Response> Handle(RegistrarNovaTaxaCommand request, CancellationToken cancellationToken)
        {
            Response response = RegistrarNovaTaxaCommandValidator.ValidarCommand(request);
            if (response.IsFailure)
                return response;

            var valorTaxaCobranca = ValorTaxaCobranca.Create(request.ValorTaxa);
            if (valorTaxaCobranca.IsFailure)
            {
                return Response.Fail(Errors.General.InvalidCommandArguments()
                                            .AddErroDetail(Errors.General.InvalidArgument("ValorTaxaInvalido", string.Join("|", valorTaxaCobranca.Messages))));
            }

            response = ObterTipoSegmentoPorId(request);
            if (response.IsFailure)
                return response;

            response = await VerficarSegmentoJaRegistrado();
            if (response.IsFailure)
                return response;

            TaxaCobranca = new TaxaCobranca(Guid.NewGuid().ToString(), valorTaxaCobranca.Value, TipoSegmento);

            response = await Registrar();
            if (response.IsFailure)
                return response;

            await Mediator.DispatchDomainEvents(TaxaCobranca);

            return Response.Ok(ResponseContent.Create(TaxaCobranca.ConverterEntidadeParaResponse()));
        }

        private Response ObterTipoSegmentoPorId(RegistrarNovaTaxaCommand request)
        {
            var response = Response.Ok();

            TipoSegmento = TipoSegmento.ObterPorIdFor(request.Segmento);
            if (TipoSegmento is null)
            {
                response = Response.Fail(Errors.General.NotFound(nameof(TipoSegmento), request.Segmento));
                Logger.LogWarning($"{response.ErrorResponse}");

                return response;
            }

            return response;
        }

        private async Task<Response> VerficarSegmentoJaRegistrado()
        {
            try
            {
                TaxaCobranca = await _taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento(TipoSegmento.Id);
                if (!string.IsNullOrEmpty(TaxaCobranca.TaxaCobrancaId))
                    return Response.Fail(Errors.RegistrarNovaTaxaErros.TaxaParaSegmentoJaRegistrada(TipoSegmento.Id));
            }
            catch (Exception ex)
            {
                return Response.Fail(Errors.General.InternalProcessError("VerficarSegmentoJaRegistrado", ex.Message));
            }

            return Response.Ok();
        }

        private async Task<Response> Registrar()
        {
            try
            {
                await _taxaCobrancaRepository.Registrar(TaxaCobranca);
            }
            catch (Exception ex)
            {
                return Response.Fail(Errors.General.InternalProcessError("VerficarSegmentoJaRegistrado", ex.Message));
            }

            return Response.Ok();
        }
    }
}