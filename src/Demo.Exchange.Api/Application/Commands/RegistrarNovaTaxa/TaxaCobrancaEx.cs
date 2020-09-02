namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;

    public static class TaxaCobrancaEx
    {
        public static TaxaResponse ConverterEntidadeParaResponse(this TaxaCobranca taxaCobranca)
            => new TaxaResponse
            {
                Id = taxaCobranca.TaxaCobrancaId,
                TipoSegmento = taxaCobranca.TipoSegmento.Id,
                ValorTaxa = taxaCobranca.ValorTaxa.Valor,
                CriadoEm = taxaCobranca.CriadoEm
            };
    }
}