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

            // Checking if the shape is displayed multiple times. If yes it can be legitimate (although rare) but
            // can also indicate unwanted recursion, so capping it.
            if (shape.Shape.DisplayedCount == null) shape.Shape.DisplayedCount = 0;

            if (shape.Shape.DisplayedCount >= Constants.MaxAllowedShapeDisplayCount)
            {
                wc.LogSecurityNotificationWithContext(typeof(DisplayFilter), "Too many displays of the same shape prevented.");
                
                return string.Empty;
            }

            shape.Shape.DisplayedCount++;

            if (!context.ShapeIsWithinAllowedRecursionDepth(shape.Metadata.Type))
            {
                wc.LogSecurityNotificationWithContext(typeof(DisplayFilter), "Too many recursive shape displays prevented.");

                return string.Empty;
            }

            context.AddCurrentShapeAsParentToShape((IShape)shape.Shape);

            return wc.Resolve<IShapeDisplay>().Display(shape.Shape);
        }
    }
}