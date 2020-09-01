namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using FluentValidation;

    public sealed class RegistrarNovaTaxaCommandValidator : AbstractValidator<RegistrarNovaTaxaCommand>
    {
        private RegistrarNovaTaxaCommandValidator()
        {
            RuleFor(x => x.Segmento)
                .NotEmpty()
                .WithErrorCode("TipoSegmentoInvalido")
                .WithMessage("Segemento não contém um valo válido entro Varejo, Personnalite, Private");

            RuleFor(x => x.ValorTaxa)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode("ValorTaxaInvalido")
                .WithMessage("Valor Taxa não deve ser menor que zero.");
        }

        public static Response ValidarCommand(RegistrarNovaTaxaCommand request)
        {
            var validator = new RegistrarNovaTaxaCommandValidator();

            var resultadoValidacao = validator.Validate(request);
            if (resultadoValidacao.IsValid)
                return Response.Ok();

            var invalidCommandArguments = Errors.General.InvalidCommandArguments();

            foreach (var erro in resultadoValidacao.Errors)
            {
                invalidCommandArguments.AddErroDetail(Errors.General.InvalidArgument(erro.ErrorCode, erro.ErrorMessage));
            }

            return Response.Fail(invalidCommandArguments);
        }
    }
}