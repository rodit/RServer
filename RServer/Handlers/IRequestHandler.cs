using RServer.Results;
using System;
using System.Net;

namespace RServer.Handlers
{
    /// <summary>
    /// An interface which can be implemented to extend request handling logic for an <see cref="RServer"/>.
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// The priority of this request handler. Handlers with a higher priority are executed first.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Attempts to handle the given context with the given service provider.
        /// </summary>
        /// <param name="services">The service provider built from <see cref="RServer.Services"/> when the server was started.</param>
        /// <param name="context">The context holding information about the request and response.</param>
        /// <returns>A non-null result if the request was handled, otherwise null and the next highest priority handler will be invoked.</returns>
        IResult Handle(IServiceProvider services, HttpListenerContext context);
    }
}
