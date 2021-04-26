using System;
using System.Collections.Generic;
using System.Text;

namespace RServer.Web.Services
{
    public interface IAttachmentLogic
    {
        bool IsAttachment(string path);
    }
}
