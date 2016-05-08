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
    public class ClassForPageTag : ClassesParsingTagBase
    {
        public override void Render(Context context, TextWriter result)
        {
            // It's easier to fake every context and create a HtmlHelper to use the ClassForPage() extension. Note that
            // we also need to encode the output (and this is done in the ClassForPage() extension method)!
            using (var stringWriter = new StringWriter())
            {
                var wc = context.GetWorkContext();
                var controllerContext = new ControllerContext(wc.HttpContext.Request.RequestContext, new DummyController());
                var html = new HtmlHelper(new ViewContext(controllerContext, new WebFormView(controllerContext, "dummy"), new ViewDataDictionary(), new TempDataDictionary(), stringWriter), new ViewPage());
                result.Write(html.ClassForPage(GetEvaluatedClassParameters(context)));
            }
        }


        private class DummyController : ControllerBase
        {
            protected override void ExecuteCore()
            {
            }
        }
    }
}