using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class ClassesParsingTagBase : Tag
    {
        protected IEnumerable<string> _classes;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            _classes = markup.ParseStringParameters();
        }
    }
}