namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    using Demo.Exchange.Domain.SeedWorks;
    using System;

    public class ValorConversaoFormula : ComputeFormula
    {
        public ValorConversaoFormula(ParametroInput parametroInput)
            : base(parametroInput)
        {
        }

        protected override Result<ParametroOutput> ExecuteCompute(ParametroInput parametroInput)
        {
            var input = (ValorConversaoParametroInput)parametroInput;

            var valorConversao = input.QuantidadeDesejada * input.TaxaConversao;

            return Result<ParametroOutput>.Ok(new ValorConversaoParametroOutput(Math.Round(valorConversao, 2)));
        }

        protected override Result<ParametroInput> ValidarFormula()
        {
            var resultoBase = base.ValidarFormula();
            if (resultoBase.IsFailure)
                return resultoBase;

            var parametro = (ValorConversaoParametroInput)ParametroInput;

            if (parametro.QuantidadeDesejada <= 0)
                return Result<ParametroInput>.Fail("Quantidade desejada não deve ser menor ou igual a zero.");

            return Result<ParametroInput>.Ok(ParametroInput);
        }
    }
}