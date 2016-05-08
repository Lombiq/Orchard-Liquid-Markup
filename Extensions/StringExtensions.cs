using System.Collections.Generic;
using System.Linq;
using DotLiquid;

namespace System
{
    public static class StringExtensions
    {
        public static KeyValuePair<string, string> ParseAsNamedParameter(this string parameterNameAndValue)
        {
            parameterNameAndValue = parameterNameAndValue.Trim();
            
            var colonIndex = parameterNameAndValue.IndexOf(':');

            if (colonIndex == -1)
            {
                return new KeyValuePair<string, string>(parameterNameAndValue, parameterNameAndValue);
            }

            return new KeyValuePair<string, string>(
                parameterNameAndValue.Substring(0, colonIndex),
                parameterNameAndValue.Substring(colonIndex + 1));
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

        public static bool IsStringParameter(this string parameterValue)
        {
            var trimmedParameterValue = parameterValue.Trim();
            return
                trimmedParameterValue.StartsWith("\"") && trimmedParameterValue.EndsWith("\"") ||
                trimmedParameterValue.StartsWith("'") && trimmedParameterValue.EndsWith("'");
        }

        public static object EvaluateAsParameter(this string parameterValue, Context context)
        {
            if (parameterValue.IsStringParameter()) return parameterValue.TrimStringParameter();
            return context[parameterValue];
        }

        public static string EvaluateAsStringProducingParameter(this string parameterValue, Context context)
        {
            var evaluatedParameter = parameterValue.EvaluateAsParameter(context);

            if (evaluatedParameter == null) return null;
            if (evaluatedParameter is string) return (string)evaluatedParameter;
            return evaluatedParameter.ToString();
        }
    }
}