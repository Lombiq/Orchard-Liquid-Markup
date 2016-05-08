using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class AddClassToCurrentShapeTag : Tag
    {
        private string _classNameParameter;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _classNameParameter = markup;
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_classNameParameter)) return;

            dynamic shape = context["Model"];
            shape.Classes.Add(_classNameParameter.EvaluateAsStringProducingParameter(context));
        }
    }
}