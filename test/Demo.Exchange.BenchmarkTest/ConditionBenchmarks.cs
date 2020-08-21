namespace Demo.Exchange.BenchmarkTest
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;
    using Demo.Exchange.Domain.SeedWorks;
    using System.Collections.Generic;
    using System.Diagnostics;

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class ConditionBenchmarks
    {
        [Benchmark(Baseline = true)]
        [Arguments(10, 10, 10, 10)]
        [Arguments(10, 10, 10, 20)]
        public void ConditionStruct(decimal conditionPrice1, decimal conditionQuantity1, decimal conditionPrice2, decimal conditionQuantity2)
        {
            var condition1 = new ConditionStruct(conditionPrice1, conditionQuantity1);
            var condition2 = new ConditionStruct(conditionPrice2, conditionQuantity2);

            Debug.WriteLine(condition1.Equals(condition2));
        }

        [Benchmark]
        [Arguments(10, 10, 10, 10)]
        [Arguments(10, 10, 10, 20)]
        public void ConditionClass(decimal conditionPrice1, decimal conditionQuantity1, decimal conditionPrice2, decimal conditionQuantity2)
        {
            var condition1 = new ConditionClass(conditionPrice1, conditionQuantity1);
            var condition2 = new ConditionClass(conditionPrice2, conditionQuantity2);

            Debug.WriteLine(condition1.Equals(condition2));
        }
    }

    public readonly struct ConditionStruct
    {
        public ConditionStruct(decimal price, decimal quantity) => (Quantity, Price) = (price, quantity);

        public decimal Quantity { get; }
        public decimal Price { get; }
    }

    public class ConditionClass : ValueObject
    {
        public ConditionClass(decimal price, decimal quantity) => (Quantity, Price) = (price, quantity);

        public decimal Quantity { get; }
        public decimal Price { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Price;
            yield return Quantity;
        }
    }
}