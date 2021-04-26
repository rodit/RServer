using RServer.Results;
using System.Net;

namespace RServer.Routers.Filters
{
    /// <summary>
    /// An interface that can be implemented to create filtering logic for routes.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Filters a request from reaching a route.
        /// </summary>
        /// <param name="context">The context to filter.</param>
        /// <returns>A non-null result if the request should be filtered, otherwise null in which case the request will not be filtered.</returns>
        IResult Filter(HttpListenerContext context);
    }
}
