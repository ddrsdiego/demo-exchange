namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class ObterTaxaCobrancaPorSegmentoBenchmarks
    {
        private IMediator mediator;
        private IServiceProvider serviceProvider;

        [GlobalSetup]
        public void GlobalSetup() => serviceProvider = ContainerConfiguration.Configure();

        [IterationSetup]
        public void IterationSetup() => mediator = serviceProvider.GetService<IMediator>();

        [IterationCleanup]
        public void IterationCleanup() => mediator = null;

        [Benchmark(Baseline = true)]
        public async Task ObterTaxaCobrancaPorSegmentoQuery()
        {
            var response = await mediator.Send(new ObterTaxaCobrancaPorSegmentoQuery("varejo"));
            Debug.WriteLine(response.Content.ValueAsJsonString);
        }

        [Benchmark]
        public async Task ObterTaxaCobrancaPorSegmentoQueryStrig()
        {
            var response = await mediator.Send(new ObterTaxaCobrancaPorSegmentoQuery("varejo"));
            Debug.WriteLine(response.Content.ValueAsJsonString);
        }

        [Benchmark]
        public async Task ObterTaxaCobrancaPorSegmentoQueryByte()
        {
            var response = await mediator.Send(new ObterTaxaCobrancaPorSegmentoQuery("varejo"));
            Debug.WriteLine(response.Content.Value);
        }
    }
}