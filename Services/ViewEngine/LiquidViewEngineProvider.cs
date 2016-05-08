using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Orchard.DisplayManagement.Descriptors.ShapeTemplateStrategy;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.Logging;
using Orchard.Mvc.ViewEngines;
using Orchard.Mvc.ViewEngines.ThemeAwareness;

namespace Lombiq.LiquidMarkup.Services.ViewEngine
{
    [OrchardFeature("Lombiq.LiquidMarkup.ViewEngine")]
    public class LiquidViewEngineProvider : IViewEngineProvider, IShapeTemplateViewEngine
    {
        private readonly Work<ILiquidTemplateService> _liquidTemplateServiceWork;
        private static readonly string[] DisabledFormats = new[] { "~/Disabled" };

        public ILogger Logger { get; set; }


        public LiquidViewEngineProvider(Work<ILiquidTemplateService> liquidTemplateServiceWork)
        {
            _liquidTemplateServiceWork = liquidTemplateServiceWork;

            Logger = NullLogger.Instance;
        }


        public IViewEngine CreateThemeViewEngine(CreateThemeViewEngineParams parameters)
        {
            var partialViewLocationFormats = new[]
            {
                parameters.VirtualPath + "/Views/{0}.liquid",
                parameters.VirtualPath + "/Views/{1}/{0}.liquid"
            };

            var areaPartialViewLocationFormats = new[]
            {
                parameters.VirtualPath + "/Views/{2}/{1}/{0}.liquid"
            };

            var viewEngine = new LiquidViewEngine(_liquidTemplateServiceWork)
            {
                MasterLocationFormats = DisabledFormats,
                ViewLocationFormats = DisabledFormats,
                PartialViewLocationFormats = partialViewLocationFormats,
                AreaMasterLocationFormats = DisabledFormats,
                AreaViewLocationFormats = DisabledFormats,
                AreaPartialViewLocationFormats = areaPartialViewLocationFormats,
                ViewLocationCache = new ThemeViewLocationCache(parameters.VirtualPath),
            };

            return viewEngine;
        }

        public IViewEngine CreateModulesViewEngine(CreateModulesViewEngineParams parameters)
        {
            // Below three lines copied from RazorViewEngineProvider. Must revisit if that class changes.
            // TBD: It would probably be better to determined the area deterministically from the module of the controller, 
            // not by trial and error.
            var areaFormats = parameters.ExtensionLocations.Select(location => location + "/{2}/Views/{1}/{0}.liquid").ToArray();

            var universalFormats = parameters.VirtualPaths
                .SelectMany(
                    x => new[]
                        {
                            x + "/Views/{0}.liquid",
                        })
                .ToArray();

            var viewEngine = new LiquidViewEngine(_liquidTemplateServiceWork)
            {
                MasterLocationFormats = DisabledFormats,
                ViewLocationFormats = universalFormats,
                PartialViewLocationFormats = universalFormats,
                AreaMasterLocationFormats = DisabledFormats,
                AreaViewLocationFormats = areaFormats,
                AreaPartialViewLocationFormats = areaFormats,
            };

            return viewEngine;
        }

        public IViewEngine CreateBareViewEngine()
        {
            return new LiquidViewEngine(_liquidTemplateServiceWork)
            {
                MasterLocationFormats = DisabledFormats,
                ViewLocationFormats = DisabledFormats,
                PartialViewLocationFormats = DisabledFormats,
                AreaMasterLocationFormats = DisabledFormats,
                AreaViewLocationFormats = DisabledFormats,
                AreaPartialViewLocationFormats = DisabledFormats,
            };
        }

        public IEnumerable<string> DetectTemplateFileNames(IEnumerable<string> fileNames)
        {
            return fileNames.Where(fileName => fileName.EndsWith(".liquid", StringComparison.OrdinalIgnoreCase));
        }
    }
}