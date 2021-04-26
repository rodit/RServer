using Microsoft.VisualStudio.TestTools.UnitTesting;
using RServer.Web.Providers;
using RServer.Web.Services;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RServer.Web.Tests
{
    [TestClass]
    public class WebTests
    {
        const string Host = "http://localhost:38323/";
        const string HostHttps = "https://localhost:38324/";

        private static HttpClient client;
        private static WebClient wc;
        private static RServer server;

        [ClassInitialize]
        public static void InitServer(TestContext context)
        {
            server = new RServer(new RServerOptions()
                .WithHost(Host)
                .WithHost(HostHttps));
            server.AddLocalFiles()
                .WithFileProvider(new LocalFileProvider("www"))
                .WithMimeService(new DictionaryMimeService()
                {
                    { ".txt", "text/plain" },
                    { ".html", "text/html" },
                    { ".pdf", "application/pdf" }
                });
            server.Start();

            client = new HttpClient();
            client.BaseAddress = new Uri(Host);

            wc = new WebClient();
        }

        [ClassCleanup]
        public static void CleanServer()
        {
            server.Stop();

            client.Dispose();
            wc.Dispose();
        }

        [TestMethod]
        public async Task TestFileContentsAndMime()
        {
            string fileContents = await File.ReadAllTextAsync("www/test.txt");

            var response = await client.GetAsync("test.txt");
            Assert.AreEqual("text/plain", response.Content.Headers.ContentType.MediaType);
            Assert.AreEqual(fileContents, await response.Content.ReadAsStringAsync());
        }

        // [TestMethod]
        public void TestHttps()
        {
            string fileContents = File.ReadAllText("www/test.txt");

            Assert.AreEqual(fileContents, wc.DownloadString(HostHttps + "test.txt"));
        }
    }
}
