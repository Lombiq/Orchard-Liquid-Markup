using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class ParametersParsingTagBase : Tag
    {
        protected IEnumerable<string> _parameters;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _parameters = markup.ParseParameters();
        }


        protected object[] GetEvaluatedParameters(Context context)
        {
            return _parameters.Select(classParameter => classParameter.EvaluateAsParameter(context)).ToArray();
        }
    }
}