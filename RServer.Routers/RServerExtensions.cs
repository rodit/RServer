namespace RServer.Routers
{
    /// <summary>
    /// Some useful extensions for <see cref="RServer"/> that allow chaining of certain setup actions.
    /// </summary>
    public static class RServerExtensions
    {
        /// <summary>
        /// Adds a new router with priority <paramref name="priority"/> to the given server.
        /// </summary>
        /// <param name="server">The server to add the router to.</param>
        /// <param name="priority">The priority of the router.</param>
        /// <returns>The new router added to the server.</returns>
        public static Router AddRouter(this RServer server, int priority = 0)
        {
            var router = new Router()
            {
                Priority = priority,
            };
            server.AddHandler(router);
            return router;
        }
    }
}
