using System;
using System.Linq;

namespace RUtils.Reflection
{
    public static class DependencyInjectionUtils
    {
        public static object Inject(this FastConstructorHandler constructor, IServiceProvider provider, params Type[] ctrParamTypes)
        {
            return constructor(ctrParamTypes.Select(t => provider.GetService(t)).ToArray());
        }

        public static DIFastConstructor BindConstructor(this Type type)
        {
            var ctr = type.GetConstructors().First();
            var paramTypes = ctr.GetParameters().Select(p => p.ParameterType);
            return new DIFastConstructor(ctr.BindFastConstructor(), paramTypes.ToArray());
        }
    }

    public class DIFastConstructor
    {
        private FastConstructorHandler _ctrHandler;
        private Type[] _ctrParamTypes;

        public DIFastConstructor(FastConstructorHandler handler, params Type[] paramTypes)
        {
            _ctrHandler = handler;
            _ctrParamTypes = paramTypes;
        }

        public object Construct(IServiceProvider services)
        {
            return _ctrHandler(_ctrParamTypes.Select(t => services.GetService(t)).ToArray());
        }

        public T Construct<T>(IServiceProvider services)
        {
            return (T)Construct(services);
        }
    }
}
