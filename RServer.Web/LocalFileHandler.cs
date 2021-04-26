using RServer.Handlers;
using RServer.Results;
using RServer.Web.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using RServer.Web.Services;
using System.IO;
using RServer.Web.Results;

namespace RServer.Web
{
    public class LocalFileHandler : IRequestHandler
    {
        private readonly List<IFileProvider> _providers = new List<IFileProvider>();

        public int Priority { get; set; }

        public IMimeService MimeService { get; set; } = new DictionaryMimeService();
        public IAttachmentLogic AttachmentLogic { get; set; } = new SetAttachmentLogic();

        public void AddProvider(IFileProvider provider)
        {
            _providers.Add(provider);
        }

        public IResult Handle(IServiceProvider services, HttpListenerContext context)
        {
            string localPath = context.Request.Url.LocalPath.Substring(1);
            string mime = MimeService.GetMime(Path.GetExtension(localPath));
            var stream = _providers.Select(p => p.Open(localPath)).FirstOrDefault();
            if (stream != null)
            {
                return AttachmentLogic.IsAttachment(localPath) ? new FileResult(stream, mime, Path.GetFileName(localPath)) : new StreamResult(stream, mime);
            }
            return null;
        }
    }
}
