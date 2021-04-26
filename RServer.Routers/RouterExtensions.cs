using RServer.Routers.Transformers;
using System;

namespace RServer.Routers
{
    /// <summary>
    /// Some useful extensions for <see cref="Router"/> that allow chaining of certain setup actions.
    /// </summary>
    public static class RouterExtensions
    {
        /// <summary>
        /// Returns the given router with the default body transformer set to a newly created instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the default transformer.</typeparam>
        /// <param name="router">The router to set the default transformer of.</param>
        public static Router WithDefaultTransformer<T>(this Router router) where T : ITransformer
        {
            return router.WithDefaultTransformer(Activator.CreateInstance<T>());
        }
        
        /// <summary>
        /// Returns the given router with the default body transformer set to the given transformer.
        /// </summary>
        /// <param name="router">The router to set the default transformer of.</param>
        /// <param name="transformer">The default transformer to set.</param>
        public static Router WithDefaultTransformer(this Router router, ITransformer transformer)
        {
            router.DefaultBodyTransformer = transformer;
            return router;
        }

        /// <summary>
        /// Returns the given router with all routes in type <typeparamref name="T"/> added.
        /// </summary>
        /// <typeparam name="T">The type who's routes should be added.</typeparam>
        /// <param name="router">The router to add the routes to.</param>
        /// See <see cref="Router.AddRoutes{T}"/>
        public static Router WithRoutes<T>(this Router router)
        {
            return router.WithRoutes(typeof(T));
        }

        /// <summary>
        /// Returns the given router with all routes in <paramref name="type"/> added.
        /// </summary>
        /// <param name="router">The router to add the routes to.</param>
        /// <param name="type">The type who's routes should be added.</param>
        /// See <see cref="Router.AddRoutes(Type)"/>
        public static Router WithRoutes(this Router router, Type type)
        {
            router.AddRoutes(type);
            return router;
        }
    }
}
