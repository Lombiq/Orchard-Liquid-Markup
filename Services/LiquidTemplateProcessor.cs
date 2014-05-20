using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotLiquid;
using Orchard.Caching.Services;
using Orchard.DisplayManagement.Implementation;
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

            dynamic templateModel = null;

            if (model is Orchard.DisplayManagement.Shapes.Shape)
            {
                templateModel = new Shape { Id = model.Id, Classes = model.Classes };
            }

            var liquidTemplate = _cacheService.Get(template, () => Template.Parse(template));
            return liquidTemplate.Render(new RenderParameters
            {
                LocalVariables = Hash.FromDictionary(new Dictionary<string, object>() { { "Model", templateModel } }),
                RethrowErrors = true
            });
        }

        public void Verify(string template)
        {
            EnsureTemplateConfigured();

            Template.Parse(template);
        }


        // This method potentially runs from multiple threads, also the first time but this is safe to do.
        private static void EnsureTemplateConfigured()
        {
            if (_templateIsConfigured) return;

            // Currently only global configuration is possible, see: https://github.com/formosatek/dotliquid/issues/93
            Template.NamingConvention = new DotLiquid.NamingConventions.CSharpNamingConvention();
            Template.RegisterSafeType(typeof(Shape), new[] { "Id", "Classes" });

            _templateIsConfigured = true;
        }
    }


    public class Shape
    {
        public string Id { get; set; }
        public IList<string> Classes { get; set; }
    }
}