namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Demo.Exchange.Application.Queries.ObterCotacaoPorMoeda;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System.Diagnostics;
    using System.Threading.Tasks;

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class ObterCotacaoPorMoedaQueryBenchmarks
    {
        private readonly IMediator _mediator;

        public ObterCotacaoPorMoedaQueryBenchmarks()
        {
            var serviceProvider = ContainerConfiguration.Configure();
            _mediator = serviceProvider.GetService<IMediator>();
        }

        [Benchmark(Baseline = true)]
        public async Task ObterCotacaoPorMoedaQueryClass()
        {
            var response = await _mediator.Send(new ObterCotacaoPorMoedaQuery("VAREJO", "USD", 1));
            Debug.WriteLine(response);
        }
    }
}