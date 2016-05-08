using Lombiq.LiquidMarkup;
using Lombiq.LiquidMarkup.Models;
using Orchard;
using Orchard.DisplayManagement;

namespace DotLiquid
{
    internal static class ContextExtensions
    {
        public static bool ShapeIsWithinAllowedRecursionDepth(this Context context, string shapeType)
        {
            var parentShape = ((StaticShape)context["Model"]).Shape;

            var currentShape = parentShape.ParentShape;
            var recursionDepth = 0;

            while (currentShape != null)
            {
                if (currentShape.Metadata.Type == shapeType) recursionDepth++;

                if (recursionDepth >= Constants.MaxAllowedShapeRecursionDepth) return false;

                currentShape = currentShape.ParentShape;
            }

            return true;
        }

        public static void AddCurrentShapeAsParentToShape(this Context context, IShape shape)
        {
            dynamic dynamicShape = shape;
            if (dynamicShape.ParentShape != null) return;
            dynamicShape.ParentShape = ((StaticShape)context["Model"]).Shape;
        }

        public static WorkContext GetWorkContext(this Context context)
        {
            return (WorkContext)((StaticShape)((StaticShape)context["Model"])["WorkContext"]).Shape;
        }
    }
}