namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Microsoft.Extensions.DependencyInjection;
    using System.Diagnostics;
    using System.Threading.Tasks;

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(targetCount: 1000)]
    public class TaxaCobrancaRepositoryBenchmarks
    {
        private readonly ITaxaCobrancaRepository _taxaCobrancaRepository;

        public TaxaCobrancaRepositoryBenchmarks()
        {
            var serviceProvider = ContainerConfiguration.Configure();
            _taxaCobrancaRepository = serviceProvider.GetService<ITaxaCobrancaRepository>();
        }

        [Benchmark(Baseline = true)]

        public async Task ObterTaxaCobrancaPorSegmento()
        {
            var result = await _taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento("VAREJO");
            Debug.WriteLine(result.TaxaCobrancaId);
        }

        [Benchmark]

        public async Task ObterTaxaCobrancaPorSegmentoClass()
        {
            var result = await _taxaCobrancaRepository.ObterTaxaCobrancaPorSegmentoClass("VAREJO");
            Debug.WriteLine(result.TaxaCobrancaId);
        }
    }
}