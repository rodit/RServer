using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RServer.Web.Providers
{
    public class LocalFileProvider : IFileProvider
    {
        public string BasePath { get; set; }

        public LocalFileProvider(string basePath = null)
        {
            BasePath = basePath;
        }

        public string GetFullPath(string path)
        {
            return Path.Combine(BasePath, path);
        }

        public Stream Open(string path)
        {
            return File.OpenRead(GetFullPath(path));
        }
    }
}
