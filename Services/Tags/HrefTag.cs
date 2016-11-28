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

            var resultUrl = string.Empty;
            var workContext = context.GetWorkContext();

            var pathResolver = workContext.Resolve<ITemplateItemProvidedPathResolver>();
            var renderingContext = context.GetTemplateRenderingContext();
            if (renderingContext.TemplateType == Models.TemplateType.TemplateContentItem && !pathResolver.IsRealVirtualPath(path))
            {
                resultUrl = pathResolver.GenerateUrlFromPath(path);
            }
            else
            {
                var webViewPage = new DummyWebViewPage
                {
                    Context = workContext.HttpContext,
                    WorkContext = workContext
                };
                resultUrl = webViewPage.Href(path, evaluatedParameters.Skip(1).ToArray());
            }

            result.Write(resultUrl);
        }


        private class DummyWebViewPage : WebViewPage<object>
        {
            public override void Execute()
            {
            }
        }
    }
}