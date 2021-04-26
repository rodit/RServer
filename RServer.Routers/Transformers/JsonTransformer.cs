using Newtonsoft.Json;
using System;
using System.IO;

namespace RServer.Routers.Transformers
{
    /// <summary>
    /// Transforms an request body into an object through a <see cref="JsonSerializer"/>.
    /// </summary>
    public class JsonTransformer : ITransformer
    {
        private JsonSerializer _serializer = new JsonSerializer();

        public object Transform(Type type, Stream stream)
        {
            using (var reader = new StreamReader(stream))
            using (var jreader = new JsonTextReader(reader))
            {
                return _serializer.Deserialize(jreader, type);
            }
        }
    }
}
