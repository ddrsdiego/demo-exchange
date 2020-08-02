namespace Demo.Exchange.Infra.Connectors
{
    using Demo.Exchange.Infra.Options;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using OpenTracing;
    using System;
    using System.Net.Http;

    public abstract class Connector
    {
        protected readonly Uri _connectorBaseUri;
        protected readonly IHttpClientFactory _httpClient;
        protected readonly IOptions<EndPointConnectorsOptions> _endPointConnectorsOptions;

        protected Connector(IHttpClientFactory httpClient, ILogger logger, ITracer tracer, string endPointConnector)
        {
            Logger = logger;
            _httpClient = httpClient;
            _connectorBaseUri = new Uri(endPointConnector);
            Tracer = tracer;
        }

        protected ILogger Logger { get; }
        protected ITracer Tracer { get; }
    }
}