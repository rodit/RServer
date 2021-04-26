using RUtils.Reflection;
using System;

namespace RServer.Routers.Filters
{
    /// <summary>
    /// Adds a filter instantiated from the given type to a route or class of routes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FilterAttribute : Attribute
    {
        private DIFastConstructor _constructor;

        /// <summary>
        /// Creates a <see cref="FilterAttribute"/> with the given filter type.
        /// </summary>
        /// <param name="filterType">The type of filter to apply to the route.</param>
        public FilterAttribute(Type filterType)
        {
            _constructor = filterType.BindConstructor();
        }

        /// <summary>
        /// Creates a filter passed to this attributes constructor from the given services.
        /// </summary>
        /// <param name="services">The service provider used to inject dependencies into the instantiated filter.</param>
        /// <returns>The instatiated filter associated with this attribute.</returns>
        public virtual IFilter CreateAndInject(IServiceProvider services)
        {
            return _constructor.Construct<IFilter>(services);
        }
    }
}
