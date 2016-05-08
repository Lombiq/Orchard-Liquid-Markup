using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.DisplayManagement;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public class DisplayTag : Tag
    {
        private const string DisplayedShapeTypesKey = "DisplayedShapeTypes";

        private string _shapeType;
        private IEnumerable<KeyValuePair<string, string>> _arguments;


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseNamedParameters();

            if (!parameters.Any()) return;

            _shapeType = parameters.First().Key;
            _arguments = parameters.Skip(1);
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_shapeType)) return;

            var wc = context.GetWorkContext();

            if (!context.ShapeIsWithinAllowedRecursionDepth(_shapeType))
            {
                wc.LogSecurityNotificationWithContext(typeof(DisplayTag), "Too many recursive shape displays prevented.");

                return;
            }

            var argumentsDictionary = _arguments.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
            var shape = wc.Resolve<IShapeFactory>().Create(_shapeType, Arguments.From(argumentsDictionary));
            context.AddCurrentShapeAsParentToShape(shape);
            result.Write(wc.Resolve<IShapeDisplay>().Display(shape));
        }
    }
}