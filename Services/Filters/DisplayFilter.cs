using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Lombiq.LiquidMarkup.Models;
using Orchard.DisplayManagement;

namespace Lombiq.LiquidMarkup.Services.Filters
{
    public static class DisplayFilter
    {
        public static string Display(Context context, StaticShape shape)
        {
            if (shape == null) return string.Empty;

            var wc = HttpContext.Current.GetWorkContext();

            if (wc == null) return string.Empty;

            return wc.Resolve<IShapeDisplay>().Display(shape.Shape);
        }


        // This would be needed for string input.
        //public static string Display(Context context, string input)
        //{
        //    if (string.IsNullOrEmpty(input)) return string.Empty;

        //    var wc = HttpContext.Current.GetWorkContext();

        //    if (wc == null) return string.Empty;

        //    var staticShape = context.Environments[0]["Model"] as StaticShape;

        //    if (staticShape == null) return string.Empty;

        //    IEnumerable<string> shapePropertySegments = input.TrimParameter().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (shapePropertySegments.First() == "Model") shapePropertySegments = shapePropertySegments.Skip(1);

        //    var shape = FetchShape(staticShape, shapePropertySegments);
        //    if (shape == null) return string.Empty;
        //    return wc.Resolve<IShapeDisplay>().Display(shape);
        //}


        //private static dynamic FetchShape(StaticShape staticShape, IEnumerable<string> shapePropertySegments)
        //{
        //    dynamic innerShape = null;

        //    foreach (var segment in shapePropertySegments)
        //    {
        //        innerShape = staticShape[segment];
        //        if (!(innerShape is StaticShape)) return null;
        //    }

        //    if (innerShape == null) return null;

        //    return ((StaticShape)innerShape).Shape;
        //}
    }
}