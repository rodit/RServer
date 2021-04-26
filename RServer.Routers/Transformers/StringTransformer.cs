using System;
using System.IO;

namespace RServer.Routers.Transformers
{
    /// <summary>
    /// Transforms a request body into a string by wrapping a <see cref="StreamReader"/> around it and then returning <see cref="StreamReader.ReadToEnd"/>
    /// </summary>
    public class StringTransformer : ITransformer
    {
        public object Transform(Type type, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
