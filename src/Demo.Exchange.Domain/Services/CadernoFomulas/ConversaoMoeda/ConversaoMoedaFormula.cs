namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    using Demo.Exchange.Domain.SeedWorks;
    using System;

    public class ConversaoMoedaFormula : ComputeFormula
    {
        public ConversaoMoedaFormula(ParametroInput parametroInput)
            : base(parametroInput)
        {
        }

        protected override Result<ParametroOutput> ExecuteCompute(ParametroInput parametroInput)
        {
            var input = (ConversaoMoedaParametroInput)parametroInput;

            var valorConversao = CadernoFormulasService.Compute(new ValorConversaoFormula(new ValorConversaoParametroInput(input.QuantidadeDesejada, input.TaxaConversao)));
            if (valorConversao.IsFailure)
                return Result<ParametroOutput>.Fail(valorConversao.Messages);

            var valorSegmento = CadernoFormulasService.Compute(new ValorSegmentoFormula(new ValorSegmentoParametroInput(input.TaxaSegmento)));
            if (valorSegmento.IsFailure)
                return Result<ParametroOutput>.Fail(valorConversao.Messages);

            var resultado = valorConversao.Value.Valor * valorSegmento.Value.Valor;

            return Result<ParametroOutput>.Ok(new ConversaoMoedaParametroOutput(Math.Round(resultado, 2)));
        }

        protected override Result<ParametroInput> ValidarFormula()
        {
            var resultadoValidarFormula = base.ValidarFormula();
            if (resultadoValidarFormula.IsFailure)
                return Result<ParametroInput>.Fail(resultadoValidarFormula.Messages);

            if (ParametroInput.GetType() != typeof(ConversaoMoedaParametroInput))
                return Result<ParametroInput>.Fail($"{nameof(ParametroInput)} não é do tipo {nameof(ConversaoMoedaParametroInput)}");

            return Result<ParametroInput>.Ok(ParametroInput);
        }
    }
}