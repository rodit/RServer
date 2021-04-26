using System;

namespace RServer.Routers.Routes
{
    /// <summary>
    /// An attribute that marks its method as a route which accepts requests matching the given pattern and given HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteAttribute : Attribute
    {
        /// <summary>
        /// The HTTP method that this attribute's route accepts.
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// The url pattern that this attribute's route accepts.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Creates a new <see cref="RouteAttribute"/> with the given HTTP method and url pattern.
        /// </summary>
        /// <param name="method">The HTTP method this route accepts.</param>
        /// <param name="pattern">The url pattern this route accepts.</param>
        public RouteAttribute(string method, string pattern)
        {
            Method = method;
            Pattern = pattern;
        }
    }
}
