namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using System.Diagnostics;

    [RankColumn]
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class TipoSegmentoBenchmarks
    {
        [Benchmark(Baseline = true)]
        public void ObterPorId_SOLUCAO_1()
        {
            var tipoSegmento = TipoSegmento.ObterPorId("VAREJO");
            Debug.WriteLine(tipoSegmento);
        }

        [Benchmark]
        public void ObterPorId_SOLUCAO_3()
        {
            var tipoSegmento = TipoSegmento.ObterPorIdFromArray("VAREJO");
            Debug.WriteLine(tipoSegmento);
        }

        [Benchmark]
        public void ObterPorId_SOLUCAO_4()
        {
            var tipoSegmento = TipoSegmento.ObterPorIdFor("private");
            Debug.WriteLine(tipoSegmento);
        }

        [Benchmark]
        public void ObterPorId_SOLUCAO_5()
        {
            var tipoSegmento = TipoSegmento.ObterPorIdIf("private");
            Debug.WriteLine(tipoSegmento);
        }

        [Benchmark]
        public void ObterPorId_SOLUCAO_2()
        {
            var tipoSegmento = TipoSegmento.ObterPorIdForSemInvariantCultureIgnoreCase("private");
            Debug.WriteLine(tipoSegmento);
        }
    }
}