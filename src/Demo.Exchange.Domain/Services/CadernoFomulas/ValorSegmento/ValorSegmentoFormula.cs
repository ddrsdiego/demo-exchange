namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    using Demo.Exchange.Domain.SeedWorks;
    using System;

    public class ValorSegmentoFormula : ComputeFormula
    {
        private const int MULTIPLICADOR = 1;

        public ValorSegmentoFormula(ParametroInput parametroInput)
            : base(parametroInput)
        {
        }

        protected override Result<ParametroOutput> ExecuteCompute(ParametroInput parametroInput)
        {
            var input = (ValorSegmentoParametroInput)parametroInput;

            var valorSegmento = input.TaxaSegmento == 0 ? MULTIPLICADOR : MULTIPLICADOR * input.TaxaSegmento;

            return Result<ParametroOutput>.Ok(new ValorConversaoParametroOutput(Math.Round(valorSegmento, 2)));
        }
    }
}