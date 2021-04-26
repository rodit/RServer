using RServer.Results;
using RServer.Routers.Filters;
using RServer.Routers.Routes;
using RServer.Routers.Transformers;
using RUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace RServer.Routers
{
    public class RouteMetadata
    {
        public Type RouteType { get; set; }
        public string HttpMethod { get; set; }
        public string Pattern { get; set; }

        public ParameterInfo[] MethodParams { get; private set; }
        public List<FilterAttribute> Filters { get; } = new List<FilterAttribute>();
        public ITransformer BodyTransformer { get; set; }

        private DIFastConstructor _constructor;
        private FastInvokeHandler _contextSetDelegate;
        private FastInvokeHandler _delegate;

        public RouteMetadata(Type type, MethodInfo method, RouteAttribute attrib)
        {
            LoadFrom(type, method, attrib);
        }

        private void LoadFrom(Type type, MethodInfo method, RouteAttribute attrib)
        {
            RouteType = type;
            HttpMethod = attrib.Method;
            Pattern = attrib.Pattern;
            MethodParams = method.GetParameters();

            Filters.AddRange(type.GetCustomAttributes<FilterAttribute>());
            Filters.AddRange(method.GetCustomAttributes<FilterAttribute>());

            var transformerAttrib = method.GetCustomAttribute<TransformerAttribute>() ?? type.GetCustomAttribute<TransformerAttribute>();
            if (transformerAttrib != null)
            {
                BodyTransformer = transformerAttrib.Transformer;
            }

            _constructor = type.BindConstructor();

            _contextSetDelegate = type.BindSetProperty(p => p.PropertyType == typeof(HttpListenerContext));

            _delegate = method.BindFastInvoker();
        }

        public IResult Call(IServiceProvider services, RouteCallMatch match, HttpListenerContext context)
        {
            object route = _constructor.Construct(services);
            _contextSetDelegate?.Invoke(route, context);
            return (IResult)_delegate(route, match.Parameters);
        }
    }
}
