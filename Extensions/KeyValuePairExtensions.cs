using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Collections.Generic
{
    public static class KeyValuePairExtensions
    {
        public static string FindParameterValue(this IEnumerable<KeyValuePair<string, string>> parameters, string name)
        {
            var parameterKvp = parameters
                .FirstOrDefault(parameter => parameter.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            return parameterKvp.Equals(default(KeyValuePair<string, string>)) ? null : parameterKvp.Value;
        }
    }
}