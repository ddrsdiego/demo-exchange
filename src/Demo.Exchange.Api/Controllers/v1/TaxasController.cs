namespace Demo.Exchange.Api.Controllers.v1
{
    using Demo.Exchange.Application.Commands.AtualizarTaxa;
    using Demo.Exchange.Application.Commands.RegistrarNovaTaxa;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Threading.Tasks;

    [ApiController]
    [ApiVersion(API_VERSION)]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/taxas")]
    public class TaxasController : Controller
    {
        private const string API_VERSION = "1";
        private readonly IMediator _mediator;

        public TaxasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTaxaCobrancaPorSegmento([FromQuery] string segmento)
        {
            var response = await _mediator.Send(new ObterTaxaCobrancaPorSegmentoQuery(segmento));
            if (response.IsFailure)
                return BadRequest(response.ErrorResponse);

            return Ok(response.PayLoad);
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> RegistrarNovaTaxa([FromBody] RegistrarNovaTaxaCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsFailure)
                return BadRequest(response.ErrorResponse);

            return Created($"{Request.Scheme}://{Request.Host}/api/v{API_VERSION}/taxas/{response.PayLoad.Id}", response.PayLoad);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> AtualizarTaxa(string id, [FromBody] AtualizarTaxaCommand command)
        {
            command.SetId(id);

            var response = await _mediator.Send(command);
            if (response.IsFailure)
                return BadRequest(response.ErrorResponse);

            return Ok();
        }
    }
}