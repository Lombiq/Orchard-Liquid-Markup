using System.Web;
using DotLiquid;
using Lombiq.LiquidMarkup.Helpers;
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

            if (!ShapeDisplayHelper.IsShapeDisplayAllowed(typeof(DisplayChildrenFilter), context, shape))
            {
                return string.Empty;
            }

            return context.GetWorkContext().Resolve<IShapeDisplay>().Display(shape.Shape);
        }
    }
}