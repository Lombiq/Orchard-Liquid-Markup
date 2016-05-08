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
        private string _title;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _title = markup.TrimStringParameter();
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_title)) return;

            var wc = context.GetWorkContext();

            var pageTitleBuilder = wc.Resolve<IPageTitleBuilder>();

            pageTitleBuilder.AddTitleParts(_title);
        }
    }
}