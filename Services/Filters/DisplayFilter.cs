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
        public static string Display(Context context, dynamic input)
        {
            if (input == null || !(input is StaticShape)) return string.Empty;

            StaticShape shape = input;
            var wc = HttpContext.Current.GetWorkContext();

            if (wc == null) return string.Empty;

            return wc.Resolve<IShapeDisplay>().Display(shape.Shape);
        }
    }
}