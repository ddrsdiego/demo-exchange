namespace Demo.Exchange.Api.Controllers.v2
{
    using Demo.Exchange.Application;
    using Demo.Exchange.Application.Models;
    using Demo.Exchange.Application.Queries.ObterCotacaoPorMoeda;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Buffers;
    using System.Net;
    using System.Net.Mime;
    using System.Text.Json;
    using System.Threading.Tasks;

    [ApiController]
    [ApiVersion(API_VERSION)]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/cotacoes")]
    public class CotacoesController : Controller
    {
        private const string API_VERSION = "2";
        private readonly IMediator _mediator;

        public CotacoesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(CotacaoPorMoedaResponse), (int)HttpStatusCode.OK)]
        public async ValueTask ObterCotacaoPorMoeda([FromQuery] string segmento, [FromQuery] string moeda, [FromQuery] decimal quantidade)
        {
            var response = await _mediator.Send(new ObterCotacaoPorMoedaQuery(segmento, moeda, quantidade));
            await GetResponse(response);
        }

        private async ValueTask GetResponse(Response response)
        {
            Response.StatusCode = response.StatusCode;
            Response.ContentType = MediaTypeNames.Application.Json;

            await Response.StartAsync();

            if (response.IsFailure)
                await Response.BodyWriter.WriteAsync(new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(response.ErrorResponse)));
            else
                Response.BodyWriter.Write(response.Content.Value);

            await Response.BodyWriter.CompleteAsync();
        }
    }
}