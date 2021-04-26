using Microsoft.Extensions.DependencyInjection;
using RServer.Handlers;
using RUtils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RServer
{
    /// <summary>
    /// A highly extensible, performant HTTP server.
    /// </summary>
    public class RServer
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly List<IRequestHandler> _handlers = new List<IRequestHandler>();

        private CancellationTokenSource _cts;
        private IServiceProvider _services;

        /// <summary>
        /// Gets or sets the options used to configure the server.
        /// </summary>
        public RServerOptions Options { get; set; }
        /// <summary>
        /// Gets the service collection used to build the service provider for this server. Modifications to this collection are recognised each time the server starts.
        /// </summary>
        public ServiceCollection Services { get; } = new ServiceCollection();

        /// <summary>
        /// Creates a new <see cref="RServer"/> with the given options.
        /// </summary>
        /// <param name="options">The options to pass to the server. If null, default options will be used.</param>
        public RServer(RServerOptions options = null)
        {
            Options = options;
        }

        /// <summary>
        /// Adds the specified handler to this server's list of handlers. This method is not thread safe and will sort the handler list in order of priority after adding the new handler.
        /// </summary>
        /// <param name="handler">The request handler to add to the server.</param>
        /// See <see cref="IRequestHandler.Priority"/>.
        public void AddHandler(IRequestHandler handler)
        {
            _handlers.Add(handler);
            _handlers.Sort(RequestHandlers.Comparer);
        }

        private void LoadOptions()
        {
            _listener.Prefixes.Clear();
            Options.Hosts.ForEach(_listener.Prefixes.Add);
        }

        /// <summary>
        /// Starts the server. Options are loaded and the server's service provider is built before listening on the underlying HTTP listener.
        /// </summary>
        public void Start()
        {
            LoadOptions();

            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            _services = Services.BuildServiceProvider();

            _listener.Start();
            Task.Run(Listen);
        }

        /// <summary>
        /// Stops the server immediately.
        /// </summary>
        public void Stop()
        {
            _cts.Cancel();
            _listener.Stop();
        }

        private void Listen()
        {
            Logger.Default.Info("RServer", "Server listening.");
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    var context = _listener.GetContext();

                    Task.Run(() => HandleContext(context));
                }
                catch (HttpListenerException e) when (e.ErrorCode == 995)
                {
                }
                catch (Exception e)
                {
                    Logger.Default.Error("RServer", "Error while getting context:", e);
                }
            }
        }

        private void HandleContext(HttpListenerContext context)
        {
            try
            {
                var result = _handlers.Select(h => h.Handle(_services, context)).FirstOrDefault(r => r != null);
                if (result == null)
                {
                    Logger.Default.Warn("HandleContext", $"No handler found for {context.Request.Url}.");
                    context.Response.StatusCode = 404;
                }
                else
                {
                    result.Respond(context);
                }
            }
            catch (Exception e)
            {
                Logger.Default.Error("HandleContext", $"Error while handling request {context.Request.Url}:", e);
            }

            try
            {
                context.Response.Close();
            }
            catch { }
        }
    }
}
