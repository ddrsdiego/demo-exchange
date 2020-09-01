namespace Demo.Exchange.Application.Commands.AtualizarTaxa
{
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.Infra.Extensions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AtualizarTaxaCommandHandler : Handler, IRequestHandler<AtualizarTaxaCommand, Response>
    {
        private TaxaCobranca TaxaCobranca;
        private ValorTaxaCobranca ValorTaxaCobranca;

        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public AtualizarTaxaCommandHandler(IMediator mediator, ILoggerFactory logger, ITaxaCobrancaRepository taxaCobrancaRepository)
            : base(mediator, logger.CreateLogger<AtualizarTaxaCommandHandler>())
        {
            _taxaCobrancaRepository = taxaCobrancaRepository;
        }

        public async Task<Response> Handle(AtualizarTaxaCommand request, CancellationToken cancellationToken)
        {
            Response response = AtualizarTaxaCommandValidator.ValidarCommand(request);
            if (response.IsFailure)
                return response;

            response = await ObterTaxaPorId(request);
            if (response.IsFailure)
                return response;

            response = CriarNovoValorTaxa(request);
            if (response.IsFailure)
                return response;

            var atualizarTaxaResultado = TaxaCobranca.AtualizarTaxa(ValorTaxaCobranca);
            if (atualizarTaxaResultado.IsFailure)
            {
                response = Response.Fail(Errors.General
                                                .InvalidCommandArguments()
                                                .AddErroDetail(Errors.AtualizarTaxaErros.ValorTaxaSemAlteracao(TaxaCobranca.ValorTaxa.Valor, request.NovaTaxa)));
                return response;
            }

            response = await AtualizarTaxaCobranca();
            if (response.IsFailure)
                return response;

            await Mediator.DispatchDomainEvents(TaxaCobranca);

            return response;
        }

        private Response CriarNovoValorTaxa(AtualizarTaxaCommand request)
        {
            var response = Response.Ok();

            var novoValorCobranca = ValorTaxaCobranca.Create(request.NovaTaxa);
            if (novoValorCobranca.IsFailure)
            {
                response = Response.Fail(Errors.General
                                                .InvalidCommandArguments()
                                                .AddErroDetail(Errors.General.InvalidArgument("ValorNovaTaxaInvalido", string.Join("|", novoValorCobranca.Messages))));
                Logger.LogWarning($"{response.ErrorResponse}");

                return response;
            }

            ValorTaxaCobranca = novoValorCobranca.Value;

            return response;
        }

        private async Task<Response> ObterTaxaPorId(AtualizarTaxaCommand request)
        {
            var response = Response.Ok();
            try
            {
                TaxaCobranca = await _taxaCobrancaRepository.ObterPorId(request.Id);
                if (string.IsNullOrEmpty(TaxaCobranca.TaxaCobrancaId))
                {
                    response = Response.Fail(Errors.General.NotFound(nameof(TaxaCobranca), request.Id));
                    Logger.LogWarning($"{response.ErrorResponse}");

                    return response;
                }
            }
            catch (Exception ex)
            {
                response = Response.Fail(Errors.General.InternalProcessError("ObterTaxaPorId", ex.Message));
                Logger.LogError(ex, $"{response.ErrorResponse}");

                return response;
            }

            return response;
        }

        private async Task<Response> AtualizarTaxaCobranca()
        {
            var response = Response.Ok();

            try
            {
                await _taxaCobrancaRepository.Atualizar(TaxaCobranca);
            }
            catch (Exception ex)
            {
                response = Response.Fail(Errors.General.InternalProcessError("AtualizarTaxaCobranca", ex.Message));
                Logger.LogError(ex, $"{response.ErrorResponse}");

                return response;
            }

            return response;
        }
    }
}