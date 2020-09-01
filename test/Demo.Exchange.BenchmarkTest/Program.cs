namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using BenchmarkDotNet.Running;
    using Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public static class Program
    {
        private const int COUNT = 1_000_000;
        private const int GEN_0 = 0;
        private const int GEN_1 = 1;
        private const int GEN_2 = 2;
        //public static List<Person> People = new List<Person>();

        private static void Main()
        {
            var serviceProvider = ContainerConfiguration.Configure();
            var mediator = serviceProvider.GetService<IMediator>();

            //var benchmarks = new ObterTaxaCobrancaPorSegmentoBenchmarks();
            //benchmarks.GlobalSetup();

            //GC.Collect();

            var before0 = GC.CollectionCount(GEN_0);
            var before1 = GC.CollectionCount(GEN_1);
            var before2 = GC.CollectionCount(GEN_2);

            var sw = Stopwatch.StartNew();

            var tasks = Enumerable.Range(0, COUNT).Select(async _ =>
            {
                var response = await mediator.Send(new ObterTaxaCobrancaPorSegmentoQuery("varejo"));
                Debug.WriteLine(response.Content.Value);
            }).ToArray();

            Task.WhenAll(tasks).GetAwaiter();

            sw.Stop();

            Console.WriteLine($"\nTime: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"#Gen 0 Time: {GC.CollectionCount(GEN_0) - before0}");
            Console.WriteLine($"#Gen 1 Time: {GC.CollectionCount(GEN_1) - before1}");
            Console.WriteLine($"#Gen 2 Time: {GC.CollectionCount(GEN_2) - before2}");
            Console.WriteLine($"Memory: {Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024} mb");

            //Console.ReadLine();

            //var enemy = new Enemy(new Point(10, 10));

            //var location = enemy.GetLocation();
            //location.X = 12;

            //Console.WriteLine($"Valor da {nameof(location)}: {location.X}");
            //Console.WriteLine($"Valor da {nameof(enemy)}: {enemy.GetLocation().X}");

            //var locationByRef = enemy.GetLocationByRef();
            //locationByRef.X = 12;

            //Console.WriteLine($"Valor da {nameof(locationByRef)}: {locationByRef.X}");
            //Console.WriteLine($"Valor da {nameof(enemy)}: {enemy.GetLocationByRef().X}");

            //ref var copy = ref enemy.GetLocationByRef();
            //copy.X = 12;

            //Console.WriteLine($"Valor da {nameof(copy)}: {copy.X}");
            //Console.WriteLine($"Valor da {nameof(enemy)}: {enemy.GetLocationByRef().X}");

            //ClienteClass clienteClass1 = new ClienteClass(22);
            //ClienteClass clienteClass2 = clienteClass1;

            //clienteClass2.Valor = 82;
            //Console.WriteLine($"Valor da {nameof(clienteClass1)}: {clienteClass1.Valor}");

            //ClienteStruct clienteStruct1 = new ClienteStruct(22);
            //ClienteStruct clienteStruct2 = clienteStruct1;

            //clienteStruct2.Valor = 82;
            //Console.WriteLine($"Valor da {nameof(clienteStruct1)}: {clienteStruct1.Valor}");

            //Console.ReadLine();

            //CacheBenchmarks
            //var summary = BenchmarkRunner.Run<Benchmark>();
            //var summary = BenchmarkRunner.Run<CacheBenchmarks>();
            //var summary = BenchmarkRunner.Run<ObterTaxaCobrancaPorSegmentoBenchmarks>();
            //var summary = BenchmarkRunner.Run<ObterCotacaoPorMoedaQueryBenchmarks>();
            //var summary = BenchmarkRunner.Run<TaxaCobrancaRepositoryBenchmarks>();
            //var summary = BenchmarkRunner.Run<ConditionBenchmarks>();
            //var summary = BenchmarkRunner.Run<IntroSetupCleanupIteration>();
        }

        //private static void CreatePerson() => People.Add(new Person("Diego Dias", 38));
    }

    public class Person
    {
        public Person(string name, int age) => (Name, Age) = (name, age);

        public int Age { get; }
        public string Name { get; }
    }

    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 2, targetCount: 3)]
    public class IntroSetupCleanupIteration
    {
        private int setupCounter;
        private int cleanupCounter;

        [IterationSetup]
        public void IterationSetup() => Console.WriteLine($"// IterationSetup ({++setupCounter})");

        [IterationCleanup]
        public void IterationCleanup() => Console.WriteLine($"// IterationCleanup ({++cleanupCounter})");

        [GlobalSetup]
        public void GlobalSetup() => Console.WriteLine("// " + "GlobalSetup");

        [GlobalCleanup]
        public void GlobalCleanup() => Console.WriteLine("// " + "GlobalCleanup");

        [Benchmark]
        public void Benchmark() => Console.WriteLine("// " + "Benchmark");
    }

    public class ClienteClass
    {
        protected ClienteClass()
        {
        }

        public ClienteClass(int valor) => Valor = valor;

        public int Valor { get; set; }
    }

    public readonly struct ClienteStruct
    {
        public ClienteStruct(int valor) => Valor = valor;

        public int Valor { get; }
    }

    public struct Point
    {
        public Point(float x, float y) => (X, Y) = (x, y);

        public float Y { get; set; }
        public float X { get; set; }
    }

    public class Enemy
    {
        private Point _location;

        public Enemy(Point location) => _location = location;

        public Point GetLocation() => _location;

        public ref Point GetLocationByRef() => ref _location;
    }
}