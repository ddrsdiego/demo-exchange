namespace Demo.Exchange.Api.Controllers.v1
{
    using Demo.Exchange.Application.Commands.AtualizarTaxa;
    using Demo.Exchange.Application.Commands.RegistrarNovaTaxa;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento;
    using Demo.Exchange.Infra.Connectors;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;

    [ApiController]
    [ApiVersion(API_VERSION)]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/taxas")]
    public class TaxasController : Controller
    {
        private const string API_VERSION = "1";
        private readonly IMediator _mediator;
        private readonly IValuesApiConnector _valuesApiConnector;

        public TaxasController(IMediator mediator, IValuesApiConnector valuesApiConnector)
        {
            _mediator = mediator;
            _valuesApiConnector = valuesApiConnector;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTaxaCobrancaPorSegmento([FromQuery] string segmento)
        {
            //var response = await _mediator.Send(new ObterTaxaCobrancaPorSegmentoQuery(segmento));
            var response = await _mediator.Send(new ObterTaxaCobrancaPorSegmentoQueryStruct(segmento));
            if (response.IsFailure)
                return BadRequest(response.ErrorResponse);

            return Content(response.PayLoad, MediaTypeNames.Application.Json);
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegistrarNovaTaxa([FromBody] RegistrarNovaTaxaRequest request)
        {
            var response = await _mediator.Send(new RegistrarNovaTaxaCommand(request.Segmento, request.ValorTaxa));
            if (response.IsFailure)
                return BadRequest(response.ErrorResponse);

            return Created($"{Request.Scheme}://{Request.Host}/api/v{API_VERSION}/taxas/{response.PayLoad.Id}", response.PayLoad);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> AtualizarTaxa(string id, [FromBody] AtualizarTaxaRequest request)
        {
            var response = await _mediator.Send(new AtualizarTaxaCommand(id, request.NovaTaxa));
            if (response.IsFailure)
                return BadRequest(response.ErrorResponse);

            return Ok();
        }

        [HttpGet]
        [Route("values")]
        public async Task<IActionResult> GetValues()
        {
            return Ok(await _valuesApiConnector.GetValues());
        }
    }
}