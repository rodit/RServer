namespace RServer.Routers.Routes
{
    /// <summary>
    /// Marks its method as a route which accepts HTTP POST requests matching the given url pattern.
    /// </summary>
    public class PostAttribute : RouteAttribute
    {
        public PostAttribute(string pattern) : base("POST", pattern) { }
    }
}
