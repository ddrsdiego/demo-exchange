namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;

    public static class TaxaCobrancaEx
    {
        public static TaxaResponse ConverterEntidadeParaResponse(this TaxaCobranca taxaCobranca)
            => new TaxaResponse(taxaCobranca.TaxaCobrancaId, taxaCobranca.TipoSegmento.Id, taxaCobranca.ValorTaxa.Valor, taxaCobranca.CriadoEm);
    }
}