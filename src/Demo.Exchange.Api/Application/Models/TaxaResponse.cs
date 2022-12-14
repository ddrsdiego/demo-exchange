namespace Demo.Exchange.Application.Models
{
    using System;

    [Serializable]
    public struct TaxaResponse
    {
        public string Id { get; set; }
        public string TipoSegmento { get; set; }
        public decimal ValorTaxa { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}