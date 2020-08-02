namespace Demo.Exchange.Api.Controllers.v2
{
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Infra.Cache;
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
        private const string API_VERSION = "2";
        private readonly IMediator _mediator;

        public TaxasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTaxaCobrancaPorSegmento([FromServices] ICacheService cacheService, [FromQuery] string segmento)
        {
            var response = await cacheService.GetCacheValueAsString(segmento);
            if (string.IsNullOrEmpty(response))
                return BadRequest();

            return Ok(response);
        }
    }
}