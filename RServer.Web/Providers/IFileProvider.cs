using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RServer.Web.Providers
{
    public interface IFileProvider
    {
        Stream Open(string path);
    }
}
