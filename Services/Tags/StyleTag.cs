using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard;
using Orchard.UI.Resources;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class StyleTag : Tag
    {
        private string _url;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _url = markup.Trim().Trim('"', '\'');
        }

        public override void Render(Context context, TextWriter result)
        {
            if (HttpContext.Current == null || string.IsNullOrEmpty(_url)) return;

            var wc = HttpContext.Current.Request.RequestContext.GetWorkContext();

            if (wc == null) return;

            wc.Resolve<IResourceManager>().Include("stylesheet", _url, _url);
        }
    }
}