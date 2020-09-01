namespace Demo.Exchange.Application
{
    using MediatR;
    using Microsoft.Extensions.Logging;

    public abstract class Handler
    {
        protected Handler(IMediator mediator, ILogger logger)
        {
            Logger = logger;
            Mediator = mediator;
        }

        protected ILogger Logger { get; }
        protected IMediator Mediator { get; }
    }
}