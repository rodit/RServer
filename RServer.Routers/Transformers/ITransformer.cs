using System;
using System.IO;

namespace RServer.Routers.Transformers
{
    /// <summary>
    /// An interface which can be implemented to create request body transformation logic.
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// Transforms a request body entity in a stream stream into an object of the given type.
        /// </summary>
        /// <param name="type">The type to transform the entity read from the stream into.</param>
        /// <param name="stream">The stream to read the entity from.</param>
        /// <returns></returns>
        object Transform(Type type, Stream stream);
    }
}
