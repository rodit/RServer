using System;

namespace RServer.Routers.Transformers
{
    /// <summary>
    /// Sets the body transformer for this attribute's route or class of routes to the given <see cref="ITransformer"/> type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TransformerAttribute : Attribute
    {
        /// <summary>
        /// The transformer to use for the body of this attribute's route or class of routes.
        /// </summary>
        public ITransformer Transformer { get; private set; }

        /// <summary>
        /// Creates a new <see cref="TransformerAttribute"/> with a transformer of the given type.
        /// </summary>
        /// <param name="transformerType"></param>
        public TransformerAttribute(Type transformerType)
        {
            Transformer = (ITransformer)Activator.CreateInstance(transformerType);
        }
    }
}
