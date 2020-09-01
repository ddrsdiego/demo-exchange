``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.450 (2004/?/20H1)
Intel Core i7-10510U CPU 1.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host]     : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT
  Job-RQCFME : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT

InvocationCount=1  UnrollFactor=1  

```
|                                 Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD | Rank | Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------------------------- |---------:|----------:|----------:|---------:|------:|--------:|-----:|------:|------:|------:|----------:|
| ObterTaxaCobrancaPorSegmentoQueryStrig | 1.744 ms | 0.1802 ms | 0.5171 ms | 1.514 ms |  1.03 |    0.42 |    1 |     - |     - |     - |  32.12 KB |
|      ObterTaxaCobrancaPorSegmentoQuery | 1.842 ms | 0.2127 ms | 0.6034 ms | 1.666 ms |  1.00 |    0.00 |    1 |     - |     - |     - |  14.63 KB |
|  ObterTaxaCobrancaPorSegmentoQueryByte | 1.857 ms | 0.1893 ms | 0.5463 ms | 1.627 ms |  1.14 |    0.51 |    1 |     - |     - |     - |  15.15 KB |
