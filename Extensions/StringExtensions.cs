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
            if (input == null) return null;
            return input.Trim().Trim('"', '\'');
        }

        public static IEnumerable<string> ParseParameters(this string parametersCommaSeparated)
        {
            parametersCommaSeparated = parametersCommaSeparated.Trim(',', ' ');

            var parametersSplit = parametersCommaSeparated
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var parameters = new List<string>();
            var parameterChunks = "";
            foreach (var parameter in parametersSplit)
            {
                if (parameter.CountOfCharacter('\'') == 1 || 
                    parameter.CountOfCharacter('"') == 1)
                {
                    if (parameterChunks.Any())
                    {
                        parameters.Add(parameterChunks + parameter);

                        parameterChunks = "";
                    }
                    else
                    {
                        parameterChunks += parameter + ",";
                    }

                    continue;
                }

                if (parameterChunks.Any())
                {
                    parameterChunks += parameter + ",";
                }
                else
                {
                    parameters.Add(parameter);
                }
            }

            return parameters;
        }

        public static IEnumerable<KeyValuePair<string, string>> ParseNamedParameters(this string parametersCommaSeparated)
        {
            return parametersCommaSeparated.ParseParameters().Select(ParseAsNamedParameter);
        }

        public static bool IsStringParameter(this string parameterValue)
        {
            if (parameterValue == null) return true;

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


        public static int CountOfCharacter(this string text, char character) =>
            text.Split(character).Length - 1;
    }
}