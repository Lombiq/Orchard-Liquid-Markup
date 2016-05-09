using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.UI.Resources;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class ScriptTag : Tag
    {
        private string _resourceReferenceParameter;
        private ResourceLocation _location = ResourceLocation.Foot;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseParameters();

            if (!parameters.Any()) return;

            _resourceReferenceParameter = parameters.First();

            if (parameters.Count() == 2 && parameters.Last().Equals("head", StringComparison.InvariantCultureIgnoreCase))
            {
                _location = ResourceLocation.Head;
            }
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_resourceReferenceParameter)) return;

            var resourceManager = context.GetWorkContext().Resolve<IResourceManager>();

            var evaluatedResourceReferenceParameter = _resourceReferenceParameter.EvaluateAsStringProducingParameter(context);
            RequireSettings script;

            // _resourceReference can be a resource name or an URL.
            if (TagName.Equals("scriptrequire", StringComparison.InvariantCultureIgnoreCase))
            {
                script = resourceManager.Require("script", evaluatedResourceReferenceParameter);
            }
            else
            {
                script = resourceManager
                    .Include("script", evaluatedResourceReferenceParameter, evaluatedResourceReferenceParameter);
            }

            script.Location = _location;
        }
    }
}