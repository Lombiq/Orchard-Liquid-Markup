using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.Environment;
using Orchard.Environment.Extensions;

namespace Lombiq.LiquidMarkup.Services.ViewEngine
{
    [OrchardFeature("Lombiq.LiquidMarkup.ViewEngine")]
    public class LiquidViewEngine : VirtualPathProviderViewEngine
    {
        private readonly Work<ILiquidTemplateService> _liquidTemplateServiceWork;

        public LiquidViewEngine(Work<ILiquidTemplateService> liquidTemplateServiceWork)
        {
            _liquidTemplateServiceWork = liquidTemplateServiceWork;
            FileExtensions = new[] { "liquid" };
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new LiquidView(_liquidTemplateServiceWork.Value, viewPath, masterPath);
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new LiquidView(_liquidTemplateServiceWork.Value, partialPath, "");
        }
    }
}