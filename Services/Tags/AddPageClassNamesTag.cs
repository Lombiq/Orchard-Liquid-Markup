using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.UI.PageClass;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class AddPageClassNamesTag : ClassesParsingTagBase
    {
        public override void Render(Context context, TextWriter result)
        {
            if (_classes == null || !_classes.Any()) return;

            var wc = context.GetWorkContext();

            var pageClassBuilder = wc.Resolve<IPageClassBuilder>();

            pageClassBuilder.AddClassNames(_classes.Select(className => (object)className).ToArray());
        }
    }
}