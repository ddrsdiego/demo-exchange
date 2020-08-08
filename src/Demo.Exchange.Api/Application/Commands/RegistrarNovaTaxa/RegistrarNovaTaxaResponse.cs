namespace Demo.Exchange.Application.Commands.RegistrarNovaTaxa
{
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.Json;

    public class RegistrarNovaTaxaResponse : Response<TaxaResponse>
    {
        public RegistrarNovaTaxaResponse(string requestId)
            : base(requestId)
        {
        }
    }

    public static class TaxaCobrancaEx
    {
        public static TaxaResponse ConverterEntidadeParaResponse(this TaxaCobranca taxaCobranca)
            => new TaxaResponse(taxaCobranca.TaxaCobrancaId, taxaCobranca.TipoSegmento.Id, taxaCobranca.ValorTaxa.Valor, taxaCobranca.CriadoEm);

        public static byte[] TaxaResponseAsByte(this TaxaCobranca taxaCobranca) => ObjectToByteArray(ConverterEntidadeParaResponse(taxaCobranca));

        public static string TaxaResponseAsString(this TaxaCobranca taxaCobranca) => JsonSerializer.Serialize(ConverterEntidadeParaResponse(taxaCobranca));

        private static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
}