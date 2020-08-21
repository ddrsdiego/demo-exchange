namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    using Demo.Exchange.Domain.SeedWorks;

    public abstract class ComputeFormula
    {
        public ComputeFormula(ParametroInput parametroInput)
        {
            ParametroInput = parametroInput;
        }

        public ParametroInput ParametroInput { get; }

        private ICadernoFormulasService _cadernoFormulasService;
        protected ICadernoFormulasService CadernoFormulasService
        {
            get
            {
                if (_cadernoFormulasService is null)
                    _cadernoFormulasService = new CadernoFormulasService();
                return _cadernoFormulasService;
            }
        }

        public virtual Result<ParametroOutput> Calcular()
        {
            var resultadoValidacaoFormula = ValidarFormula();
            if (resultadoValidacaoFormula.IsFailure)
                return Result<ParametroOutput>.Fail(resultadoValidacaoFormula.Messages);

            return ExecuteCompute(resultadoValidacaoFormula.Value);
        }

        protected virtual Result<ParametroInput> ValidarFormula()
        {
            if (ParametroInput is null)
                return Result<ParametroInput>.Fail("InputParameter esta inválido.");

            return Result<ParametroInput>.Ok(ParametroInput);
        }

        protected abstract Result<ParametroOutput> ExecuteCompute(ParametroInput parametroInput);
    }
}