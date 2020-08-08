namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Running;
    using System;

    public static class Program
    {
        public static void Main(string[] args)
        {
            //ClienteClass clienteClass1 = new ClienteClass(22);
            //ClienteClass clienteClass2 = clienteClass1;

            //clienteClass2.Valor = 82;
            //Console.WriteLine($"Valor da {nameof(clienteClass1)}: {clienteClass1.Valor}");

            //ClienteStruct clienteStruct1 = new ClienteStruct(22);
            //ClienteStruct clienteStruct2 = clienteStruct1;

            //clienteStruct2.Valor = 82;
            //Console.WriteLine($"Valor da {nameof(clienteStruct1)}: {clienteStruct1.Valor}");

            //Console.ReadLine();

            BenchmarkRunner.Run<Benchmark>();
        }
    }

    public class ClienteClass
    {
        public ClienteClass(int valor) => Valor = valor;

        public int Valor { get; set; }
    }

    public struct ClienteStruct
    {
        public ClienteStruct(int valor) => Valor = valor;

        public int Valor { get; set; }
    }

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class Benchmark
    {
        private static readonly BenchmarkHelper benchmarkHelper = new BenchmarkHelper();

        [Benchmark(Baseline = true)]
        public void CriarClienteClass()
        {
            benchmarkHelper.CriarClienteClass(1);
        }

        [Benchmark()]
        public void CriarClienteStruct()
        {
            benchmarkHelper.CriarClienteStruct(1);
        }
    }
}