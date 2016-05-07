using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.UI.Resources;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class RegisterLinkTag : Tag
    {
        private string _condition;
        private string _href;
        private string _rel;
        private string _title;
        private string _type;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseNamedParameters();

            _condition = parameters.FindParameterValue("condition");
            _href = parameters.FindParameterValue("href");
            _rel = parameters.FindParameterValue("rel");
            _title = parameters.FindParameterValue("title");
            _type = parameters.FindParameterValue("type");
        }

        public override void Render(Context context, TextWriter result)
        {
            var wc = HttpContext.Current.GetWorkContext();

            if (wc == null) return;

            var resourceManager = wc.Resolve<IResourceManager>();

            resourceManager.RegisterLink(new LinkEntry
                {
                    Condition = _condition,
                    Href = _href,
                    Rel = _rel,
                    Title = _title,
                    Type = _type
                });
        }
    }
}