using RServer.Routers.ParamReplacers;
using System;
using System.Net;
using System.Text.RegularExpressions;

namespace RServer.Routers
{
    public class RouteCallMatch
    {
        public bool Success { get; set; }
        public object[] Parameters { get; set; }
        public Exception FatalError { get; set; }

        public bool TryMatch(RouteMetadata route, HttpListenerContext context, ParamReplacer paramReplacer)
        {
            try
            {
                if (context.Request.HttpMethod.Equals(route.HttpMethod, StringComparison.OrdinalIgnoreCase))
                {
                    string pattern = paramReplacer.ReplacePattern(route.Pattern);
                    bool isPost = context.Request.HttpMethod.Equals("post", StringComparison.OrdinalIgnoreCase);
                    int argLen = route.MethodParams.Length - (isPost ? 1 : 0);
                    var match = Regex.Match(context.Request.Url.LocalPath, $"^{pattern}$");
                    if (match.Success && argLen == match.Groups.Count - 1)
                    {
                        Parameters = new object[route.MethodParams.Length];
                        for (int i = 0; i < argLen; i++)
                        {
                            var param = route.MethodParams[i];
                            Parameters[i] = Convert.ChangeType(match.Groups[param.Name].Value, param.ParameterType);
                        }
                        if (isPost)
                        {
                            try
                            {
                                Parameters[argLen] = route.BodyTransformer.Transform(route.MethodParams[argLen].ParameterType, context.Request.InputStream);
                            }
                            catch (Exception e)
                            {
                                FatalError = e;
                            }
                        }
                        return Success = true;
                    }
                }
            }
            catch (Exception) { }
            return Success = false;
        }
    }
}
