namespace Demo.Exchange.Application.Models
{
    using System;

    [Serializable]
    public readonly struct TaxaResponse
    {
        public TaxaResponse(string id, string tipoSegmento, decimal valorTaxa, DateTime criadoEm) =>
            (Id, TipoSegmento, ValorTaxa, CriadoEm) = (id, tipoSegmento, valorTaxa, criadoEm);

        public string Id { get; }
        public string TipoSegmento { get; }
        public decimal ValorTaxa { get; }
        public DateTime CriadoEm { get; }
    }
}