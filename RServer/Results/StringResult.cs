using System.Net;
using System.Text;

namespace RServer.Results
{
    /// <summary>
    /// A result that responds with a string using the specified encoding.
    /// </summary>
    public class StringResult : IResult
    {
        /// <summary>
        /// The string to respond with.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The encoding to respond with.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Creates a new <c>StringResult</c> which will respond with the string representation of <c>val</c>
        /// (by calling <see cref="object.ToString"/> on the given object).
        /// </summary>
        /// <param name="val">The object to respond with.</param>
        public StringResult(object val) : this(val.ToString()) { }

        /// <summary>
        /// Creates a new <see cref="StringResult"/> which will respond with the given string.
        /// </summary>
        /// <param name="text">The text to respond with.</param>
        public StringResult(string text) : this(text, Encoding.UTF8) { }

        /// <summary>
        /// Creates a new <see cref="StringResult"/> which will respond with the given string and encoding.
        /// </summary>
        /// <param name="text">The text to respond with.</param>
        /// <param name="encoding">The encoding to respond with.</param>
        public StringResult(string text, Encoding encoding)
        {
            Text = text;
            Encoding = encoding;
        }

        /// <inheritdoc/>
        public virtual void Respond(HttpListenerContext context)
        {
            context.Response.OutputStream.Write(Encoding.GetBytes(Text));
        }

        public static implicit operator StringResult(string str)
        {
            return new StringResult(str);
        }
    }
}
