using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.UI.Resources;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class SetMetaTag : Tag
    {
        private string _charset;
        private string _content;
        private string _httpEquiv;
        private string _name;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseNamedParameters();

            _charset = parameters.FindParameterValue("charset");
            _content = parameters.FindParameterValue("content");
            _httpEquiv = parameters.FindParameterValue("httpequiv");
            _name = parameters.FindParameterValue("name");
        }

        public override void Render(Context context, TextWriter result)
        {
            var wc = context.GetWorkContext();

            var resourceManager = wc.Resolve<IResourceManager>();

            resourceManager.SetMeta(new MetaEntry
            {
                Charset = _charset,
                Content = _content,
                HttpEquiv = _httpEquiv,
                Name = _name
            });
        }
    }
}