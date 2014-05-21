using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Lombiq.LiquidMarkup.Models;
using Lombiq.LiquidMarkup.Services.Filters;
using Lombiq.LiquidMarkup.Services.Tags;
using Orchard.Caching.Services;
using Orchard.DisplayManagement.Implementation;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment.Extensions;
using Orchard.Templates.Services;

namespace Lombiq.LiquidMarkup.Services
{
    [OrchardFeature("Lombiq.LiquidMarkup.Templates")]
    public class LiquidTemplateProcessor : ITemplateProcessor
    {
        private static bool _templateIsConfigured;

        private readonly ICacheService _cacheService;


        public string Type
        {
            get { return "Liquid"; }
        }


        public LiquidTemplateProcessor(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }


        public string Process(string template, string name, DisplayContext context = null, dynamic model = null)
        {
            EnsureTemplateConfigured();

            var templateModel = new StaticShape(model);

            var liquidTemplate = _cacheService.Get(template, () => Template.Parse(template));
            return liquidTemplate.Render(new RenderParameters
            {
                LocalVariables = Hash.FromAnonymousObject(new { Model = templateModel }),
                RethrowErrors = true
            });
        }

        public void Verify(string template)
        {
            EnsureTemplateConfigured();

            Template.Parse(template);
        }


        // This method potentially runs from multiple threads, also the first time but this is safe to do so.
        private static void EnsureTemplateConfigured()
        {
            if (_templateIsConfigured) return;

            // Currently only global configuration is possible, see: https://github.com/formosatek/dotliquid/issues/93
            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            Template.RegisterSafeType(typeof(ShapeMetadata), new[] { "Type", "DisplayType", "Position", "PlacementSource", "Prefix", "Wrappers", "Alternates", "WasExecuted" });
            Template.RegisterTag<StyleTag>("style");
            Template.RegisterTag<ScriptTag>("script");
            Template.RegisterTag<DisplayTag>("display");
            Template.RegisterTag<DisplayTag>("Display");
            Template.RegisterFilter(typeof(DisplayFilter));

            _templateIsConfigured = true;
        }
    }
}