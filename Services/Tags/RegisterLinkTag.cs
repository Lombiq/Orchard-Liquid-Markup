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
        private string _conditionParameter;
        private string _hrefParameter;
        private string _relParameter;
        private string _titleParameter;
        private string _typeParameter;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseNamedParameters();

            _conditionParameter = parameters.FindParameterValue("condition");
            _hrefParameter = parameters.FindParameterValue("href");
            _relParameter = parameters.FindParameterValue("rel");
            _titleParameter = parameters.FindParameterValue("title");
            _typeParameter = parameters.FindParameterValue("type");
        }

        public override void Render(Context context, TextWriter result)
        {
            var wc = context.GetWorkContext();

            var resourceManager = wc.Resolve<IResourceManager>();

            resourceManager.RegisterLink(new LinkEntry
                {
                    Condition = _conditionParameter.EvaluateAsStringProducingParameter(context),
                    Href = _hrefParameter.EvaluateAsStringProducingParameter(context),
                    Rel = _relParameter.EvaluateAsStringProducingParameter(context),
                    Title = _titleParameter.EvaluateAsStringProducingParameter(context),
                    Type = _typeParameter.EvaluateAsStringProducingParameter(context)
                });
        }
    }
}