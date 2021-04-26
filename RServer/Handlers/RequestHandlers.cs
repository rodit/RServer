using RUtils.Comparers;
using System.Collections.Generic;

namespace RServer.Handlers
{
    /// <summary>
    /// Utilities for request handlers.
    /// </summary>
    public static class RequestHandlers
    {
        /// <summary>
        /// The comparator used to sort the request handler list based on <see cref="IRequestHandler.Priority"/>.
        /// </summary>
        public static readonly Comparer<IRequestHandler> Comparer = new FunctionComparer<IRequestHandler>((h0, h1) => h1.Priority - h0.Priority);
    }
}
