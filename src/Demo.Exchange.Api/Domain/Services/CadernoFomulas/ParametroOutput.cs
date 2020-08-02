namespace Demo.Exchange.Domain.Services.CadernoFomulas
{
    public abstract class ParametroOutput
    {
        public ParametroOutput(decimal valor)
        {
            Valor = valor;
        }

        public decimal Valor { get; }

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}