using RServer.Handlers;

namespace RServer
{
    /// <summary>
    /// Some useful extensions for <see cref="RServer"/> that allow chaining of certain setup actions.
    /// </summary>
    public static class RServerExtensions
    {
        /// <summary>
        /// Returns the given server with the given request handler added to it.
        /// </summary>
        /// <param name="server">The server to add the handler to.</param>
        /// <param name="handler">The handler to add to the server.</param>
        /// <see cref="RServer.AddHandler(IRequestHandler)"/>
        public static RServer WithHandler(this RServer server, IRequestHandler handler)
        {
            server.AddHandler(handler);
            return server;
        }
    }
}
