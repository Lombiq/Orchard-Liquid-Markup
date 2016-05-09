using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.Mvc.ViewEngines.Razor;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class HrefTag : ParametersParsingTagBase
    {
        public override void Render(Context context, TextWriter result)
        {
            if (_parameters == null || !_parameters.Any()) return;

            var evaluatedParameters = GetEvaluatedParameters(context);

            var firstEvaluatedParameter = evaluatedParameters.First();
            if (firstEvaluatedParameter == null) return;

            var path = firstEvaluatedParameter.ToString();

            if (string.IsNullOrEmpty(path)) return;

            // Converting the URL into a virtual relative path.
            if (!path.StartsWith("~"))
            {
                if (!path.StartsWith("/"))
                {
                    path = "/" + path;
                }

                path = "~" + path;
            }

            var wc = context.GetWorkContext();
            var webViewPage = new DummyWebViewPage
            {
                Context = wc.HttpContext,
                WorkContext = wc
            };
            result.Write(webViewPage.Href(path, evaluatedParameters.Skip(1).ToArray()));
        }


        private class DummyWebViewPage : WebViewPage<object>
        {
            public override void Execute()
            {
            }
        }
    }
}