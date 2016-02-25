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
        private string _resourceReference;
        private ResourceLocation _location = ResourceLocation.Foot;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseParameters();

            if (!parameters.Any()) return;

            _resourceReference = parameters.First();

            if (parameters.Count() == 2 && parameters.Last() == "head")
            {
                _location = ResourceLocation.Head;
            }
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_resourceReference)) return;

            var wc = HttpContext.Current.GetWorkContext();

            if (wc == null) return;

            var resourceManager = wc.Resolve<IResourceManager>();

            RequireSettings script;

            // _resourceReference can be a resource name or an URL.
            if (TagName == "scriptrequire") script = resourceManager.Require("script", _resourceReference);
            else script = resourceManager.Include("script", _resourceReference, _resourceReference);

            script.Location = _location;
        }
    }
}