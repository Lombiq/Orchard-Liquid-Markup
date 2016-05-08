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
        private string _charsetParameter;
        private string _contentParameter;
        private string _httpEquivParameter;
        private string _nameParameter;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseNamedParameters();

            _charsetParameter = parameters.FindParameterValue("charset");
            _contentParameter = parameters.FindParameterValue("content");
            _httpEquivParameter = parameters.FindParameterValue("httpequiv");
            _nameParameter = parameters.FindParameterValue("name");
        }

        public override void Render(Context context, TextWriter result)
        {
            var wc = context.GetWorkContext();

            var resourceManager = wc.Resolve<IResourceManager>();

            resourceManager.SetMeta(new MetaEntry
            {
                Charset = _charsetParameter.EvaluateAsStringProducingParameter(context),
                Content = _contentParameter.EvaluateAsStringProducingParameter(context),
                HttpEquiv = _httpEquivParameter.EvaluateAsStringProducingParameter(context),
                Name = _nameParameter.EvaluateAsStringProducingParameter(context)
            });
        }
    }
}