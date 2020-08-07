namespace Demo.Exchange.Infra.Connectors
{
    using Demo.Exchange.Infra.Options;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using OpenTracing;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public struct ExchangeRate
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public interface IExchangeRatesApiConnector
    {
        Task<KeyValuePair<string, decimal>> OberUltimaCotacaoPorMoeda(string moeda);
    }

    public class ExchangeRatesApiConnector : Connector, IExchangeRatesApiConnector
    {
        private readonly AppConfigOptions _appConfigOptions;

        public ExchangeRatesApiConnector(IHttpClientFactory httpClient,
                                         ILoggerFactory logger,
                                         IOptions<AppConfigOptions> appConfigOptions,
                                         IOptions<EndPointConnectorsOptions> endPointConnectorsOptions,
                                         ITracer tracer)
            : base(httpClient, logger.CreateLogger<ExchangeRatesApiConnector>(), tracer, endPointConnectorsOptions.Value.ExchangeRatesApiConnector)
        {
            _appConfigOptions = appConfigOptions.Value;
        }

        public async Task<KeyValuePair<string, decimal>> OberUltimaCotacaoPorMoeda(string moeda)
        {
            var endpoint = string.Concat(_connectorBaseUri.AbsoluteUri.TrimEnd('/'), $"{ConnectorRoutes.OberUltimaCotacaoPorMoeda(moeda)}");

            try
            {
                var client = _httpClient.CreateClient(nameof(ExchangeRatesApiConnector));

                using (var scope = Tracer.BuildSpan("ExchangeRatesApiConnector::OberUltimaCotacaoPorMoeda").StartActive(finishSpanOnDispose: true))
                {
                    var httpResponseMessage = await client.GetAsync(endpoint);

                    httpResponseMessage.EnsureSuccessStatusCode();

                    var content = await httpResponseMessage.Content.ReadAsStringAsync();

                    var exchangeRate = JsonSerializer.Deserialize<ExchangeRate>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                    return exchangeRate.Rates.FirstOrDefault(x => x.Key.Equals(_appConfigOptions.MoedaLocal, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Falha ao consultar cotações de moeda estrangeira no Endpoint {endpoint}");
                throw;
            }
        }
    }
}