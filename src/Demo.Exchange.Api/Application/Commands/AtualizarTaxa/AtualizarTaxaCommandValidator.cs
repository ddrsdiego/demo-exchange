namespace Demo.Exchange.Application.Commands.AtualizarTaxa
{
    using FluentValidation;

    public sealed class AtualizarTaxaCommandValidator : AbstractValidator<AtualizarTaxaCommand>
    {
        private AtualizarTaxaCommandValidator()
        {
            RuleFor(x => x.NovaTaxa)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode("NovaTaxaInvalido")
                .WithMessage("Valor Nova Taxa não deve ser menor que zero.");
        }

        public static Response ValidarCommand(AtualizarTaxaCommand request)
        {
            var validator = new AtualizarTaxaCommandValidator();

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid)
                return Response.Ok();

            var invalidCommandArguments = Errors.General.InvalidCommandArguments();

            for (int i = 0; i < resultadoValidacao.Errors.Count; i++)
            {
                var erro = resultadoValidacao.Errors[i];
                invalidCommandArguments.AddErroDetail(Errors.General.InvalidArgument(erro.ErrorCode, erro.ErrorMessage));
            }

            return Response.Fail(invalidCommandArguments);
        }
    }
}