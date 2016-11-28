using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using DotLiquid;
using Orchard.UI.Resources;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class StyleTag : ResourceManagingTagBase
    {
        private string _resourceReferenceParameter;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _resourceReferenceParameter = markup;
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_resourceReferenceParameter)) return;

            var evaluatedResourceReferenceParameter = _resourceReferenceParameter.EvaluateAsStringProducingParameter(context);

            // _resourceReference can be a resource name or an URL.
            if (TagName.Equals("stylerequire", StringComparison.InvariantCultureIgnoreCase))
            {
                RequireResource("stylesheet", evaluatedResourceReferenceParameter, context);
            }
            else
            {
                IncludeResource("stylesheet", evaluatedResourceReferenceParameter, context);
            }
        }
    }
}