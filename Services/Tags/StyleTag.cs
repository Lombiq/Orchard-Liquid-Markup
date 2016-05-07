using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using DotLiquid;
using Orchard.UI.Resources;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class StyleTag : Tag
    {
        private string _resourceReference;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _resourceReference = markup.TrimParameter();
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_resourceReference)) return;

            var wc = HttpContext.Current.GetWorkContext();

            if (wc == null) return;

            var resourceManager = wc.Resolve<IResourceManager>();

            // _resourceReference can be a resource name or an URL.
            if (TagName.Equals("stylerequire", StringComparison.InvariantCultureIgnoreCase))
            {
                resourceManager.Require("stylesheet", _resourceReference);
            }
            else resourceManager.Include("stylesheet", _resourceReference, _resourceReference);
        }
    }
}