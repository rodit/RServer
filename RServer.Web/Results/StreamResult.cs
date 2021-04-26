using RServer.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RServer.Web.Results
{
    public class StreamResult : IResult
    {
        private Stream _stream;
        private string _mimeType;
        private int _statusCode;
        private bool _close;

        public StreamResult(Stream stream, string mimeType, int statusCode = 200, bool close = true)
        {
            _stream = stream;
            _mimeType = mimeType;
            _statusCode = statusCode;
            _close = close;
        }

        public virtual void Respond(HttpListenerContext context)
        {
            context.Response.StatusCode = _statusCode;
            context.Response.ContentType = _mimeType;
            _stream.CopyTo(context.Response.OutputStream);
            if (_close)
            {
                _stream.Close();
            }
        }
    }
}
