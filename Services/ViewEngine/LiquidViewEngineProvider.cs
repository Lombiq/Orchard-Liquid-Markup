using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                parameters.VirtualPath + "/Views/{0}.liquid"
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
            var areaFormats = new[]
            {
                "~/Core/{2}/Views/{1}/{0}.liquid",
                "~/Modules/{2}/Views/{1}/{0}.liquid",
                "~/Themes/{2}/Views/{1}/{0}.liquid",
            };

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