``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.450 (2004/?/20H1)
Intel Core i7-10510U CPU 1.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.401
  [Host]     : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT
  DefaultJob : .NET Core 3.1.7 (CoreCLR 4.700.20.36602, CoreFX 4.700.20.37001), X64 RyuJIT


```
|                         Method |     Mean |   Error |  StdDev | Ratio | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------- |---------:|--------:|--------:|------:|------:|------:|------:|----------:|
| ObterCotacaoPorMoedaQueryClass | 135.1 ms | 2.57 ms | 4.50 ms |  1.00 |     - |     - |     - | 214.66 KB |
