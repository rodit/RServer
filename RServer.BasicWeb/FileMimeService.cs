using RServer.Web.Services;
using System.IO;
using System.Linq;

namespace RServer.BasicWeb
{
    public class FileMimeService : DictionaryMimeService
    {
        public FileMimeService(string file)
        {
            if (File.Exists(file))
            {
                foreach ((string key, string value) in File.ReadLines(file).Select(l => l.Split('=')).Where(l => l.Length == 2).Select(l => (l[0], l[1])))
                {
                    Add(key, value);
                }
            }
        }
    }
}
