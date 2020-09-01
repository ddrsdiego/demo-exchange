namespace Demo.Exchange.UnitTest.Application
{
    using Demo.Exchange.Application.Commands.RegistrarNovaTaxa;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Domain.AggregateModel.TaxaModel;
    using Demo.Exchange.UnitTest.FakesData;
    using FluentAssertions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    [TestFixture]
    public class RegistrarNovaTaxaCommandHandlerTest
    {
        private IMediator mediator;
        private ILoggerFactory logger;
        private ITaxaCobrancaRepository taxaCobrancaRepository;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            logger = Substitute.For<ILoggerFactory>();
            taxaCobrancaRepository = Substitute.For<ITaxaCobrancaRepository>();
        }

        [Test]
        public async Task Deve_Falhar_Dados_Command_Estao_Invalidos()
        {
            //arrange
            var sut = CreateSut();
            var command = FakeData.RegistrarNovaTaxaCommandTodosParametrosInvalidos;

            //act
            var resultado = await sut.Handle(command, CancellationToken.None);

            //assert
            resultado.IsFailure.Should().BeTrue();
        }

        [Test]
        public async Task Deve_Falhar_Registar_Taxa_Quando_ValorTaxa_For_Invalida()
        {
            const string VALORTAXAINVALIDO = "ValorTaxaInvalido";

            //arrange
            var sut = CreateSut();
            var command = FakeData.RegistrarNovaTaxaValorTaxaInvalido;

            //act
            var resultado = await sut.Handle(command, CancellationToken.None);

            //assert
            resultado.IsFailure.Should().BeTrue();
            resultado.ErrorResponse.Should().NotBeNull();
            resultado.ErrorResponse.Error.Should().NotBeNull();
            resultado.ErrorResponse.Error.Details.Count.Should().Be(1);
            resultado.ErrorResponse.Error.Details.First(x => x.Code.Equals(VALORTAXAINVALIDO)).Should().NotBeNull();
        }

        [Test]
        public async Task Deve_Falhar_Registar_Taxa_Quando_TipoSegmento_For_Invalido()
        {
            const string NOT_FOUND = "NotFound";

            //arrange
            var sut = CreateSut();
            var command = FakeData.RegistrarNovaTaxaCommandTipoSegmentoInvalido;

            //act
            var resultado = await sut.Handle(command, CancellationToken.None);

            //assert
            resultado.IsFailure.Should().BeTrue();
            resultado.ErrorResponse.Should().NotBeNull();
            resultado.ErrorResponse.Error.Should().NotBeNull();
            resultado.ErrorResponse.Error.Code.Should().Be(NOT_FOUND);
        }

        [Test]
        public async Task Deve_Falhar_Registar_Taxa_Quando_TaxaCobrancaRepository_Lancar_Excecao()
        {
            const string INTERNAL_PROCESS_ERROR = "InternalProcessError";

            //arrange
            var sut = CreateSut();
            var command = FakeData.RegistrarNovaTaxaCommandVarejoValorUm;

            taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento(Arg.Any<string>()).Returns(_ => Task.FromException(new Exception()));

            //act
            var resultado = await sut.Handle(command, CancellationToken.None);

            //assert
            resultado.IsFailure.Should().BeTrue();
            resultado.ErrorResponse.Should().NotBeNull();
            resultado.ErrorResponse.Error.Should().NotBeNull();
            resultado.ErrorResponse.Error.Code.Should().Be(INTERNAL_PROCESS_ERROR);
        }

        [Test]
        public async Task Deve_Falhar_Registar_Taxa_Quando_Taxa_Por_Segmento_Ja_Estiver_Registrada()
        {
            const string TAXA_PARA_SEGMENTO_JA_REGISTRADA = "TaxaParaSegmentoJaRegistrada";

            //arrange
            var sut = CreateSut();
            var command = FakeData.RegistrarNovaTaxaCommandVarejoValorUm;

            taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento(Arg.Any<string>()).Returns(_ => Task.FromResult(FakeData.TaxaCobrancaValidaVarejoValorUm));

            //act
            var resultado = await sut.Handle(command, CancellationToken.None);

            //assert
            resultado.IsFailure.Should().BeTrue();
            resultado.ErrorResponse.Should().NotBeNull();
            resultado.ErrorResponse.Error.Should().NotBeNull();
            resultado.ErrorResponse.Error.Code.Should().Be(TAXA_PARA_SEGMENTO_JA_REGISTRADA);
        }

        [Test]
        public async Task Deve_Registar_Nova_Taxa_Quando_Command_Estiver_Valido()
        {
            const decimal VALOR_TAXA_ESPERADO = 1M;

            //arrange
            var sut = CreateSut();
            var command = FakeData.RegistrarNovaTaxaCommandVarejoValorUm;

            taxaCobrancaRepository.ObterTaxaCobrancaPorSegmento(Arg.Any<string>()).Returns(_ => Task.FromResult(FakeData.TaxaCobrancaNula));

            //act
            var resultado = await sut.Handle(command, CancellationToken.None);

            //assert
            await taxaCobrancaRepository.Received(1).Registrar(Arg.Any<TaxaCobranca>());

            var taxaResponse = resultado.Content.GetRaw<TaxaResponse>();

            resultado.IsSuccess.Should().BeTrue();
            taxaResponse.ValorTaxa.Should().Be(VALOR_TAXA_ESPERADO);
            taxaResponse.TipoSegmento.Should().Be(TipoSegmento.Varejo.Id);
        }

        private RegistrarNovaTaxaCommandHandler CreateSut() => new RegistrarNovaTaxaCommandHandler(mediator, logger, taxaCobrancaRepository);

        [TearDown]
        public void TearDown()
        {
            logger = null;
            mediator = null;
            taxaCobrancaRepository = null;
        }
    }
}