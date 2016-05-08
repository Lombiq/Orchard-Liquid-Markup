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
            if (_classParameters == null || !_classParameters.Any()) return;

            context.GetWorkContext().Resolve<IPageClassBuilder>().AddClassNames(GetEvaluatedClassParameters(context));
        }
    }
}