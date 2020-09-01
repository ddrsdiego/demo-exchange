namespace Demo.Exchange.Domain.AggregateModel.TaxaModel
{
    using Demo.Exchange.Domain.SeedWorks;
    using System;
    using System.Diagnostics.CodeAnalysis;

    public readonly struct ValorTaxaCobranca : IComparable<ValorTaxaCobranca>
    {
        public ValorTaxaCobranca(decimal valor) => Valor = valor;

        public static Result<ValorTaxaCobranca> Create(decimal valor)
        {
            if (valor < 0)
                return Result<ValorTaxaCobranca>.Fail("Valor da taxa não deve ser menor que zero.");

            return Result<ValorTaxaCobranca>.Ok(new ValorTaxaCobranca(valor));
        }

        public int CompareTo([AllowNull] ValorTaxaCobranca other)
        {
            return other.CompareTo(other);
        }

        public decimal Valor { get; }

        public static implicit operator ValorTaxaCobranca(decimal valorTaxa)
        {
            var resultadoValor = Create(valorTaxa);
            if (resultadoValor.IsFailure)
                throw new ArgumentOutOfRangeException(string.Join("|", resultadoValor.Messages));

            return resultadoValor.Value;
        }
    }
}