using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Lombiq.LiquidMarkup.Models;
using Orchard.DisplayManagement;

namespace Lombiq.LiquidMarkup.Helpers
{
    internal static class ShapeDisplayHelper
    {
        public static bool IsShapeDisplayAllowed(Type displayFilterType, Context context, StaticShape staticShape)
        {
            var wc = context.GetWorkContext();
            var shape = staticShape.Shape;
            var shapeType = staticShape.Metadata.Type;

            // Checking if the shape is displayed multiple times. If yes it can be legitimate (although rare) but can 
            // also indicate unwanted recursion, so capping it.
            if (shape.DisplayedCount == null) shape.DisplayedCount = 0;

            if (shape.DisplayedCount >= Constants.MaxAllowedShapeDisplayCount)
            {
                wc.LogSecurityNotificationWithContext(
                    displayFilterType, 
                    "Too many displays of the same shape (" + shapeType + ") prevented.");

                return false;
            }

            shape.DisplayedCount++;

            if (!context.ShapeIsWithinAllowedRecursionDepth(shapeType))
            {
                wc.LogSecurityNotificationWithContext(displayFilterType, "Too many recursive displays of the " + shapeType + " shape prevented.");

                return false;
            }

            context.AddCurrentShapeAsParentToShape((IShape)shape);

            return true;
        }
    }
}