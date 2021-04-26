using System;
using System.Collections.Generic;
using System.Text;

namespace RServer.Web.Services
{
    public interface IMimeService
    {
        void Add(string extension, string mime);
        string GetMime(string extension);
    }
}
