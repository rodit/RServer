using System.Net;

namespace RServer.Results
{
    /// <summary>
    /// An interface representing the result of a request.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Writes this result to the given context.
        /// </summary>
        /// <param name="context">The context to write the response to.</param>
        void Respond(HttpListenerContext context);
    }
}
