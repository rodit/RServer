using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RServer.Results;
using RServer.Routers.Filters;
using RServer.Routers.Routes;
using RServer.Routers.Transformers;
using System.Net;

namespace RServer.Routers.Tests
{
    [TestClass]
    public class RouterTests
    {
        const string Host = "http://localhost:38323/";

        private static RServer server;

        [ClassInitialize]
        public static void InitServer(TestContext context)
        {
            server = new RServer(new RServerOptions()
                .WithHost(Host));
            server.AddRouter()
                .AddRoutes<TestRoutes>();
            server.Start();
        }

        [ClassCleanup]
        public static void CleanServer()
        {
            server.Stop();
        }

        [TestMethod]
        public void TestBasicRoute()
        {
            Assert.AreEqual("Test string.", Get("test"));
        }

        [TestMethod]
        public void TestParamRoute()
        {
            Assert.AreEqual("1383", Get("test/1383/abc"));
            Assert.AreEqual("2524321", Get("test/2524321/abc"));
            Assert.AreEqual("135224", Get("test/135224/abc"));
        }

        [TestMethod]
        public void TestIncorrectRoute()
        {
            Assert.AreNotEqual("28472", Get("test/28472/abcd"));
            Assert.AreNotEqual("3422", Get("test/3422"));
            Assert.AreNotEqual("23524", Get("test/23524/"));
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void TestFilteredRoute()
        {
            Assert.AreNotEqual("Test filtered response.", Get("test/filtered"));
        }

        [TestMethod]
        public void TestMultiRoute()
        {
            Assert.AreEqual("37429,True", Get("test/37429/1/true"));
            Assert.AreEqual("55144,False", Get("test/55144/1/false"));
        }

        [TestMethod]
        public void TestPost()
        {
            Assert.AreEqual("testvalue1!testvalue2", Post("test/post", new TestRequest()
            {
                TestParam1 = "testvalue1",
                TestParam2 = "testvalue2"
            }));
        }

        [TestMethod]
        public void TestPostWithParam()
        {
            Assert.AreEqual("testvalue1!testvalue2#88428", Post("test/88428/post", new TestRequest()
            {
                TestParam1 = "testvalue1",
                TestParam2 = "testvalue2"
            }));
        }

        private string Get(string url)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadString(Host + url);
            }
        }

        private string Post(string url, object body)
        {
            using (var wc = new WebClient())
            {
                return wc.UploadString(Host + url, JsonConvert.SerializeObject(body));
            }
        }
    }

    [Transformer(typeof(JsonTransformer))]
    public class TestRoutes
    {
        [Get("/test")]
        public StringResult Test()
        {
            return "Test string.";
        }

        [Get("/test/{param:int}/abc")]
        public IResult TestParam(int param)
        {
            return new StringResult(param);
        }

        [TestFilter.Attribute]
        [Get("/test/filtered")]
        public StringResult TestFiltered()
        {
            return "Test filtered response.";
        }

        [Get("/test/{param1:int}/1/{param2:bool}")]
        public StringResult TestMultiParam(int param1, bool param2)
        {
            return param1 + "," + param2;
        }

        [Post("/test/post")]
        public StringResult TestPost(TestRequest req)
        {
            return req.TestParam1 + "!" + req.TestParam2;
        }

        [Post("/test/{param1:int}/post")]
        public StringResult TestPostParam(int param1, TestRequest req)
        {
            return req.TestParam1 + "!" + req.TestParam2 + "#" + param1;
        }
    }

    public class TestFilter : IFilter
    {
        public IResult Filter(HttpListenerContext context)
        {
            return new StatusResult(401);
        }

        public class Attribute : FilterAttribute
        {
            public Attribute() : base(typeof(TestFilter)) { }
        }
    }

    public class TestRequest
    {
        [JsonProperty("param1")]
        public string TestParam1 { get; set; }
        [JsonProperty("param2")]
        public string TestParam2 { get; set; }
    }
}
