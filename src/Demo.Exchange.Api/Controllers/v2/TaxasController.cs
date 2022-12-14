namespace Demo.Exchange.Api.Controllers.v2
{
    using Demo.Exchange.Application;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Application.Queries.ObterTaxaCobrancaPorSegmento;
    using Demo.Exchange.Infra.Cache;
    using Demo.Exchange.Infra.Cache.Memcached;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Net;
    using System.Net.Mime;
    using System.Text.Json;
    using System.Threading.Tasks;

    [ApiController]
    [ApiVersion(API_VERSION)]
    [Produces(CONTENT_TYPE_PRODUCER)]
    [Route("api/v{version:apiVersion}/taxas")]
    public class TaxasController : Controller
    {
        private const string API_VERSION = "2";
        private const string CONTENT_TYPE_PRODUCER = "application/json";

        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;

        public TaxasController(IMediator mediator, ICacheService cacheService)
        {
            _mediator = mediator;
            _cacheService = cacheService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.OK)]
        public async ValueTask ObterTaxaCobrancaPorSegmentoQueryByte([FromQuery] string segmento)
            => await GetResponse(await _mediator.Send(new ObterTaxaCobrancaPorSegmentoQuery(segmento)));

        [HttpGet]
        [Route("{segmento}/byte")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TaxaResponse), (int)HttpStatusCode.OK)]
        public async ValueTask ObterTaxaCobrancaPorSegmentoAsByte(string segmento, [FromServices] ICacheProvider cacheProvider)
        {
            var taxaResponse = await cacheProvider.Get<byte[]>(segmento);
            if (taxaResponse is null)
                await GetResponse(Application.Response.Fail(Errors.General.NotFound(nameof(TaxaResponse), segmento)));

            await GetResponse(Application.Response.Ok(ResponseContent.Create(taxaResponse, typeof(TaxaResponse))));
        }

        private async ValueTask GetResponse(Response response)
        {
            Response.StatusCode = response.StatusCode;
            Response.ContentType = MediaTypeNames.Application.Json;

            await Response.StartAsync();

            if (response.IsFailure)
                await Response.BodyWriter.WriteAsync(new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(response.ErrorResponse)));
            else
                await Response.BodyWriter.WriteAsync(response.Content.Value);

            await Response.BodyWriter.CompleteAsync();
        }
    }
}