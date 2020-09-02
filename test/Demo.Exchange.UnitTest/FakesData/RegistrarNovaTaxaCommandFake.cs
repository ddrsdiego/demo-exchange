namespace Demo.Exchange.UnitTest.FakesData
{
    using Demo.Exchange.Application.Commands.RegistrarNovaTaxa;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;

    public static partial class FakeData
    {
        private const decimal VALOR_TAXA_VALIDO = 1M;
        private const decimal VALOR_TAXA_INVALIDO = -1;
        private const string TIPO_SEGMENTO_VAREJO = "VAREJO";
        private const string TIPO_SEGMENTO_INVALIDO = "VAREJO_ERRADO";

        public static RegistrarNovaTaxaCommand RegistrarNovaTaxaCommandComSegmentoNulo => new RegistrarNovaTaxaCommand(null, VALOR_TAXA_VALIDO);
        public static RegistrarNovaTaxaCommand RegistrarNovaTaxaCommandComSegmentoVazio => new RegistrarNovaTaxaCommand(string.Empty, VALOR_TAXA_VALIDO);
        public static RegistrarNovaTaxaCommand RegistrarNovaTaxaValorTaxaInvalido => new RegistrarNovaTaxaCommand(TIPO_SEGMENTO_VAREJO, VALOR_TAXA_INVALIDO);
        public static RegistrarNovaTaxaCommand RegistrarNovaTaxaCommandTodosParametrosInvalidos => new RegistrarNovaTaxaCommand(string.Empty, VALOR_TAXA_INVALIDO);
        public static RegistrarNovaTaxaCommand RegistrarNovaTaxaCommandTipoSegmentoInvalido => new RegistrarNovaTaxaCommand(TIPO_SEGMENTO_INVALIDO, VALOR_TAXA_VALIDO);
        public static RegistrarNovaTaxaCommand RegistrarNovaTaxaCommandVarejoValorUm => new RegistrarNovaTaxaCommand(TipoSegmento.Varejo.Id, VALOR_TAXA_VALIDO);
    }
}