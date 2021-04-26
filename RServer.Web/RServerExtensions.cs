namespace RServer.Web
{
    public static class RServerExtensions
    {
        public static LocalFileHandler AddLocalFiles(this RServer server)
        {
            var handler = new LocalFileHandler();
            server.AddHandler(handler);
            return handler;
        }
    }
}
