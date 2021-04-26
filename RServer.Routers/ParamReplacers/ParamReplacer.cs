using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RServer.Routers.ParamReplacers
{
    /// <summary>
    /// Provides a mapping of route parameter type names to regex matchers that validate the type's input.
    /// </summary>
    public class ParamReplacer
    {
        private readonly Dictionary<string, string> _replacements = new Dictionary<string, string>()
        {
            { "", ".*" },
            { "int", "-?[0-9]+" },
            { "bool", "true|false" }
        };

        /// <summary>
        /// Maps the route parameter type name to the given regex matcher that will replace it.
        /// </summary>
        /// <param name="name">The name of the parameter type.</param>
        /// <param name="replacement">The regex that will be used to validate the value of this parameter when matching the route.</param>
        public void AddReplacement(string name, string replacement)
        {
            _replacements[name] = replacement;
        }

        /// <summary>
        /// Replaces a route parameter pattern <c>{name:type}</c> with a regex that will match it.
        /// </summary>
        /// <param name="pattern">The route parameter pattern.</param>
        /// <returns>The regex that will match the given route parameter pattern.</returns>
        public string ReplacePattern(string pattern)
        {
            return Regex.Replace(pattern, "{([a-zA-Z0-9_]+):([a-zA-Z0-9_]*)}", m =>
            {
                if (_replacements.TryGetValue(m.Groups[2].Value, out string replace))
                {
                    return $"(?<{m.Groups[1].Value}>{replace})";
                }
                throw new Exception($"Invalid parameter matcher {m.Groups[2].Value}.");
            });
        }
    }
}
