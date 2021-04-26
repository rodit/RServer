using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RServer.Results
{
    /// <summary>
    /// A simple result that responds with only an HTTP status code.
    /// </summary>
    public class StatusResult : IResult
    {
        /// <summary>
        /// The HTTP status code to write.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Creates a new <c>StatusResult</c> with the given status code.
        /// </summary>
        /// <param name="code">The status code to send when writing this result's response.</param>
        public StatusResult(int code)
        {
            StatusCode = code;
        }

        /// <inheritdoc/>
        public virtual void Respond(HttpListenerContext context)
        {
            context.Response.StatusCode = StatusCode;
        }
    }
}
