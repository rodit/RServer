using RServer.Handlers;
using RServer.Results;
using RServer.Routers.ParamReplacers;
using RServer.Routers.Routes;
using RServer.Routers.Transformers;
using RUtils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace RServer.Routers
{
    /// <summary>
    /// Handles routed requests by matching them to added routes.
    /// </summary>
    /// See <see cref="RouteAttribute"/>.
    public class Router : IRequestHandler
    {
        private readonly List<RouteMetadata> _routes = new List<RouteMetadata>();

        public int Priority { get; set; }
        /// <summary>
        /// Used to map url paramter type names to regular expressions that validate them.
        /// </summary>
        public ParamReplacer ParamReplacer { get; set; } = new ParamReplacer();
        /// <summary>
        /// The default <see cref="ITransformer"/> used to transform body streams of requests made to this router.
        /// </summary>
        public ITransformer DefaultBodyTransformer { get; set; } = new StringTransformer();

        /// <summary>
        /// Adds all methods marked with the <see cref="RouteAttribute"/> in the type <typeparamref name="T"/> to this router's list of routes.
        /// </summary>
        /// <typeparam name="T">The type that who's methods should be added as routes to the router.</typeparam>
        public void AddRoutes<T>()
        {
            AddRoutes(typeof(T));
        }

        /// <summary>
        /// Adds all methods marked with the <see cref="RouteAttribute"/> in the type <paramref name="type"/> to this router's list of routes.
        /// </summary>
        /// <param name="type">The type that who's methods should be added as routes to the router.</param>
        public void AddRoutes(Type type)
        {
            foreach (var method in type.GetMethods())
            {
                var attrib = method.GetCustomAttribute<RouteAttribute>();
                if (attrib != null)
                {
                    RouteMetadata meta = new RouteMetadata(type, method, attrib);
                    if (meta.BodyTransformer == null)
                    {
                        meta.BodyTransformer = DefaultBodyTransformer;
                    }
                    _routes.Add(meta);
                    Logger.Default.Info("Router", $"Found route {type.Name}#{method.Name} for {attrib.Pattern}.");
                }
            }
        }

        public IResult Handle(IServiceProvider services, HttpListenerContext context)
        {
            var match = new RouteCallMatch();
            foreach (var route in _routes)
            {
                if (match.TryMatch(route, context, ParamReplacer))
                {
                    var filterResult = route.Filters.Select(f => f.CreateAndInject(services).Filter(context)).FirstOrDefault(r => r != null);
                    return filterResult ?? route.Call(services, match, context);
                }
                if (match.FatalError != null)
                {
                    Logger.Default.Error("Router", $"Error while trying to match router request {context.Request.Url}:", match.FatalError);
                    return null;
                }
            }
            return null;
        }
    }
}
