using System;

namespace Demo.Consumer.Controllers
{
    using Demo.Exchange.Api;
    using Microsoft.AspNetCore.Mvc;
    using OpenTracing;
    using OpenTracing.Propagation;
    using OpenTracing.Tag;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.Json;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ITracer _tracer;
        private readonly IHttpClientFactory _httpClientFactory;

        public ValuesController(IHttpClientFactory httpClientFactory, ITracer tracer)
        {
            _tracer = tracer;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("taxa/string")]
        public async Task<IActionResult> GetTaxaResponse()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5000/api/v2/taxas/VAREJO/string");

            return Ok(JsonSerializer.Deserialize<TaxaResponse>(await response.Content.ReadAsStringAsync()));
        }

        [HttpGet]
        [Route("taxa/byte")]
        public async Task<IActionResult> GetTaxaResponseAsByte()
        {
            var client = _httpClientFactory.CreateClient();
            var responseAsByte = await client.GetByteArrayAsync("http://localhost:5000/api/v2/taxas/VAREJO/byte");

            return Ok(ByteArrayToObject<TaxaResponse>(responseAsByte));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var headers = Request.Headers.ToDictionary(k => k.Key, v => v.Value[0]);
            using var scope = StartServerSpan(_tracer, headers, "producingValues");
            await Task.Delay(2000);
            return new[] { "Hello", "OpenTracing!" };
        }

        public static IScope StartServerSpan(ITracer tracer, IDictionary<string, string> headers, string operationName)
        {
            ISpanBuilder spanBuilder;
            try
            {
                var parentSpanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, new TextMapExtractAdapter(headers));

                spanBuilder = tracer.BuildSpan(operationName);
                if (parentSpanCtx != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
                }
            }
            catch (Exception)
            {
                spanBuilder = tracer.BuildSpan(operationName);
            }

            // TODO could add more tags like http.url
            return spanBuilder.WithTag(Tags.SpanKind, Tags.SpanKindServer).StartActive(true);
        }

        private object ByteArrayToObject<T>(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return (T)binForm.Deserialize(memStream);
        }
    }
}

namespace Demo.Exchange.Api
{
    [Serializable]
    internal struct TaxaResponse
    {
        public string Id { get; set; }
        public string TipoSegmento { get; set; }
        public decimal ValorTaxa { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}