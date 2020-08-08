``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-10510U CPU 1.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.302
  [Host]     : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
  DefaultJob : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT


```
|             Method |      Mean |     Error |    StdDev | Ratio | RatioSD | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------- |----------:|----------:|----------:|------:|--------:|-----:|-------:|------:|------:|----------:|
| CriarClienteStruct | 0.3010 ns | 0.0384 ns | 0.0539 ns |  0.08 |    0.02 |    1 |      - |     - |     - |         - |
|  CriarClienteClass | 3.6923 ns | 0.1055 ns | 0.2723 ns |  1.00 |    0.00 |    2 | 0.0057 |     - |     - |      24 B |
