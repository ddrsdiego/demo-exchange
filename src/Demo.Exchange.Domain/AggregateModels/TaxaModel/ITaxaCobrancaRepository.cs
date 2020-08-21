namespace Demo.Exchange.Domain.AggregateModel.TaxaModel
{
    using Demo.Exchange.Domain.SeedWorks;
    using System.Threading.Tasks;

    public interface ITaxaCobrancaRepository : IRepository<TaxaCobranca>
    {
        Task Registrar(TaxaCobranca taxaCobranca);

        Task<TaxaCobranca> ObterPorId(string id);

        Task<TaxaCobranca> ObterTaxaCobrancaPorSegmento(string segmento);
        Task<TaxaCobranca> ObterTaxaCobrancaPorSegmentoClass(string segmento);

        Task Atualizar(TaxaCobranca taxaCobranca);
    }
}