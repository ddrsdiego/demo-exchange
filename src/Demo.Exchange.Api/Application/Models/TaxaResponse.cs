namespace Demo.Exchange.Application.Models
{
    using Newtonsoft.Json;
    using System;

    [Serializable]
    public struct TaxaResponse
    {
        [JsonConstructor]
        public TaxaResponse(string id, string tipoSegmento, decimal valorTaxa)
            : this(id, tipoSegmento, valorTaxa, DateTime.Now)
        {
        }

        [JsonConstructor]
        public TaxaResponse(string id, string tipoSegmento, decimal valorTaxa, DateTime criadoEm) =>
            (Id, TipoSegmento, ValorTaxa, CriadoEm) = (id, tipoSegmento, valorTaxa, criadoEm);

        public string Id { get; set; }
        public string TipoSegmento { get; set; }
        public decimal ValorTaxa { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}