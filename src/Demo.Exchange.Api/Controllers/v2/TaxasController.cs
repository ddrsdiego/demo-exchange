namespace Demo.Exchange.Api.Controllers.v2
{
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Infra.Cache;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;

    [ApiController]
    [ApiVersion(API_VERSION)]
    [Produces(CONTENT_TYPE_PRODUCER)]
    [Route("api/v{version:apiVersion}/taxas")]
    public class TaxasController : Controller
    {
        private readonly IMediator _mediator;
        private const string API_VERSION = "2";
        private const string CONTENT_TYPE_PRODUCER = "application/json";
        private readonly ICacheService _cacheService;

        public TaxasController(IMediator mediator, ICacheService cacheService)
        {
            _mediator = mediator;
            _cacheService = cacheService;
        }

        [HttpGet]
        [Route("{segmento}/string")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTaxaCobrancaPorSegmentoAsString(string segmento)
        {
            var responseAsString = await _cacheService.GetCacheValueAsString($"STACKEXCHANGE-STRING-{segmento}");
            if (string.IsNullOrEmpty(responseAsString))
                return NotFound();

            return Content(responseAsString, MediaTypeNames.Application.Json);
        }

        [HttpGet]
        [Route("{segmento}/byte")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ObterTaxaCobrancaPorSegmentoAsByte(string segmento)
        {
            var responseAsByte = await _cacheService.GetCacheValueAsByte($"BYTE-{segmento}");
            if (responseAsByte is null)
                return NotFound();

            return File(responseAsByte, CONTENT_TYPE_PRODUCER);
        }
    }
}