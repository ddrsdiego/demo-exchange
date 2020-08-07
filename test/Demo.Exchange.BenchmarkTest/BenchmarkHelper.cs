namespace Demo.Exchange.BenchmarkTest
{
    public class BenchmarkHelper
    {
        public ClienteClass CriarClienteClass(int valor) => new ClienteClass(valor);

        public ClienteStruct CriarClienteStruct(int valor) => new ClienteStruct(valor);
    }
}