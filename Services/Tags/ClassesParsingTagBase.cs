using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class ClassesParsingTagBase : Tag
    {
        protected IEnumerable<string> _classParameters;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _classParameters = markup.ParseParameters();
        }


        protected object[] GetEvaluatedClassParameters(Context context)
        {
            return _classParameters.Select(classParameter => classParameter.EvaluateAsParameter(context)).ToArray();
        }
    }
}