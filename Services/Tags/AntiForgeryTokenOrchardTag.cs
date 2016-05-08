using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.Mvc.Html;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class AntiForgeryTokenOrchardTag : Tag
    {
        public override void Render(Context context, TextWriter result)
        {
            context.WriteHtmlHelperOutputToResult(html => html.AntiForgeryTokenOrchard(), result);
        }
    }
}