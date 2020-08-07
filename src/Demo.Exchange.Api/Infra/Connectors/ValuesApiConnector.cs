namespace Demo.Exchange.Infra.Connectors
{
    using Demo.Exchange.Infra.Options;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using OpenTracing;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IValuesApiConnector
    {
        Task<List<string>> GetValues();
    }

    public class ValuesApiConnector : Connector, IValuesApiConnector
    {
        public ValuesApiConnector(IHttpClientFactory httpClient, ILoggerFactory logger, ITracer tracer, IOptions<EndPointConnectorsOptions> endPointConnectorsOptions)
            : base(httpClient, logger.CreateLogger<ValuesApiConnector>(), tracer, endPointConnectorsOptions.Value.ValuesApiConnector)
        {
        }

        public async Task<List<string>> GetValues()
        {
            var client = _httpClient.CreateClient(nameof(ValuesApiConnector));
            var endpoint = string.Concat(_connectorBaseUri.AbsoluteUri.TrimEnd('/'), "/values");

            using (Tracer.BuildSpan("ValuesApiConnector::GetValues").StartActive(finishSpanOnDispose: true))
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<string>>(await client.GetStringAsync(endpoint));
            }
        }
    }
}