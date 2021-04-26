using RServer.Web;
using RServer.Web.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RServer.BasicWeb
{
    public class Program
    {
        const string HostsFile = "hosts.list";

        public static void Main(string[] args)
        {
            List<string> hosts = File.Exists(HostsFile) ? File.ReadAllLines(HostsFile).ToList() : new List<string>();

            var server = new RServer(new RServerOptions().WithHosts(hosts));

            server.AddLocalFiles()
                .WithFileProvider(new LocalFileProvider("www"))
                .WithMimeService(new FileMimeService("mime.map"));

            server.Start();

            while (Console.ReadLine() != ".exit") ;
        }
    }
}
