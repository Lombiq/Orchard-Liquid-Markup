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
        private Dictionary<string, object> _arguments = new Dictionary<string, object>();


        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            var parameters = markup.ParseParameters();

            if (!parameters.Any()) return;

            _shapeType = parameters.First();

            if (parameters.Count() > 1)
            {
                foreach (var parameter in parameters.Skip(1))
                {
                    var parameterSegments = parameter.TrimParameter()
                        .Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(segment => segment.TrimParameter())
                        .ToArray();

                    if (parameterSegments.Length == 2)
                    {
                        _arguments[parameterSegments[0]] = parameterSegments[1];
                    }
                }
            }
        }

        public override void Render(Context context, TextWriter result)
        {
            if (string.IsNullOrEmpty(_shapeType)) return;

            var wc = HttpContext.Current.GetWorkContext();

            if (wc == null) return;

            if (!context.ShapeIsWithinAllowedRecursionDepth(_shapeType))
            {
                wc.LogSecurityNotificationWithContext(typeof(DisplayTag), "Too many recursive shape displays prevented.");

                return;
            }

            var shape = wc.Resolve<IShapeFactory>().Create(_shapeType, Arguments.From(_arguments));
            context.AddCurrentShapeAsParentToShape(shape);
            result.Write(wc.Resolve<IShapeDisplay>().Display(shape));
        }
    }
}