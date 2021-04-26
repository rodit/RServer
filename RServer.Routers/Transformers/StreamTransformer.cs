using System;
using System.IO;

namespace RServer.Routers.Transformers
{
    /// <summary>
    /// Transforms a request stream into itself. This is useful if a route reads directly from the request body stream.
    /// </summary>
    public class StreamTransformer : ITransformer
    {
        public object Transform(Type type, Stream stream)
        {
            return stream;
        }
    }
}
