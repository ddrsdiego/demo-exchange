namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    using Demo.Exchange.Domain.SeedWorks;

    public interface ICadernoFormulasService
    {
        Result<ParametroOutput> Compute(ComputeFormula computeFormula);
    }

    public class CadernoFormulasService : ICadernoFormulasService
    {
        public Result<ParametroOutput> Compute(ComputeFormula computeFormula) => computeFormula.Calcular();
    }
}