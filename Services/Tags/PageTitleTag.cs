using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.UI.PageTitle;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class PageTitleTag : Tag
    {
        private string _titleParameter;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _titleParameter = markup;
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_titleParameter)) return;

            context
                .GetWorkContext()
                .Resolve<IPageTitleBuilder>()
                .AddTitleParts(_titleParameter.EvaluateAsStringProducingParameter(context));
        }
    }
}