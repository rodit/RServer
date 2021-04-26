using Newtonsoft.Json;
using RServer.Results;

namespace RServer.Routers.Results
{
    /// <summary>
    /// A result that responds with the given object serialized as a JSON string.
    /// </summary>
    public class JsonResult : StringResult
    {
        /// <summary>
        /// Creates a new JSON result from the given object.
        /// </summary>
        /// <param name="obj">The object that this result should serialize.</param>
        public JsonResult(object obj) : base(JsonConvert.SerializeObject(obj)) { }
    }
}
