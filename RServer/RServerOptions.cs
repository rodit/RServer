using System;
using System.Collections.Generic;
using System.Text;

namespace RServer
{
    /// <summary>
    /// A class holding options that an <c>RServer</c> configures itself with when started.
    /// </summary>
    public class RServerOptions
    {
        /// <summary>
        /// A list of host names (prefixes) that the server should listen on.
        /// </summary>
        public List<string> Hosts { get; } = new List<string>();

        /// <summary>
        /// Adds the specified host to the list of hosts in these options.
        /// </summary>
        /// <param name="host">The host to add.</param>
        public RServerOptions WithHost(string host)
        {
            Hosts.Add(host);
            return this;
        }

        /// <summary>
        /// Adds all of the specified hosts to the list of hosts in these options.
        /// </summary>
        /// <param name="hosts">The hosts to add.</param>
        public RServerOptions WithHosts(IEnumerable<string> hosts)
        {
            Hosts.AddRange(hosts);
            return this;
        }
    }
}
