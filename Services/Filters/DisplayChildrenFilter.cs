using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Lombiq.LiquidMarkup.Helpers;
using Lombiq.LiquidMarkup.Models;
using Orchard.DisplayManagement;
using Orchard.Mvc.Spooling;

namespace Lombiq.LiquidMarkup.Services.Filters
{
    public static class DisplayChildrenFilter
    {
        public static string DisplayChildren(Context context, dynamic input)
        {
            if (input == null || !(input is StaticShape)) return string.Empty;

            StaticShape shape = input;
            var wc = context.GetWorkContext();

            var writer = new HtmlStringWriter();
            foreach (var item in shape.Shape)
            {
                if (!ShapeDisplayHelper.IsShapeDisplayAllowed(typeof(DisplayChildrenFilter), context, new StaticShape(item, wc)))
                {
                    return string.Empty;
                }

                writer.Write(wc.Resolve<IShapeDisplay>().Display(item));
            }
            return writer.ToString();
        }
    }
}