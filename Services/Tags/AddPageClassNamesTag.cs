using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.UI.PageClass;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class AddPageClassNamesTag : ParametersParsingTagBase
    {
        public override void Render(Context context, TextWriter result)
        {
            if (_parameters == null || !_parameters.Any()) return;

            context.GetWorkContext().Resolve<IPageClassBuilder>().AddClassNames(GetEvaluatedParameters(context));
        }
    }
}