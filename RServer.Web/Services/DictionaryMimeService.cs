using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RServer.Web.Services
{
    public class DictionaryMimeService : IMimeService, IEnumerable<KeyValuePair<string, string>>
    {
        private Dictionary<string, string> _extMimeMap = new Dictionary<string, string>();

        public string DefaultMime { get; set; } = "application/octet-stream";

        public void Add(string extension, string mimeType)
        {
            _extMimeMap[extension] = mimeType;
        }

        public string GetMime(string extension)
        {
            return _extMimeMap.TryGetValue(extension, out string mime) ? mime : DefaultMime;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _extMimeMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _extMimeMap.GetEnumerator();
        }
    }
}
