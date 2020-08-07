namespace Demo.Exchange.Infra.Repositories
{
    using Demo.Exchange.Infra.Options;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using MySql.Data.MySqlClient;
    using OpenTracing;
    using System.Data;

    public abstract class Repository
    {
        private readonly IOptions<ConnectionStringOptions> _connectionString;

        public Repository(ILogger logger, ITracer tracer, IOptions<ConnectionStringOptions> connectionString)
        {
            Logger = logger;
            _connectionString = connectionString;
            Tracer = tracer;
        }

        protected ILogger Logger { get; }
        protected string ConnectionString => _connectionString.Value.MySqlConnection;
        protected ITracer Tracer { get; }

        protected IDbConnection GetConnection() => new MySqlConnection(ConnectionString);
    }
}