using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotLiquid;
using Orchard.UI.PageClass;
using Orchard.Mvc.Html;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class ClassForPageTag : ParametersParsingTagBase
    {
        public override void Render(Context context, TextWriter result)
        {
            // It's easier to fake every context and create a HtmlHelper to use the ClassForPage() extension. Note that
            // we also need to encode the output (and this is done in the ClassForPage() extension method)!
            context.WriteHtmlHelperOutputToResult(
                html => html.ClassForPage(GetEvaluatedParameters(context)),
                result);
        }
    }
}