namespace Demo.Exchange.Application.Queries.ObterCotacaoPorMoeda
{
    using FluentValidation;

    public sealed class ObterCotacaoPorMoedaQueryValidator : AbstractValidator<ObterCotacaoPorMoedaQuery>
    {
        private ObterCotacaoPorMoedaQueryValidator()
        {
            RuleFor(x => x.Segmento)
                .NotEmpty()
                .WithErrorCode("TipoSegmentoInvalido")
                .WithMessage("Segemento não contém um valo válido entro Varejo, Personnalite, Private");

            RuleFor(x => x.Quantidade)
                .GreaterThanOrEqualTo(1)
                .WithErrorCode("QuantidadeDesejadaInvalida")
                .WithMessage("Quantidade desejada para conversão deve ser maior que zero.");

            RuleFor(x => x.Moeda)
                .NotEmpty()
                .WithErrorCode("MoedaConversaoInvalida")
                .WithMessage("Moeda desejada para conversão não deve ser nulo ou vazio.");
        }

        public static Response ValidarQuery(ObterCotacaoPorMoedaQuery request)
        {
            var validador = new ObterCotacaoPorMoedaQueryValidator();
            var resultado = validador.Validate(request);

            if (resultado.IsValid)
                return Response.Ok();

            var invalidQueryParameters = Errors.General.InvalidQueryParameters();
            foreach (var failure in resultado.Errors)
                invalidQueryParameters.AddErroDetail(Errors.General.InvalidArgument(failure.ErrorCode, failure.ErrorMessage));

            return Response.Fail(invalidQueryParameters);
        }
    }
}