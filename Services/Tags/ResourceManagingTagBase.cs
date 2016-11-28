using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using DotLiquid;
using Orchard.UI.Resources;
using Piedone.HelpfulLibraries.Utilities;

namespace Lombiq.LiquidMarkup.Services.Tags
{
    public abstract class ResourceManagingTagBase : Tag
    {
        protected RequireSettings RequireResource(string resourceType, string resourceName, Context context)
        {
            return context.GetWorkContext().Resolve<IResourceManager>().Require(resourceType, resourceName);
        }

        protected RequireSettings IncludeResource(string resourceType, string resourcePath, Context context)
        {
            var workContext = context.GetWorkContext();
            var resourceManager = workContext.Resolve<IResourceManager>();
            var pathResolver = workContext.Resolve<ITemplateItemProvidedPathResolver>();

            var renderingContext = context.GetTemplateRenderingContext();
            // If a template file is being rendered then resources paths can be used as usual from themes; if a 
            // template item is rendered then relative virtual paths should be handled the same way (those reference
            // flat files).
            if (renderingContext.TemplateType == Models.TemplateType.TemplateFile ||
                (resourcePath.StartsWith("~") && pathResolver.IsRealVirtualPath(resourcePath)))
            {
                var resourceRegister = new ResourceRegister(
                    new DummyWebPage { VirtualPath = renderingContext.TemplatePath },
                    resourceManager,
                    resourceType);
                return resourceRegister.Include(resourcePath);
            }
            else
            {
                if (!resourcePath.StartsWith("//") &&
                    !resourcePath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !resourcePath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    resourcePath = pathResolver.GenerateUrlFromPath(resourcePath);
                }

                return resourceManager.Include(resourceType, resourcePath, resourcePath);
            }
        }


        private class DummyWebPage : WebPageBase, IViewDataContainer
        {
            public ViewDataDictionary ViewData
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }


            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}