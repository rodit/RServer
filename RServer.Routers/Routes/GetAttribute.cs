namespace RServer.Routers.Routes
{
    /// <summary>
    /// Marks its method as a route which accepts HTTP GET requests matching the given url pattern.
    /// </summary>
    public class GetAttribute : RouteAttribute
    {
        public GetAttribute(string pattern) : base("GET", pattern) { }
    }
}
