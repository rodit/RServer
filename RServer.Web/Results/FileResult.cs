using System.IO;
using System.Net;

namespace RServer.Web.Results
{
    public class FileResult : StreamResult
    {
        private string _name;

        public FileResult(Stream stream, string mimeType, string name) : base(stream, mimeType)
        {
            _name = name;
        }

        public override void Respond(HttpListenerContext context)
        {
            context.Response.Headers.Set("Content-Disposition", $"attachment; filename=\"{_name}\"");
            base.Respond(context);
        }
    }
}
