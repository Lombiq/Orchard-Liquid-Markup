using System;
using System.IO;
using System.Web.Mvc;
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

        public static void WriteHtmlHelperOutputToResult(
            this Context context, 
            Func<HtmlHelper, object> outputFactory, 
            TextWriter result)
        {
            using (var stringWriter = new StringWriter())
            {
                var wc = context.GetWorkContext();
                var controllerContext = new ControllerContext(wc.HttpContext.Request.RequestContext, new DummyController());
                var htmlHelper = new HtmlHelper(
                    new ViewContext(
                        controllerContext, 
                        new WebFormView(controllerContext, "dummy"), 
                        new ViewDataDictionary(), 
                        new TempDataDictionary(), 
                        stringWriter), 
                    new ViewPage());

                result.Write(outputFactory(htmlHelper));
            }
        }


        private class DummyController : ControllerBase
        {
            protected override void ExecuteCore()
            {
            }
        }
    }
}