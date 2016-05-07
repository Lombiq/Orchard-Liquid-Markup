using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class StringExtensions
    {
        public static KeyValuePair<string, string> ParseAsNamedParameter(this string parameterNameAndValue)
        {
            var parameterSegments = parameterNameAndValue
                        .Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(TrimStringParameter);

            return new KeyValuePair<string, string>(parameterSegments.First(), parameterSegments.Last());
        }

        public static string TrimStringParameter(this string input)
        {
            return input.Trim().Trim('"', '\'');
        }

        public static IEnumerable<string> ParseParameters(this string parametersCommaSeparated)
        {
            parametersCommaSeparated = parametersCommaSeparated.Trim();
            return parametersCommaSeparated
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(parameter => parameter.Trim());
        }

        public static IEnumerable<KeyValuePair<string, string>> ParseNamedParameters(this string parametersCommaSeparated)
        {
            return parametersCommaSeparated.ParseParameters().Select(ParseAsNamedParameter);
        }
    }
}