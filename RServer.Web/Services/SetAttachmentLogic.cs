using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RServer.Web.Services
{
    public class SetAttachmentLogic : IAttachmentLogic
    {
        private readonly HashSet<string> _nonAttachmentExts = new HashSet<string>()
        {
            ".html",
            ".htm",
            ".js",
            ".css",
            ".txt",
            ".md",
            ".pdf"
        };

        public SetAttachmentLogic AddException(string extension)
        {
            _nonAttachmentExts.Add(extension);
            return this;
        }

        public bool IsAttachment(string path)
        {
            string ext = Path.GetExtension(path);
            return !_nonAttachmentExts.Contains(ext);
        }
    }
}
